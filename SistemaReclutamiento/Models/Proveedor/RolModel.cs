using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class RolModel
    {
        string _conexion;
        public RolModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<RolEntidad> RolListarJson()
        {
            List<RolEntidad> lista = new List<RolEntidad>();
            string consulta = @"SELECT 
                                rol_id, 
                                rol_nombre, 
                                rol_estado,
                                rol_fecha_reg,
                                rol_fecha_act, 
                                fk_usuario
	                                FROM seguridad.seg_rol
                                where rol_estado='A';";
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
                                var rol = new RolEntidad
                                {

                                    rol_id = ManejoNulos.ManageNullInteger(dr["rol_id"]),
                                    rol_nombre = ManejoNulos.ManageNullStr(dr["rol_nombre"]),
                                    rol_estado = ManejoNulos.ManageNullStr(dr["rol_estado"]),
                                    rol_fecha_reg = ManejoNulos.ManageNullDate(dr["rol_fecha_reg"]),
                                    rol_fecha_act = ManejoNulos.ManageNullDate(dr["rol_fecha_act"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"])
                                };
                                lista.Add(rol);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }
    }
}