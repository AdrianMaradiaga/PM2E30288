using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using PM2E30288.Models;
using System.Collections.ObjectModel;

namespace PM2E30288.Views;

public partial class Lista : ContentPage
{
	FirebaseClient client = new FirebaseClient("https://pm2e30288-default-rtdb.firebaseio.com/");
    public ObservableCollection<Notas> ListaNotas { get; set; } = new ObservableCollection<Notas>();

    public Lista()
    {
        InitializeComponent();
        CargarNotas();

        BindingContext = this;
    }

    public void CargarNotas()
    {
        ListaNotas.Clear();

        client.Child("Notas")
            .AsObservable<Notas>()
            .Subscribe(Notas =>
            {
               if(Notas != null && Notas.Object != null)
                {
                    ListaNotas.Add(Notas.Object);
                } 
            });
    }

    private async void nuevoButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CrearNotaPage));
    }
}