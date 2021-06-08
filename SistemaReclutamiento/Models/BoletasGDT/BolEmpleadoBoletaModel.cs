using Npgsql;
using SistemaReclutamiento.Entidades.BoletasGDT;
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
            string consulta = @"INSERT INTO boletas_gdt.bol_empleado_boleta(
	emp_co_trab, emp_co_empr, emp_anio, emp_periodo, emp_quincena, emp_ruta_pdf, emp_enviado, emp_descargado, emp_fecha_reg, emp_no_trab, emp_apel_pat, emp_apel_mat, emp_direc_mail, emp_nro_cel, emp_tipo_doc)
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
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: totalInsertados, error: error);
        }
    }
}