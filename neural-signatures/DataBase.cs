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
        {//Подключение к БД
            get
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string PathFolder1 = Path.GetDirectoryName(path);
                string PathFolder2 = Path.GetDirectoryName(PathFolder1);
                PathFolder1 = Path.GetDirectoryName(PathFolder2);
                // PathFolder1 = PathFolder1.ToString() + @"\Documents.mdf";
                string connection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
                //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + PathFolder1 + ";Integrated Security=True";
                // @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = D:\neural - signatures\neural - signatures\Documents.mdf; Integrated Security = True; User Instance=True";
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
            catch (Exception) { return "Возникла ошибка добавления!"; }
            finally { sqlconnection.Close(); }
            return "Данные занесены в таблицу!";
        }
        public static string insertWithoutFIO(string name_document, string text_document, DateTime? date_validity = null)
        {//Добавление в БД нового документа 
            sqlconnection = new SqlConnection(Connection);
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                   $"INSERT INTO DocumentsAll(name_document, text_document, date_document{ (date_validity != null ? ",date_validity" : "")}) VALUES(@name_document, @text_document, @date_document{ (date_validity != null ? ",@date_validity" : "")})", sqlconnection);
                com.Parameters.AddWithValue("name_document", name_document);
                com.Parameters.AddWithValue("text_document", text_document);
                com.Parameters.AddWithValue("date_document", DateTime.Now);
                if (date_validity != null)
                    com.Parameters.AddWithValue("date_validity", date_validity);
                com.ExecuteNonQuery();
            }
            catch (Exception) { return "Возникла ошибка добавления!"; }
            finally { sqlconnection.Close(); }
            return "Данные занесены в таблицу!";
        }
        public static string insertFIOToDB(string FIO)
        {//Добавление нового
            sqlconnection = new SqlConnection(Connection);
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                    "INSERT INTO FIO(FIO) VALUES(@FIO)", sqlconnection);
                com.Parameters.AddWithValue("FIO", FIO);

                com.ExecuteNonQuery();
            }
            catch (Exception) { }
            finally { sqlconnection.Close(); }
            return "Данные занесены в таблицу!";
        }
        public static List<TableDoc> SelectAllDoc()
        {
            sqlconnection = new SqlConnection(Connection);
            List<TableDoc> DocAll = new List<TableDoc>();
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                    "SELECT * FROM DocumentsAll", sqlconnection);

                SqlDataReader sqlreader = com.ExecuteReader();
                while (sqlreader.Read())
                {
                    DocAll.Add(new TableDoc(sqlreader["name_document"].ToString(), sqlreader["name_people"].ToString(), sqlreader["date_document"].ToString(), sqlreader["date_validity"].ToString()));
                }
                sqlreader.Close();
                com.Dispose();
            }
            catch (Exception) { }
            finally { sqlconnection.Close(); }
            return DocAll;
        }

        public static List<TableDoc> SelectPersonDoc(string person)
        {
            sqlconnection = new SqlConnection(Connection);
            List<TableDoc> DocAll = new List<TableDoc>();
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand(
                    "SELECT * FROM DocumentsAll WHERE name_people =@person", sqlconnection);
                com.Parameters.AddWithValue("person", person);
                SqlDataReader sqlreader = com.ExecuteReader();
                while (sqlreader.Read())
                {
                    DocAll.Add(new TableDoc(sqlreader["name_document"].ToString(), sqlreader["name_people"].ToString(), sqlreader["date_document"].ToString(), sqlreader["date_validity"].ToString()));
                }
                sqlreader.Close();
                com.Dispose();
            }
            catch (Exception) { }
            finally { sqlconnection.Close(); }
            return DocAll;
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
            catch (Exception) { }
            finally { sqlconnection.Close(); }
            return FIOs;
        }

        public static List<Neiron> SelectNeiroins()
        {
            List<Neiron> Neirons = new List<Neiron>();
            sqlconnection = new SqlConnection(Connection);

            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM Neirons", sqlconnection);

                SqlDataReader sqlreader = com.ExecuteReader();
                while (sqlreader.Read())
                {
                    Neirons.Add(sqlreader["Neiron"] as Neiron);
                }
                sqlreader.Close();
                com.Dispose();
            }
            catch (Exception) { }
            finally { sqlconnection.Close(); }
            return Neirons;
        }

        public static string insertNeiron(Neiron neir)
        {//Добавление в БД нового документа 
            sqlconnection = new SqlConnection(Connection);
            try
            {
                sqlconnection.Open();
                SqlCommand com = new SqlCommand("SELECT COUNT(*) FROM Neirons WHERE id>=0", sqlconnection);
                int count = (int)com.ExecuteScalar();
                com.ExecuteNonQuery();

                com = new SqlCommand(
                   $"INSERT INTO Neirons VALUES({count + 1},{neir})", sqlconnection);
                com.ExecuteNonQuery();
            }
            catch (Exception) { return "Возникла ошибка добавления!"; }
            finally { sqlconnection.Close(); }
            return "Данные занесены в таблицу!";
        }
    }
}