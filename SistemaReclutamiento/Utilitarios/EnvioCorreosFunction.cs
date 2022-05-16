using Microsoft.AspNet.SignalR;
using SistemaReclutamiento.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Utilitarios
{
    public class EnvioCorreosFunction
    {
        public static void SendProgressBoletas(string progressMessage, decimal porcentaje, bool hide, string connectionId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<EnvioCorreoHub>();
            hubContext.Clients.Client(connectionId).AddProgressBoletas(progressMessage, porcentaje + "%", hide);
        }
    }
}