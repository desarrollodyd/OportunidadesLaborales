using Npgsql;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.SeguridadIntranet
{
    public class SEG_UsuarioEmpresaDAL
    {
        string _conexion = string.Empty;
        public SEG_UsuarioEmpresaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<SEG_UsuarioEmpresaEntidad> GetListadoUsuarioEmpresaPorUsuario(int usuario_id)
        {
            string consulta = @"select usuario_id,empresa_id from intranet.seg_usuarioempresa where usuario_id=@usuario_id";
            List<SEG_UsuarioEmpresaEntidad> lista = new List<SEG_UsuarioEmpresaEntidad>();
            try
            {
                using(var con=new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta,con);
                    query.Parameters.AddWithValue("@usuario_id", usuario_id);
                    using(var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new SEG_UsuarioEmpresaEntidad
                            {
                                usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]),
                                empresa_id = ManejoNulos.ManageNullInteger(dr["empresa_id"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            }catch(Exception ex)
            {
                lista.Clear();
            }
            return lista;
        }
        public bool InsertarUsuarioEmpresaDAL(SEG_UsuarioEmpresaEntidad usuarioEmpresa)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO intranet.seg_usuarioempresa
           (usuario_id,empresa_id) VALUES (@p0,@p1)";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuarioEmpresa.usuario_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(usuarioEmpresa.empresa_id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta= false;
            }

            return respuesta;
        }
        public bool UsuarioEmpresaEliminarPorUsuarioId(int usuario_id)
        {
            bool respuesta = false;
            string consulta = @"delete from intranet.seg_usuarioempresa where usuario_id=@p0";
            try
            {
                using(var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuario_id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }catch(Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}