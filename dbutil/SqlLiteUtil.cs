using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace VGMToolbox.dbutil
{
    public sealed class SqlLiteUtil
    {
        private SqlLiteUtil() { }
        
        public static DataTable GetSimpleDataTable(string databasePath,
            string tableName, string orderByField)
        {
            StringBuilder sqlCommand = new StringBuilder();
            
            SQLiteConnection conn;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataAdapter da;

            DataTable dt = null;
            DataSet ds = new DataSet();

            try
            {
                conn = new SQLiteConnection(String.Format(CultureInfo.InvariantCulture, "Data Source={0};Version=3;Read Only=True;UseUTF16Encoding=True;", databasePath));
                conn.Open();

                sqlCommand.AppendFormat(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tableName);

                if (!String.IsNullOrEmpty(orderByField))
                {
                    sqlCommand.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY {0}", orderByField);
                }

                cmd.Connection = conn;
                cmd.CommandText = sqlCommand.ToString();                
                                
                da = new SQLiteDataAdapter(cmd);                
                
                ds.Reset();
                ds.Locale = CultureInfo.InvariantCulture;
                da.Fill(ds);
                
                dt = ds.Tables[0];
                dt.Locale = CultureInfo.InvariantCulture;

                conn.Close();

                return dt;
            }
            catch (Exception)
            {
                return null;                
            }
        }

        public static DataTable GetSimpleDataItem(string databasePath,
            string tableName, string itemField, string itemId)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataAdapter da;

            DataTable dt = null;
            DataSet ds = new DataSet();

            try
            {
                conn = new SQLiteConnection(String.Format(CultureInfo.InvariantCulture, 
                    "Data Source={0};Version=3;Read Only=True;", databasePath));
                conn.Open();

                cmd.CommandText = String.Format(CultureInfo.InvariantCulture, 
                    "SELECT * FROM {0} WHERE {1} = '{2}'",
                    tableName, itemField, itemId);
                cmd.Connection = conn;

                da = new SQLiteDataAdapter(cmd);
                ds.Reset();
                ds.Locale = CultureInfo.InvariantCulture;
                da.Fill(ds);
                
                dt = ds.Tables[0];
                dt.Locale = CultureInfo.InvariantCulture;

                conn.Close();

                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
