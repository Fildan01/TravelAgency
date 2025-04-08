using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TravelAgency
{
    public class TourCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string imageLink { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool isBlock { get; set; }
        public TourCard(int Id, string title, string imageLink, string description, double price, bool isBlock)
        {
            this.Id = Id;
            this.Title = title;
            this.imageLink = imageLink;
            this.Description = description;
            Price = price;
            this.isBlock = isBlock;
        }

        public Border getCard()
        {
            Image image = new Image
            {
                Source = new BitmapImage(new Uri($"{imageLink}", UriKind.Absolute)),
                Height = 203,
                Stretch = Stretch.UniformToFill
            };


            // Заголовок
            TextBlock title = new TextBlock
            {
                Text = Title,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6A0DAD")),
                Margin = new Thickness(0, 10, 0, 5)
            };

            TextBlock price = new TextBlock
            {
                Text = Price.ToString(),
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#121212")),
                Margin = new Thickness(0, 10, 0, 5)
            };

            // Описание
            TextBlock description = new TextBlock
            {
                Text = Description,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            Button button = new Button();
            if (!isBlock) {

                button = new Button
                {
                    Content = "Записаться на тур",
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6A0DAD")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 10, 0, 0),
                    Padding = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                };
                button.Click += Button_Click;
                
            }
            else
            {
                button = new Button
                {
                    Content = "Отказаться от тура",
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6A0DAD")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 10, 0, 0),
                    Padding = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                   
                };
                button.Click += Button_Click1;
            }
                // Кнопка


                // StackPanel
                StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(image);
            stackPanel.Children.Add(title);
            stackPanel.Children.Add(price);
            stackPanel.Children.Add(description);
            stackPanel.Children.Add(button);


            // Border
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6A0DAD")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Child = stackPanel
            };

            return border;
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM public.\"ToursUsers\" WHERE iduser = @User AND idtour = @Tour;", conn))
                {
                    cmd.Parameters.AddWithValue("@User", App.currentUser.id);
                    cmd.Parameters.AddWithValue("@Tour", Id);
                    cmd.ExecuteNonQuery();
                    App.tourInCurrentUser.Remove(Id);
                    App.userWindows.GetTours();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //INSERT INTO public."ToursUsers"(iduser, idtour) VALUES (@User, @Tour);

            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO public.\"ToursUsers\"(iduser, idtour) VALUES (@User, @Tour);", conn))
                {
                    cmd.Parameters.AddWithValue("@User", App.currentUser.id);
                    cmd.Parameters.AddWithValue("@Tour", Id);
                    cmd.ExecuteNonQuery();
                    App.tourInCurrentUser.Add(Id);
                    App.userWindows.GetTours();
                }
            }

        }
    }
}
