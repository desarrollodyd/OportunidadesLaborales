using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class postulanteController : Controller
    {
        postulanteModel postulantebl = new postulanteModel();
        string rutaCv = ConfigurationManager.AppSettings["PathArchivos"];
        string rutaPerfil=ConfigurationManager.AppSettings["PathImagenesPerfil"];
        // GET: postulante
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostulanteEditarJson(postulanteEntidad postulante)
        {
            var errormensaje = "";
            bool respuestaConsulta = true;         
            try
            {
                respuestaConsulta = postulantebl.PostulanteEditarJson(postulante);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult PostulanteInsertarInformacionAdicionalJson(usuarioPersonaEntidad persona)
        {
            HttpPostedFileBase file = Request.Files[0];
            postulanteEntidad postulante = new postulanteEntidad();
            bool respuestaConsulta = true ;
            string extension = "";
            string rutaInsertar = "";
            string errormensaje = "";
            int tamanioMaximo = 4194304;
            if (file.ContentLength > 0 || file != null)
            {
                if (file.ContentLength <= tamanioMaximo)
                {
                    extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        var nombreArchivo = (Path.GetFileNameWithoutExtension(file.FileName).ToLower() + "_" + persona.pos_id.ToString()+"_" + DateTime.Now.ToString("yyyyMMddHHmmss")+extension);
                        rutaInsertar = Path.Combine(Server.MapPath(rutaCv), nombreArchivo);
                        file.SaveAs(rutaInsertar);
                        postulante.pos_cv = nombreArchivo;
                        respuestaConsulta = true;
                    }
                    else
                    {
                        errormensaje = "Solo se admiten archivos word o pdf.";
                        respuestaConsulta = false;
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }
                }
                else
                {
                    errormensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }
                
            }
            else {
                postulante.pos_cv = "";
            }          
            postulante.pos_referido = persona.pos_referido;
            postulante.pos_nombre_referido = persona.pos_nombre_referido;
            postulante.pos_id = persona.pos_id;
            try
            {
                respuestaConsulta = postulantebl.PostulanteInsertarInformacionAdicionalJson(postulante);              
            }
            catch (Exception ex) {
                errormensaje = ex.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
            //if (file == null) return;
            //string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
            //file.SaveAs(Server.MapPath("~/Uploads/" + archivo));
        }
        [HttpPost]
        public ActionResult PostulanteSubirFotoJson()
        {
            HttpPostedFileBase file = Request.Files[0];
            postulanteEntidad postulante = new postulanteEntidad();
            postulante.pos_id = Convert.ToInt32(Request.Params["postulanteID"]);
            postulanteEntidad postulanteFotoAnt = (postulanteEntidad)Session["postulante"];
            postulante.pos_foto = postulanteFotoAnt.pos_foto;

            bool respuestaConsulta = true;
            string extension = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
            string errormensaje = "";
            int tamanioMaximo = 4194304;
            if (file.ContentLength > 0 || file != null)
            {
                if (file.ContentLength <= tamanioMaximo)
                {
                    extension = Path.GetExtension(file.FileName);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        var nombreArchivo = (Path.GetFileNameWithoutExtension(file.FileName).ToLower() + "_" + postulante.pos_id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                        rutaInsertar = Path.Combine(rutaPerfil , nombreArchivo);
                        rutaAnterior = Path.Combine(rutaPerfil , postulante.pos_foto);

                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            System.IO.File.Delete(rutaAnterior);
                        }

                        file.SaveAs(rutaInsertar);
                        postulante.pos_foto = nombreArchivo;
                        errormensaje = "Imagen Subida Correctamente";
                        respuestaConsulta = true;
                    }
                    else
                    {
                        errormensaje = "Solo se admiten archivos jpg o png.";
                        respuestaConsulta = false;
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }
                }
                else
                {
                    errormensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }

            }
            else
            {
                postulante.pos_foto = "";
            }
            postulante.pos_fecha_act = DateTime.Now;
            postulante.pos_id = postulante.pos_id;
            try
            {
                respuestaConsulta = postulantebl.PostulanteSubirFotoJson(postulante);
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + " ,Llame Administrador";
            }
            var mipostulante = postulantebl.PostulanteIdObtenerJson(postulante.pos_id);
            Session.Remove("postulante");
            Session["postulante"] = mipostulante;
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        
        }
    }
}