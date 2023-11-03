using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace BancoAtlantida_API_REST.tools
{
    public  class ConectionDB
    {
        //private string connectionString = @"Server=tcp:atlantida.database.windows.net,1433;Initial Catalog=atlantidaDB;Persist Security Info=False;User ID=henry15ea;Password=$cosmosdb15ea;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
       private string connectionString = @"Server=LAPTOP-8S0156K3\SQLEXPRESS;Database=AtlantidaDB;User Id=henry15ea;Password=demolition;";

        protected string ConnectionString { get => connectionString; set => connectionString = value; }
        public SqlConnection fn_GetConnection()
        {
            SqlConnection conn = null;
            conn = new SqlConnection(ConnectionString);
            return conn;

        }

        //funcion que verifica el estado de la conexion
        public bool fn_StatusConection(SqlConnection con) {
            bool result = false;

            if (con.State == ConnectionState.Open)
            {

                return true;
            }

            return result;
        }

        //end class
    }
}
