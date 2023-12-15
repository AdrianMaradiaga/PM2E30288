using PM2E30288.Models;

namespace PM2E30288.Views;
[QueryProperty(nameof(Detalle),"Detalle")]
public partial class DetallesNotaPage : ContentPage
{
	Notas detalle;
	public Notas Detalle { get => detalle; set { detalle = value; OnPropertyChanged(); }}
	public DetallesNotaPage()
	{
		InitializeComponent();
		BindingContext = this;
	}
}