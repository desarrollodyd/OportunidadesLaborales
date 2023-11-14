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
        public List<DetalleContadoresGameEntidad> ListarDetalleContadoresGamePorFechaOperacionYSala(DateTime fechaInicio,DateTime fechaFin, int codSala) {
            List<DetalleContadoresGameEntidad> lista = new List<DetalleContadoresGameEntidad>();

            string consulta = @"SET dateformat dmy;

                                            DECLARE @StartDate DATE = @fechaInicio;
                                            DECLARE @EndDate DATE = @fechaFin;
                                            DECLARE @CodSala int = @sala

                                            SELECT 
                                                CodDetalleContadoresGame,
                                                CodContadoresGame,
                                                CodMaquina,
                                                CodSala,
                                                CodEmpresa,
                                                CodMoneda,
                                                FechaOperacion,
                                                CoinInIni,
                                                CoinInFin,
                                                CoinOutIni,
                                                CoinOutFin,
                                                JackpotIni,
                                                JackpotFin,
                                                HandPayIni,
                                                HandPayFin,
                                                CancelCreditIni,
                                                CancelCreditFin,
                                                GamesPlayedIni,
                                                GamesPlayedFin,
                                                ProduccionPorSlot1,
                                                ProduccionPorSlot2Reset,
                                                ProduccionPorSlot3Rollover,
                                                ProduccionPorSlot4Prueba,
                                                ProduccionTotalPorSlot5Dia,
                                                TipoCambio,
                                                FechaRegistro,
                                                FechaModificacion,
                                                Activo,
                                                Estado,
                                                SaldoCoinIn,
                                                SaldoCoinOut,
                                                SaldoJackpot,
                                                SaldoGamesPlayed,
                                                CodUsuario,
                                                RetiroTemporal,
                                                TiempoJuego
                                            FROM 
                                                dbo.DetalleContadoresGame
                                            WHERE 
                                                CodSala = @CodSala
                                                AND FechaOperacion BETWEEN @StartDate AND @EndDate
                                           ";
            try {
                using(var con = new SqlConnection(_conexionAdministrativo)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaInicio", fechaInicio.Date);
                    query.Parameters.AddWithValue("@fechaFin",fechaFin.Date);
                    query.Parameters.AddWithValue("@sala", codSala);
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
                                    TiempoJuego = ManejoNulos.ManageNullDecimal(dr["TiempoJuego"])
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