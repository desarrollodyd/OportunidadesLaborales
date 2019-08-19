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
        /// <summary>
        /// Metodo para obtener un usuario por Id
        /// </summary>
        /// <param name="id"> (int) Id de Usuario</param>
        /// <returns>(usuarioEntidad)retorna un Usuario</returns>
        public usuarioEntidad UsuarioObtenerxID(int id)
        {
            usuarioEntidad usuario = new usuarioEntidad();
            string consulta = @"SELECT usu_id,usu_nombre,usu_estado,usu_clave_temp,fk_persona 
	                            FROM seguridad.seg_usuario where usu_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]);
                                usuario.usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]);
                                //usuario.usu_contrasenia = ManejoNulos.ManageNullStr(dr["usu_contraseña"]);
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
                                usuario.usu_clave_temp = ManejoNulos.ManageNullStr(dr["usu_clave_temp"]);
                                usuario.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }
        /// <summary>
        /// Metodo para Validar Credenciales para Formulario de Login y otorgar Acceso al Sistema
        /// </summary>
        /// <param name="usu_login">(string) nombre de usuario obtenido de formulario de login</param>
        /// <returns>(usuarioEntidad)retorna el usuario cuya columna usu_nombre coincida con el parametro usu_login (usu_nombre es una direccion email)</returns>
        public usuarioEntidad ValidarCredenciales(string usu_login)
        {
            usuarioEntidad usuario = new usuarioEntidad();
            string consulta = @"SELECT usu_id,lower(usu_nombre) as usu_nombre,usu_contraseña,usu_estado,fk_persona,usu_clave_temp
	                            FROM seguridad.seg_usuario where lower(usu_nombre)=@p0";
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
                                usuario.usu_clave_temp = ManejoNulos.ManageNullStr(dr["usu_clave_temp"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }
        /// <summary>
        /// Metodo para Validar Una cuenta de usuario creada y que no ha sido activada
        /// </summary>
        /// <param name="usuario">(usuarioEntidad) datos del usuario</param>
        /// <returns>true o false</returns>
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
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        /// <summary>
        /// Metodo para Insertar un nuevo Usuario
        /// </summary>
        /// <param name="usuario"> (usuarioEntidad) datos del usuario a insertar</param>
        /// <returns>retorna idUsuarioInsertado; que es id del usuario que se inserto</returns>
        public int UsuarioInsertarJson(usuarioEntidad usuario)
        {
            int idUsuarioInsertado = 0;
            string consulta = @"INSERT INTO seguridad.seg_usuario(
	                            fk_persona, usu_nombre, usu_contraseña, usu_estado, usu_clave_temp, usu_cambio_pass,usu_fecha_reg)
                                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)
                                returning usu_id; ";
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
                    query.Parameters.AddWithValue("@p4", usuario.usu_clave_temp);
                    query.Parameters.AddWithValue("@p5", usuario.usu_cambio_pass);
                    query.Parameters.AddWithValue("@p6", usuario.usu_fecha_reg);
                    //query.ExecuteNonQuery();
                    //response = true;
                    idUsuarioInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return idUsuarioInsertado;
        }
        /// <summary>
        /// Metodo para Obtener un token de acceso que se crea para activaciones o cambios de contraseña, encriptado con SHA512
        /// </summary>
        /// <param name="token">token obtenido desde URL</param>
        /// <returns>(usuarioEntidad) usuario al que le corresponde el token enviado</returns>
        public usuarioEntidad UsuarioObtenerTokenJson(string token)
        {
            usuarioEntidad usuario = new usuarioEntidad();
            string consulta = @"SELECT usu_id,usu_nombre,usu_contraseña,usu_estado,usu_clave_temp
	                            FROM seguridad.seg_usuario where usu_clave_temp=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", token);
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
                                usuario.usu_clave_temp = ManejoNulos.ManageNullStr(dr["usu_clave_temp"]);
                           
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }
        public bool UsuarioEditarEstadoJson(int id, string password)
        {
            bool response = false;
            string consulta = @"
                UPDATE seguridad.seg_usuario
                SET 
                usu_estado=@p0,
                usu_contraseña=@p1,
                usu_clave_temp=@p2
	            WHERE usu_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", "A");
                    query.Parameters.AddWithValue("@p1", password);
                    query.Parameters.AddWithValue("@p2", "");
                    query.Parameters.AddWithValue("@p3", id);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool UsuarioEditarContraseniaJson(int id, string password)
        {
            bool response = false;
            string consulta = @"
                UPDATE seguridad.seg_usuario
                SET 
                usu_contraseña=@p0
	            WHERE usu_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", password);
                    query.Parameters.AddWithValue("@p1", id);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        /// <summary>
        /// Metodo que sirve para verificar que nombres de usuario no se repitan
        /// </summary>
        /// <param name="direccion_correo">(string) direccion de correo obtenida de formulario de Registro</param>
        /// <returns>retorna la informacion del usuario buscado</returns>
        public usuarioEntidad UsuarioObtenerxCorreo(string direccion_correo)
        {
            usuarioEntidad usuario = new usuarioEntidad();
            string consulta = @"SELECT usu_id,usu_nombre,usu_estado
	                            FROM seguridad.seg_usuario where usu_nombre=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", direccion_correo);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]);
                                usuario.usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]);                      
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }
    }
}