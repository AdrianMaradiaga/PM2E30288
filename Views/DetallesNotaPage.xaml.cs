using Firebase.Database;
using Firebase.Database.Query;
using PM2E30288.Models;
using System.Collections.ObjectModel;

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

    private async void EliminarNota_Clicked(object sender, EventArgs e)
    {
        // Verifica si el detalle no es nulo y tiene un Id_nota válido
        if (Detalle != null && !string.IsNullOrEmpty(Detalle.Id_nota))
        {
            // Elimina la nota
            await EliminarNota(Detalle.Id_nota);
            Lista listaPage = new Lista();
            listaPage.CargarNotas();
            (Application.Current.MainPage as Lista)?.LimpiarLista();


            // Después de eliminar, regresa a la página anterior
            await Shell.Current.GoToAsync("..");
        }
    }



    private async Task EliminarNota(string idNota)
    {
        var client = new FirebaseClient("https://pm2e30288-default-rtdb.firebaseio.com/");
        await client.Child("Notas").Child(idNota).DeleteAsync();
    }
}