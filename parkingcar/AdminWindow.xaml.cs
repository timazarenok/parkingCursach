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
using System.Windows.Shapes;

namespace parkingcar
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            SetTypes();
        }
        private void SetTypes()
        {
            DataTable dt = DB.Select("select * from Types");
            List<string> types = new List<string>();
            foreach(DataRow dr in dt.Rows)
            {
                types.Add(dr["name"].ToString());
            }
            Types.ItemsSource = types;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(DB.Command($"insert into Types values('{TypeName.Text}')"))
            {
                MessageBox.Show("Успешно добавлено");
                SetTypes();
            }
        }

        private void AddPosition_Click(object sender, RoutedEventArgs e)
        {
            int id_type = DB.GetId($"select * from Types where name='{Types.SelectedItem}'");
            if (DB.Command($"insert into Positions values({id_type}, {Number.Text}, 0)"))
            {
                MessageBox.Show("Место добавлено");
            }
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void ParkingOtchet_Click(object sender, RoutedEventArgs e)
        {
            ParkingOtchet window = new ParkingOtchet();
            window.Show();
        }
    }
}
