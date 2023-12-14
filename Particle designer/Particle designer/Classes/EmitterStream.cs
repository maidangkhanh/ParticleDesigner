using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Particle_designer
{
	public class EmitterStream
	{
		public bool IsEnabled
		{
			get => _is_enable;
			set
			{
				if (_is_enable == value) return;

				if (_is_enable = value)
				{
					timer.Start();
				}
				else
				{
					ClearAll();
					timer.Stop();
				}
			}
		}

		public string Primitive { get; set; }
		public Color ColorStart { get; set; }
		public Color ColorEnd { get; set; }
		public Vector ScaleStart { get; set; }
		public Vector ScaleEnd { get; set; }
		public bool IsRotated { get; set; }
		public Vector Position { get; set; }                // Start postiion
		public Vector Velocity { get; set; }
		public Vector Acceleration { get; set; }
		public Vector Direction { get; set; }               // In degree angle
		public double Gravity { get; set; }
		public double GravityDirection { get; set; }        // In degree angle
		public Vector EmissionRate { get; set; }            // Emission times per second
		public Vector BurstRate { get; set; }               // Particle per emission
		public double Lifespan { get; set; }                // In second
		public bool IsCrossOn { get; set; }

		public List<Particle> ParticleList { get; set; }

		private readonly Random random;
		private double time_pass = 0d;
		private double time_next = 0d;
		private bool _is_enable;
		private readonly DispatcherTimer timer;
		private const int FPS = 30;
		private const double DELTA_TIME = 1d / FPS;

		public EmitterStream()
		{
			IsEnabled = false;
			ParticleList = new List<Particle>(1024);
			time_pass = 0d;
			time_next = 0d;
			random = new Random();
			timer = new DispatcherTimer(DispatcherPriority.Send) { Interval = TimeSpan.FromSeconds(DELTA_TIME) };
			timer.Tick += UpdateFrame;
		}

		/// Create a randomized particle
		public Particle CreateParticle()
		{
			double mag_velocity = random.NextDouble()*(Velocity.Y - Velocity.X) + Velocity.X;
			double mag_acceleration = random.NextDouble()*(Acceleration.Y - Acceleration.X) + Acceleration.X;
			double direction = random.NextDouble() * (Direction.Y - Direction.X) + Direction.X;
			direction *= Math.PI / 180d;
			Vector direction_vector = new Vector(Math.Cos(direction), Math.Sin(direction));

			return new Particle(this)
			{
				//Primitive = this.Primitive,
				IsRotated = this.IsRotated,
				Position = this.Position,
				Velocity = mag_velocity * direction_vector,
				Acceleration = mag_acceleration * direction_vector,
				Direction = direction,
				Gravity = this.Gravity,
				GravityDirection = this.GravityDirection * Math.PI / 180d,
				Lifespan = this.Lifespan,
			};
		}

		/// Clear all particle in the list
		public void ClearAll()
		{
			ParticleList.Clear();
			GC.Collect();
		}

		/// Update all particles in the list every frame if IsEnable
		private void UpdateFrame(object sender, EventArgs e)
		{
			// Exceptions
			if (BurstRate.X > BurstRate.Y) return;
			if (EmissionRate.X > EmissionRate.Y) return;

			if (time_pass >= time_next)
			{
				// Remove timeout particle
				if (ParticleList.RemoveAll(p => p.IsTimeout() == true) > 0)
					GC.Collect();

				// Add new particles
				int number_of_particle = random.Next((int)BurstRate.X, (int)BurstRate.Y + 1);
				int loop = random.Next((int)EmissionRate.X, (int)EmissionRate.Y + 1);

				for (int i = 0; i < number_of_particle; i++)
					ParticleList.Add(CreateParticle());

				time_next += 1d / loop;
			}

			// Update particles
			foreach (Particle particle in ParticleList)
				particle.UpdateFrame(DELTA_TIME);

			time_pass += DELTA_TIME;
		}

		/// Display all the particles if IsEnable
		public void Display(OpenGL gl)
		{
			if (IsEnabled)
			{
				foreach (Particle particle in ParticleList)
					particle.Display(gl);
			}
		}

		/// Load a JSON file and save to the EmitterStream
		public static EmitterStream Load(string filename)
		{
			using (StreamReader r = new StreamReader(filename)) // Open JSON file to read
			{
				string json = r.ReadToEnd();
				r.Close();

				JObject setting = JObject.Parse(json); // Store JSON data to JSON object

				// Load data from JSON object
				EmitterStream emitterStream = new EmitterStream();
				emitterStream.Primitive = (string)setting["Primitive"];
				emitterStream.ColorStart = (Color)ColorConverter.ConvertFromString((string)setting["ColorStart"]);
				emitterStream.ColorEnd = (Color)ColorConverter.ConvertFromString((string)setting["ColorEnd"]);
				emitterStream.ScaleStart = Vector.Parse((string)setting["ScaleStart"]);
				emitterStream.ScaleEnd = Vector.Parse((string)setting["ScaleEnd"]);
				emitterStream.IsRotated = (bool)setting["IsRotated"];
				emitterStream.Position = Vector.Parse((string)setting["Position"]);
				emitterStream.Velocity = Vector.Parse((string)setting["Velocity"]);
				emitterStream.Acceleration = Vector.Parse((string)setting["Acceleration"]);
				emitterStream.Direction = Vector.Parse((string)setting["Direction"]);
				emitterStream.Gravity = double.Parse((string)setting["Gravity"]);
				emitterStream.GravityDirection = double.Parse((string)setting["GravityDirection"]);
				emitterStream.EmissionRate = Vector.Parse((string)setting["EmissionRate"]);
				emitterStream.BurstRate = Vector.Parse((string)setting["BurstRate"]);
				emitterStream.Lifespan = double.Parse((string)setting["Lifespan"]);

				return emitterStream;
			}
		}

		/// Save the current EmitterStream to a file
		public bool Save(string filename)
		{
			JObject setting = new JObject(); // Init JSON object

			// Store data to JSON object
			setting["Primitive"] = this.Primitive;
			setting["ColorStart"] = this.ColorStart.ToString();
			setting["ColorEnd"] = this.ColorEnd.ToString();
			setting["ScaleStart"] = this.ScaleStart.ToString();
			setting["ScaleEnd"] = this.ScaleEnd.ToString();
			setting["IsRotated"] = this.IsRotated;
			setting["Position"] = this.Position.ToString();
			setting["Velocity"] = this.Velocity.ToString();
			setting["Acceleration"] = this.Acceleration.ToString();
			setting["Direction"] = this.Direction.ToString();
			setting["Gravity"] = this.Gravity;
			setting["GravityDirection"] = this.GravityDirection;
			setting["EmissionRate"] = this.EmissionRate.ToString();
			setting["BurstRate"] = this.BurstRate.ToString();
			setting["Lifespan"] = this.Lifespan;

			using (StreamWriter fs = new StreamWriter(filename)) // Write JSON object to file
			{
				fs.Write(setting.ToString());
				fs.Close();
			}
			return true;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder(1024);

			builder.Append($"Position: {Position}");
			builder.Append(Environment.NewLine);

			builder.Append($"Velocity: {Velocity}");
			builder.Append(Environment.NewLine);

			builder.Append($"Acceleration: {Acceleration}");
			builder.Append(Environment.NewLine);

			builder.Append($"ScaleStart: {ScaleStart}");
			builder.Append(Environment.NewLine);

			builder.Append($"ScaleEnd: {ScaleEnd}");
			builder.Append(Environment.NewLine);

			builder.Append($"ColorStart: {ColorStart}");
			builder.Append(Environment.NewLine);

			builder.Append($"ColorEnd: {ColorEnd}");
			builder.Append(Environment.NewLine);

			builder.Append($"Direction: {Direction}");
			builder.Append(Environment.NewLine);

			builder.Append($"Lifespan: {Lifespan}");
			builder.Append(Environment.NewLine);

			builder.Append($"Gravity: {Gravity}");
			builder.Append(Environment.NewLine);

			builder.Append($"GravityDirection: {GravityDirection}");
			builder.Append(Environment.NewLine);

			builder.Append($"Primitive: {Primitive}");
			builder.Append(Environment.NewLine);

			return builder.ToString();
		}
	}
}
