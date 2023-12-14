using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for InputScalar.xaml
	/// </summary>
	public partial class InputScalar : UserControl, INotifyPropertyChanged
	{
		public string Text { get; set; }
		public string TextValue
		{
			get => _text_value;
			set
			{
				if (value == _text_value) return;

				_text_value = value;
				if (double.TryParse(value, out double result))
				{
					Value = result;
					OnPropertyChanged();
				}
			}

		}
		public double Value { get; private set; }

		private string _text_value;

		public event PropertyChangedEventHandler PropertyChanged;

		public InputScalar()
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
