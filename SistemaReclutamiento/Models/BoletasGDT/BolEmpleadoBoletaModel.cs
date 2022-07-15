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
    public class BolEmpleadoBoletaModel
    {
        string _conexion;
        public BolEmpleadoBoletaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (int totalInsertados, claseError error) BoolEmpleadoBoletaInsertarMasivoJson(string values)
        {
            //bool response = false;
            int totalInsertados = 0;
            string consulta = @"INSERT INTO intranet.bol_empleado_boleta(
	emp_co_trab, emp_co_empr, emp_anio, emp_periodo, emp_ruta_pdf, emp_enviado, emp_descargado, emp_fecha_reg, emp_no_trab, emp_apel_pat, emp_apel_mat, emp_direc_mail, emp_nro_cel, emp_tipo_doc)
	VALUES " +values;
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);

                    totalInsertados = query.ExecuteNonQuery();
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (totalInsertados: totalInsertados, error: error);
        }
        public (List<BolEmpleadoBoletaEntidad> lista, claseError error) BoolEmpleadoBoletaListarJson(string emp_co_empr, string anio, string periodo)
        {
            claseError error = new claseError();
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            string consulta = @"SELECT emp_co_trab, emp_co_empr, emp_anio, emp_periodo, 
                            emp_ruta_pdf, emp_enviado, emp_descargado, emp_fecha_act, emp_fecha_reg, emp_no_trab, 
                            emp_apel_pat, emp_apel_mat, emp_direc_mail, emp_nro_cel, emp_tipo_doc
	                            FROM intranet.bol_empleado_boleta
                                where emp_co_empr=@p0
	                                and emp_anio=@p1
	                                and emp_periodo=@p2;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_co_empr);
                    query.Parameters.AddWithValue("@p1", anio);
                    query.Parameters.AddWithValue("@p2", periodo);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var boleta = new BolEmpleadoBoletaEntidad
                                {
                                    emp_co_trab = ManejoNulos.ManageNullStr(dr["emp_co_trab"]),
                                    emp_co_empr = ManejoNulos.ManageNullStr(dr["emp_co_empr"]),
                                    emp_anio = ManejoNulos.ManageNullStr(dr["emp_anio"]),
                                    emp_periodo = ManejoNulos.ManageNullStr(dr["emp_periodo"]),
                                    emp_ruta_pdf = ManejoNulos.ManageNullStr(dr["emp_ruta_pdf"]),
                                    emp_enviado = ManejoNulos.ManageNullInteger(dr["emp_enviado"]),
                                    emp_descargado = ManejoNulos.ManageNullInteger(dr["emp_descargado"]),
                                    emp_fecha_act = ManejoNulos.ManageNullDate(dr["emp_fecha_act"]),
                                    emp_fecha_reg = ManejoNulos.ManageNullDate(dr["emp_fecha_reg"]),
                                    emp_no_trab = ManejoNulos.ManageNullStr(dr["emp_no_trab"]),
                                    emp_apel_pat = ManejoNulos.ManageNullStr(dr["emp_apel_pat"]),
                                    emp_apel_mat = ManejoNulos.ManageNullStr(dr["emp_apel_mat"]),
                                    emp_direc_mail = ManejoNulos.ManageNullStr(dr["emp_direc_mail"]),
                                    emp_nro_cel = ManejoNulos.ManageNullStr(dr["emp_nro_cel"]),
                                    emp_tipo_doc = ManejoNulos.ManageNullStr(dr["emp_tipo_doc"]),
                                };

                                listaBoletas.Add(boleta);
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
            return (lista: listaBoletas, error: error);
        }
        public (bool eliminado, claseError error) BoolEmpleadoBoletaEliminarMasivoJson(string emp_co_empr, string anio, string periodo)
        {
            claseError error = new claseError();
            bool eliminado = false;
            string consulta = @"delete from intranet.bol_empleado_boleta where emp_ruta_pdf in(
                                SELECT emp_ruta_pdf
	                                FROM intranet.bol_empleado_boleta
	                                where emp_co_empr=@p0
		                                and emp_anio=@p1
		                                and emp_periodo=@p2
                                )";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(emp_co_empr));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(anio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(periodo));
                    query.ExecuteNonQuery();
                    eliminado = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (eliminado: eliminado, error: error);
        }
        public (bool editado, claseError error) BoolEmpleadoBoletaEditarEnvioJson(string emp_ruta_pdf,DateTime fechaAct)
        {
            claseError error = new claseError();
            bool editado = false;
            string consulta = @"UPDATE intranet.bol_empleado_boleta
	                            SET emp_enviado=emp_enviado+1,emp_fecha_act=@p1
	                            WHERE emp_ruta_pdf=@p2;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(fechaAct));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(emp_ruta_pdf));
                    query.ExecuteNonQuery();
                    editado = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (editado: editado, error: error);
        }
        public (List<BolEmpleadoBoletaEntidad> lista, claseError error) BoolEmpleadoBoletaListarxEmpleadoJson(string emp_co_empr, string anio, string periodo,string emp_co_trab)
        {
            claseError error = new claseError();
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            string consulta = @"SELECT emp_co_trab, emp_co_empr, emp_anio, emp_periodo, 
                            emp_ruta_pdf, emp_enviado, emp_descargado, emp_fecha_act, emp_fecha_reg, emp_no_trab, 
                            emp_apel_pat, emp_apel_mat, emp_direc_mail, emp_nro_cel, emp_tipo_doc
	                            FROM intranet.bol_empleado_boleta
                                where emp_co_empr=@p0
	                                and emp_anio=@p1
	                                and emp_periodo=@p2 and emp_co_trab=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_co_empr);
                    query.Parameters.AddWithValue("@p1", anio);
                    query.Parameters.AddWithValue("@p2", periodo);
                    query.Parameters.AddWithValue("@p3", emp_co_trab);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var boleta = new BolEmpleadoBoletaEntidad
                                {
                                    emp_co_trab = ManejoNulos.ManageNullStr(dr["emp_co_trab"]),
                                    emp_co_empr = ManejoNulos.ManageNullStr(dr["emp_co_empr"]),
                                    emp_anio = ManejoNulos.ManageNullStr(dr["emp_anio"]),
                                    emp_periodo = ManejoNulos.ManageNullStr(dr["emp_periodo"]),
                                    emp_ruta_pdf = ManejoNulos.ManageNullStr(dr["emp_ruta_pdf"]),
                                    emp_enviado = ManejoNulos.ManageNullInteger(dr["emp_enviado"]),
                                    emp_descargado = ManejoNulos.ManageNullInteger(dr["emp_descargado"]),
                                    emp_fecha_act = ManejoNulos.ManageNullDate(dr["emp_fecha_act"]),
                                    emp_fecha_reg = ManejoNulos.ManageNullDate(dr["emp_fecha_reg"]),
                                    emp_no_trab = ManejoNulos.ManageNullStr(dr["emp_no_trab"]),
                                    emp_apel_pat = ManejoNulos.ManageNullStr(dr["emp_apel_pat"]),
                                    emp_apel_mat = ManejoNulos.ManageNullStr(dr["emp_apel_mat"]),
                                    emp_direc_mail = ManejoNulos.ManageNullStr(dr["emp_direc_mail"]),
                                    emp_nro_cel = ManejoNulos.ManageNullStr(dr["emp_nro_cel"]),
                                    emp_tipo_doc = ManejoNulos.ManageNullStr(dr["emp_tipo_doc"]),
                                };

                                listaBoletas.Add(boleta);
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
            return (lista: listaBoletas, error: error);
        }
        public (List<BolEmpleadoBoletaEntidad> lista, claseError error) BoolEmpleadoBoletaListarxEmpleadoFechasJson(string emp_co_trab, string stringAnio, string stringPeriodo)
        {
            claseError error = new claseError();
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            string consulta = @"SELECT emp_co_trab, emp_co_empr, emp_anio, emp_periodo, 
                            emp_ruta_pdf, emp_enviado, emp_descargado, emp_fecha_act, emp_fecha_reg, emp_no_trab, 
                            emp_apel_pat, emp_apel_mat, emp_direc_mail, emp_nro_cel, emp_tipo_doc
	                            FROM intranet.bol_empleado_boleta
                                where emp_co_trab=@p0
	                                and emp_anio in(" + stringAnio + ")" +
                                    "and emp_periodo in (" + stringPeriodo + ");";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_co_trab);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var boleta = new BolEmpleadoBoletaEntidad
                                {
                                    emp_co_trab = ManejoNulos.ManageNullStr(dr["emp_co_trab"]),
                                    emp_co_empr = ManejoNulos.ManageNullStr(dr["emp_co_empr"]),
                                    emp_anio = ManejoNulos.ManageNullStr(dr["emp_anio"]),
                                    emp_periodo = ManejoNulos.ManageNullStr(dr["emp_periodo"]),
                                    emp_ruta_pdf = ManejoNulos.ManageNullStr(dr["emp_ruta_pdf"]),
                                    emp_enviado = ManejoNulos.ManageNullInteger(dr["emp_enviado"]),
                                    emp_descargado = ManejoNulos.ManageNullInteger(dr["emp_descargado"]),
                                    emp_fecha_act = ManejoNulos.ManageNullDate(dr["emp_fecha_act"]),
                                    emp_fecha_reg = ManejoNulos.ManageNullDate(dr["emp_fecha_reg"]),
                                    emp_no_trab = ManejoNulos.ManageNullStr(dr["emp_no_trab"]),
                                    emp_apel_pat = ManejoNulos.ManageNullStr(dr["emp_apel_pat"]),
                                    emp_apel_mat = ManejoNulos.ManageNullStr(dr["emp_apel_mat"]),
                                    emp_direc_mail = ManejoNulos.ManageNullStr(dr["emp_direc_mail"]),
                                    emp_nro_cel = ManejoNulos.ManageNullStr(dr["emp_nro_cel"]),
                                    emp_tipo_doc = ManejoNulos.ManageNullStr(dr["emp_tipo_doc"]),
                                };
                                listaBoletas.Add(boleta);
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
            return (lista: listaBoletas, error: error);
        }
        public (List<BolEmpleadoBoletaEntidad> lista, claseError error) BoolEmpleadoBoletaListarxEmpleadoEmpresaFechasJson(string emp_co_empr,string emp_co_trab, string stringAnio)
        {
            claseError error = new claseError();
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            string consulta = @"SELECT emp_co_trab, emp_co_empr, emp_anio, emp_periodo, 
                            emp_ruta_pdf, emp_enviado, emp_descargado, emp_fecha_act, emp_fecha_reg, emp_no_trab, 
                            emp_apel_pat, emp_apel_mat, emp_direc_mail, emp_nro_cel, emp_tipo_doc
	                            FROM intranet.bol_empleado_boleta
                                where emp_co_trab=@p0 and emp_co_empr=@p1
	                                and emp_anio in(" + stringAnio + ");";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emp_co_trab);
                    query.Parameters.AddWithValue("@p1", emp_co_empr);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var boleta = new BolEmpleadoBoletaEntidad
                                {
                                    emp_co_trab = ManejoNulos.ManageNullStr(dr["emp_co_trab"]),
                                    emp_co_empr = ManejoNulos.ManageNullStr(dr["emp_co_empr"]),
                                    emp_anio = ManejoNulos.ManageNullStr(dr["emp_anio"]),
                                    emp_periodo = ManejoNulos.ManageNullStr(dr["emp_periodo"]),
                                    emp_ruta_pdf = ManejoNulos.ManageNullStr(dr["emp_ruta_pdf"]),
                                    emp_enviado = ManejoNulos.ManageNullInteger(dr["emp_enviado"]),
                                    emp_descargado = ManejoNulos.ManageNullInteger(dr["emp_descargado"]),
                                    emp_fecha_act = ManejoNulos.ManageNullDate(dr["emp_fecha_act"]),
                                    emp_fecha_reg = ManejoNulos.ManageNullDate(dr["emp_fecha_reg"]),
                                    emp_no_trab = ManejoNulos.ManageNullStr(dr["emp_no_trab"]),
                                    emp_apel_pat = ManejoNulos.ManageNullStr(dr["emp_apel_pat"]),
                                    emp_apel_mat = ManejoNulos.ManageNullStr(dr["emp_apel_mat"]),
                                    emp_direc_mail = ManejoNulos.ManageNullStr(dr["emp_direc_mail"]),
                                    emp_nro_cel = ManejoNulos.ManageNullStr(dr["emp_nro_cel"]),
                                    emp_tipo_doc = ManejoNulos.ManageNullStr(dr["emp_tipo_doc"]),
                                };
                                listaBoletas.Add(boleta);
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
            return (lista: listaBoletas, error: error);
        }
        public int BoolEmpleadoBoletaInsertarJson(BolEmpleadoBoletaEntidad empleado)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO intranet.bol_empleado_boleta
