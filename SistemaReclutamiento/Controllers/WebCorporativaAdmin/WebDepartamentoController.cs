using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.WebCorporativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebDepartamentoController : Controller
    {
        WebDepartamentoModel departamentobl = new WebDepartamentoModel();
        // GET: WebDepartamento
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WebDepartamentoListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebDepartamentoEntidad> listaDepartamentos = new List<WebDepartamentoEntidad>();
            claseError error = new claseError();
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoListarJson();
                error = departamentoTupla.error;
                listaDepartamentos = departamentoTupla.lista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Departamentos";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Departamentos";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaDepartamentos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult WebDepartamentoIdObtenerJson(int dep_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            WebDepartamentoEntidad departamento = new WebDepartamentoEntidad();
            claseError error = new claseError();
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoIdObtenerJson(dep_id);
                error = departamentoTupla.error;
                departamento = departamentoTupla.departamento;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Departamento Obtennido";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo obtener informacion del Departamento";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = departamento, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebDepartamentoInsertarJson(WebDepartamentoEntidad departamento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            claseError error = new claseError();
            int idInsertadoo = 0;
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoInsertarJson(departamento);
                error = departamentoTupla.error;
               
                if (error.Key.Equals(string.Empty))
                {
                    idInsertadoo = departamentoTupla.idDeptatamentoInsertado;
                    if (idInsertadoo > 0) {
                        mensaje = "Departamento Insertado";
                        respuesta = true;
                    }
                    else
                    {
                        mensaje = "Ocurrio un error al Insertar";
                    }
                  
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo insertar del Departamento";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = departamento, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebDepartamentoEditarJson(WebDepartamentoEntidad departamento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            claseError error = new claseError();
            bool editado = false;
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoEditarJson(departamento);
                error = departamentoTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    editado = departamentoTupla.DepartamentoEditado;
                    if (editado !=false)
                    {
                        mensaje = "Departamento Editado";
                        respuesta = true;
                    }
                    else
                    {
                        mensaje = "Ocurrio un error al Editar";
                    }
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo editar el Departamento";
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = departamento, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult WebDepartamentoEliminarJson(int dep_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            claseError error = new claseError();
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoEliminarJson(dep_id);
                error = departamentoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Departamento Eliminado";
                    respuesta = departamentoTupla.DepartamentoEliminado;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo Eliminar";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}