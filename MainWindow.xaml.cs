using System.Text;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.Json;

namespace Zadanie_rentry
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private int licznikPobran = 0;

        public MainWindow()
        {
            InitializeComponent();

            DateTime teraz = DateTime.Now;
            GodzinaTextBlock.Text = teraz.ToString("HH:mm:ss");
            DataTextBlock.Text = teraz.ToString("dd.MM.yyyy");
            DataPelnaTextBlock.Text = teraz.ToString("dddd, d MMMM yyyy", new CultureInfo("pl-PL"));

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += (a , b) =>
            {
                DateTime teraz = DateTime.Now;
                GodzinaTextBlock.Text = teraz.ToString("HH:mm:ss");
                DataTextBlock.Text = teraz.ToString("dd.MM.yyyy");
                DataPelnaTextBlock.Text = teraz.ToString("dddd, d MMMM yyyy", new CultureInfo("pl-PL"));
            };

            timer.Start();
        }

        private async void WarszawaButton_Click(object sender, RoutedEventArgs e)
        {
            await PobierzPogodeAsync("https://api.open-meteo.com/v1/forecast?latitude=52.2298&longitude=21.0118&current=temperature_2m,wind_speed_10m,weather_code");
        }

        private async void GdanskButton_Click(object sender, RoutedEventArgs e)
        {
            await PobierzPogodeAsync("https://api.open-meteo.com/v1/forecast?latitude=54.3523&longitude=18.6491&current=temperature_2m,wind_speed_10m,weather_code");
        }

        private async Task PobierzPogodeAsync(string adresZapytania)
        {
            StatusTextBlock.Text = "Pobieram...";

            string json = await httpClient.GetStringAsync(adresZapytania);
            using JsonDocument dokument = JsonDocument.Parse(json);

            JsonElement root = dokument.RootElement;
            JsonElement current = root.GetProperty("current");

            string nazwaMiasta = adresZapytania.Contains("latitude=52.2298") ? "Warszawa" : "Gdańsk";
            LokalizacjaTextBlock.Text = $"Lokalizacja: {nazwaMiasta}";
            TemperaturaTextBlock.Text = $"Temperatura: {current.GetProperty("temperature_2m").GetDouble():F1}°C";
            PredkoscWiatruTextBlock.Text = $"Prędkość wiatru: {current.GetProperty("wind_speed_10m").GetDouble():F1} km/h";
            licznikPobran++;

            //wyswietlanie + pogrubienie
            StatusTextBlock.Inlines.Clear();
            StatusTextBlock.Inlines.Add(new Bold(new Run($"Pobrano {licznikPobran} razy")));
            StatusTextBlock.Inlines.Add(new Run($" o {DateTime.Now:HH:mm:ss}"));

        }
    }
}
