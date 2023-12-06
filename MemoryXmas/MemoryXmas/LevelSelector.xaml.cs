using MatchingXmas.Misc;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Diagnostics;
using System.Windows.Input;

namespace MatchingXmas
{
    /// <summary>
    /// Interaction logic for LevelSelector.xaml
    /// </summary>
    public partial class LevelSelector : UserControl
    {
        public LevelSelector()
        {
            InitializeComponent();

            // Create an ImageBrush for the background with a blur effect
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = ImageUtilities.GetBitmapImage(Images.LevelsBackground, Extensions.Png);
            imageBrush.Stretch = Stretch.UniformToFill;

            // Apply the ImageBrush to the BackgroundImage
            BackgroundImage.Source = imageBrush.ImageSource;

            // Apply a BlurEffect to the BackgroundImage
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 10;
            BackgroundImage.Effect = blurEffect;

            // Set the LevelsImage Source as before
            LevelsImage.Source = ImageUtilities.GetBitmapImage(Images.Levels, Extensions.Png);

            LevelsImage.MouseUp += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            {
                Point mousePosition = e.GetPosition((IInputElement)sender);
                int levelClicked = GetLevelClicked(mousePosition.X, mousePosition.Y);
                if (levelClicked > -1)
                {
                    SoundUtilities.PlaySoundEffect(Sounds.Click, Extensions.Mp3);
                    this.Content = new MatchingPane(levelClicked);
                }
            };

            MenuButton.MouseEnter += (object sender, MouseEventArgs e) =>
            {
                MenuButton.Opacity = .7;
            };
            MenuButton.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                MenuButton.Opacity = .8;
            };
            MenuButton.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                MenuButton.Opacity = .6;
            };
            MenuButton.MouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                MenuButton.Opacity = .7;
                SoundUtilities.PlaySoundEffect(Sounds.Click, Extensions.Mp3);
                MainWindow.Instance.Content = new StartMenu();
            };
        }

        public int GetLevelClicked(double mouseX, double mouseY)
        {
            var balls = new List<(int x, int y, int r, int level)>
            {
                (207, 123, 15, 1),
                (122, 251, 16, 2),
                (93, 520, 16, 3),
                (319, 296, 16, 4),
                (76, 365, 16, 5),
                (245, 140, 16, 6),
                (276, 259, 16, 7),
                (146, 376, 20, 8),
                (205, 470, 16, 9),
                (259, 501, 24, 10),
                (237, 302, 24, 11),
                (203, 177, 24, 12),
                (177, 425, 24, 13),
                (264, 353, 24, 14),
                (233, 228, 24, 15),
                (100, 399, 24, 16),
                (198, 265, 24, 17),
                (307, 400, 24, 18),
                (152, 500, 24, 19),
                (241, 432, 24, 20),
                (204, 65, 24, 21)
            };

            foreach (var ball in balls)
            {
                double dx = mouseX - ball.x;
                double dy = mouseY - ball.y;
                if (dx * dx + dy * dy <= ball.r * ball.r)
                {
                    return ball.level;
                }
            }

            return -1;
        }
    }
}
