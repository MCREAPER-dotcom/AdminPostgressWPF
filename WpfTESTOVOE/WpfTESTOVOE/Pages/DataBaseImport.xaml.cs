using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfTESTOVOE.Extension;
using WpfTESTOVOE.Model;
using static System.Net.Mime.MediaTypeNames;

namespace WpfTESTOVOE.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataBaseImport.xaml
    /// </summary>
    public partial class DataBaseImport : Page
    {
        ThreadSafeObservableCollection<ModelImportElements> Importelements;
        public DataBaseImport()
        {
            InitializeComponent();
            Importelements= new ThreadSafeObservableCollection<ModelImportElements>();
            GridDataBaseImportContent.ItemsSource = Importelements;
        }
        private void Target_Drop(object sender, DragEventArgs e)
        {
            string[] files;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = ((string[])e.Data.GetData(DataFormats.FileDrop));
                if (files.Length>0)
                    foreach (string file in files) 
                    {
                        ///заранее поразумеваю, что csv файл будет нужного для бд формата, для уменьшения времени разработки
                        ///стараюсь избегать импорт системныцх таблиц
                        ///про транзакции тоже не было сказано в тз
                        if (System.IO.Path.GetExtension(file) == ".csv" &&
                            !(System.IO.Path.GetFileNameWithoutExtension(file)).Contains("pg_catalog.") &&
                            !(System.IO.Path.GetFileNameWithoutExtension(file)).Contains("information_schema."))
                        {
                            ListBoxFile.Items.Add(System.IO.Path.GetFullPath(file));
                            Importelements.Add(new ModelImportElements(System.IO.Path.GetFileNameWithoutExtension(file), "0", System.IO.Path.GetFullPath(file)));
                        }

                    }
            }
        }
        private async void ImportData(object sender, RoutedEventArgs e)
        {
            int i = 0;
            var str = "";
            if (Threadcount.Text == "" || Threadcount.Text == null)
                str = "1";
            else
                str = Threadcount.Text;
            if (int.TryParse(str, out i))
            {
                if (Convert.ToInt32(str) > 0)
                {
                    int index = 0;
                    ThreadPool.SetMaxThreads(Convert.ToInt32(str), Convert.ToInt32(str));
                    foreach (var element in Importelements)
                        ThreadPool.QueueUserWorkItem((state) => ImportExportExt.ImpoortFile(element.filePath, element.file, Importelements, Importelements.ElementAt(index++)));
                }
                else
                    MessageBox.Show("Число меньше 0");
            }
            else
                MessageBox.Show("число потоков не соответствует числу");
        }
    }
}
