using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for EmitterControl.xaml
	/// </summary>
	public partial class EmitterControl : UserControl
	{
		public EmitterStream Emitter { get; set; }

		public EmitterControl()
		{
			Emitter = new EmitterStream();

			InitializeComponent();
			DataContext = this;
		}

		/// Property changed
		private void PrimitiveChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Primitive = (sender as InputPrimitive).SelectedItem;
		}
		private void ColorStartChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.ColorStart = (sender as InputColor).SelectedColor.Value;
		}
		private void ColorEndChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.ColorEnd = (sender as InputColor).SelectedColor.Value;
		}
		private void ScaleStartChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.ScaleStart = (sender as InputVector).Vector2;
		}
		private void ScaleEndChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.ScaleEnd = (sender as InputVector).Vector2;
		}
		private void RotateParticleChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.IsRotated = (sender as InputCheckBox).IsChecked;
		}
		private void PositionChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Position = (sender as InputVector).Vector2;
		}
		private void DirectionChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Direction = (sender as InputVector).Vector2;
		}
		private void VelocityChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Velocity = (sender as InputVector).Vector2;
		}
		private void AccelerationChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Acceleration = (sender as InputVector).Vector2;
		}
		private void GravityChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Gravity = (sender as InputScalar).Value;
		}
		private void GravityDirectionChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.GravityDirection = (sender as InputScalar).Value;
		}
		private void EmissionRateChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.EmissionRate = (sender as InputVector).Vector2;
		}
		private void BurstRateChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.BurstRate = (sender as InputVector).Vector2;
		}
		private void LifespanChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.Lifespan = (sender as InputScalar).Value;
		}
		private void ToggleCrossChanged(object sender, PropertyChangedEventArgs e)
		{
			Emitter.IsCrossOn = (sender as InputCheckBox).IsChecked;
		}

		/// Start emitter stream
		private void StartStream(object sender, RoutedEventArgs e)
		{
			(sender as ToggleButton).Content = "Stop stream";
			Emitter.IsEnabled = true;
		}

		/// Stop emitter stream
		private void StopStream(object sender, RoutedEventArgs e)
		{
			(sender as ToggleButton).Content = "Start stream";
			Emitter.IsEnabled = false;
		}

		/// Testing
		private void UniformGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				MessageBox.Show(Emitter.ToString());
		}
	}
}
