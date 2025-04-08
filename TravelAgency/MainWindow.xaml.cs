using Npgsql;
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

namespace TravelAgency;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public int? loginAttempts = 0;
    public MainWindow()
    {
        InitializeComponent();
        
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        
         User user = new User();
        string login = fieldLogin.Text;
        string password = fieldPassword.Password;

        using(var conn = new NpgsqlConnection(App._connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT id, \"RoleId\", \"Login\", \"Password\", \"Firstname\", \"Lastname\", \"Surname\", \"Email\", \"isBlocked\" FROM public.\"Users\" WHERE \"Login\" = @Login;", conn))
            {
                cmd.Parameters.AddWithValue("@Login", login);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User()
                    {
                        id = reader.GetInt32(0),
                        roleId = reader.GetInt32(1),
                        login = reader.IsDBNull(2) ? null : reader.GetString(2),
                        password = reader.IsDBNull(3) ? null : reader.GetString(3),
                        firstname = reader.IsDBNull(4) ? null : reader.GetString(4),
                        lastname = reader.IsDBNull(5) ? null : reader.GetString(5),
                        surname = reader.IsDBNull(6) ? null : reader.GetString(6),
                        email = reader.IsDBNull(7) ? null : reader.GetString(7),
                        isBlocked = reader.GetBoolean(8),
                    };
                }
            }
                if(user.password == password && !user.isBlocked)
                {
                    App.currentUser = user;
                    if(user.roleId == 1)
                {
                    //Пользователь
                    App.userWindows = new UserWindows();
                    App.userWindows.Show();
                    this.Close();
                } else if (user.roleId == 2)
                {
                    //Модер
                    ManagerWindow manager = new ManagerWindow();
                    manager.Show();
                }
                else if (user.roleId == 3)
                {
                    AdminWindows adminWindows = new AdminWindows();
                    adminWindows.Show();
                    this.Close();
                }
                }
                else if (user.isBlocked)
                {
                    MessageBox.Show("Ваша учётная запись заблокированна!");
                }
                else
                {
                    loginAttempts += 1;
                    if (loginAttempts >= 3 && user.roleId != 3)
                    {
                        MessageBox.Show("Ваша учётная запись заблокированна!");
                        using (var connect = new NpgsqlConnection(App._connectionString))
                        {
                            connect.Open();
                        using (var command = new NpgsqlCommand("UPDATE public.\"Users\" SET \"isBlocked\" = true WHERE id = @Id;", connect))
                        {
                            command.Parameters.AddWithValue("@Id", user.id);
                            command.ExecuteNonQuery();
                        }
                        }
              
                    }
                    else
                    {
                        MessageBox.Show("А с паролем не попал!");
                    }
                }
            }
        }
    
}