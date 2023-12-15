using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Plugin.Maui.Audio;
using PM2E30288.Models;
using System.Diagnostics.Tracing;

namespace PM2E30288.Views;


public partial class CrearNotaPage : ContentPage
{
	FirebaseClient client = new FirebaseClient("https://pm2e30288-default-rtdb.firebaseio.com/");
	FirebaseStorage storage = new FirebaseStorage("pm2e30288.appspot.com");
	private string urlImg { get; set; }
	private string urlAudio { get; set;}
    readonly IAudioManager _audioManager;
	readonly IAudioRecorder _audioRecorder;

    private bool isRecording = false;


    public CrearNotaPage(IAudioManager audioManager)
	{
		InitializeComponent();
		BindingContext = this;
        _audioManager = audioManager;
        _audioRecorder = audioManager.CreateRecorder();
    }



    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        // Validar que todos los campos estén llenos
        if (string.IsNullOrWhiteSpace(descripcionEntry.Text) ||
            fechaPicker.Date == DateTime.MinValue ||
            string.IsNullOrWhiteSpace(urlImg) ||
            string.IsNullOrWhiteSpace(urlAudio))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos antes de guardar.", "OK");
            return;
        }

        // Crear y guardar la nota
        var nota = new Notas
        {
            Descripcion = descripcionEntry.Text,
            Fecha = fechaPicker.Date,
            Photo_Record = urlImg,
            Audio_Record = urlAudio
        };

        // Realizar la operación PostAsync
        var firebaseObject = await client.Child("Notas").PostAsync(nota);

        // Obtener el ID asignado por Firebase desde la URL del objeto creado
        string nuevoId = firebaseObject.Key;

        // Asignar el ID a la propiedad Id_nota de la nota
        nota.Id_nota = nuevoId;

        // Actualizar la nota con el ID asignado
        await client.Child("Notas").Child(nuevoId).PutAsync(nota);
        Lista listaPage = new Lista();
        listaPage.CargarNotas();

        // Ir a la página anterior
        await Shell.Current.GoToAsync("..");
    }



    private async void seleccionarImgButton_Clicked(object sender, EventArgs e)
    {
		var img = await MediaPicker.PickPhotoAsync();
		if (img != null)
		{
			var stream = await img.OpenReadAsync();
			urlImg = await new FirebaseStorage("pm2e30288.appspot.com")
								.Child("Images")
								.Child(DateTime.Now.ToString("ddMMyyyyhhmmss") + img.FileName)
								.PutAsync(stream);
			imgElement.Source = urlImg;			
        }
    }

	private async void grabarAudioButton_Clicked(object sender, EventArgs e)
	{
		if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
		{
			return;
		}

        isRecording = !isRecording;
        if (!_audioRecorder.IsRecording)
		{
            grabarAudioButton.BackgroundColor = Color.FromRgba("#12a351");

            await _audioRecorder.StartAsync();
		}
		else
		{
            grabarAudioButton.BackgroundColor = Color.FromRgba("#444444");

            var audioRecorded = await _audioRecorder.StopAsync();
			var player = AudioManager.Current.CreatePlayer(audioRecorded.GetAudioStream());
			player.Play();

            var audioStream = audioRecorded.GetAudioStream();
            urlAudio = await storage
                .Child("Audio")
                .Child(DateTime.Now.ToString("ddMMyyyyhhmmss") + ".wav")
                .PutAsync(audioStream);
        }
	}
}