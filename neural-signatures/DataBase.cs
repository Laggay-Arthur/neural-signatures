using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
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
<<<<<<< HEAD
                PathFolder1 = PathFolder1.ToString() + @"\Documents.mdf";
                string connection = //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + PathFolder1 + ";Integrated Security=True";
                                    // @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = D:\neural - signatures\neural - signatures\Documents.mdf; Integrated Security = True; User Instance=True";
                ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
                connection.Replace(@"D:\neural-signatures\neural-signatures\", PathFolder1.ToString());
=======
               // PathFolder1 = PathFolder1.ToString() + @"\Documents.mdf";
                string connection = //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + PathFolder1 + ";Integrated Security=True";
                                    // @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = D:\neural - signatures\neural - signatures\Documents.mdf; Integrated Security = True; User Instance=True";
                ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
                connection = connection.Replace("D:\\neural-signatures\\neural-signatures", PathFolder1.ToString());
                text = PathFolder1.ToString();
>>>>>>> e94789b82e44f9ea0c5c0c22bc7362385a7ff80a
                
                return connection;
            }
        }
<<<<<<< HEAD
        public async Task<string> insert(string name_document,string name_people,string text_document, DateTime date_validity) { 
        
=======
        public async Task<string> insert(string name_document, string name_people, string text_document, DateTime date_validity)
        {

>>>>>>> e94789b82e44f9ea0c5c0c22bc7362385a7ff80a
            sqlconnection = new SqlConnection(Connection);
            await sqlconnection.OpenAsync();
            //SqlDataReader sqlreader = null;
            SqlCommand com = new SqlCommand(
<<<<<<< HEAD
               
=======

>>>>>>> e94789b82e44f9ea0c5c0c22bc7362385a7ff80a
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
<<<<<<< HEAD
           
=======

>>>>>>> e94789b82e44f9ea0c5c0c22bc7362385a7ff80a
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
<<<<<<< HEAD
           

           sqlreader = com.ExecuteReader();
=======


            sqlreader = com.ExecuteReader();
>>>>>>> e94789b82e44f9ea0c5c0c22bc7362385a7ff80a
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
