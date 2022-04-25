using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class UsuarioController : Controller
    {
        private UsuarioModel segUsuarioBl = new UsuarioModel();
        [HttpPost]
        public ActionResult ListadoUsuarioJson()
        {
            string mensaje = string.Empty;
            bool respuesta = false;
            List<UsuarioPersonaEntidad> listaUsuario = new List<UsuarioPersonaEntidad>();
            try
            {
                var listaUsuTupla = segUsuarioBl.IntranetListarUsuariosJson();
                listaUsuario = listaUsuTupla.listaUsuarios.Where(x=>x.usu_estado.Equals("A")).ToList();
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new{mensaje,respuesta,data=listaUsuario});
        }
    }
}