using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfTESTOVOE.Extension;
using WpfTESTOVOE.Model;
using static System.Net.Mime.MediaTypeNames;

namespace WpfTESTOVOE.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataBaseExport.xaml
    /// </summary>
    public partial class DataBaseExport : Page
    {
        ThreadSafeObservableCollection<ModelExportElements> exportelements;
        DataTable Data;
        public DataBaseExport()
        {
            InitializeComponent();
            exportelements = new ThreadSafeObservableCollection<ModelExportElements>();
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
            GridDataBaseExportContent.ItemsSource = exportelements;
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
                        schema = dataselect.ItemArray[0].ToString();
                        break;
                    }
                }
                var tempmodel = new ModelExportElements($"{schema}.{text}");
                if (exportelements.Count > 0)
                {
                    foreach (var element in exportelements)
                    {
                        if (element.elements == tempmodel.elements)
                            return;
                    }
                }
                exportelements.Add(tempmodel);

            }
        }
        private void CheckExportData(object sender, RoutedEventArgs e)
        {
            exportelements.Clear();
            exportelements = new ThreadSafeObservableCollection<ModelExportElements>();
        }
        private async void ExportData(object sender, RoutedEventArgs e)
        {
            if (exportelements.Count <= 0)
                MessageBox.Show($"exportelements.Count {exportelements.Count}");
            else
            {
                try
                {
                    var threadnum = 1;
                    if (Threadcount.Text != "" && Threadcount.Text != null)
                        threadnum = Convert.ToInt32(Threadcount.Text);
                    if (threadnum <= 0)
                    {
                        MessageBox.Show("Число меньше 0");
                    }
                    else
                    {
                        int index = 0;
                        ThreadPool.SetMaxThreads(threadnum, threadnum);
                        foreach (var tempdata in exportelements)
                        {
                            ThreadPool.QueueUserWorkItem((state) => ImportExportExt.RecordFileAsync($"{tempdata.elements}", exportelements, exportelements.ElementAt(index++)));
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error Request");
                }
            }
        }
    }
}
