using System;
using System.Collections.Generic;
using System.Data;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace parkingcar
{
    /// <summary>
    /// Логика взаимодействия для ParkingOtchet.xaml
    /// </summary>
    public partial class ParkingOtchet : Window
    {
        public ParkingOtchet()
        {
            InitializeComponent();
            SetAllParks();
        }
        private void SetAllParks()
        {
            DataTable dt = DB.Select($"select Positions.number as number, status, Automobiles.number as autonumbers from UsersPositions join Positions on Positions.id = id_pos join Automobiles on Automobiles.id = id_auto");
            List<Park> parks = new List<Park>();
            foreach(DataRow dr in dt.Rows)
            {
                parks.Add(new Park {
                    Number = dr["number"].ToString(),
                    Status = dr["status"].ToString(),
                    AutoNumber = dr["autonumber"].ToString()
                });
            }
            Otchet.ItemsSource = parks;
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application ExcelApp = new Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.ColumnWidth = 15;

            ExcelApp.Cells[1, 1] = "Номер места";
            ExcelApp.Cells[1, 2] = "Статус";
            ExcelApp.Cells[1, 3] = "Номер автомобиля";

            var list = Otchet.Items.OfType<Park>().ToList();

            for (int j = 0; j < list.Count; j++)
            {
                ExcelApp.Cells[j + 2, 1] = list[j].Number;
                ExcelApp.Cells[j + 2, 2] = list[j].Status;
                ExcelApp.Cells[j + 2, 3] = list[j].AutoNumber;
            }
            ExcelApp.Visible = true;
        }
    }
}
