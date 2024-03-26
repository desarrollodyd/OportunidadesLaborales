using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models {
    public class DetalleContadoresGameModel {
        string _conexionAdministrativo;
        public DetalleContadoresGameModel() {
            _conexionAdministrativo = ConfigurationManager.ConnectionStrings["conexionAdministrativo"].ConnectionString;
        }
        public List<DetalleContadoresGameEntidad> ListarDetalleContadoresGamePorFechaOperacionYSala(DateTime fechaInicio,DateTime fechaFin, string stringSalas) {
            List<DetalleContadoresGameEntidad> lista = new List<DetalleContadoresGameEntidad>();
            string consulta = $@"SET dateformat dmy;

                                            DECLARE @StartDate DATE = @fechaInicio;
                                            DECLARE @EndDate DATE = @fechaFin;

                                            SELECT  
                                                cgame.CodDetalleContadoresGame,
                                                cgame.CodContadoresGame,
                                                cgame.CodMaquina,
                                                cgame.CodSala,
                                                cgame.CodEmpresa,
                                                cgame.CodMoneda,
                                                cgame.FechaOperacion,
                                                cgame.CoinInIni,
                                                cgame.CoinInFin,
                                                cgame.CoinOutIni,
                                                cgame.CoinOutFin,
                                                cgame.JackpotIni,
                                                cgame.JackpotFin,
                                                cgame.HandPayIni,
                                                cgame.HandPayFin,
                                                cgame.CancelCreditIni,
                                                cgame.CancelCreditFin,
                                                cgame.GamesPlayedIni,
                                                cgame.GamesPlayedFin,
                                                cgame.ProduccionPorSlot1,
                                                cgame.ProduccionPorSlot2Reset,
                                                cgame.ProduccionPorSlot3Rollover,
                                                cgame.ProduccionPorSlot4Prueba,
                                                cgame.ProduccionTotalPorSlot5Dia,
                                                cgame.TipoCambio,
                                                cgame.FechaRegistro,
                                                cgame.FechaModificacion,
                                                cgame.Activo,
                                                cgame.Estado,
                                                cgame.SaldoCoinIn,
                                                cgame.SaldoCoinOut,
                                                cgame.SaldoJackpot,
                                                cgame.SaldoGamesPlayed,
                                                cgame.CodUsuario,
                                                cgame.RetiroTemporal,
                                                cgame.TiempoJuego,
                                                maq.CodMaquinaLey,
maq.CodAlterno
                                            FROM 
                                                dbo.DetalleContadoresGame as cgame
join dbo.Maquina as maq
on cgame.CodMaquina=maq.CodMaquina
                                            WHERE 
                                                cgame.FechaOperacion BETWEEN @StartDate AND @EndDate
                                                {stringSalas}
                                           ";
            try {
                using(var con = new SqlConnection(_conexionAdministrativo)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaInicio", fechaInicio.Date);
                    query.Parameters.AddWithValue("@fechaFin",fechaFin.Date);
              
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var maquina = new DetalleContadoresGameEntidad() {
                                    CodContadoresGame = ManejoNulos.ManageNullInteger(dr["CodContadoresGame"]),
                                    CodDetalleContadoresGame = ManejoNulos.ManageNullInteger(dr["CodDetalleContadoresGame"]),
                                    CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                    CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                    FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                    CoinInIni = ManejoNulos.ManageNullDecimal(dr["CoinInIni"]),
                                    CoinInFin = ManejoNulos.ManageNullDecimal(dr["CoinInFin"]),
                                    CoinOutIni = ManejoNulos.ManageNullDecimal(dr["CoinOutIni"]),
                                    CoinOutFin = ManejoNulos.ManageNullDecimal(dr["CoinOutFin"]),
                                    JackpotIni = ManejoNulos.ManageNullDecimal(dr["JackpotIni"]),
                                    JackpotFin = ManejoNulos.ManageNullDecimal(dr["JackpotFin"]),
                                    HandPayIni = ManejoNulos.ManageNullDecimal(dr["HandPayIni"]),
                                    HandPayFin = ManejoNulos.ManageNullDecimal(dr["HandPayFin"]),
                                    CancelCreditIni = ManejoNulos.ManageNullDecimal(dr["CancelCreditIni"]),
                                    CancelCreditFin = ManejoNulos.ManageNullDecimal(dr["CancelCreditFin"]),
                                    GamesPlayedIni = ManejoNulos.ManageNullDecimal(dr["GamesPlayedIni"]),
                                    GamesPlayedFin = ManejoNulos.ManageNullDecimal(dr["GamesPlayedFin"]),
                                    ProduccionPorSlot1 = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot1"]),
                                    ProduccionPorSlot2Reset = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot2Reset"]),
                                    ProduccionPorSlot3Rollover = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot3Rollover"]),
                                    ProduccionPorSlot4Prueba = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot4Prueba"]),
                                    ProduccionTotalPorSlot5Dia = ManejoNulos.ManageNullDecimal(dr["ProduccionTotalPorSlot5Dia"]),
                                    TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                    Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                    SaldoCoinIn = ManejoNulos.ManageNullDecimal(dr["SaldoCoinIn"]),
                                    SaldoCoinOut = ManejoNulos.ManageNullDecimal(dr["SaldoCoinOut"]),
                                    SaldoJackpot = ManejoNulos.ManageNullDecimal(dr["SaldoJackpot"]),
                                    SaldoGamesPlayed = ManejoNulos.ManageNullDecimal(dr["SaldoGamesPlayed"]),
                                    CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                    RetiroTemporal = ManejoNulos.ManageNullInteger(dr["RetiroTemporal"]),
                                    TiempoJuego = ManejoNulos.ManageNullDecimal(dr["TiempoJuego"]),
                                    CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                    CodAlterno = ManejoNulos.ManageNullStr(dr["CodAlterno"]),
                                };
                                lista.Add(maquina);
                            }
                        }
                    }
                }

            } catch(Exception ex) {
                lista = new List<DetalleContadoresGameEntidad>();
            }
            return lista;
        }
    }
}