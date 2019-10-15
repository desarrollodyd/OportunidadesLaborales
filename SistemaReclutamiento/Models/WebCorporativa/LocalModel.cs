using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.WebCorporativa
{
    public class LocalModel
    {
        string _conexion;
        public LocalModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
     
    }
}