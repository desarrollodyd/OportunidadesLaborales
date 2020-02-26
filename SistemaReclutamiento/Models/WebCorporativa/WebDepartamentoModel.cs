using Npgsql;
using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.WebCorporativa
{
    public class WebDepartamentoModel
    {
        string _conexion;
        public WebDepartamentoModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<WebDepartamentoEntidad> lista, claseError error) WebDepartamentoListarJson()
        {
            List<WebDepartamentoEntidad> lista = new List<WebDepartamentoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT dep_id, dep_nombre, dep_imagen, dep_imagen_detalle
	                                FROM web_corporativa.web_departamento order by dep_nombre;";
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
                                var Departamento = new WebDepartamentoEntidad
                                {

                                    dep_id = ManejoNulos.ManageNullInteger(dr["dep_id"]),
                                    dep_nombre = ManejoNulos.ManageNullStr(dr["dep_nombre"]),
                                    dep_imagen = ManejoNulos.ManageNullStr(dr["dep_imagen"]),
                                    dep_imagen_detalle = ManejoNulos.ManageNullStr(dr["dep_imagen_detalle"]),
                                };

                                lista.Add(Departamento);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista, error: error);
        }

        public (WebDepartamentoEntidad departamento, claseError error) WebDepartamentoIdObtenerJson(int dep_id)
        {
            WebDepartamentoEntidad departamento = new WebDepartamentoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT dep_id, dep_nombre, dep_imagen, dep_imagen_detalle
	                                FROM web_corporativa.web_departamento where dep_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", dep_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                departamento.dep_id = ManejoNulos.ManageNullInteger(dr["dep_id"]);
                                departamento.dep_nombre = ManejoNulos.ManageNullStr(dr["dep_nombre"]);
                                departamento.dep_imagen = ManejoNulos.ManageNullStr(dr["dep_imagen"]);
                                departamento.dep_imagen_detalle = ManejoNulos.ManageNullStr(dr["dep_imagen_detalle"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (departamento: departamento, error: error);
        }

        public (int idDeptatamentoInsertado, claseError error) WebDepartamentoInsertarJson(WebDepartamentoEntidad departamento)
        {
            //bool response = false;
            int idDeptatamentoInsertado = 0;
            string consulta = @"INSERT INTO web_corporativa.web_departamento(
	                                 dep_nombre, dep_imagen, dep_imagen_detalle)
	                                VALUES (@p0, @p1, @p2)
                                returning dep_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(departamento.dep_nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(departamento.dep_imagen));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(departamento.dep_imagen_detalle));
                    idDeptatamentoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idDeptatamentoInsertado: idDeptatamentoInsertado, error: error);
        }
        public (bool DepartamentoEditado, claseError error) WebDepartamentoEditarJson(WebDepartamentoEntidad departamento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE web_corporativa.web_departamento
	                            SET  dep_nombre=@p0, dep_imagen=@p1, dep_imagen_detalle=@p2
	                            WHERE dep_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(departamento.dep_nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(departamento.dep_imagen));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(departamento.dep_imagen_detalle));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(departamento.dep_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (DepartamentoEditado: response, error: error);
        }
        public (bool DepartamentoEliminado, claseError error) WebDepartamentoEliminarJson(int dep_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM web_corporativa.web_departamento
	                                WHERE dep_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(dep_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (DepartamentoEliminado: response, error: error);
        }

    }
}