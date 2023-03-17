using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;

namespace SistemaReclutamiento.Models {
    public class MaquinaDetalleModel {

        string _conexionAdministrativo;
        public MaquinaDetalleModel() {
            _conexionAdministrativo = ConfigurationManager.ConnectionStrings["conexionAdministrativo"].ConnectionString;
        }
        public MaquinaDetalleEntidad ListarMaquinaDetalleAdministrativo(string codMaquina) {
            MaquinaDetalleEntidad item = new MaquinaDetalleEntidad();

            string consulta = @"SELECT 
		                            maq.CodMaquina,
		                            maq.CodLinea,
		                            maq.CodJuego,
		                            maq.CodSala,
		                            maq.CodModeloMaquina,
		                            mom.CodMarcaMaquina,
		                            maq.CodContrato,
		                            maq.CodFicha,
		                            maq.CodMaquinaLey,
		                            lin.Nombre as NombreLinea,
		                            maq.NroSerie as NroSerie,
		                            jue.Nombre as NombreJuego,
		                            sal.Nombre as NombreSala,
		                            mom.Nombre as NombreModeloMaquina,
		                            con.Descripcion as DescripcionContrato,
		                            fic.Nombre as NombreFicha,
		                            mar.Nombre as NombreMarcaMaquina, 
		                            maq.Token as Token
                                FROM [BD_S3K_ADMINISTRATIVO_DATA].[dbo].[Maquina] maq
                                INNER JOIN Linea lin ON lin.CodLinea = maq.CodLinea
                                INNER JOIN Juego jue ON jue.CodJuego = maq.CodJuego
                                INNER JOIN Sala sal ON sal.CodSala = maq.CodSala
                                INNER JOIN ModeloMaquina mom ON mom.CodModeloMaquina = maq.CodModeloMaquina
                                INNER JOIN MarcaMaquina mar ON mar.CodMarcaMaquina = mom.CodMarcaMaquina
                                INNER JOIN Contrato con ON con.CodContrato = maq.CodContrato
                                INNER JOIN Ficha fic ON fic.CodFicha = maq.CodFicha
                                WHERE CodMaquina=@p0";
            try {
                using(var con = new SqlConnection(_conexionAdministrativo)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codMaquina);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {

                                item.CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]);
                                item.CodLinea = ManejoNulos.ManageNullInteger(dr["CodLinea"]);
                                item.CodJuego = ManejoNulos.ManageNullInteger(dr["CodJuego"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.CodModeloMaquina = ManejoNulos.ManageNullInteger(dr["CodModeloMaquina"]);
                                item.CodMarcaMaquina = ManejoNulos.ManageNullInteger(dr["CodMarcaMaquina"]);
                                item.CodContrato = ManejoNulos.ManageNullInteger(dr["CodContrato"]);
                                item.CodFicha = ManejoNulos.ManageNullInteger(dr["CodFicha"]);
                                item.CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]);
                                item.NombreLinea = ManejoNulos.ManageNullStr(dr["NombreLinea"]);
                                item.NroSerie = ManejoNulos.ManageNullStr(dr["NroSerie"]);
                                item.NombreJuego = ManejoNulos.ManageNullStr(dr["NombreJuego"]);
                                item.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                item.NombreModeloMaquina = ManejoNulos.ManageNullStr(dr["NombreModeloMaquina"]);
                                item.DescripcionContrato = ManejoNulos.ManageNullStr(dr["DescripcionContrato"]);
                                item.NombreFicha = ManejoNulos.ManageNullStr(dr["NombreFicha"]);
                                item.NombreMarcaMaquina = ManejoNulos.ManageNullStr(dr["NombreMarcaMaquina"]);
                                item.Token = ManejoNulos.ManageNullDouble(dr["Token"]);

                            }
                        }
                    }
                }

            } catch(Exception ex) {
                item = new MaquinaDetalleEntidad();
            }
            return item;
        }
        public List<MaquinaDetalleEntidad> ListarMaquinasAdministrativo() {
            List<MaquinaDetalleEntidad> lista = new List<MaquinaDetalleEntidad>();

            string consulta = @"SELECT 
		                            maq.CodMaquina,
		                            maq.CodLinea,
		                            maq.CodJuego,
		                            maq.CodSala,
		                            maq.CodModeloMaquina,
		                            mom.CodMarcaMaquina,
		                            maq.CodContrato,
		                            maq.CodFicha,
		                            maq.CodMaquinaLey,
		                            lin.Nombre as NombreLinea,
		                            maq.NroSerie as NroSerie,
		                            jue.Nombre as NombreJuego,
		                            sal.Nombre as NombreSala,
		                            mom.Nombre as NombreModeloMaquina,
		                            con.Descripcion as DescripcionContrato,
		                            fic.Nombre as NombreFicha,
		                            mar.Nombre as NombreMarcaMaquina, 
		                            maq.Token as Token
                                FROM [BD_S3K_ADMINISTRATIVO_DATA].[dbo].[Maquina] maq
                                INNER JOIN Linea lin ON lin.CodLinea = maq.CodLinea
                                INNER JOIN Juego jue ON jue.CodJuego = maq.CodJuego
                                INNER JOIN Sala sal ON sal.CodSala = maq.CodSala
                                INNER JOIN ModeloMaquina mom ON mom.CodModeloMaquina = maq.CodModeloMaquina
                                INNER JOIN MarcaMaquina mar ON mar.CodMarcaMaquina = mom.CodMarcaMaquina
                                INNER JOIN Contrato con ON con.CodContrato = maq.CodContrato
                                INNER JOIN Ficha fic ON fic.CodFicha = maq.CodFicha ORDER BY maq.CodMaquinaLey ASC";
            try {
                using(var con = new SqlConnection(_conexionAdministrativo)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var maquina = new MaquinaDetalleEntidad() {
                                    CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                    CodLinea = ManejoNulos.ManageNullInteger(dr["CodLinea"]),
                                    CodJuego = ManejoNulos.ManageNullInteger(dr["CodJuego"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodModeloMaquina = ManejoNulos.ManageNullInteger(dr["CodModeloMaquina"]),
                                    CodMarcaMaquina = ManejoNulos.ManageNullInteger(dr["CodMarcaMaquina"]),
                                    CodContrato = ManejoNulos.ManageNullInteger(dr["CodContrato"]),
                                    CodFicha = ManejoNulos.ManageNullInteger(dr["CodFicha"]),
                                    CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                    NombreLinea = ManejoNulos.ManageNullStr(dr["NombreLinea"]),
                                    NroSerie = ManejoNulos.ManageNullStr(dr["NroSerie"]),
                                    NombreJuego = ManejoNulos.ManageNullStr(dr["NombreJuego"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    NombreModeloMaquina = ManejoNulos.ManageNullStr(dr["NombreModeloMaquina"]),
                                    DescripcionContrato = ManejoNulos.ManageNullStr(dr["DescripcionContrato"]),
                                    NombreFicha = ManejoNulos.ManageNullStr(dr["NombreFicha"]),
                                    NombreMarcaMaquina = ManejoNulos.ManageNullStr(dr["NombreMarcaMaquina"]),
                                    Token = ManejoNulos.ManageNullDouble(dr["Token"]),
                                };
                                lista.Add(maquina);
                            }
                        }
                    }
                }

            } catch(Exception ex) {
                lista = new List<MaquinaDetalleEntidad>();
            }
            return lista;
        }


        public List<MaquinaDetalleEntidad> ListarMaquinasAdministrativo(string codSala) {
            List<MaquinaDetalleEntidad> lista = new List<MaquinaDetalleEntidad>();

            string consulta = @"SELECT 
		                            maq.CodMaquina,
		                            maq.CodLinea,
		                            maq.CodJuego,
		                            maq.CodSala,
		                            maq.CodModeloMaquina,
		                            mom.CodMarcaMaquina,
		                            maq.CodContrato,
		                            maq.CodFicha,
		                            maq.CodMaquinaLey,
		                            lin.Nombre as NombreLinea,
		                            maq.NroSerie as NroSerie,
		                            jue.Nombre as NombreJuego,
		                            sal.Nombre as NombreSala,
		                            mom.Nombre as NombreModeloMaquina,
		                            con.Descripcion as DescripcionContrato,
		                            fic.Nombre as NombreFicha,
		                            mar.Nombre as NombreMarcaMaquina, 
		                            maq.Token as Token
                                FROM [BD_S3K_ADMINISTRATIVO_DATA].[dbo].[Maquina] maq
                                INNER JOIN Linea lin ON lin.CodLinea = maq.CodLinea
                                INNER JOIN Juego jue ON jue.CodJuego = maq.CodJuego
                                INNER JOIN Sala sal ON sal.CodSala = maq.CodSala
                                INNER JOIN ModeloMaquina mom ON mom.CodModeloMaquina = maq.CodModeloMaquina
                                INNER JOIN MarcaMaquina mar ON mar.CodMarcaMaquina = mom.CodMarcaMaquina
                                INNER JOIN Contrato con ON con.CodContrato = maq.CodContrato
                                INNER JOIN Ficha fic ON fic.CodFicha = maq.CodFicha 
                                WHERE maq.CodSala = @p0
                                ORDER BY maq.CodMaquinaLey ASC";
            try {
                using(var con = new SqlConnection(_conexionAdministrativo)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codSala);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var maquina = new MaquinaDetalleEntidad() {
                                    CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                    CodLinea = ManejoNulos.ManageNullInteger(dr["CodLinea"]),
                                    CodJuego = ManejoNulos.ManageNullInteger(dr["CodJuego"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodModeloMaquina = ManejoNulos.ManageNullInteger(dr["CodModeloMaquina"]),
                                    CodMarcaMaquina = ManejoNulos.ManageNullInteger(dr["CodMarcaMaquina"]),
                                    CodContrato = ManejoNulos.ManageNullInteger(dr["CodContrato"]),
                                    CodFicha = ManejoNulos.ManageNullInteger(dr["CodFicha"]),
                                    CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                    NombreLinea = ManejoNulos.ManageNullStr(dr["NombreLinea"]),
                                    NroSerie = ManejoNulos.ManageNullStr(dr["NroSerie"]),
                                    NombreJuego = ManejoNulos.ManageNullStr(dr["NombreJuego"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    NombreModeloMaquina = ManejoNulos.ManageNullStr(dr["NombreModeloMaquina"]),
                                    DescripcionContrato = ManejoNulos.ManageNullStr(dr["DescripcionContrato"]),
                                    NombreFicha = ManejoNulos.ManageNullStr(dr["NombreFicha"]),
                                    NombreMarcaMaquina = ManejoNulos.ManageNullStr(dr["NombreMarcaMaquina"]),
                                    Token = ManejoNulos.ManageNullDouble(dr["Token"]),
                                };
                                lista.Add(maquina);
                            }
                        }
                    }
                }

            } catch(Exception ex) {
                lista = new List<MaquinaDetalleEntidad>();
            }
            return lista;
        }
    }
}