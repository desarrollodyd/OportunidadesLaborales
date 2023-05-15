using SistemaReclutamiento.AdministrativoModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace SistemaReclutamiento.Context
{
    public class AdministrativoDbContext:DbContext
    {
        public AdministrativoDbContext() : base("conexionAdministrativo")
        {
        }

        public DbSet<DetalleContadoresGame> DetalleContadoresGame { get; set; }
        public DbSet<EventosPorSlot> EventosPorSlot { get; set; }
        public static AdministrativoDbContext Create()
        {
            return new AdministrativoDbContext();
        }
    }
}