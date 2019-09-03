using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Utilitarios
{
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |
                AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class SeguridadMenu:Attribute
    {

        public SeguridadMenu(bool activa = true)
        {
            activar = activa;
        }
        private bool defaultactivar = true;
        public bool activar
        {
            get
            {
                return defaultactivar;
            }
            set
            {
                defaultactivar = value;
            }
        }
    }
}