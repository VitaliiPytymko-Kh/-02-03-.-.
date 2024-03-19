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
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;
using System.Windows.Forms;

namespace Дз_02_03_Питимко_В.Л_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = null;
        string cs = "";
        DataTable table = null;
        SqlDataReader reader = null;
        SqlCommand cmd = null;
        string text = null;

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = @"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = Sklad; Integrated Security =SSPI; ";
            conn.ConnectionString= cs;
            conn.Open();
        }
       

        private void ExecuteQueryAndRefreshDataGrid(string query)
        {
            try
            {
                if (reader != null) reader.Close();

                TextBox1.Text = query;

                cmd = new SqlCommand(query, conn);

                table = new DataTable();

                reader = cmd.ExecuteReader();
                int line = 0;

                do
                {
                    while (reader.Read())
                    {
                        if (line == 0)
                        {
                            
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                       DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());

                DataView Source = new DataView(table);
                DataGrid1.ItemsSource = Source;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void ShowProductWithMaxQuantity()
        {
            try
            {
                if (reader != null) reader.Close();

                string query = @"
            SELECT TOP 1 Товар.Название, SUM(Поставки.Количество) AS Общее_количество
            FROM Товар
            JOIN Поставки ON Товар.ТоварID = Поставки.ТоварID
            GROUP BY Товар.ТоварID, Товар.Название
            ORDER BY Общее_количество DESC;";

                ExecuteQueryAndRefreshDataGrid(query);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ShowProductWithMinQuantity()
        {
            try
            {
                if (reader != null) reader.Close();

                string query = @"
            SELECT TOP 1 Товар.Название, SUM(Поставки.Количество) AS Общее_количество
            FROM Товар
            JOIN Поставки ON Товар.ТоварID = Поставки.ТоварID
            GROUP BY Товар.ТоварID, Товар.Название
            ORDER BY Общее_количество ASC;";

                ExecuteQueryAndRefreshDataGrid(query);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ShowProductWithMinCost()
        {
            try
            {
                if (reader != null) reader.Close();

                string query = @"
            SELECT TOP 1 Товар.Название, MIN(Поставки.Себестоимость) AS Минимальная_себестоимость
            FROM Товар
            JOIN Поставки ON Товар.ТоварID = Поставки.ТоварID
            GROUP BY Товар.ТоварID, Товар.Название
            ORDER BY Минимальная_себестоимость ASC;";

                ExecuteQueryAndRefreshDataGrid(query);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ShowProductWithMaxCost()
        {
            try
            {
                if (reader != null) reader.Close();

                string query = @"
            SELECT TOP 1 Товар.Название, MAX(Поставки.Себестоимость) AS Максимальная_себестоимость
            FROM Товар
            JOIN Поставки ON Товар.ТоварID = Поставки.ТоварID
            GROUP BY Товар.ТоварID, Товар.Название
            ORDER BY Максимальная_себестоимость DESC;";

                ExecuteQueryAndRefreshDataGrid(query);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Очищаем DataGrid
                DataGrid1.ItemsSource = null;

                // Получаем текст запроса из TextBox1
                string query = TextBox1.Text;

                // Проверяем, что запрос не пустой
                if (string.IsNullOrEmpty(query))
                {
                    System.Windows.MessageBox.Show("Enter a valid SQL query.");
                    return;
                }

                // Выполняем запрос и обновляем DataGrid
                ExecuteQueryAndRefreshDataGrid(query);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }


      


        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем DataGrid
            DataGrid1.ItemsSource = null;
         
            // Восстанавливаем текст подсказки в TextBlock1
            TextBlock1.Text = "Enter your request";

            // Отображение всей информации о товаре;
            ExecuteQueryAndRefreshDataGrid("SELECT * FROM Товар");
        }

       
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            // Пример использования метода для выполнения запроса
            ExecuteQueryAndRefreshDataGrid("SELECT DISTINCT Тип FROM Товар");
        }



        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            // Отображение всех поставщиков
            ExecuteQueryAndRefreshDataGrid("SELECT * FROM Поставщик");
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            ShowProductWithMaxQuantity();
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
           ShowProductWithMinQuantity();
        }

        private void Button7_Click(object sender,RoutedEventArgs e)
        {
            ShowProductWithMinCost();
        }
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            ShowProductWithMaxCost();
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            // Отображение всех поставок
            ExecuteQueryAndRefreshDataGrid("SELECT * FROM Поставки");
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            QueryWindow1 window1= new();
            window1.ShowDialog();
        }
    }
}