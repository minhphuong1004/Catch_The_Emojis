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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CatchTheEmoji
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer minigameTimer = new DispatcherTimer(); //DispatcherTimer goi 1 ham lap theo chu ky co dinh
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
            minigameTimer.Interval = TimeSpan.FromMilliseconds(40); //40ms cchay 1 lan 
            minigameTimer.Tick += MinigameLoop;
            minigameTimer.Start();

            this.Focusable = true;
            this.Focus();
        }

        private void AddEmoji()
        {
            string[] emojis = { "🌷", "💗", "🍀", "🎧", "🎀", "🌸", "💎", "🍓", "🍒", "⏾" , "🦋" , "🍰" };
            string emojiChar = emojis[random.Next(emojis.Length)];

            TextBlock emoji = new TextBlock
            {
                Text = emojiChar,
                FontSize = 32,
                Foreground = new SolidColorBrush(Color.FromRgb(164, 31, 58))
            };

            double x = random.Next(0, (int)(minigameCanvas.ActualWidth - 40));
            Canvas.SetLeft(emoji, x);
            Canvas.SetTop(emoji, 0);
            minigameCanvas.Children.Add(emoji);
        }

        private void MinigameLoop(object sender, EventArgs e)
        {
            //Emoji roi
            foreach (var emoji in minigameCanvas.Children.OfType<TextBlock>().ToList())
            {
                double y = Canvas.GetTop(emoji) + 4; //toc do roi cua emoji
                Canvas.SetTop(emoji, y);

                if (y > minigameCanvas.ActualHeight)
                {
                    minigameCanvas.Children.Remove(emoji);
                }
            }

            //Sinh emoji moi
            if (random.Next(14) == 0)
            {
                AddEmoji();
            }

            //Fade trail chuot
            foreach (var dot in minigameCanvas.Children.OfType<Ellipse>().ToList())
            {
                dot.Opacity -= 0.03;
                if (dot.Opacity <= 0)
                    minigameCanvas.Children.Remove(dot);
            }
        }

        //Hieu ung mouse trail
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(minigameCanvas); //vi tri cua chuot tren canvas

            Ellipse dot = new Ellipse  //dom tron dkinh 6px
            {
                Width = 6,
                Height = 6,
                Fill = new SolidColorBrush(Color.FromRgb(  //random mau cho dep
                    (byte)random.Next(220, 255),
                    (byte)random.Next(60, 120),
                    (byte)random.Next(100, 180))),
                Opacity = 0.7  //cang ve 1 cang dam
            };

            //track theo chuot vi tri chuot tren canvas
            Canvas.SetLeft(dot, pos.X);
            Canvas.SetTop(dot, pos.Y);
            minigameCanvas.Children.Add(dot);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(minigameCanvas);  //luu toa do click chuot

            foreach (var emoji in minigameCanvas.Children.OfType<TextBlock>().ToList()) //lay vi tri emoji
            {
                double x = Canvas.GetLeft(emoji);
                double y = Canvas.GetTop(emoji);

                //kiem tra xem click trung emoji khong
                if (Math.Abs(pos.X - x) < 30 && Math.Abs(pos.Y - y) < 30)  //click trong pham vi 30px cua emoji se coi nhu bat trung
                {
                    collectBox.Text += emoji.Text;
                    minigameCanvas.Children.Remove(emoji);
                    Title = $"Catch The Emoji ₍ᐢ. .ᐢ₎ | Score: {collectBox.Text.Length}";
                    break;
                }
            }
        }

        
        private void ClearBtn_Click (object sender, RoutedEventArgs e)
        {
            collectBox.Clear();
            Title = $"Catch The Emoji ₍ᐢ. .ᐢ₎ | Score: 0";   //Tra Score ve 0 khi click nut clear
        }


        //Phim tat
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) 
                Close();
            if (e.Key == Key.Space)
                collectBox.Clear();
        }
    }
}
