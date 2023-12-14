using Microsoft.Win32;
using SharpGL;
using SharpGL.WPF;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private EmitterStream emitter;
		
		/// Constructor
		public MainWindow()
		{
			InitializeComponent();

			emitter = emitter_control.Emitter;
		}

		/// OpenGL
		private void gl_control_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
		{
			// Get the OpenGL object.
			OpenGL gl = (sender as OpenGLControl).OpenGL;

			// Set the clear color.
			gl.ClearColor(0, 0, 0, 0);

			// Set the projection matrix.
			gl.MatrixMode(OpenGL.GL_PROJECTION);

			// Load the identity.
			gl.LoadIdentity();

		}
		private void gl_control_Resized(object sender, OpenGLRoutedEventArgs args)
		{
			// Get the OpenGL object.
			OpenGL gl = (sender as OpenGLControl).OpenGL;

			// Set the projection matrix.
			gl.MatrixMode(OpenGL.GL_PROJECTION);

			// Load the identity.
			gl.LoadIdentity();

			// Create a perspective transformation.
			gl.Viewport(0, 0, (int)gl_control.ActualWidth, (int)gl_control.ActualHeight);
			gl.Ortho2D(0, gl_control.ActualWidth, gl_control.ActualHeight, 0);

			emitter.Position = new Vector(gl_control.ActualWidth / 2d, gl_control.ActualHeight / 2d);
		}
		private void gl_control_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
		{
			// Get the OpenGL object.
			OpenGL gl = (sender as OpenGLControl).OpenGL;

			// Clear the color and depth buffer.
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			// Draw a red cross at the center
			if (emitter != null && emitter.IsCrossOn)
			{
				gl.Color(1f, 0f, 0f);
				double r = 10;
				double t = 1;
				gl.Rect(emitter.Position.X - r, emitter.Position.Y - t, emitter.Position.X + r, emitter.Position.Y + t);
				gl.Rect(emitter.Position.X - t, emitter.Position.Y - r, emitter.Position.X + t, emitter.Position.Y + r);
			}

			// Emitter start here
			emitter?.Display(gl);

			// Draw immediately instead of waiting a time to draw
			gl.Flush();
		}

		/// Loading & Saving
		private void AllowSavingAndLoading(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (!emitter_control?.Emitter?.IsEnabled) ?? false;
		}

		/// Display a save particle emitter dialog
		private void SaveEmitterStream(object sender, ExecutedRoutedEventArgs e)
		{
			Directory.CreateDirectory("Example");
			
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Title = "Save particle emitter stream to a file";
			dialog.Filter = "Particle emitter stream (*.pet)|*.pet";
			//dialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\Examples";

			if (dialog.ShowDialog() == true)
			{
				string filename = dialog.FileName;
				emitter.Save(filename);
			}
		}

		/// Display a load particle emitter dialog
		private void LoadEmitterStream(object sender, ExecutedRoutedEventArgs e)
		{
			Directory.CreateDirectory("Example");
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Load particle emitter stream from a file";
			dialog.Filter = "Particle emitter stream (*.pet)|*.pet";
			//dialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\Examples";
			
			if (dialog.ShowDialog() == true)
			{
				string filename = dialog.FileName;
				EmitterStream emitter_stream = EmitterStream.Load(filename);
				if (emitter_stream == null)
				{
					MessageBox.Show("Invalid format file");
				}
				else
				{
					emitter_stream.Position = emitter_control.Emitter.Position;
					emitter_control.Emitter = emitter_stream;
					emitter = emitter_stream;
				}
			}
		}

		/// Toggle emitter_control
		private void ToggleParticleEmitterStreamTab(object sender, RoutedEventArgs e)
		{
			emitter_control.Visibility = emitter_control.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
