using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.SqlClient;
using Npgsql;
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
            string consulta = @"SELECT usu_id,usu_nombre,usu_contraseña,usu_estado,fk_persona
	                            FROM seguridad.seg_usuario where usu_nombre=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usu_login);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]);
                                usuario.usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]);
                                usuario.usu_contrasenia = ManejoNulos.ManageNullStr(dr["usu_contraseña"]);
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
                                usuario.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                //usuario.usuarioAvatarExtension = ManejoNulos.ManageNullStr(dr["usuarioAvatarExtension"]);
                                //usuario.usuarioToken = ManejoNulos.ManageNullStr(dr["usuarioToken"]);
                                //usuario.usuarioValidado = ManejoNulos.ManageNullInteger(dr["usuarioValidado"]);
                                //usuario.personaId = ManejoNulos.ManageNullInteger(dr["personaId"]);
                                //usuario.usuarioEstado = ManejoNulos.ManageNullInteger(dr["usuarioEstado"]);
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
            string consulta = @"UPDATE seguridad.seg_usuario
            SET usu_estado = 'A'
            WHERE usuarioID = @p1";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);               
                    query.Parameters.AddWithValue("@p1", usuario.usu_id);
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
                string consulta = @"INSERT INTO seguridad.seg_usuario(
                                                [fk_persona]
                                                ,[usu_nombre]
                                                ,[usu_contrasenia]
                                                ,[usu_estado])
                                               
                                                VALUES
                                                (@p0,@p1,@p2,@p3)";
                try
                {
                    using (var con = new NpgsqlConnection(_conexion))
                    {
                        con.Open();
                        var query = new NpgsqlCommand(consulta, con);
                        query.Parameters.AddWithValue("@p0", usuario.fk_persona);
                        query.Parameters.AddWithValue("@p1", usuario.usu_nombre);
                        query.Parameters.AddWithValue("@p2", usuario.usu_contrasenia);
                        query.Parameters.AddWithValue("@p3", usuario.usu_estado);
                       
                                   
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