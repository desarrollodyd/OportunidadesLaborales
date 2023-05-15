using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.AdministrativoModels
{
    [Table("EventosPorSlot", Schema = "dbo")]
    public class EventosPorSlot
    {
        [Key]
        public int CodEventosPorSlot { get; set; }

        public int CodMaquina { get; set; }

        public int CodSala { get; set; }

        public int CodEmpresa { get; set; }
        [ForeignKey("DetalleContadoresGame")]
        public int? CodDetalleContadoresGame { get; set; }

        public int CodTipoEvento { get; set; }

        public int CodEventoOnline { get; set; }

        public DateTime? FechaEvento { get; set; }

        public DateTime FechaOperacion { get; set; }

        public decimal CoinInIni { get; set; }

        public decimal CoinInFin { get; set; }

        public decimal CoinOutIni { get; set; }

        public decimal CoinOutFin { get; set; }

        public decimal JackpotIni { get; set; }

        public decimal JackpotFin { get; set; }

        public decimal HandPayIni { get; set; }

        public decimal HandPayFin { get; set; }

        public decimal CancelCreditIni { get; set; }

        public decimal CancelCreditFin { get; set; }

        public decimal ProduccionPorSlot { get; set; }

        public decimal TicketInIni { get; set; }

        public decimal TicketInFin { get; set; }

        public decimal TicketOutIni { get; set; }

        public decimal TicketOutFin { get; set; }

        public decimal EftInIni { get; set; }

        public decimal EftInFin { get; set; }

        public decimal EftOutIni { get; set; }

        public decimal EftOutFin { get; set; }

        public decimal BonusInIni { get; set; }

        public decimal BonusInFin { get; set; }

        public decimal BonusOutIni { get; set; }

        public decimal BonusOutFin { get; set; }

        public decimal PhInIni { get; set; }

        public decimal PhInFin { get; set; }

        public decimal PhOutIni { get; set; }

        public decimal PhOutFin { get; set; }

        public decimal ContadorProgresivoIni { get; set; }

        public decimal ContadorProgresivoFin { get; set; }

        public decimal TicketPromocionIni { get; set; }

        public decimal TicketPromocionFin { get; set; }

        public decimal EftPromocionIni { get; set; }

        public decimal EftPromocionFin { get; set; }

        public decimal TrueInIni { get; set; }

        public decimal TrueInFin { get; set; }

        public decimal TrueOutIni { get; set; }

        public decimal TrueOutFin { get; set; }

        public decimal TotalDropIni { get; set; }

        public decimal TotalDropFin { get; set; }

        public decimal TmpebwIni { get; set; }

        public decimal TmpebwFin { get; set; }

        public decimal TapebwIni { get; set; }

        public decimal TapebwFin { get; set; }

        public decimal TappwIni { get; set; }

        public decimal TappwFin { get; set; }

        public decimal TmppwIni { get; set; }

        public decimal TmppwFin { get; set; }

        public decimal BillIni { get; set; }

        public decimal BillFin { get; set; }

        public decimal Bill1Ini { get; set; }

        public decimal Bill1Fin { get; set; }

        public decimal Bill2Ini { get; set; }

        public decimal Bill2Fin { get; set; }

        public decimal Bill5Ini { get; set; }

        public decimal Bill5Fin { get; set; }

        public decimal Bill10Ini { get; set; }

        public decimal Bill10Fin { get; set; }

        public decimal Bill50Ini { get; set; }

        public decimal Bill50Fin { get; set; }

        public decimal Bill100Ini { get; set; }

        public decimal Bill100Fin { get; set; }

        public decimal Bill200Ini { get; set; }

        public decimal Bill200Fin { get; set; }
        public virtual DetalleContadoresGame DetalleContadoresGame { get; set; }
    }
}