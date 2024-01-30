using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTESTOVOE.MVC
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {

        }

        public IMainWindowsCodeBehind CodeBehind { get; set; }


        private RelayCommand _LoadDataBaseContent;
        public RelayCommand LoadDataBaseContent
        {
            get
            {
                return _LoadDataBaseContent = _LoadDataBaseContent ??
                  new RelayCommand(OnLoadDataBaseContent, CanLoadDataBaseContent);
            }
        }
        private bool CanLoadDataBaseContent()
        {
            return true;
        }
        private void OnLoadDataBaseContent()
        {
            CodeBehind.LoadView(ViewType.DataBaseContent);
        }

        private RelayCommand _LoadDataBaseTableContent;
        public RelayCommand LoadDataBaseTableContent
        {
            get
            {
                return _LoadDataBaseTableContent = _LoadDataBaseTableContent ??
                  new RelayCommand(OnLoadDataBaseTableContent, CanLoadDataBaseTableContent);
            }
        }
        private bool CanLoadDataBaseTableContent()
        {
            return true;
        }
        private void OnLoadDataBaseTableContent()
        {
            CodeBehind.LoadView(ViewType.DataBaseTableContent);
        }


        private RelayCommand _LoadDataBaseExport;
        public RelayCommand LoadDataBaseExport
        {
            get
            {
                return _LoadDataBaseExport = _LoadDataBaseExport ??
                  new RelayCommand(OnLoadDataBaseExport, CanLoadDataBaseExport);
            }
        }
        private bool CanLoadDataBaseExport()
        {
            return true;
        }
        private void OnLoadDataBaseExport()
        {
            CodeBehind.LoadView(ViewType.DataBaseExport);
        }



        private RelayCommand _LoadDataBaseImport;
        public RelayCommand LoadDataBaseImport
        {
            get
            {
                return _LoadDataBaseImport = _LoadDataBaseImport ??
                  new RelayCommand(OnLoadDataBaseImport, CanLoadDataBaseImport);
            }
        }
        private bool CanLoadDataBaseImport()
        {
            return true;
        }
        private void OnLoadDataBaseImport()
        {
            CodeBehind.LoadView(ViewType.DataBaseImport);
        }
    }
}
