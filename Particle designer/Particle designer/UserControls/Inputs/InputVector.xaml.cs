using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for InputVector.xaml
	/// </summary>
	public partial class InputVector : UserControl, INotifyPropertyChanged
	{
		public string Text { get; set; }
		public Vector Vector2
		{
			get => vector2;
			set
			{
				if (value == vector2) return;

				vector2 = value;
				OnPropertyChanged();
			}
		}
		public string X
		{
			get => x;
			set
			{
				if (double.TryParse(value, out double result))
					Vector2 = new Vector(result, Vector2.Y);

				x = value;
			}
		}
		public string Y
		{
			get => y;
			set
			{
				if (double.TryParse(value, out double result))
					Vector2 = new Vector(Vector2.X, result);

				y = value;
			}
		}

		private string x, y;
		private Vector vector2;

		public event PropertyChangedEventHandler PropertyChanged;

		public InputVector()
		{
			InitializeComponent();
			DataContext = this;
		}

		protected void OnPropertyChanged([CallerMemberName] string property_name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
		}
	}
}
