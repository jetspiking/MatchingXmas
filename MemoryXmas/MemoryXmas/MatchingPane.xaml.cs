using MatchingXmas.Matching;
using MatchingXmas.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchingXmas
{
    public partial class MatchingPane : UserControl
    {
        private Emoji.Wpf.TextBlock? _clickedSymbol;
        private Border? _clickedBorderSymbol;
        private Int32 _solvedCount = 0;

        public MatchingPane(Int32 level)
        {
            InitializeComponent();

            Images levelImage;
            Enum.TryParse<Images>($"Level{level - 1}", true, out levelImage);

            Sounds sounds;
            Enum.TryParse<Sounds>($"Music{(level % 4)}", false, out sounds);

            QuitButton.MouseEnter += (object sender, MouseEventArgs e) =>
            {
                QuitButton.Opacity = .7;
            };
            QuitButton.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                QuitButton.Opacity = .8;
            };
            QuitButton.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                QuitButton.Opacity = .6;
            };
            QuitButton.MouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                QuitButton.Opacity = .7;
                SoundUtilities.PlaySoundEffect(Sounds.Click, Extensions.Mp3);
                MainWindow.Instance.Content = new LevelSelector();
            };

            Level levelObject = new Level(levelImage, sounds, (level) * 2 + 9);
            LoadLevel(levelObject);
        }

        public void LoadLevel(Level level)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = ImageUtilities.GetBitmapImage(level.Image, Extensions.Png);
            imageBrush.Stretch = Stretch.UniformToFill;

            BackgroundImage.Source = imageBrush.ImageSource;

            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 3;
            BackgroundImage.Effect = blurEffect;

            SoundUtilities.PlaySoundLooping(level.Soundtrack, Extensions.Mp3);


            List<String> randomizedSymbolSet = new List<String>();
            for (Int32 i = 0; i < level.ToMatch - 1; i++)
            {
                randomizedSymbolSet.Add(Symbols.ChristmasSymbols[i]);
                randomizedSymbolSet.Add(Symbols.ChristmasSymbols[i]);
            }
            Random random = new();
            randomizedSymbolSet = randomizedSymbolSet.OrderBy(a => random.Next()).ToList();

            Int32 columns = IconsGrid.ColumnDefinitions.Count;

            List<Int32> Indices = new();
            for (Int32 i = 0; i < randomizedSymbolSet.Count; i++)
            {
                Border card = new Border();
                card.Margin = new Thickness(2, 2, 2, 2);
                card.BorderThickness = new Thickness(1, 1, 1, 1);
                card.BorderBrush = new SolidColorBrush(Colors.Black);
                card.HorizontalAlignment = HorizontalAlignment.Stretch;
                card.VerticalAlignment = VerticalAlignment.Stretch;
                SolidColorBrush backgroundBrush = new SolidColorBrush(Colors.DarkRed);
                backgroundBrush.Opacity = .8;
                card.Background = backgroundBrush;

                Emoji.Wpf.TextBlock textBlock = new Emoji.Wpf.TextBlock();
                textBlock.Text = randomizedSymbolSet[i];
                textBlock.FontSize = 40;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.Visibility = Visibility.Hidden;
                card.Child = textBlock;

                card.MouseEnter += (object sender, MouseEventArgs e) =>
                {
                    card.Background.Opacity = .7;
                };
                card.MouseLeave += (object sender, MouseEventArgs e) =>
                {
                    card.Background.Opacity = .8;
                };
                card.MouseDown += (object sender, MouseButtonEventArgs e) =>
                {
                    card.Background.Opacity = .5;
                };
                card.MouseUp += async (object sender, MouseButtonEventArgs e) =>
                {
                    card.Background.Opacity = .8;

                    if (_clickedSymbol == null || _clickedBorderSymbol == null)
                    {
                        _clickedSymbol = textBlock;
                        _clickedBorderSymbol = card;
                        _clickedSymbol.Visibility = Visibility.Visible;
                        SoundUtilities.PlaySoundEffect(Sounds.CardView, Extensions.Mp3);
                    }
                    else
                    {
                        if (_clickedSymbol.Text == textBlock.Text && textBlock != _clickedSymbol)
                        {
                            _clickedBorderSymbol.Visibility = Visibility.Hidden;
                            card.Visibility = Visibility.Hidden;
                            if (++this._solvedCount == randomizedSymbolSet.Count/2)
                            {
                                SoundUtilities.PlaySoundEffect(Sounds.LevelComplete, Extensions.Mp3);
                                MainWindow.Instance.Content = new LevelSelector();
                            } else
                            {
                                SoundUtilities.PlaySoundEffect(Sounds.Correct, Extensions.Mp3);
                            }
                        }
                        else
                        {
                            SoundUtilities.PlaySoundEffect(Sounds.Incorrect, Extensions.Mp3);
                            Emoji.Wpf.TextBlock localBlock = _clickedSymbol;
                            textBlock.Visibility = Visibility.Visible;

                            new Thread(() =>
                            {
                                Thread.Sleep(500);
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    localBlock.Visibility = Visibility.Hidden;
                                    textBlock.Visibility = Visibility.Hidden;
                                }));
                            }).Start();
                        }
                        _clickedSymbol = null;
                        _clickedBorderSymbol = null;
                        return;
                    }

                    _clickedSymbol = textBlock;
                    _clickedBorderSymbol = card;
                };

                Int32 row = i / columns; // Calculate row index
                Int32 column = i % columns; // Calculate column index

                if (!Indices.Contains(row))
                {
                    Indices.Add(row);
                    RowDefinition rowDefinition = new();
                    rowDefinition.Height = new GridLength(70);
                    IconsGrid.RowDefinitions.Add(rowDefinition);
                }

                Grid.SetRow(card, row);
                Grid.SetColumn(card, column);

                IconsGrid.Children.Add(card);
            }
        }
    }
}
