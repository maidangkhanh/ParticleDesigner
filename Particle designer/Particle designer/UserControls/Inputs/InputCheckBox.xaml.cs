using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for InputScalar.xaml
	/// </summary>
	public partial class InputCheckBox : UserControl, INotifyPropertyChanged
	{
		public string Text { get; set; }
		public bool IsChecked
		{
			get => _is_checked;
			set
			{
				if (value == _is_checked) return;

				_is_checked = value;
				OnPropertyChanged();
			}
		}

		private bool _is_checked;

		public event PropertyChangedEventHandler PropertyChanged;

		public InputCheckBox()
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
