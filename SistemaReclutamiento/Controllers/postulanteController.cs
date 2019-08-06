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
        educacionBasicaModel educacionbasicabl = new educacionBasicaModel();
        educacionSuperiorModel educacionsuperiorbl = new educacionSuperiorModel();
        experienciaModel experienciabl = new experienciaModel();
        idiomaModel idiomabl = new idiomaModel();
        ofimaticaModel ofimaticabl = new ofimaticaModel();
        postgradoModel postgradobl = new postgradoModel();
        ofimaticaHerramientaModel ofimaticaHerramientabl = new ofimaticaHerramientaModel();
        configuracionModel configuracionbl = new configuracionModel();
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
            string nemonic = "RUTA_CV_POSTULANTE";
            HttpPostedFileBase file = Request.Files[0];
            var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            postulanteEntidad postulante = (postulanteEntidad)Session["postulante"]; ;

            bool respuestaConsulta = true ;
            string extension = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
            string errormensaje = "";
            int tamanioMaximo = 4194304;
            if (file.ContentLength > 0 && file != null)
            {
                if (file.ContentLength <= tamanioMaximo)
                {
                    extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        var nombreArchivo = (persona.pos_id.ToString()+"_" + DateTime.Now.ToString("yyyyMMddHHmmss")+extension);
                        rutaInsertar = Path.Combine("" + configuracion.config_nombre, nombreArchivo);
                        rutaAnterior = Path.Combine("" + configuracion.config_nombre, postulante.pos_cv);

                        if (!Directory.Exists(configuracion.config_nombre))
                        {
                            System.IO.Directory.CreateDirectory(configuracion.config_nombre);
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
                if (postulante.pos_cv == "")
                {
                    postulante.pos_cv = "";
                }
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
                        rutaImagenes.Postulante_CV(configuracion.config_nombre, postulante.pos_cv);
                        errormensaje = "Se Registro Correctamente";
                    }
                    else
                    {
                        errormensaje = "Error al registrar ";
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
            string foto_default = "user.png";
            string nemonic = "RUTA_FOTO_POSTULANTE";
            configuracionModel configuracionbl = new configuracionModel();
            postulanteEntidad postulante = new postulanteEntidad();
            var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            postulante.pos_id = Convert.ToInt32(Request.Params["postulanteID"]);
            postulanteEntidad postulanteFotoAnt = (postulanteEntidad)Session["postulante"];
            postulante.pos_foto = postulanteFotoAnt.pos_foto;

            bool respuestaConsulta = true;
            string extension = "";
            string rutaPerfilDefault="";
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
                        var nombreArchivo = (postulante.pos_id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                        rutaInsertar = Path.Combine("" + configuracion.config_nombre , nombreArchivo);
                        rutaAnterior = Path.Combine("" + configuracion.config_nombre , postulante.pos_foto);
                        rutaPerfilDefault = Path.Combine("" + configuracion.config_nombre, foto_default);

                        if(!Directory.Exists(configuracion.config_nombre))
                        {
                            System.IO.Directory.CreateDirectory(configuracion.config_nombre);
                        }

                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            if (!rutaAnterior.Equals(rutaPerfilDefault))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
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
            rutaImagenes.imagenPostulante_CV(configuracion.config_nombre,postulante.pos_foto);
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        
        }
        public void DescargarArchivo()
        {
            string nemonic = "RUTA_CV_POSTULANTE";
            postulanteEntidad postulante = (postulanteEntidad)Session["postulante"];
            var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            string postulante_cv = @"" + configuracion.config_nombre + "/" + postulante.pos_cv;
            if (postulante_cv != null)
            {
                if (System.IO.File.Exists(postulante_cv))
                {
                    FileInfo ObjArchivo = new System.IO.FileInfo(postulante_cv);
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + postulante.pos_cv);
                    Response.AddHeader("Content-Length", ObjArchivo.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(ObjArchivo.FullName);
                    Response.End();
                }
            }
        }
        /*Migracion de datos de tablas de postulante a tablas de postulaciones*/
        [HttpPost]
        public ActionResult PostulanteMigrarDataJson(int fk_oferta_laboral)
        {
            postulanteEntidad postulante = (postulanteEntidad)Session["postulante"];
            educacionBasicaEntidad educacionbasica = new educacionBasicaEntidad();
            educacionSuperiorEntidad educacionsuperior = new educacionSuperiorEntidad();
            experienciaEntidad experiencia = new experienciaEntidad();
            idiomaEntidad idioma = new idiomaEntidad();
            ofimaticaEntidad ofimatica = new ofimaticaEntidad();
            postgradoEntidad postgrado = new postgradoEntidad();
            //ofimaticaHerramientaEntidad ofimaticaHerramienta = new ofimaticaHerramientaEntidad();


            List<educacionBasicaEntidad> listaeducacionBasica = new List<educacionBasicaEntidad>();
            List<educacionSuperiorEntidad> listaeducacionSuperior = new List<educacionSuperiorEntidad>();
            List<experienciaEntidad> listaexperiencia = new List<experienciaEntidad>();
            List<idiomaEntidad> listaidioma = new List<idiomaEntidad>();
            List<ofimaticaEntidad> listaofimatica = new List<ofimaticaEntidad>();
            List<postgradoEntidad> listapostgrado = new List<postgradoEntidad>();
            bool respuestaConsulta = false;
            string errormensaje = "";            
            try
            {
                respuestaConsulta = postulantebl.PostulanteTablaPostulacionInsertarJson(postulante, fk_oferta_laboral);
                if (respuestaConsulta)
                {
                    listaeducacionBasica = educacionbasicabl.EducacionBasicaListaporPostulanteJson(postulante.pos_id);
                    listaeducacionSuperior = educacionsuperiorbl.EducacionSuperiorListaporPostulanteJson(postulante.pos_id);
                    listaexperiencia = experienciabl.ExperienciaListaporPostulanteJson(postulante.pos_id);
                    listaidioma = idiomabl.IdiomaListaporPostulanteJson(postulante.pos_id);
                    listaofimatica = ofimaticabl.OfimaticaListaporPostulanteJson(postulante.pos_id);
                    listapostgrado = postgradobl.PostgradoListaporPostulanteJson(postulante.pos_id);
                    if (listaeducacionBasica.Count > 0) {
                        foreach (educacionBasicaEntidad item in listaeducacionBasica)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionEducacionBasicaInsertarJson(item,fk_oferta_laboral);
                        }
                    }
                    if (listaeducacionSuperior.Count > 0) {
                        foreach(educacionSuperiorEntidad item in listaeducacionSuperior)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionEducacionSuperiorInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listaexperiencia.Count > 0)
                    {
                        foreach (experienciaEntidad item in listaexperiencia)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionExperienciaInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listaidioma.Count > 0)
                    {
                        foreach (idiomaEntidad item in listaidioma)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionIdiomaInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listaofimatica.Count > 0)
                    {
                        foreach (ofimaticaEntidad item in listaofimatica)
                        {
                            var ofimaticaHerramienta = ofimaticaHerramientabl.OfimaticaHerramientaIdObtenerJson(item.fk_herramienta);
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionOfimaticaInsertarJson(item,ofimaticaHerramienta.her_descripcion, fk_oferta_laboral);
                        }
                    }
                    if (listapostgrado.Count > 0)
                    {
                        foreach (postgradoEntidad item in listapostgrado)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionPostgradoInsertarJson(item, fk_oferta_laboral);
                        }
                    }

                }
                else {
                    errormensaje = "Error al Postular";
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + ", Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

    }
}