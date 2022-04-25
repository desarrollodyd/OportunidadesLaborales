using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.BoletasGDT;
using SistemaReclutamiento.Models.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.SeguridadIntranet
{
    [autorizacion(false)]
    public class UsuarioEmpresaController : Controller
    {
        private BolEmpresaModel bolEmpresaDAL = new BolEmpresaModel();
        private SEG_UsuarioEmpresaDAL usuarioEmpresaDAL = new SEG_UsuarioEmpresaDAL();
        UsuarioModel usuarioBL = new UsuarioModel();
        PersonaModel personaBL = new PersonaModel();
        // GET: UsuarioEmpresa
        public ActionResult UsuarioEmpresaVista()
        {
            return View("~/Views/SeguridadIntranet/UsuarioEmpresaVista.cshtml");
        }
        [HttpPost]
        public ActionResult GetListadoUsuarioEmpresaPorUsuario(int usuario_id)
        {
            List<SEG_UsuarioEmpresaEntidad> listaUsuarioEmpresa = new List<SEG_UsuarioEmpresaEntidad>();
            List<BolEmpresaEntidad> listaempresas = new List<BolEmpresaEntidad>();
            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();
            bool respuesta = false;
            string mensaje = string.Empty;
            try
            {
                usuario = usuarioBL.UsuarioObtenerxID(usuario_id);
                if (usuario.usu_id != 0)
                {
                    persona = personaBL.PersonaIdObtenerJson(usuario.fk_persona);
                }
                var listaempresasTupla = bolEmpresaDAL.BolEmpresaListarJson();
                listaempresas = listaempresasTupla.lista;
                listaUsuarioEmpresa = usuarioEmpresaDAL.GetListadoUsuarioEmpresaPorUsuario(usuario_id);
                foreach(var item in listaUsuarioEmpresa)
                {
                    int index = listaempresas.FindIndex(x => x.emp_id == item.empresa_id);
                    if (index >= 0)
                    {
                        listaempresas[index].seleccionado = true;
                    }
                }
                respuesta = true;
                mensaje = "ListandoRegistros";
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta,dataEmpresas=listaempresas,dataUsuario=usuario,dataPersona=persona });
        }
        [HttpPost]
        public ActionResult InsertarUsuarioEmpresaJson(SEG_UsuarioEmpresaEntidad usuarioEmpresa)
        {
            string mensaje = string.Empty;
            bool respuesta = false;
            try
            {
                respuesta = usuarioEmpresaDAL.InsertarUsuarioEmpresaDAL(usuarioEmpresa);
                if (respuesta)
                {
                    mensaje = "Registro Insertado";
                }
                else
                {
                    mensaje = "No se pudo Insertar el Registro";
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [HttpPost]
        public ActionResult EliminarUsuarioEmpresaJson(SEG_UsuarioEmpresaEntidad usuarioEmpresa)
        {
            string mensaje = string.Empty;
            bool respuesta = false;
            try
            {
                respuesta = usuarioEmpresaDAL.EliminarUsuarioEmpresaDAL(usuarioEmpresa);
                if (respuesta)
                {
                    mensaje = "Registro Eliminado";
                }
                else
                {
                    mensaje = "No se pudo Eliminar el Registro";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
    }
}
