using MatchingXmas.Misc;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchingXmas
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        public Settings Settings { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Settings = new Settings();
            MainWindow.Instance = this;

            this.Content = new StartMenu();
        }
    }
}