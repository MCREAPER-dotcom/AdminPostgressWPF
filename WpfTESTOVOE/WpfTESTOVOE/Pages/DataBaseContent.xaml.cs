using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTESTOVOE.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataBaseContent.xaml
    /// </summary>
    public partial class DataBaseContent : Page
    {
        public DataBaseContent()
        {
            InitializeComponent();
            ///вывожу вообще всю информацию, как понял по заданию
            var data=BDConnect.DataBaseTableConnect("select * \r\nfrom information_schema.columns ");
            if (data != null)
            {
                GridDataBaseContent.ItemsSource = data.DefaultView;
            }
            else
            {
                MessageBox.Show("No Connect");
            }
        }

    }
}
