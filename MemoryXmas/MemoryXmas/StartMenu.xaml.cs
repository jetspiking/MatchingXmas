using MatchingXmas.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl
    {
        public StartMenu()
        {
            InitializeComponent();

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = ImageUtilities.GetBitmapImage(Images.Startup, Extensions.Png);
            imageBrush.Stretch = Stretch.UniformToFill;
            this.Background = imageBrush;

            StartButton.MouseEnter += (object sender, MouseEventArgs e) =>
            {
                StartButton.Opacity = .7;
            };
            StartButton.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                StartButton.Opacity = .8;
            };
            StartButton.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                StartButton.Opacity = .6;
            };
            StartButton.MouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                StartButton.Opacity = .7;
                SoundUtilities.PlaySoundEffect(Sounds.Click, Extensions.Mp3);
                MainWindow.Instance.Content = new LevelSelector();
            };

            AudioButton.MouseEnter += (object sender, MouseEventArgs e) =>
            {
                AudioButton.Opacity = .7;
            };
            AudioButton.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                AudioButton.Opacity = .8;
            };
            AudioButton.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                AudioButton.Opacity = .6;
            };
            AudioButton.MouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                AudioButton.Opacity = .7;
                MainWindow.Instance.Settings.IsMusicEnabled = !MainWindow.Instance.Settings.IsMusicEnabled;
                DoAudioCheck();
                SoundUtilities.PlaySoundEffect(Sounds.Click, Extensions.Mp3);
            };
            SoundUtilities.PlaySoundLooping(Sounds.Music4, Extensions.Mp3);
            SoundUtilities.GetMusicInstance().Volume = .3;
        }

        private void DoAudioCheck()
        {
            if (MainWindow.Instance.Settings.IsMusicEnabled)
            {
                SoundUtilities.GetMusicInstance().Volume = .3;
                AudioButton.Content = "🔊♫";
            }
            else
            {
                SoundUtilities.GetMusicInstance().Volume = 0;
                AudioButton.Content = "🔇♫";
            }
        }
    }
}
