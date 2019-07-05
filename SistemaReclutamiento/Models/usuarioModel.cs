using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class usuarioModel
    {
        string _conexion = string.Empty;
        public usuarioModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public usuarioEntidad ValidarCredenciales(string usu_login)
        {
            usuarioEntidad usuario = new usuarioEntidad();
            string consulta = @"SELECT [usuarioId]
                                  ,[personaId]
                                  ,[usuarioEmail]
                                  ,[usuarioContrasenia]
                                  ,[usuarioToken]
                                  ,[usuarioAvatarExtension]
                                  ,[usuarioEstado]
                                  ,[usuarioFechaCreacion]
                                  ,[usuarioValidado]
                              FROM [dbo].[Usuario] as u
                                where u.usuarioEmail = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usu_login);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usuarioId = ManejoNulos.ManageNullInteger(dr["usuarioId"]);
                                usuario.usuarioEmail = ManejoNulos.ManageNullStr(dr["usuarioEmail"]);
                                usuario.usuarioContrasenia = ManejoNulos.ManageNullStr(dr["usuarioContrasenia"]);
                                usuario.usuarioAvatarExtension = ManejoNulos.ManageNullStr(dr["usuarioAvatarExtension"]);
                                usuario.usuarioToken = ManejoNulos.ManageNullStr(dr["usuarioToken"]);
                                usuario.usuarioValidado = ManejoNulos.ManageNullInteger(dr["usuarioValidado"]);
                                usuario.personaId = ManejoNulos.ManageNullInteger(dr["personaId"]);
                                usuario.usuarioEstado = ManejoNulos.ManageNullInteger(dr["usuarioEstado"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
            }
            return usuario;
        }
        public bool UsuarioValidarEmailJson(usuarioEntidad usuario)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[Usuario]
            SET [usuarioValidado] = 1
            WHERE usuarioID = @p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);               
                    query.Parameters.AddWithValue("@p1", usuario.usuarioId);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }
            public bool UsuarioInsertarJson(usuarioEntidad usuario)
            {
                bool response = false;
                string consulta = @"INSERT INTO [dbo].[Usuario](
                                                [personaId]
                                                ,[usuarioEmail]
                                                ,[usuarioContrasenia]
                                                ,[usuarioEstado]
                                                ,[usuarioValidado]
                                                ,[usuarioFechaCreacion])
                                                VALUES
                                                (@p0,@p1,@p2,@p3,@p4,@p5)";
                try
                {
                    using (var con = new SqlConnection(_conexion))
                    {
                        con.Open();
                        var query = new SqlCommand(consulta, con);
                        query.Parameters.AddWithValue("@p0", usuario.personaId);
                        query.Parameters.AddWithValue("@p1", usuario.usuarioEmail);
                        query.Parameters.AddWithValue("@p2", usuario.usuarioContrasenia);
                        query.Parameters.AddWithValue("@p3", usuario.usuarioEstado);
                        query.Parameters.AddWithValue("@p4", usuario.usuarioValidado);
                        query.Parameters.AddWithValue("@p5", usuario.usuarioFechaCreacion);                
                        query.ExecuteNonQuery();
                        response = true;
                    }
                }
                catch (Exception ex)
                {
                }
                return response;
            }
    }
}