(emp_co_trab, 
emp_co_empr, 
emp_anio, 
emp_periodo, 
emp_ruta_pdf, 
emp_enviado, 
emp_descargado, 
emp_fecha_reg, 
emp_no_trab, 
emp_apel_pat, 
emp_apel_mat, 
emp_direc_mail, 
emp_nro_cel, 
emp_tipo_doc)
	VALUES 
(
@emp_co_trab, 
@emp_co_empr, 
@emp_anio, 
@emp_periodo, 
@emp_ruta_pdf, 
@emp_enviado, 
@emp_descargado, 
@emp_fecha_reg, 
@emp_no_trab, 
@emp_apel_pat, 
@emp_apel_mat, 
@emp_direc_mail, 
@emp_nro_cel, 
@emp_tipo_doc)
returning btc_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@emp_co_trab", ManejoNulos.ManageNullStr(empleado.emp_co_trab));
                    query.Parameters.AddWithValue("@emp_co_empr", ManejoNulos.ManageNullStr(empleado.emp_co_empr));
                    query.Parameters.AddWithValue("@emp_anio", ManejoNulos.ManageNullStr(empleado.emp_anio));
                    query.Parameters.AddWithValue("@emp_periodo", ManejoNulos.ManageNullStr(empleado.emp_periodo));
                    query.Parameters.AddWithValue("@emp_ruta_pdf", ManejoNulos.ManageNullStr(empleado.emp_ruta_pdf));
                    query.Parameters.AddWithValue("@emp_enviado", ManejoNulos.ManageNullInteger(empleado.emp_enviado));
                    query.Parameters.AddWithValue("@emp_descargado", ManejoNulos.ManageNullInteger(empleado.emp_descargado));
                    query.Parameters.AddWithValue("@emp_fecha_reg", ManejoNulos.ManageNullDate(empleado.emp_fecha_reg));
                    query.Parameters.AddWithValue("@emp_no_trab", ManejoNulos.ManageNullStr(empleado.emp_no_trab));
                    query.Parameters.AddWithValue("@emp_apel_pat", ManejoNulos.ManageNullStr(empleado.emp_apel_pat));
                    query.Parameters.AddWithValue("@emp_apel_mat", ManejoNulos.ManageNullStr(empleado.emp_apel_mat));
                    query.Parameters.AddWithValue("@emp_direc_mail", ManejoNulos.ManageNullStr(empleado.emp_direc_mail));
                    query.Parameters.AddWithValue("@emp_nro_cel", ManejoNulos.ManageNullStr(empleado.emp_nro_cel));
                    query.Parameters.AddWithValue("@emp_tipo_doc", ManejoNulos.ManageNullStr(empleado.emp_tipo_doc));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                idInsertado = 0;
            }
            return idInsertado;
        }
    }
}