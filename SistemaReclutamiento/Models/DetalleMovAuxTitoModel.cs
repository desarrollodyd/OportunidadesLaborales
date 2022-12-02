using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class DetalleMovAuxTitoModel
    {
        string _conexionAdministrativo;
        public DetalleMovAuxTitoModel()
        {
            _conexionAdministrativo = ConfigurationManager.ConnectionStrings["conexionAdministrativo"].ConnectionString;
        }
        public List<DetalleMovAuxTitoEntidad> ListarDetalleMovAuxTitoAdministrativo(DateTime FechaIni,DateTime FechaFin,int CodSala)
        {
            List<DetalleMovAuxTitoEntidad> lista = new List<DetalleMovAuxTitoEntidad>();

            string consulta = @"select 
                                TitoCortesia,
                                TitoCortesiaNoDest,
                                TitoPromocion,
                                TitoPromocionNoDest,
		                        Estado,
                                FechaTicketIni,
                                FechaTicketFin	
                                from DetalleMovAuxTito
                                where TitoCortesia+TitoCortesiaNoDest+TitoPromocion+TitoPromocionNoDest>0
                                and 
                                ( FechaOperacion between @p0 and @p1
                                or FechaOperacionIni between @p0 and @p1)
                                and CodSala=@p2";
            try
            {
                using (var con = new SqlConnection(_conexionAdministrativo))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", FechaIni);
                    query.Parameters.AddWithValue("@p1", FechaFin);
                    query.Parameters.AddWithValue("@p2", CodSala);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var detalleMovAuxTitoEntidad = new DetalleMovAuxTitoEntidad()
                                {
                                    TitoCortesia = ManejoNulos.ManageNullDouble(dr["TitoCortesia"]),
                                    TitoCortesiaNoDest = ManejoNulos.ManageNullDouble(dr["TitoCortesiaNoDest"]),
                                    TitoPromocion = ManejoNulos.ManageNullDouble(dr["TitoPromocion"]),
                                    TitoPromocionNoDest = ManejoNulos.ManageNullDouble(dr["TitoPromocionNoDest"]),
                                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                    FechaTicketIni = ManejoNulos.ManageNullDate(dr["FechaTicketIni"]),
                                    FechaTicketFin = ManejoNulos.ManageNullDate(dr["FechaTicketFin"]),
                                };
                                lista.Add(detalleMovAuxTitoEntidad);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lista = new List<DetalleMovAuxTitoEntidad>();
            }
            return lista;
        }
    }
}