using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection;

        static DataBaseManager()
        {
            DataBaseManager.stringConnection = "Server=.;Database=20230622SP;Trusted_Connection=True;";
        }

        public static string GetImagenComida(string tipo)
        {
            try 
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = "SELECT * FROM comidas WHERE tipo_comida = @tipo";
                    SqlCommand command = new SqlCommand(query, DataBaseManager.connection);
                    command.Parameters.AddWithValue("tipo", tipo);
                    DataBaseManager.connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string url = reader.GetString(2);
                        return url;
                    }
                    else
                    {
                        throw new ComidaInvalidaExeption("No hay elmentos de ese tipo de comida");
                    }
                }
            }
            catch(Exception ex)
            {
                throw new DataBaseManagerException("Error al obtener datos de la base",ex);
            }
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T elemento) where T : IComestible, new()
        {
            try 
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = "INSERT INTO tickets (empleado,ticket) values (@empleado,@ticket)";
                    SqlCommand command = new SqlCommand(query, DataBaseManager.connection);
                    command.Parameters.AddWithValue("empleado", nombreEmpleado);
                    //command.Parameters.AddWithValue("ticket", elemento.Ticket);
                    DataBaseManager.connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                
            }
            catch (Exception ex) 
            {
                throw new DataBaseManagerException("Error al insertar datos a la base", ex);
            }
        }
    }
}
