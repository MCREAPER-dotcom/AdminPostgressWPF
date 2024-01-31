using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTESTOVOE
{
    public class BDConnect
    {
        public static DataTable? DataBaseTableConnect(string command)
        {
            using NpgsqlConnection con = GetConnection();
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                NpgsqlDataReader reader;
                try
                {
                    reader = GetCommand(con,command).ExecuteReader();
                    DataTable data = new DataTable();
                    data.Load(reader);
                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
        public static NpgsqlCommand GetCommand(NpgsqlConnection con,string command)
        {
            NpgsqlCommand _command = con.CreateCommand();
            _command.CommandType = System.Data.CommandType.Text;
            _command.CommandText = command;
            return _command;
        }
    }
}
