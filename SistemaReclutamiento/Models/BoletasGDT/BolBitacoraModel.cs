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
    public class BolBitacoraModel
    {
        string _conexion;
        public BolBitacoraModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (int idInsertado, claseError error) BitacoraInsertarJson(BolBitacoraEntidad bitacora)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO intranet.bol_bitacora(
	                                btc_usuario_id, btc_accion, btc_vista, btc_fecha_reg, btc_estado, btc_co_trab, btc_ruta_pdf)
	                                VALUES ( @p0, @p1, @p2, @p3, @p4, @p5, @p6)
                                returning btc_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(bitacora.btc_usuario_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(bitacora.btc_accion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(bitacora.btc_vista));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(bitacora.btc_fecha_reg));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(bitacora.btc_estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(bitacora.btc_co_trab));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(bitacora.btc_ruta_pdf));

                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }
        public (List<BolBitacoraEntidad> lista, claseError error) BitacoraListarFiltrosJson(DateTime fechaInicio,DateTime fechaFin)
        {
            claseError error = new claseError();
            List<BolBitacoraEntidad> listaBitacora = new List<BolBitacoraEntidad>();
            string consulta = @"SELECT btc_id, btc_usuario_id, btc_accion, btc_vista, btc_fecha_reg, btc_estado, btc_co_trab, btc_ruta_pdf
	                            FROM intranet.bol_bitacora where btc_fecha_reg between @p0 and @p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaInicio);
                    query.Parameters.AddWithValue("@p1", fechaFin);
              

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var bitacora = new BolBitacoraEntidad
                                {
                                    btc_id = ManejoNulos.ManageNullInteger(dr["btc_id"]),
                                    btc_usuario_id = ManejoNulos.ManageNullInteger(dr["btc_usuario_id"]),
                                    btc_accion = ManejoNulos.ManageNullStr(dr["btc_accion"]),
                                    btc_vista = ManejoNulos.ManageNullStr(dr["btc_vista"]),
                                    btc_fecha_reg = ManejoNulos.ManageNullDate(dr["btc_fecha_reg"]),
                                    btc_estado = ManejoNulos.ManageNullInteger(dr["btc_estado"]),
                                    btc_co_trab = ManejoNulos.ManageNullStr(dr["btc_co_trab"]),
                                    btc_ruta_pdf = ManejoNulos.ManageNullStr(dr["btc_ruta_pdf"]),
                                };

                                listaBitacora.Add(bitacora);
                            }
                        }
                    }
                    //Set Cliente y Sala
                    foreach (var m in listaBitacora)
                    {
                        SetUsuario(m, con);
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (lista: listaBitacora, error: error);
        }
        private void SetUsuario(BolBitacoraEntidad bitacora, NpgsqlConnection context)
        {
            var command = new NpgsqlCommand(@"SELECT usu_nombre, fk_persona
	                                        FROM seguridad.seg_usuario where usu_id=@p0;", context);
            command.Parameters.AddWithValue("@p0", bitacora.btc_usuario_id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    bitacora.Usuario = new Entidades.UsuarioEntidad()
                    {
                        usu_nombre = ManejoNulos.ManageNullStr(reader["usu_nombre"]),
                        fk_persona = ManejoNulos.ManageNullInteger(reader["fk_persona"]),
                    };
                }
            };
        }
    }
}