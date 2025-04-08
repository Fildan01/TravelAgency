using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TravelAgency
{
    /// <summary>
    /// Логика взаимодействия для UserWindows.xaml
    /// </summary>
    public partial class UserWindows : Window
    {
        public Collection<TourCard> tourCards = new Collection<TourCard>(); 
        public UserWindows()
        {
            InitializeComponent();
            GetTours();
        }

        public void GetTours()
        {
            tourCards.Clear();
            listTorus.Children.Clear();
            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT t.id, t.title, t.image, t.description, t.price, (tu.iduser IS NOT NULL) AS is_registered FROM public.\"Tours\" t LEFT JOIN public.\"ToursUsers\" tu ON t.id = tu.idtour AND tu.iduser = @User;", conn))
                {
                    cmd.Parameters.AddWithValue("@User", App.currentUser.id);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tourCards.Add(new TourCard(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDouble(4), reader.GetBoolean(5)));
                    }
                }
            }

            

            foreach (TourCard tour in tourCards) listTorus.Children.Add(tour.getCard());
        }
    }
}
