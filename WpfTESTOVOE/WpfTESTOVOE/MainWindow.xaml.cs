using Npgsql;
using System;
using System.Collections.Generic;
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
using System.Configuration;
using WpfTESTOVOE.MVC;
using WpfTESTOVOE.Pages;
using System.Data;

namespace WpfTESTOVOE
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowsCodeBehind
    {
        DataBaseContent view1;
        DataBaseTableContent view2;
        DataBaseExport view3;
        DataBaseImport view4;
        public MainWindow()
        {
            view1 = new DataBaseContent();
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //загрузка вьюмодел для кнопок меню
            MenuViewModel vm = new MenuViewModel();
            //даем доступ к этому кодбихайнд
            vm.CodeBehind = this;
            //делаем эту вьюмодел контекстом данных
            this.DataContext = vm;
            //загрузка стартовой View
            LoadView(ViewType.DataBaseContent);
        }
        public void LoadView(ViewType typeView)
        {
            switch (typeView)
            {
                case ViewType.DataBaseContent:
                    view1 = new DataBaseContent();
                    OutputView.Content = view1;
                    break;
            }
            
            switch (typeView)
            {
                case ViewType.DataBaseTableContent:
                    view2 = new DataBaseTableContent();
                    OutputView.Content = view2;
                    break;
            }
            switch (typeView)
            {
                case ViewType.DataBaseExport:
                    view3 = new DataBaseExport();
                    OutputView.Content = view3;
                    break;
            }
            switch (typeView)
            {
                case ViewType.DataBaseImport:
                    view4 = new DataBaseImport();
                    OutputView.Content = view4;
                    break;
            }
        }
    }

}
