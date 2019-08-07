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
        static configuracionModel configuracionbl = new configuracionModel();
        //static string rutaPostulantePerfil = "RUTA_FOTO_POSTULANTE";
        //static string rutaPostulanteCv = "RUTA_CV_POSTULANTE";
        //static string rutaPostulacionPerfil = "RUTA_FOTO_POSTULACION";
        //static string rutaPostulacionCv = "RUTA_CV_POSTULACION";
        static configuracionEntidad rutaPerfilPostulante = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_FOTO_POSTULANTE");
        static configuracionEntidad rutaCvPostulante = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_CV_POSTULANTE");
        static configuracionEntidad rutaPerfilPostulacion = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_FOTO_POSTULACION");
        static configuracionEntidad rutaCvPostulacion = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_CV_POSTULACION");
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
            //rutaPostulanteCv = "RUTA_CV_POSTULANTE";
            HttpPostedFileBase file = Request.Files[0];
            //var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(rutaPostulanteCv);
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
                        rutaInsertar = Path.Combine("" + rutaCvPostulante.config_nombre, nombreArchivo);
                        rutaAnterior = Path.Combine("" + rutaCvPostulante.config_nombre, postulante.pos_cv);

                        if (!Directory.Exists(rutaCvPostulante.config_nombre))
                        {
                            System.IO.Directory.CreateDirectory(rutaCvPostulante.config_nombre);
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
                        rutaImagenes.Postulante_CV(rutaCvPostulante.config_nombre, postulante.pos_cv);
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
            //string nemonic = "RUTA_FOTO_POSTULANTE";
            //configuracionModel configuracionbl = new configuracionModel();
            postulanteEntidad postulante = new postulanteEntidad();
            //var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(rutaPostulantePerfil);
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
                        rutaInsertar = Path.Combine("" + rutaPerfilPostulante.config_nombre , nombreArchivo);
                        rutaAnterior = Path.Combine("" + rutaPerfilPostulante.config_nombre , postulante.pos_foto);
                        rutaPerfilDefault = Path.Combine("" + rutaPerfilPostulante.config_nombre, foto_default);

                        if(!Directory.Exists(rutaPerfilPostulante.config_nombre))
                        {
                            System.IO.Directory.CreateDirectory(rutaPerfilPostulante.config_nombre);
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
            Session["postulante"]=postulantebl.PostulanteIdObtenerJson(postulante.pos_id);
            RutaImagenes rutaImagenes = new RutaImagenes();
            rutaImagenes.imagenPostulante_CV(rutaPerfilPostulante.config_nombre,postulante.pos_foto);
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        
        }
        public void DescargarArchivo()
        {
            //string nemonic = "RUTA_CV_POSTULANTE";
            postulanteEntidad postulante = (postulanteEntidad)Session["postulante"];
            //var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            string postulante_cv = @"" + rutaCvPostulante.config_nombre + "/" + postulante.pos_cv;
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
        /// <summary>
        /// Metodo para Migrar Datos de Tablas gdt_per... a tablas gdt_pos... de Postgres
        /// </summary>
        /// <param name="fk_oferta_laboral">Id de la oferta Laboral a la que se postula (int)</param>
        /// 
        /// <returns>Devuelve un Json de respuesta</returns>
        [HttpPost]
        public ActionResult PostulanteMigrarDataJson(int fk_oferta_laboral)
        {
            postulanteEntidad postulante = (postulanteEntidad)Session["postulante"];           
            List<educacionBasicaEntidad> listaeducacionBasica = new List<educacionBasicaEntidad>();
            List<educacionSuperiorEntidad> listaeducacionSuperior = new List<educacionSuperiorEntidad>();
            List<experienciaEntidad> listaexperiencia = new List<experienciaEntidad>();
            List<idiomaEntidad> listaidioma = new List<idiomaEntidad>();
            List<ofimaticaEntidad> listaofimatica = new List<ofimaticaEntidad>();
            List<postgradoEntidad> listapostgrado = new List<postgradoEntidad>();
            string extension = "";
            string nombreArchivo = "";
            string rutaCopiar = "";
            string rutaAnterior = "";
            bool respuestaConsulta = false;
            int idPostulacion = 0;
            string errormensaje = "";            
            try
            {

                idPostulacion = postulantebl.PostulanteTablaPostulacionInsertarJson(postulante, fk_oferta_laboral);
                if (idPostulacion>0)
                {
                    /*migrar foto de perfil*/
                    if (postulante.pos_foto != "" || postulante.pos_foto != null)
                    {
                        string[] extesionFotoPostulante = postulante.pos_foto.Split('.');
                        extension = extesionFotoPostulante[1];
                        nombreArchivo = (fk_oferta_laboral + "_" + postulante.pos_id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + idPostulacion + "." + extension);
                        rutaAnterior = Path.Combine("" + rutaPerfilPostulante.config_nombre, postulante.pos_foto);
                        rutaCopiar = Path.Combine("" + rutaPerfilPostulacion.config_nombre, nombreArchivo);
                        if (!Directory.Exists(rutaPerfilPostulacion.config_nombre))
                        {
                            Directory.CreateDirectory(rutaPerfilPostulacion.config_nombre);
                            System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                        }
                        else
                        {
                            System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                        }
                    }
                    /*Migrar Cv*/
                    if (postulante.pos_cv != "" || postulante.pos_cv != null)
                    {
                        string[] extesionCvPostulante = postulante.pos_cv.Split('.');
                        extension = extesionCvPostulante[1];
                        nombreArchivo = (fk_oferta_laboral + "_" + postulante.pos_id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + idPostulacion + "." + extension);
                        rutaAnterior = Path.Combine("" + rutaCvPostulante.config_nombre, postulante.pos_cv);
                        rutaCopiar = Path.Combine("" + rutaCvPostulacion.config_nombre, nombreArchivo);
                        if (!Directory.Exists(rutaCvPostulacion.config_nombre))
                        {
                            Directory.CreateDirectory(rutaCvPostulacion.config_nombre);
                            System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                        }
                        else
                        {
                            System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                        }
                    }
                    /*Migrar Data de Tablas*/
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
                    errormensaje = "Error al Migrar";
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