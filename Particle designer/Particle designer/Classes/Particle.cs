using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Particle_designer
{
	public class Particle
	{
		public string Primitive { get => emitter.Primitive; }
		public Color ColorStart { get => emitter.ColorStart; }
		public Color ColorEnd { get => emitter.ColorEnd; }
		public Vector ScaleStart { get => emitter.ScaleStart; }
		public Vector ScaleEnd { get => emitter.ScaleEnd; }
		public bool IsRotated { get; set; }
		public Vector Position { get; set; }                // Start postiion
		public Vector Velocity { get; set; }
		public Vector Acceleration { get; set; }
		public double Direction { get; set; }               // radian
		public double Gravity { get; set; }
		public double GravityDirection { get; set; }        // Radian
		public double TimeAlive { get; set; }               // The time alive of this particle from the first creation
		public double Lifespan { get; set; }                // In second

		private Vector facing_direction;
		private EmitterStream emitter;

		public Particle(EmitterStream emitter)
		{
			TimeAlive = 0;
			facing_direction = new Vector(1, 0);
			this.emitter = emitter;
		}

		/// Update particle's properties including position, velocity, ...
		public void UpdateFrame(double delta_time)
		{
			Vector prev_pos = Position;
			Position += Velocity * delta_time;
			Velocity += (Gravity * new Vector(Math.Cos(GravityDirection), Math.Sin(GravityDirection)) + Acceleration) * delta_time;
			TimeAlive += delta_time;
			facing_direction = Position - prev_pos;
		}

		/// Display particle to OpenGL
		public void Display(OpenGL gl)
		{
			// Initialize
			double time_rate = TimeAlive / Lifespan;
			Color c = updateColor((float)time_rate);
			Matrix matrix = createTransformationMatrix(time_rate);
			uint drawMode = OpenGL.GL_POLYGON;

			List<Vector> vertexlist = null;
			Vector V1, V2, V3, V4, V5, V6;
			double x, y;
			int r;

			// The particle shape depends on the primitive
			switch (Primitive)
			{
				case "Line":
					drawMode = OpenGL.GL_LINES;
					vertexlist = new List<Vector>()
					{
						new Vector(-5, 0),
						new Vector(5, 0)
					};
					break;

				case "Square":
					vertexlist = new List<Vector>()
					{
						new Vector(-5,5),
						new Vector(5,5),
						new Vector(5,-5),
						new Vector(-5,-5)
					};
					break;

				case "Pentagon":
					r = 5;
					vertexlist = new List<Vector>(5);
					double angle = 0;

					for (int i = 0; i < 5; i++)
					{
						vertexlist.Add(r * new Vector(Math.Cos(angle), Math.Sin(angle)));
						angle += 2d * Math.PI / 5d;
					}
					break;

				case "Hexagon":
					r = 5;
					x = Math.Cos(60 * Math.PI / 180) * r;
					y = Math.Sin(60 * Math.PI / 180) * r;
					V1 = new Vector(x, y);
					V2 = new Vector(r, 0);

					x = Math.Cos(60 * Math.PI / 180) * r;
					y = Math.Sin(60 * Math.PI / 180) * r;
					V3 = new Vector(x, -y);
					V4 = new Vector(-V3.X, V3.Y);
					V5 = new Vector(-V2.X, V2.Y);
					V6 = new Vector(-V1.X, V1.Y);

					vertexlist = new List<Vector>() { V1, V2, V3, V4, V5, V6 };
					break;

				case "Circle":
					r = 5;
					vertexlist = new List<Vector>(64);
					angle = 0d;

					for (int i = 0; i < 64; i++)
					{
						vertexlist.Add(r * new Vector(Math.Cos(angle), Math.Sin(angle)));
						angle += 2d * Math.PI / 64d;
					}
					break;
			}

			// Transform the particle
			for (int i = 0; i < vertexlist.Count; i++)
				vertexlist[i] = Vector.Multiply(vertexlist[i], matrix);

			// Draw the particle
			draw(gl, drawMode, c, vertexlist);
		}

		/// Return wheter the particle time alive has passed lifespan
		public bool IsTimeout()
		{
			return TimeAlive > Lifespan;
		}

		/// Return the new color depends on the time_rate
		public Color updateColor(float time_rate)
		{
			return ColorStart * (1 - time_rate) + ColorEnd * time_rate;
		}

		/// Return the new scale depends on the time_rate
		public Vector updateScaleStart(double time_rate)
		{
			return (1 - time_rate) * ScaleStart + time_rate * ScaleEnd;
		}

		/// Return thetransform matrix depends on the time_rate
		public Matrix createTransformationMatrix(double time_rate)
		{
			Vector scale = updateScaleStart(time_rate);
			Matrix matrix = new Matrix();
			matrix.Scale(scale.X, scale.Y);

			if (IsRotated)
				matrix.Rotate(Math.Atan2(facing_direction.Y, facing_direction.X) * 180d / Math.PI);

			return matrix;
		}

		/// Draw a particle
		public void draw(OpenGL gl, uint drawMode, Color c, List<Vector> result)	
		{
			gl.Color(c.R, c.G, c.B, c.A);
			gl.Begin(drawMode);

			for (int i = 0; i < result.Count; i++)
				gl.Vertex(Position.X + result[i].X, Position.Y + result[i].Y);

			gl.End();
		}
	}
}
