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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTESTOVOE.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataBaseTableContent.xaml
    /// </summary>
    public partial class DataBaseTableContent : Page
    {
        DataTable Data;
        public DataBaseTableContent()
        {

            InitializeComponent();
            ///исключаю системные каталоги и не таблицы
            Data = BDConnect.DataBaseTableConnect("SELECT Table_Schema,Table_Name  FROM information_schema.tables where table_type='BASE TABLE' and table_schema!='pg_catalog' and table_schema!='information_schema' ");
            if (Data != null)
            {
                GridDataBaseContent.ItemsSource = Data.DefaultView;
            }
            else
            {
                MessageBox.Show("No Connect");
            }
        }
        private void CellsSelected(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null)
            {
                string schema = "";
                var text = (cell.Content as TextBlock).Text;
                foreach (var dataselect in Data.Select())
                {
                    if (dataselect.ItemArray[1].ToString() == text)
                    {
                        schema= dataselect.ItemArray[0].ToString();
                        break;
                    }
                }
                DataTable data;
                if (schema != "")
                    try 
                    {
                        data = BDConnect.DataBaseTableConnect($"select * \r\nfrom {schema}.{text}");
                    }
                    catch
                    {
                        data = null;
                    }
                else
                    data = null;
                if (data != null)
                {
                    GridDataBaseCellContent.ItemsSource = data.DefaultView;
                }
                else
                {
                    MessageBox.Show("Error Request");
                }

            }
        }
    }
}
