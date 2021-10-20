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
    public class BolEmpresaModel
    {
        string _conexion;
        public BolEmpresaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<BolEmpresaEntidad> lista, claseError error) BolEmpresaListarJson()
        {
            claseError error = new claseError();
            List<BolEmpresaEntidad> listaEmpresas = new List<BolEmpresaEntidad>();
            string consulta = @"SELECT emp_id, 
                        emp_co_ofisis, emp_nomb, emp_nomb_corto, 
                        emp_depa, emp_prov, emp_rucs, emp_pais, 
                        emp_firma_visible, emp_firma_img,emp_nom_rep_legal
	                        FROM intranet.bol_empresa;";
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
                                var empresa = new BolEmpresaEntidad
                                {
                                    emp_id= ManejoNulos.ManageNullInteger(dr["emp_id"]),
                                    emp_co_ofisis = ManejoNulos.ManageNullStr(dr["emp_co_ofisis"]),
                                    emp_nomb = ManejoNulos.ManageNullStr(dr["emp_nomb"]),
                                    emp_nomb_corto = ManejoNulos.ManageNullStr(dr["emp_nomb_corto"]),
                                    emp_depa = ManejoNulos.ManageNullStr(dr["emp_depa"]),
                                    emp_prov = ManejoNulos.ManageNullStr(dr["emp_prov"]),
                                    emp_rucs = ManejoNulos.ManageNullStr(dr["emp_rucs"]),
                                    emp_pais = ManejoNulos.ManageNullStr(dr["emp_pais"]),
                                    emp_firma_visible = ManejoNulos.ManageNullInteger(dr["emp_firma_visible"]),
                                    emp_firma_img = ManejoNulos.ManageNullStr(dr["emp_firma_img"]),
                                    emp_nom_rep_legal = ManejoNulos.ManageNullStr(dr["emp_nom_rep_legal"]),
                                };
                                listaEmpresas.Add(empresa);
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
            return (lista: listaEmpresas, error: error);
        }
        public (BolEmpresaEntidad empresa, claseError error) BolEmpresaIdObtenerJson(int emp_id)
        {
            BolEmpresaEntidad empresa = new BolEmpresaEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT emp_id, emp_co_ofisis, emp_nomb, emp_nomb_corto, 
                            emp_depa, emp_prov, emp_rucs, emp_pais, emp_firma_visible, emp_firma_img,emp_nom_rep_legal
	                            FROM intranet.bol_empresa where emp_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                empresa.emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]);
                                empresa.emp_co_ofisis = ManejoNulos.ManageNullStr(dr["emp_co_ofisis"]);
                                empresa.emp_nomb = ManejoNulos.ManageNullStr(dr["emp_nomb"]);
                                empresa.emp_nomb_corto = ManejoNulos.ManageNullStr(dr["emp_nomb_corto"]);
                                empresa.emp_depa = ManejoNulos.ManageNullStr(dr["emp_depa"]);
                                empresa.emp_prov = ManejoNulos.ManageNullStr(dr["emp_prov"]);
                                empresa.emp_rucs = ManejoNulos.ManageNullStr(dr["emp_rucs"]);
                                empresa.emp_pais = ManejoNulos.ManageNullStr(dr["emp_pais"]);
                                empresa.emp_firma_visible = ManejoNulos.ManageNullInteger(dr["emp_firma_visible"]);
                                empresa.emp_firma_img = ManejoNulos.ManageNullStr(dr["emp_firma_img"]);
                                empresa.emp_nom_rep_legal = ManejoNulos.ManageNullStr(dr["emp_nom_rep_legal"]);
                            }
                        }
                    }
                    //
                    SetDetalle(empresa, con);
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (empresa, error);
        }
        public (BolEmpresaEntidad empresa, claseError error) BolEmpresaObtenerxOfisisIdJson(string emp_co_ofisis)
        {
            BolEmpresaEntidad empresa = new BolEmpresaEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT emp_id, emp_co_ofisis, emp_nomb, emp_nomb_corto, 
                            emp_depa, emp_prov, emp_rucs, emp_pais, emp_firma_visible, emp_firma_img,emp_nom_rep_legal
	                            FROM intranet.bol_empresa where emp_co_ofisis=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_co_ofisis);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                empresa.emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]);
                                empresa.emp_co_ofisis = ManejoNulos.ManageNullStr(dr["emp_co_ofisis"]);
                                empresa.emp_nomb = ManejoNulos.ManageNullStr(dr["emp_nomb"]);
                                empresa.emp_nomb_corto = ManejoNulos.ManageNullStr(dr["emp_nomb_corto"]);
                                empresa.emp_depa = ManejoNulos.ManageNullStr(dr["emp_depa"]);
                                empresa.emp_prov = ManejoNulos.ManageNullStr(dr["emp_prov"]);
                                empresa.emp_rucs = ManejoNulos.ManageNullStr(dr["emp_rucs"]);
                                empresa.emp_pais = ManejoNulos.ManageNullStr(dr["emp_pais"]);
                                empresa.emp_firma_visible = ManejoNulos.ManageNullInteger(dr["emp_firma_visible"]);
                                empresa.emp_firma_img = ManejoNulos.ManageNullStr(dr["emp_firma_img"]);
                                empresa.emp_nom_rep_legal = ManejoNulos.ManageNullStr(dr["emp_nom_rep_legal"]);
                            }
                        }
                    }
                    //
                    SetDetalle(empresa, con);
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (empresa, error);
        }
        private void SetDetalle(BolEmpresaEntidad empresa, NpgsqlConnection context)
        {
            List<BolDetCertEmpresaEntidad> listaDetalle = new List<BolDetCertEmpresaEntidad>();
            var command = new NpgsqlCommand(@"SELECT det_id, det_ruta_cert, det_nomb_cert, 
                                            det_pass_cert, det_estado_cert, det_en_uso, det_empr_id
	                                            FROM intranet.bol_det_cert_empresa where det_empr_id=@p0;", context);
            command.Parameters.AddWithValue("@p0", empresa.emp_id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var detalle = new BolDetCertEmpresaEntidad()
                        {
                            det_id = ManejoNulos.ManageNullInteger(reader["det_id"]),
                            det_ruta_cert = ManejoNulos.ManageNullStr(reader["det_ruta_cert"]),
                            det_nomb_cert = ManejoNulos.ManageNullStr(reader["det_nomb_cert"]),
                            det_pass_cert = ManejoNulos.ManageNullStr(reader["det_pass_cert"]),
                            det_estado_cert = ManejoNulos.ManageNullInteger(reader["det_estado_cert"]),
                            det_en_uso = ManejoNulos.ManageNullInteger(reader["det_en_uso"]),
                            det_empr_id = ManejoNulos.ManageNullInteger(reader["det_empr_id"]),
                        };
                        listaDetalle.Add(detalle);
                    }
                    empresa.DetalleCerts = listaDetalle;
                }
            };
        }
        public (int idInsertado, claseError error) BolEmpresaInsertarJson(BolEmpresaEntidad empresa)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO intranet.bol_empresa(
	                                emp_co_ofisis, emp_nomb, emp_nomb_corto, 
                                    emp_depa, emp_prov, emp_rucs, emp_pais, emp_firma_visible, emp_firma_img,emp_nom_rep_legal)
	                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8,@p9)
                                returning emp_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(empresa.emp_co_ofisis));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(empresa.emp_nomb));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(empresa.emp_nomb_corto));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(empresa.emp_depa));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(empresa.emp_prov));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(empresa.emp_rucs));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(empresa.emp_pais));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(empresa.emp_firma_visible));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(empresa.emp_firma_img));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullStr(empresa.emp_nom_rep_legal));

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
    }
}