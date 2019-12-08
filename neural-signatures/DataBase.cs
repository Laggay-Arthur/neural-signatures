using System;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace neural_signatures
{
    static class DataBase
    {
        public static string text = "";
        public static SqlConnection sqlconnection;
        public static System.Windows.Controls.ComboBox combobox;
        public static void Init(ref System.Windows.Controls.ComboBox combo) => combobox = combo;

        public static string Connection
        {
            get
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string PathFolder1 = Path.GetDirectoryName(path);
                string PathFolder2 = Path.GetDirectoryName(PathFolder1);
                PathFolder1 = Path.GetDirectoryName(PathFolder2);
                // PathFolder1 = PathFolder1.ToString() + @"\Documents.mdf";
                string connection = //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + PathFolder1 + ";Integrated Security=True";
                                    // @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = D:\neural - signatures\neural - signatures\Documents.mdf; Integrated Security = True; User Instance=True";
                connection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
                //connection = connection.Replace("D:\\neural-signatures\\neural-signatures", PathFolder1);
                text = PathFolder1;

                return connection;
            }
        }
        public static string insert(string name_document, string name_people, string text_document, DateTime? date_validity = null)
        {//Добавление в БД нового документа 
            sqlconnection = new SqlConnection(Connection);
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                   $"INSERT INTO DocumentsAll(name_document, name_people, text_document, date_document{ (date_validity != null ? ",date_validity" : "")}) VALUES(@name_document, @name_people, @text_document, @date_document{ (date_validity != null ? ",@date_validity" : "")})", sqlconnection);
                com.Parameters.AddWithValue("name_document", name_document);
                com.Parameters.AddWithValue("name_people", name_people);
                com.Parameters.AddWithValue("text_document", text_document);
                com.Parameters.AddWithValue("date_document", DateTime.Now);
                if (date_validity != null)
                    com.Parameters.AddWithValue("date_validity", date_validity);
                com.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return "Возникла ошибка добавления!";
            }
            finally
            {
                sqlconnection.Close();
            }
            return "Данные занесены в таблицу!";
        }

        public static string insertFIOToDB(string FIO)
        {
            sqlconnection = new SqlConnection(Connection);
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                    "INSERT INTO FIO(FIO) VALUES(@FIO)", sqlconnection);
                com.Parameters.AddWithValue("FIO", FIO);

                com.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            finally
            { sqlconnection.Close(); }
            return "Данные занесены в таблицу!";
        }
        public static void SelectAll()
        {
            sqlconnection = new SqlConnection(Connection);
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                    "SELECT * FROM DocumentsAll", sqlconnection);

                SqlDataReader sqlreader = com.ExecuteReader();
                while (sqlreader.Read())
                {
                    text = sqlreader["text_document"].ToString();
                }
                sqlreader.Close();
                com.Dispose();
            }
            catch (Exception)
            {

            }
            finally
            { sqlconnection.Close(); }
        }

        public static List<string> SelectFIO()
        {
            List<string> FIOs = new List<string>();
            sqlconnection = new SqlConnection(Connection);

            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM FIO", sqlconnection);

                SqlDataReader sqlreader = com.ExecuteReader();
                while (sqlreader.Read())
                {
                    FIOs.Add(sqlreader["FIO"].ToString());
                }
                sqlreader.Close();
                com.Dispose();
            }
            catch (Exception)
            { }
            finally
            { sqlconnection.Close(); }
            return FIOs;
        }
    }
}