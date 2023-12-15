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

    public async void CargarNotas()
    {
        LimpiarLista();

        var notas = await client.Child("Notas").OnceAsync<Notas>();

        // Ordena las notas por fecha en orden descendente
        var notasOrdenadas = notas.Select(nota => nota.Object)
                                 .OrderByDescending(n => n.Fecha)
                                 .ToList();

        foreach (var nota in notasOrdenadas)
        {
            ListaNotas.Add(nota);
        }

        // Asigna la lista ordenada a la propiedad ListaNotas
        BindingContext = this;
    }





    private async void nuevoButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CrearNotaPage));
    }

    private async void NotasCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Notas nota = e.CurrentSelection.FirstOrDefault() as Notas;
        var parametro = new Dictionary<string, object>
        {
            ["Detalle"] = nota,

        };
        await Shell.Current.GoToAsync(nameof(DetallesNotaPage), parametro);
    }

    private void recargarButton_Clicked(object sender, EventArgs e)
    {
        // Al hacer clic en el botón de recarga, vuelve a cargar las notas
        CargarNotas();
    }

    public void LimpiarLista()
    {
        ListaNotas.Clear();
    }
}