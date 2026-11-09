using System.Text;
using System.Globalization;
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

namespace Zadanie_rentry
{
    public partial class MainWindow : Window
    {
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
    }
}
