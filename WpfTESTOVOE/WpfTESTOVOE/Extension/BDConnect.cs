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
        public static DataTable DataBaseTableConnect(string Command)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    NpgsqlDataReader reader;
                    try
                    {
                        reader = GetCommand(con,Command).ExecuteReader();
                    }
                    catch 
                    {
                        return null;
                    }
                    try
                    {
                        DataTable data = new DataTable();
                        data.Load(reader);
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
        public static NpgsqlCommand GetCommand(NpgsqlConnection con,string Command)
        {
            NpgsqlCommand command = GetConnection().CreateCommand();
            command.Connection = con;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = Command;
            return command;
        }
    }
}
