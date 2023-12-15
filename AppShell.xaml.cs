using PM2E30288.Views;

namespace PM2E30288
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CrearNotaPage), typeof(CrearNotaPage));
            Routing.RegisterRoute(nameof(DetallesNotaPage), typeof(DetallesNotaPage));
        }
    }
}