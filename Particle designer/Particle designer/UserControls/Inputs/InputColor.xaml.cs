using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for InputColor.xaml
	/// </summary>
	public partial class InputColor : UserControl, INotifyPropertyChanged
	{
		public string Text { get; set; }
		public Color? SelectedColor
		{
			get => _selected_color;
			set
			{
				if (!value.HasValue) return;
				if (value == _selected_color) return;

				_selected_color = value.Value;
				OnPropertyChanged();
			}
		}

		private Color _selected_color;

		public event PropertyChangedEventHandler PropertyChanged;

		public InputColor()
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
