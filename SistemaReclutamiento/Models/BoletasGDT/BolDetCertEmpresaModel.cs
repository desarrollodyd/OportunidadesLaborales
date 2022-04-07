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
    public class BolDetCertEmpresaModel
    {
        string _conexion;
        public BolDetCertEmpresaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<BolDetCertEmpresaEntidad> lista, claseError error) BolDetCertEmpresaListarxEmpresaIdJson(int det_empr_id)
        {
            claseError error = new claseError();
            List<BolDetCertEmpresaEntidad> listaDetalle = new List<BolDetCertEmpresaEntidad>();
            string consulta = @"SELECT det_id, det_ruta_cert, det_nomb_cert, 
                                det_pass_cert, det_estado_cert, det_en_uso, det_empr_id,det_fecha_reg
	                                FROM intranet.bol_det_cert_empresa where det_empr_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", det_empr_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var detalle = new BolDetCertEmpresaEntidad
                                {
                                    det_id = ManejoNulos.ManageNullInteger(dr["det_id"]),
                                    det_ruta_cert = ManejoNulos.ManageNullStr(dr["det_ruta_cert"]),
                                    det_nomb_cert = ManejoNulos.ManageNullStr(dr["det_nomb_cert"]),
                                    det_pass_cert = ManejoNulos.ManageNullStr(dr["det_pass_cert"]),
                                    det_estado_cert = ManejoNulos.ManageNullInteger(dr["det_estado_cert"]),
                                    det_en_uso = ManejoNulos.ManageNullInteger(dr["det_en_uso"]),
                                    det_empr_id = ManejoNulos.ManageNullInteger(dr["det_empr_id"]),
                                    det_fecha_reg = ManejoNulos.ManageNullDate(dr["det_fecha_reg"]),
                                };
                                listaDetalle.Add(detalle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (lista: listaDetalle, error: error);
        }
        public (BolDetCertEmpresaEntidad detalle, claseError error) BolDetCertEmpresaIdObtenerJson(int det_id)
        {
            BolDetCertEmpresaEntidad detalle = new BolDetCertEmpresaEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT det_id, det_ruta_cert, det_nomb_cert, 
                                det_pass_cert, det_estado_cert, det_en_uso, det_empr_id,det_fecha_reg
	                                FROM intranet.bol_det_cert_empresa where det_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", det_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                detalle.det_id = ManejoNulos.ManageNullInteger(dr["det_id"]);
                                detalle.det_ruta_cert = ManejoNulos.ManageNullStr(dr["det_ruta_cert"]);
                                detalle.det_nomb_cert = ManejoNulos.ManageNullStr(dr["det_nomb_cert"]);
                                detalle.det_pass_cert = ManejoNulos.ManageNullStr(dr["det_pass_cert"]);
                                detalle.det_estado_cert = ManejoNulos.ManageNullInteger(dr["det_estado_cert"]);
                                detalle.det_en_uso = ManejoNulos.ManageNullInteger(dr["det_en_uso"]);
                                detalle.det_empr_id = ManejoNulos.ManageNullInteger(dr["det_empr_id"]);
                                detalle.det_fecha_reg = ManejoNulos.ManageNullDate(dr["det_fecha_reg"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (detalle, error);
        }
        public (int idInsertado, claseError error) BolDetCertEmpresaInsertarJson(BolDetCertEmpresaEntidad detalle)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO intranet.bol_det_cert_empresa(
	                            det_ruta_cert, det_nomb_cert, det_pass_cert, det_estado_cert, det_en_uso, det_empr_id,det_fecha_reg)
	                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5,@p6)
                                returning det_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(detalle.det_ruta_cert));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(detalle.det_nomb_cert));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(detalle.det_pass_cert));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(detalle.det_estado_cert));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(detalle.det_en_uso));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(detalle.det_empr_id));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(detalle.det_fecha_reg));

                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }
        public (bool editado, claseError error) BolDetCertEmpresaEditarUsoJson(BolDetCertEmpresaEntidad detalle)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.bol_det_cert_empresa
	                                SET det_en_uso=@p0
	                                WHERE det_id=@p1 ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(detalle.det_en_uso));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(detalle.det_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (editado: response, error: error);
        }
        public (bool editado, claseError error) BolDetCertEmpresaQuitarUsoJson(BolDetCertEmpresaEntidad detalle)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.bol_det_cert_empresa
	                                SET det_en_uso=0
	                                WHERE det_empr_id=@p1 ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(detalle.det_empr_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (editado: response, error: error);
        }
        public (bool editado, claseError error) BolDetCertEmpresaEliminarJson(BolDetCertEmpresaEntidad detalle)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"delete from intranet.bol_det_cert_empresa
	                                WHERE det_id=@p1 ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(detalle.det_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (editado: response, error: error);
        }
    }
}