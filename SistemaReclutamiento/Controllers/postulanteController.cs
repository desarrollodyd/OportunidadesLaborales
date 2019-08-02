using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
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
        ofimaticaHerramientaModel ofimaticaherramientabl = new ofimaticaHerramientaModel();
        ofimaticaModel ofimaticabl = new ofimaticaModel();
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
            postulanteEntidad postulante = (postulanteEntidad)Session["postulante"];

            bool respuestaConsulta = true ;
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
                    if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        var nombreArchivo = (Path.GetFileNameWithoutExtension(file.FileName).ToLower() + "_" + persona.pos_id.ToString()+"_" + DateTime.Now.ToString("yyyyMMddHHmmss")+extension);
                        rutaInsertar = Path.Combine("" + rutaCv, nombreArchivo);
                        rutaAnterior = Path.Combine("" + rutaCv, postulante.pos_cv);

                        if (!Directory.Exists(rutaCv))
                        {
                            System.IO.Directory.CreateDirectory(rutaCv);
                        }

                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            System.IO.File.Delete(rutaAnterior);
                        }

                        file.SaveAs(rutaInsertar);
                        postulante.pos_cv = nombreArchivo;
                        errormensaje = "CV Subido Correctamente";
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
            postulante.pos_id = postulante.pos_id;
            try
            {
                if (respuestaConsulta)
                {
                    respuestaConsulta = postulantebl.PostulanteInsertarInformacionAdicionalJson(postulante);
                    if (respuestaConsulta)
                    {
                        Session.Remove("postulante");
                        Session["postulante"] = postulante;
                        RutaImagenes rutaImagenes = new RutaImagenes();
                        rutaImagenes.Postulante_CV(postulante.pos_cv);
                        errormensaje = "CV Subido Correctamente";
                    }
                    else
                    {
                        errormensaje = "Error al registrar cv";
                    }
                }
                             
            }
            catch (Exception ex) {
                errormensaje = ex.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
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
                    if (extension == ".jpg" || extension == ".png" || extension == ".PNG" || extension == ".JPG" || extension == ".JPEG" || extension == ".jpeg")
                    {
                        var nombreArchivo = (Path.GetFileNameWithoutExtension(file.FileName).ToLower() + "_" + postulante.pos_id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                        rutaInsertar = Path.Combine(""+rutaPerfil , nombreArchivo);
                        rutaAnterior = Path.Combine("" + rutaPerfil , postulante.pos_foto);

                        if(!Directory.Exists(rutaPerfil))
                        {
                            System.IO.Directory.CreateDirectory(rutaPerfil);
                        }

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
                
                if (respuestaConsulta)
                {
                    respuestaConsulta = postulantebl.PostulanteSubirFotoJson(postulante);
                    if (respuestaConsulta)
                    {
                        errormensaje = "Imagen Subida Correctamente";
                    }
                    else
                    {
                        errormensaje = "Error al registrar Imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + " ,Llame Administrador";
            }
            Session.Remove("postulante");
            Session["postulante"] = postulante;
            RutaImagenes rutaImagenes = new RutaImagenes();
            rutaImagenes.imagenPostulante_CV(postulante.pos_foto);
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        
        }
        [HttpPost]
        public ActionResult PostulanteMigrarDataJson(postulanteEntidad postulante, int fk_oferta_laboral)
        {
            bool respuestaConsulta = true;
            string errormensaje = "";
            List<ofimaticaEntidad> ofimatica = new List<ofimaticaEntidad>();
            ofimaticaHerramientaEntidad ofimaticaherramienta = new ofimaticaHerramientaEntidad();
            ofimatica = ofimaticabl.OfimaticaListaporPostulanteJson(postulante.pos_id);
            try
            {
                /*Insertar Tabla postulacion*/
                respuestaConsulta = postulantebl.PostulanteTablaPostulacionInsertarJson(postulante, fk_oferta_laboral);
                if (respuestaConsulta)
                {
                    errormensaje = "Se Inserto en Tabla Postulaciones";

                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + ", LLame Administrador";
            }
            return Json(new {respuesta=respuestaConsulta, mensaje=errormensaje });
        }
    }
}