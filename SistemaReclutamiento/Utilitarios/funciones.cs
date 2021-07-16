using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SistemaReclutamiento.Utilitarios
{
    public class funciones
    {
        /// <summary>
        ///CONEXION => BD_SEGURIDAD_PJ2
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static String consulta(string tabla, string query = "")
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartArray();

            if (query.Length == 0)
            {
                query = "SELECT * FROM " + tabla + "";
            }
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        jsonWriter.WriteStartObject();

                        int fields = reader.FieldCount;
                        for (int i = 0; i < fields; i++)
                        {
                            jsonWriter.WritePropertyName(reader.GetName(i));
                            jsonWriter.WriteValue(reader[i]);
                        }

                        jsonWriter.WriteEndObject();


                    }
                    jsonWriter.WriteEndArray();
                }
            }
            return sb.ToString();

        }
    }
}