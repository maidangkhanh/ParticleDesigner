using System.Windows.Controls;

namespace Particle_designer
{
	/// <summary>
	/// Interaction logic for VectorHeader.xaml
	/// </summary>
	public partial class VectorHeader : UserControl
	{
		public string First { get; set; }
		public string Last { get; set; }

		public VectorHeader()
		{
			InitializeComponent();
			DataContext = this;
		}
	}
}
