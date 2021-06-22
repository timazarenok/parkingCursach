using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace parkingcar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUsersAutomobiles();
            SetPositions();
        }
        private void SetUsersAutomobiles()
        {
            DataTable dt = DB.Select($"select * from Automobiles where id_user={DB.UserID}");
            List<Auto> autos = new List<Auto>();
            foreach(DataRow dr in dt.Rows)
            {
                autos.Add(new Auto
                {
                    Id = dr["id"].ToString(),
                    Number = dr["number"].ToString()
                });
            }
            Automobiles.ItemsSource = autos;
        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int[] matches = Regex.Matches(b.Content.ToString(), "\\d+")
                .Cast<Match>()
                .Select(x => int.Parse(x.Value))
                .ToArray();
            int number = 0;
            foreach (int match in matches)
                number = match;
            string status = DB.Select($"select * from Positions where number={number}").Rows[0]["status"].ToString();
            if (status == "True")
            {
                MessageBox.Show("Место занято");
            }
            else
            {
                Auto a = (Auto)Automobiles.SelectedItem;
                if(a.Id == null)
                {
                    MessageBox.Show("Выберите авто");
                }
                else
                {
                    string a_type = DB.Select($"select Types.name as type from Automobiles join Types on Types.id = id_type where Automobiles.id={a.Id}").Rows[0]["type"].ToString();
                    int id_p = DB.GetId($"select * from Positions where number={number}");
                    string p_type = DB.Select($"select Types.name as type from Positions join Types on Types.id = id_type where Positions.id={id_p}").Rows[0]["type"].ToString();
                    if(a_type != p_type)
                    {
                        MessageBox.Show("Тип авто не соответствует типу места");
                    }
                    else
                    {
                        if (DB.Command($"insert into UsersPositions values({id_p}, {a.Id})"))
                        {
                            MessageBox.Show("Место забронировано");
                            DB.Command($"update Positions set status=1 where id={id_p}");
                            SetPositions();
                        }
                    }
                }
            }
        }
        private void SetPositions()
        {
            DataTable dt = DB.Select("select * from Positions join Types on Types.id = id_type");
            List<Position> positions = new List<Position>();
            foreach(DataRow dr in dt.Rows)
            {
                positions.Add(new Position {
                    Type = dr["name"].ToString(),
                    Number = dr["number"].ToString(),
                    Status = dr["status"].ToString(),
                });
            }
            Positions.Children.RemoveRange(0, Positions.Children.Count);
            foreach(Position position in positions)
            {
                Button button = new Button();
                button.Content = position.Number + "\n" + position.Type;
                button.Width = 100;
                button.Height = 50;
                button.Margin = new Thickness(10,10,10,10);
                button.Click += Change_Click;
                button.HorizontalContentAlignment = HorizontalAlignment.Center;
                button.Background = position.Status == "False" ? Brushes.Green : Brushes.Red;
                Positions.Children.Add(button);
            }
        }

        private void Automobiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Automobiles.SelectedItem = sender;
        }
        private void UpdateEvent(object sender, EventArgs e)
        {
            SetUsersAutomobiles();
        }

        private void AddAuto_Click(object sender, RoutedEventArgs e)
        {
            AutoAdd window = new AutoAdd();
            window.Show();
            window.Closed += UpdateEvent;
        }
    }
}
