using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using WpfTESTOVOE.Extension;
using WpfTESTOVOE.Model;

namespace WpfTESTOVOE
{
    public class ImportExportExt
    {
        public async static void RecordFileAsync(string path, ThreadSafeObservableCollection<ModelExportElements> exportelements, ModelExportElements element)
        {
            int strigcount = 0;
            DataTable dt = BDConnect.DataBaseTableConnect($"select * from {path}");
            using StreamWriter swc = new StreamWriter($"{GetPath()}{path}.csv", false, Encoding.GetEncoding(1251));
            var columns = dt.Columns.Cast<DataColumn>();
            swc.WriteLine(string.Join(";", columns));
            strigcount++;
            int index = exportelements.IndexOf(element);
            exportelements.ChangeItem(index, new ModelExportElements(path, Convert.ToString(strigcount)));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                using StreamWriter sw = new StreamWriter($"{GetPath()}{path}.csv", true, Encoding.GetEncoding(1251));
                sw.WriteLine(string.Join(";", dt.Rows[i].ItemArray));
                strigcount++;
                exportelements.ChangeItem(index, new ModelExportElements(path, Convert.ToString(strigcount)));
            }
            dt.Clear();
            exportelements.ChangeItem(index, new ModelExportElements(path, Convert.ToString(strigcount),true));
        }
        public async static void ImpoortFileAsync(string path, string fileName, ThreadSafeObservableCollection<ModelImportElements> importelements, ModelImportElements element)
        {
            using (NpgsqlConnection con = BDConnect.GetConnection())
            {

                try
                {
                    con.Open();
                }
                catch(Exception ex)
                {
                    return ;
                }

                if (!File.Exists(path))
                {
                    return ;
                }
                if (con.State == System.Data.ConnectionState.Open)
                {
                    ///проверка на присутствие таблицы в бд
                    NpgsqlDataReader reader = BDConnect.GetCommand(con, $"select exists(select * from information_schema.tables where table_name='{fileName.Split('.')[1]}') AS table_existence;").ExecuteReader();
                    

                    DataTable data = new DataTable();
                    data.Load(reader);
                    if (Convert.ToBoolean(data.Rows[0][0]) == true)
                    {
                        ///опять же для ускорения разработки , роняю все связи, т.к. в задании не было указания о необходимости их сохранения
                        BDConnect.GetCommand(con, $"DROP TABLE {fileName} CASCADE").ExecuteReader();

                    }
                    await Task.Run(() => Insert(path,fileName, importelements,element));
                }
                return ;
            }
        }
        private async static void Insert(string path, string fileName, ThreadSafeObservableCollection<ModelImportElements> importelements, ModelImportElements element)
        {
            using (NpgsqlConnection con = BDConnect.GetConnection())
            {

                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    return;
                }

                if (!File.Exists(path))
                {
                    return;
                }
                if (con.State == System.Data.ConnectionState.Open)
                {
                    var stringCount = 0;
                    using (var streamreader = new StreamReader(path))
                    {
                        string header = streamreader.ReadLine();
                        stringCount++;
                        int index = importelements.IndexOf(element);
                        importelements.ChangeItem(index, new ModelImportElements(fileName, Convert.ToString(stringCount),path));
                        string header1 = header.Replace(";", " text, ") + " text";
                        string header2 = " '@" + header.Replace(";", "' , '@") + "'";
                        string header3 = header.Replace(";", " , ");

                        BDConnect.GetCommand(con, $"CREATE TABLE IF NOT EXISTS  {fileName}({header1});").ExecuteNonQuery();

                        string line;
                        string[] currentRow1;
                        currentRow1 = header2.Split(',');
                        string sqlPersonInfo;
                        NpgsqlCommand sqlInsert;
                        while ((line = streamreader.ReadLine()) != null)
                        {

                            List<string> currentRow2 = new List<string>();
                            string liner = line.Replace(";", " , ");

                            ///простая конструкция не хотела юзаться , которая line.splite(" , "), возможно из-за версии языка чуть ниже 8, хотя по документации должна работать
                            ///StringSplitOptions.None + чекнуть кодировку фАЙЛА
                            currentRow2 = line.Split(new string[] { " , " }, StringSplitOptions.RemoveEmptyEntries).ToList()[0].Split(';').ToList();

                            int temprowcount = currentRow2.Count;
                            if (temprowcount < currentRow1.Length)
                            {
                                for (int i = 0; i < currentRow1.Length - temprowcount; i++)
                                    currentRow2.Add("");
                            }
                            string tempRow = "";

                            foreach (var row in currentRow2)
                            {
                                tempRow += "'" + row + "',";
                            }
                            tempRow = tempRow.Remove(tempRow.Length - 1);
                            sqlPersonInfo = $@"
                                        INSERT INTO {fileName} 
                                            ({header3})
                                        VALUES 
                                            ({tempRow})
                                    "
                            ;
                            BDConnect.GetCommand(con, sqlPersonInfo).ExecuteNonQuery();

                            stringCount++;
                            importelements.ChangeItem(index, new ModelImportElements(fileName, Convert.ToString(stringCount), path));

                        }
                        importelements.ChangeItem(index, new ModelImportElements(fileName, Convert.ToString(stringCount), path,true));

                    }
                }
            }

        }
        /// <summary>
        /// захардкодил путь экспорта, т.к. четких указаний не было
        /// </summary>
        /// <returns></returns>
        private static string GetPath()
        {
            return @"C:\Users\MSI\Desktop\TESTOVOE\WpfTESTOVOE\WpfTESTOVOE\ExportFiles\";
        }
    }
}
