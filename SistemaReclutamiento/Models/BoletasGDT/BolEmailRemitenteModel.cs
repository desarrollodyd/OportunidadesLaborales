using Npgsql;
using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.BoletasGDT
{
    public class BolEmailRemitenteModel
    {
        string _conexion;
        public BolEmailRemitenteModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int BolEmailRemitenteInsertarJson(BolEmailRemitenteEntidad email)
        {
            int idInsertado = 0;
            string consulta = @"INSERT INTO intranet.bol_email_remitente(
	email_nombre, email_direccion, email_password, email_ssl, email_smtp, email_puerto, email_estado, email_limite, email_cantidad_envios, email_ultimo_envio)
	VALUES 
(@email_nombre, @email_direccion, @email_password, @email_ssl, @email_smtp, @email_puerto, @email_estado, @email_limite, @email_cantidad_envios, @email_ultimo_envio)
                                returning email_id;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@email_nombre", ManejoNulos.ManageNullStr(email.email_nombre));
                    query.Parameters.AddWithValue("@email_direccion", ManejoNulos.ManageNullStr(email.email_direccion));
                    query.Parameters.AddWithValue("@email_password", ManejoNulos.ManageNullStr(email.email_password));
                    query.Parameters.AddWithValue("@email_ssl", ManejoNulos.ManegeNullBool(email.email_ssl));
                    query.Parameters.AddWithValue("@email_smtp", ManejoNulos.ManageNullStr(email.email_smtp));
                    query.Parameters.AddWithValue("@email_puerto", ManejoNulos.ManageNullInteger(email.email_puerto));
                    query.Parameters.AddWithValue("@email_estado", ManejoNulos.ManageNullInteger(email.email_estado));
                    query.Parameters.AddWithValue("@email_limite", ManejoNulos.ManageNullInteger(email.email_limite));
                    query.Parameters.AddWithValue("@email_cantidad_envios", ManejoNulos.ManageNullInteger(email.email_cantidad_envios));
                    query.Parameters.AddWithValue("@email_ultimo_envio", ManejoNulos.ManageNullDate(email.email_ultimo_envio));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return idInsertado;
        }
        public List<BolEmailRemitenteEntidad> BolEmailRemitenteListarJson()
        {
            claseError error = new claseError();
            List<BolEmailRemitenteEntidad> lista = new List<BolEmailRemitenteEntidad>();
            string consulta = @"SELECT email_id, 
email_nombre, 
email_direccion, 
email_ssl, 
email_smtp, 
email_puerto, 
email_estado, 
email_limite, 
email_cantidad_envios, 
email_ultimo_envio
	FROM intranet.bol_email_remitente;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var bitacora = new BolEmailRemitenteEntidad
                                {
                                    email_id=ManejoNulos.ManageNullInteger(dr["email_id"]),
                                    email_nombre = ManejoNulos.ManageNullStr(dr["email_nombre"]),
                                    email_direccion = ManejoNulos.ManageNullStr(dr["email_direccion"]),
                                    email_ssl = ManejoNulos.ManegeNullBool(dr["email_ssl"]),
                                    email_smtp = ManejoNulos.ManageNullStr(dr["email_smtp"]),
                                    email_puerto = ManejoNulos.ManageNullInteger(dr["email_puerto"]),
                                    email_estado = ManejoNulos.ManageNullInteger(dr["email_estado"]),
                                    email_limite = ManejoNulos.ManageNullInteger(dr["email_limite"]),
                                    email_cantidad_envios = ManejoNulos.ManageNullInteger(dr["email_cantidad_envios"]),
                                    email_ultimo_envio = ManejoNulos.ManageNullDate(dr["email_ultimo_envio"]),
                                };

                                lista.Add(bitacora);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lista;
        }
        public BolEmailRemitenteEntidad BolEmailRemitenteIdObtenerJson(int email_id)
        {
            BolEmailRemitenteEntidad email = new BolEmailRemitenteEntidad();
            string consulta = @"SELECT email_id, 
email_nombre, 
email_direccion, 
email_password, 
email_ssl, 
email_smtp, 
email_puerto, 
email_estado, 
email_limite, 
email_cantidad_envios, 
email_ultimo_envio
	FROM intranet.bol_email_remitente where email_id=@email_id;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@email_id", email_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                               email = new BolEmailRemitenteEntidad
                                {
                                    email_id = ManejoNulos.ManageNullInteger(dr["email_id"]),
                                    email_nombre = ManejoNulos.ManageNullStr(dr["email_nombre"]),
                                    email_direccion = ManejoNulos.ManageNullStr(dr["email_direccion"]),
                                    email_password = ManejoNulos.ManageNullStr(dr["email_password"]),
                                    email_ssl = ManejoNulos.ManegeNullBool(dr["email_ssl"]),
                                    email_smtp = ManejoNulos.ManageNullStr(dr["email_smtp"]),
                                    email_puerto = ManejoNulos.ManageNullInteger(dr["email_puerto"]),
                                    email_estado = ManejoNulos.ManageNullInteger(dr["email_estado"]),
                                    email_limite = ManejoNulos.ManageNullInteger(dr["email_limite"]),
                                    email_cantidad_envios = ManejoNulos.ManageNullInteger(dr["email_cantidad_envios"]),
                                    email_ultimo_envio = ManejoNulos.ManageNullDate(dr["email_ultimo_envio"]),
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return email;
        }
    }
}