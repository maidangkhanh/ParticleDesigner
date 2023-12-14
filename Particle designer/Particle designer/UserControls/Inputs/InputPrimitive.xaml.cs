using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for InputScalar.xaml
	/// </summary>
	public partial class InputPrimitive : UserControl, INotifyPropertyChanged
	{
		public string Text { get; set; }
		public List<string> Items { get; private set; }
		public string SelectedItem { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public InputPrimitive()
		{
			InitializeComponent();
			DataContext = this;
			Items = new List<string>()
			{
				"Square",
				"Circle",
				"Line",
				"Pentagon",
				"Hexagon"
			};
		}

		protected void OnPropertyChanged([CallerMemberName] string property_name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
		}

		private void cb_primitive_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItem = (sender as ComboBox).SelectedValue as string;
			OnPropertyChanged();
		}
	}
}
