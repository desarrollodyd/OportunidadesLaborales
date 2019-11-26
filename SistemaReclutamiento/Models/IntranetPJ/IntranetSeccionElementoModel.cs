using Npgsql;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.IntranetPJ
{
    public class IntranetSeccionElementoModel
    {
        //string _conexion;
        //public IntranetSeccionElementoModel()
        //{
        //    _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        //}
        //public (List<IntranetSeccionElementoEntidad> intranetSeccionElementoLista, claseError error) IntranetSeccionElementoListarJson()
        //{
        //    List<IntranetSeccionElementoEntidad> lista = new List<IntranetSeccionElementoEntidad>();
        //    claseError error = new claseError();
        //    string consulta = @"SELECT sec_id, sec_orden, sec_estado, fk_menu
	       //                         FROM intranet.int_SeccionElemento
        //                                order by sec_orden;";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            using (var dr = query.ExecuteReader())
        //            {
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        var SeccionElemento = new IntranetSeccionElementoEntidad
        //                        {

        //                            sec_id = ManejoNulos.ManageNullInteger(dr["sec_id"]),
        //                            sec_orden = ManejoNulos.ManageNullInteger(dr["sec_orden"]),
        //                            sec_estado = ManejoNulos.ManageNullStr(dr["sec_estado"]),
        //                            fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
        //                        };

        //                        lista.Add(SeccionElemento);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Key = ex.Data.Count.ToString();
        //        error.Value = ex.Message;
        //    }
        //    return (intranetSeccionElementoLista: lista, error: error);
        //}
        //public (IntranetSeccionElementoEntidad intranetSeccionElemento, claseError error) IntranetSeccionElementoIdObtenerJson(int sec_id)
        //{
        //    IntranetSeccionElementoEntidad intranetSeccionElemento = new IntranetSeccionElementoEntidad();
        //    claseError error = new claseError();
        //    string consulta = @"SELECT sec_id, sec_orden, sec_estado, fk_menu
	       //                         FROM intranet.int_SeccionElemento where sec_id=@p0;";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", sec_id);
        //            using (var dr = query.ExecuteReader())
        //            {
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        intranetSeccionElemento.sec_id = ManejoNulos.ManageNullInteger(dr["sec_id"]);
        //                        intranetSeccionElemento.sec_orden = ManejoNulos.ManageNullInteger(dr["sec_orden"]);
        //                        intranetSeccionElemento.sec_estado = ManejoNulos.ManageNullStr(dr["sec_estado"]);
        //                        intranetSeccionElemento.fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Key = ex.Data.Count.ToString();
        //        error.Value = ex.Message;
        //    }
        //    return (intranetSeccionElemento: intranetSeccionElemento, error: error);
        //}
        //public (int idIntranetSeccionElementoInsertado, claseError error) IntranetSeccionElementoInsertarJson(IntranetSeccionElementoEntidad intranetSeccionElemento)
        //{
        //    //bool response = false;
        //    int idIntranetSeccionElementoInsertado = 0;
        //    string consulta = @"
        //                        INSERT INTO intranet.int_SeccionElemento(
	       //                     sec_orden, sec_estado, fk_menu)
	       //                     VALUES ( @p0, @p1, @p2)
        //                        returning sec_id;";
        //    claseError error = new claseError();
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetSeccionElemento.sec_orden));
        //            query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSeccionElemento.sec_estado));
        //            query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(intranetSeccionElemento.fk_menu));
        //            idIntranetSeccionElementoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
        //            //query.ExecuteNonQuery();
        //            //response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Key = ex.Data.Count.ToString();
        //        error.Value = ex.Message;
        //    }
        //    return (idIntranetSeccionElementoInsertado: idIntranetSeccionElementoInsertado, error: error);
        //}

        //public (bool intranetSeccionElementoEditado, claseError error) IntranetSeccionElementoEditarJson(IntranetSeccionElementoEntidad intranetSeccionElemento)
        //{
        //    claseError error = new claseError();
        //    bool response = false;
        //    string consulta = @"UPDATE UPDATE intranet.int_SeccionElemento
	       //                 SET  sec_orden=@p0, sec_estado=@p1, fk_menu=@p2
	       //                 WHERE sec_id=@p3;";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSeccionElemento.sec_orden));
        //            query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetSeccionElemento.sec_estado));
        //            query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetSeccionElemento.fk_menu));
        //            query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetSeccionElemento.sec_id));
        //            query.ExecuteNonQuery();
        //            response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Key = ex.Data.Count.ToString();
        //        error.Value = ex.Message;
        //    }
        //    return (intranetSeccionElementoEditado: response, error: error);
        //}
        //public (bool intranetSeccionElementoEliminado, claseError error) IntranetSeccionElementoEliminarJson(int sec_id)
        //{
        //    bool response = false;
        //    string consulta = @"DELETE FROM intranet.int_SeccionElemento
	       //                         WHERE sec_id=@p0;";
        //    claseError error = new claseError();
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();

        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(sec_id));
        //            query.ExecuteNonQuery();
        //            response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Key = ex.Data.Count.ToString();
        //        error.Value = ex.Message;
        //    }

        //    return (intranetSeccionElementoEliminado: response, error: error);
        //}
    }
}