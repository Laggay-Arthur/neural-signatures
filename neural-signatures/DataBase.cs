using System;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace neural_signatures
{
    class DataBase
    {
        public string text = "";
        public SqlConnection sqlconnection;
        public string Connection
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
                ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
                connection = connection.Replace("D:\\neural-signatures\\neural-signatures", PathFolder1);
                text = PathFolder1;
                
                return connection;
            }
        }
        public async Task<string> insert(string name_document, string name_people, string text_document, DateTime date_validity)
        {

            sqlconnection = new SqlConnection(Connection);
            await sqlconnection.OpenAsync();
            //SqlDataReader sqlreader = null;
            SqlCommand com = new SqlCommand(

                "INSERT INTO DocumentsAll() VALUES(@name_document, @name_people, @text_document, @date_document,@date_validity)", sqlconnection);
            com.Parameters.AddWithValue("name_document", name_document);
            com.Parameters.AddWithValue("name_people", name_people);
            com.Parameters.AddWithValue("text_document", text_document);
            com.Parameters.AddWithValue("date_document", DateTime.Now);
            com.Parameters.AddWithValue("date_validdity", date_validity);
            await com.ExecuteNonQueryAsync();
            sqlconnection.Close();
            return "Данные занесены в таблицу!";
        }
        public string insert(string name_document, string name_people, string text_document)
        {
            string connect = Connection;
            sqlconnection = new SqlConnection(connect);
            sqlconnection.Open();
            //SqlDataReader sqlreader = null;
            SqlCommand com = new SqlCommand(

                "INSERT INTO DocumentsAll(name_document, name_people, text_document, date_document) VALUES(@name_document, @name_people, @text_document, @date_document)", sqlconnection);
            com.Parameters.AddWithValue("name_document", name_document);
            com.Parameters.AddWithValue("name_people", name_people);
            com.Parameters.AddWithValue("text_document", text_document);
            com.Parameters.AddWithValue("date_document", DateTime.Now);

            com.ExecuteNonQuery();
            sqlconnection.Close();
            return "Данные занесены в таблицу!";
        }
        public void SelectAll()
        {
            string connect = Connection;
            sqlconnection = new SqlConnection(connect);
            sqlconnection.Open();
            SqlDataReader sqlreader = null;
            SqlCommand com = new SqlCommand(

                "SELECT * FROM DocumentsAll", sqlconnection);


            sqlreader = com.ExecuteReader();
            while (sqlreader.Read())
            {
                text = sqlreader["text_document"].ToString();
            }
            sqlreader.Close();
            sqlconnection.Close();
            //return "Данные занесены в таблицу!";
        }
    }
}
