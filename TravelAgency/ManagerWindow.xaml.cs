using ClosedXML.Excel;
using Npgsql;
using Npgsql.BackendMessages;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace TravelAgency
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        int id = -1;
        public Collection<TourCard> tourCards = new Collection<TourCard>();
        public ManagerWindow()
        {
            InitializeComponent(); GetTours();
        }

        public void GetTours()
        {
            listTour.Items.Clear();
            tourCards.Clear();
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

            foreach (TourCard tour in tourCards) listTour.Items.Add($"{tour.Id.ToString()} : {tour.Title}");
        }

        private void listTour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            id = listTour.SelectedIndex;
            if (id != -1)
            {
                TitleBox.Text = tourCards[id].Title;
                DescriptionBox.Text = tourCards[id].Description;
                ImageUrlBox.Text = tourCards[id].imageLink;
                PriceBox.Text = tourCards[id].Price.ToString();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (id != 1)
            {
                using (var conn = new NpgsqlConnection(App._connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("DELETE FROM public.\"Tours\" WHERE id = @Id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", tourCards[id].Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                GetTours();
                id = -1;
            }
            else
            {
                MessageBox.Show("Сначала выбирите пользователя!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(App._connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO public.\"Tours\"(title, description, image, price) VALUES (@Title, @Description, @Image, @Price);", conn))
                {
                    cmd.Parameters.AddWithValue("@Title", TitleBox.Text);
                    cmd.Parameters.AddWithValue("@Description", DescriptionBox.Text);
                    cmd.Parameters.AddWithValue("@Image", ImageUrlBox.Text);
                    cmd.Parameters.AddWithValue("@Price", double.Parse(PriceBox.Text));
                    cmd.ExecuteNonQuery();
                    GetTours();
                    id = -1;
                }
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string query = "SELECT id, title, description, image, price FROM public.\"Tours\"";
            DataTable dt = new DataTable();

            // Подключаемся к базе данных и выполняем запрос
            using (var connection = new NpgsqlConnection(App._connectionString))
            {
                connection.Open(); // Открываем соединение

                using (var adapter = new NpgsqlDataAdapter(query, connection))
                {
                    adapter.Fill(dt); // Заполняем DataTable данными
                }
            }

            // Выводим результаты для проверки (например, для отладки)
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine($"{row["id"]} - {row["title"]} - {row["price"]}");
            }

            // Создаем новый Excel файл
            using (var workbook = new XLWorkbook())
            {
                // Создаем рабочий лист
                var worksheet = workbook.Worksheets.Add("Tours Report");

                // Заголовки столбцов
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Image";
                worksheet.Cell(1, 5).Value = "Price";

                // Форматируем заголовки
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Заполняем данными таблицу
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = Convert.ToInt32(dt.Rows[i]["id"]);  // Преобразуем к int
                    worksheet.Cell(i + 2, 2).Value = Convert.ToString(dt.Rows[i]["title"]);  // Преобразуем к string
                    worksheet.Cell(i + 2, 3).Value = Convert.ToString(dt.Rows[i]["description"]);  // Преобразуем к string
                    worksheet.Cell(i + 2, 4).Value = Convert.ToString(dt.Rows[i]["image"]);  // Преобразуем к string
                    worksheet.Cell(i + 2, 5).Value = Convert.ToDecimal(dt.Rows[i]["price"]);  // Преобразуем к decimal
                }

                // Форматирование данных
                var dataRange = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(dt.Rows.Count + 1, 5));
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Автоматическое изменение ширины столбцов
                worksheet.Columns().AdjustToContents();

                // Сохраняем Excel файл
                string filePath = @"C:\1\ToursReport.xlsx";
                workbook.SaveAs(filePath);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
       
            // Запрос для получения данных о пользователях и их сумме потраченных денег
            string query = @"
        SELECT 
            u.""Firstname"" || ' ' || u.""Lastname"" AS full_name,
            SUM(t.price) AS total_spent
        FROM 
            public.""ToursUsers"" tu
        JOIN 
            public.""Tours"" t ON tu.idtour = t.id
        JOIN 
            public.""Users"" u ON tu.iduser = u.id
        GROUP BY 
            u.id, u.""Firstname"", u.""Lastname""
        ORDER BY 
            total_spent DESC;"; // Сортировка по сумме потраченных денег от большего к меньшему

            DataTable dt = new DataTable();

            // Подключаемся к базе данных и выполняем запрос
            using (var connection = new NpgsqlConnection(App._connectionString))
            {
                connection.Open(); // Открываем соединение

                using (var adapter = new NpgsqlDataAdapter(query, connection))
                {
                    adapter.Fill(dt); // Заполняем DataTable данными
                }
            }

            // Выводим результаты для проверки (например, для отладки)
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine($"{row["full_name"]} - {row["total_spent"]}");
            }

            // Создаем новый Excel файл
            using (var workbook = new XLWorkbook())
            {
                // Создаем рабочий лист
                var worksheet = workbook.Worksheets.Add("Users Spending Report");

                // Заголовки столбцов
                worksheet.Cell(1, 1).Value = "Full Name";
                worksheet.Cell(1, 2).Value = "Total Spent";

                // Форматируем заголовки
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Заполняем данными таблицу
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = Convert.ToString(dt.Rows[i]["full_name"]);  // Имя пользователя
                    worksheet.Cell(i + 2, 2).Value = Convert.ToDecimal(dt.Rows[i]["total_spent"]);  // Сумма потраченных денег
                }

                // Форматирование данных
                var dataRange = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(dt.Rows.Count + 1, 2));
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Автоматическое изменение ширины столбцов
                worksheet.Columns().AdjustToContents();

                // Сохраняем Excel файл
                string filePath = @"C:\1\UsersSpendingReport.xlsx";
                workbook.SaveAs(filePath);
            }

            Console.WriteLine("Excel файл успешно сохранен.");
        

    }
}
}
