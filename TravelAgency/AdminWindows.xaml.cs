using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace TravelAgency
{
    /// <summary>
    /// Логика взаимодействия для AdminWindows.xaml
    /// </summary>
    public partial class AdminWindows : Window
    {
        int? selectedUserId = 0;
        Collection<User> allUser = new Collection<User>();
        public AdminWindows()
        {
            InitializeComponent();
            showAllUser();
                
        }

        public void showAllUser()
        {
            allUser.Clear();
            users.Items.Clear();
            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, \"RoleId\", \"Login\", \"Password\", \"Firstname\", \"Lastname\", \"Surname\", \"Email\", \"isBlocked\" FROM public.\"Users\";", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allUser.Add(new User()
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
                        });
                    }
                }
            }
            foreach(User user in allUser) users.Items.Add($"{user.id} : {user.email}");
        }

        

        private void users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(users.SelectedIndex != -1)
            {
                var selectedUser = allUser[users.SelectedIndex];
                field_login.Text = selectedUser.login;
                field_password.Password = selectedUser.password;
                field_email.Text = selectedUser.email;
                field_firstname.Text = selectedUser.firstname;
                field_lastname.Text = selectedUser.lastname;
                field_surname.Text = selectedUser.surname;
                field_phone.Text = selectedUser.phone;
                field_role.Text = selectedUser.roleId.ToString();
                field_isBlocked.IsChecked = selectedUser.isBlocked;
                selectedUserId = selectedUser.id;
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO public.\"Users\"(\"RoleId\", \"Login\", \"Password\", \"Firstname\", \"Lastname\", \"Surname\", \"Email\", \"isBlocked\") VALUES ( @RoleId, @Login, @Password, @Firstname, @Lastname, @Surname, @Email, @IsBlocked);", conn))
                {
                    cmd.Parameters.AddWithValue("@RoleId", int.Parse(field_role.Text));
                    cmd.Parameters.AddWithValue("@Login", field_login.Text);
                    cmd.Parameters.AddWithValue("@Password", field_password.Password);
                    cmd.Parameters.AddWithValue("@Firstname", field_firstname.Text);
                    cmd.Parameters.AddWithValue("@Lastname", field_lastname.Text);
                    cmd.Parameters.AddWithValue("@Surname", field_surname.Text);
                    cmd.Parameters.AddWithValue("@Email", field_email.Text);
                    cmd.Parameters.AddWithValue("@IsBlocked", field_isBlocked.IsChecked);

                    cmd.ExecuteNonQuery();
                }
            }

            showAllUser();
        }

        private void Button_del_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUserId != 0)
            {
                using (var conn = new NpgsqlConnection(App._connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("DELETE FROM public.\"Users\" WHERE id = @Id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", selectedUserId);
                        cmd.ExecuteNonQuery();
                    }
                }

                showAllUser();
            }
            else
            {
                MessageBox.Show("Сначала выбирите пользователя!");
            }
        }

        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                { 
                    using (var cmd = new NpgsqlCommand("UPDATE public.\"Users\" SET \"RoleId\" = @RoleId, \"Login\" = @Login, \"Password\" = @Password, \"Firstname\" = @Firstname, \"Lastname\" = @Lastname, \"Surname\" = @Surname, \"Email\" = @Email, \"isBlocked\" = @IsBlocked WHERE \"id\" = @UserId;", conn))
                    {
                        cmd.Parameters.AddWithValue("@RoleId", int.Parse(field_role.Text));
                        cmd.Parameters.AddWithValue("@Login", field_login.Text);
                        cmd.Parameters.AddWithValue("@Password", field_password.Password);
                        cmd.Parameters.AddWithValue("@Firstname", field_firstname.Text);
                        cmd.Parameters.AddWithValue("@Lastname", field_lastname.Text);
                        cmd.Parameters.AddWithValue("@Surname", field_surname.Text);
                        cmd.Parameters.AddWithValue("@Email", field_email.Text);
                        cmd.Parameters.AddWithValue("@IsBlocked", field_isBlocked.IsChecked);
                        cmd.Parameters.AddWithValue("@UserId", selectedUserId); 

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            showAllUser();

        }
    }
}
