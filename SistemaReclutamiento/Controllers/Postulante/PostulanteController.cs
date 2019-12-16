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

    public class PostulanteController : Controller
    {
        PosPreguntaOLAModel preguntabl = new PosPreguntaOLAModel();
        PosRespuestaOLAModel respuestabl = new PosRespuestaOLAModel();
        PostulanteModel postulantebl = new PostulanteModel();
        EducacionBasicaModel educacionbasicabl = new EducacionBasicaModel();
        EducacionSuperiorModel educacionsuperiorbl = new EducacionSuperiorModel();
        ExperienciaModel experienciabl = new ExperienciaModel();
        IdiomaModel idiomabl = new IdiomaModel();
        OfimaticaModel ofimaticabl = new OfimaticaModel();
        PostgradoModel postgradobl = new PostgradoModel();
        EstOfimaticaModel ofimaticaHerramientabl = new EstOfimaticaModel();
        static ConfiguracionModel configuracionbl = new ConfiguracionModel();
        static ConfiguracionEntidad rutaPerfilPostulante = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_FOTO_POSTULANTE");
        static ConfiguracionEntidad rutaCvPostulante = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_CV_POSTULANTE");
        static ConfiguracionEntidad rutaPerfilPostulacion = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_FOTO_POSTULACION");
        static ConfiguracionEntidad rutaCvPostulacion = configuracionbl.ConfiguracionObtenerporNemonicJson("RUTA_CV_POSTULACION");
        // GET: postulante
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostulanteEditarJson(PostulanteEntidad postulante)
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
        public ActionResult PostulanteInsertarInformacionAdicionalJson(UsuarioPersonaEntidad persona)
        {
            //rutaPostulanteCv = "RUTA_CV_POSTULANTE";
            HttpPostedFileBase file = Request.Files[0];
            //var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(rutaPostulanteCv);
            PostulanteEntidad postulante = (PostulanteEntidad)Session["postulante"]; ;

            bool respuestaConsulta = true;
            string extension = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
            string errormensaje = "";
            int tamanioMaximo = 4194304;
            try
            {
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                        {
                            var nombreArchivo = (persona.pos_id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
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
                else
                {
                    if (postulante.pos_cv == "")
                    {
                        postulante.pos_cv = "";
                    }
                }
            }
            catch (Exception ex)
            {
                respuestaConsulta = false;
                errormensaje = ex.Message;
                return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
            }
            //postulante.pos_url_perfil = persona.pos_url_perfil;
            postulante.pos_referido = persona.pos_referido;
            postulante.pos_nombre_referido = persona.pos_nombre_referido;
            postulante.pos_id = postulante.pos_id;
            postulante.pos_familia_amigos = persona.pos_familia_amigos;
            postulante.pos_fam_ami_desc = ManejoNulos.ManageNullStr( persona.pos_fam_ami_desc);
            postulante.pos_trabajo_pj = persona.pos_trabajo_pj;
            postulante.pos_trab_pj_desc = ManejoNulos.ManageNullStr(persona.pos_trab_pj_desc);
            postulante.pos_trab_otra_empresa = ManejoNulos.ManageNullStr(persona.pos_trab_otra_empresa);
            try
            {
                if (respuestaConsulta)
                {
                    respuestaConsulta = postulantebl.PostulanteInsertarInformacionAdicionalJson(postulante);
                    if (respuestaConsulta)
                    {
                        Session.Remove("postulante");
                        Session["postulante"] = postulantebl.PostulanteIdObtenerJson(postulante.pos_id);
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
                respuestaConsulta = false;
                errormensaje = ex.Message;
                return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
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
            PostulanteEntidad postulante = new PostulanteEntidad();
            //var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(rutaPostulantePerfil);
            postulante.pos_id = Convert.ToInt32(Request.Params["postulanteID"]);
            PostulanteEntidad postulanteFotoAnt = (PostulanteEntidad)Session["postulante"];
            postulante.pos_foto = postulanteFotoAnt.pos_foto;

            bool respuestaConsulta = true;
            string extension = "";
            string rutaPerfilDefault = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
            string errormensaje = "";
            int tamanioMaximo = 4194304;
            try
            {
                if (file.ContentLength > 0 || file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png" || extension == ".PNG" || extension == ".JPG" || extension == ".JPEG" || extension == ".jpeg")
                        {
                            var nombreArchivo = (postulante.pos_id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                            rutaInsertar = Path.Combine("" + rutaPerfilPostulante.config_nombre, nombreArchivo);
                            rutaAnterior = Path.Combine("" + rutaPerfilPostulante.config_nombre, postulante.pos_foto);
                            rutaPerfilDefault = Path.Combine("" + rutaPerfilPostulante.config_nombre, foto_default);

                            if (!Directory.Exists(rutaPerfilPostulante.config_nombre))
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
            }
            catch (Exception ex) {
                respuestaConsulta = false;
                errormensaje = ex.Message;
                return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
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
                respuestaConsulta = false;
                errormensaje = ex.Message;
                return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
            }
            Session.Remove("postulante");
            Session["postulante"] = postulantebl.PostulanteIdObtenerJson(postulante.pos_id);
            RutaImagenes rutaImagenes = new RutaImagenes();
            rutaImagenes.imagenPostulante_CV(rutaPerfilPostulante.config_nombre, postulante.pos_foto);
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });

        }

        public void DescargarArchivo()
        {
            //string nemonic = "RUTA_CV_POSTULANTE";
            PostulanteEntidad postulante = (PostulanteEntidad)Session["postulante"];
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

        [HttpPost]
        public ActionResult PostulantePostularJson(string[] preguntas, string form, int fk_oferta_laboral) {
            PostulanteEntidad postulante = (PostulanteEntidad)Session["postulante"];
            int fk_postulacion = 0;
            if (preguntas == null) {
                return Json(new { respuesta = false, mensaje = "No hay Preguntas" });
            }
            if (form.Equals(string.Empty))
            {
                return Json(new { respuesta = false, mensaje = "No ha respondido ninguna Pregunta" });
            }
            if (postulante.fk_nacionalidad == 0)
            {
                return Json(new { respuesta = false, mensaje = "Debe Completar sus Datos Personales para Postular a una Oferta" });
            }
            fk_postulacion = PostulanteMigrarDataJson(fk_oferta_laboral);
            if (fk_postulacion == 0)
            {
                return Json(new { respuesta = false, mensaje = "Error al Migrar Data de Postulante" });
            }
            bool respuestaConsulta = false;
            string errormensaje = "";
            PosPreguntaOLAEntidad pregunta = new PosPreguntaOLAEntidad();
            PosRespuestaOLAEntidad respuesta = new PosRespuestaOLAEntidad();
            int id = 0, idPreguntaInsertada = 0, contador = 0, contadorRespuestas=0;

            string[] respuestas = form.Split('&');
            pregunta.fk_postulacion = fk_postulacion;

            if (preguntas.Length > 0)
            {
                foreach (var m in preguntas)
                {
                    string[] split = m.Split('~');
                    pregunta.pol_pregunta = split[0];
                    id = Convert.ToInt32(split[1]);
                    //respuesta.fk_pos_pregunta_ol = id;
                    string[] splitRespuesta = Convert.ToString(respuestas[contadorRespuestas]).Split('=');
                    if (splitRespuesta[1].ToString() != "")
                    {
                        respuesta.rol_respuesta = Convert.ToString(splitRespuesta[1]);
                    }
                    else
                    {
                        contadorRespuestas++;
                        splitRespuesta = Convert.ToString(respuestas[contadorRespuestas]).Split('=');
                        respuesta.rol_respuesta = Convert.ToString(splitRespuesta[1]);

                    }
                    try
                    {
                        var preguntaInsertada = preguntabl.PosPreguntaOLAInsertarJson(pregunta);
                        idPreguntaInsertada = preguntaInsertada.idPreguntaInsertada;
                        errormensaje = preguntaInsertada.error.Value;
                        if (idPreguntaInsertada > 0)
                        {
                            respuesta.fk_pos_pregunta_ol = idPreguntaInsertada;
                            var respuestaPreguntaInsertada = respuestabl.PosRespuestaOLAInsertarJson(respuesta);
                            respuestaConsulta = respuestaPreguntaInsertada.response;
                            errormensaje = respuestaPreguntaInsertada.error.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { respuesta = false, mensaje = ex.Message });
                    }
                    contador++;
                    contadorRespuestas++;
                }
            }
            else {
                return Json(new { respuesta = false, mensaje = "No hay Preguntas" });
            }
            if (errormensaje.Equals(string.Empty)) {
                errormensaje = "Postulacion Realizada.";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        /// <summary>
        /// Metodo para Migrar Datos de Tablas gdt_per... a tablas gdt_pos... de Postgres
        /// </summary>
        /// <param name="fk_oferta_laboral">Id de la oferta Laboral a la que se postula (int)</param>
        /// 
        /// <returns>Devuelve un Json de respuesta</returns>

 
        public int PostulanteMigrarDataJson(int fk_oferta_laboral)
        {
            PostulanteEntidad postulante = (PostulanteEntidad)Session["postulante"];           
            List<EducacionBasicaEntidad> listaeducacionBasica = new List<EducacionBasicaEntidad>();
            List<EducacionSuperiorEntidad> listaeducacionSuperior = new List<EducacionSuperiorEntidad>();
            List<ExperienciaEntidad> listaexperiencia = new List<ExperienciaEntidad>();
            List<IdiomaEntidad> listaidioma = new List<IdiomaEntidad>();
            List<OfimaticaEntidad> listaofimatica = new List<OfimaticaEntidad>();
            List<PostgradoEntidad> listapostgrado = new List<PostgradoEntidad>();
            //string extension = "";
            //string nombreArchivo = "";
            //string rutaCopiar = "";
            //string rutaAnterior = "";
            bool respuestaConsulta = false;
            int idPostulacion = 0;
            string errormensaje = "";            
            try
            {
                idPostulacion = postulantebl.PostulanteTablaPostulacionInsertarJson(postulante, fk_oferta_laboral);
                if (idPostulacion>0)
                {
                    /*migrar foto de perfil*/
                    //if (postulante.pos_foto != "" || postulante.pos_foto != null)
                    //{
                    //    string[] extesionFotoPostulante = postulante.pos_foto.Split('.');
                    //    extension = extesionFotoPostulante[1];
                    //    nombreArchivo = (fk_oferta_laboral + "_" + postulante.pos_id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + idPostulacion + "." + extension);
                    //    rutaAnterior = Path.Combine("" + rutaPerfilPostulante.config_nombre, postulante.pos_foto);
                    //    rutaCopiar = Path.Combine("" + rutaPerfilPostulacion.config_nombre, nombreArchivo);
                    //    if (!Directory.Exists(rutaPerfilPostulacion.config_nombre))
                    //    {
                    //        Directory.CreateDirectory(rutaPerfilPostulacion.config_nombre);
                    //        System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                    //    }
                    //    else
                    //    {
                    //        System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                    //    }
                    //    errormensaje = "Archivos Copiados Correctamente";
                    //}
                    /*Migrar Cv*/
                    //if (postulante.pos_cv != "" || postulante.pos_cv != null)
                    //{
                    //    string[] extesionCvPostulante = postulante.pos_cv.Split('.');
                    //    extension = extesionCvPostulante[1];
                    //    nombreArchivo = (fk_oferta_laboral + "_" + postulante.pos_id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + idPostulacion + "." + extension);
                    //    rutaAnterior = Path.Combine("" + rutaCvPostulante.config_nombre, postulante.pos_cv);
                    //    rutaCopiar = Path.Combine("" + rutaCvPostulacion.config_nombre, nombreArchivo);
                    //    if (!Directory.Exists(rutaCvPostulacion.config_nombre))
                    //    {
                    //        Directory.CreateDirectory(rutaCvPostulacion.config_nombre);
                    //        System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                    //    }
                    //    else
                    //    {
                    //        System.IO.File.Copy(rutaAnterior, rutaCopiar, false);
                    //    }
                    //}
                    /*Migrar Data de Tablas*/
                    listaeducacionBasica = educacionbasicabl.EducacionBasicaListaporPostulanteJson(postulante.pos_id);
                    listaeducacionSuperior = educacionsuperiorbl.EducacionSuperiorListaporPostulanteJson(postulante.pos_id);
                    listaexperiencia = experienciabl.ExperienciaListaporPostulanteJson(postulante.pos_id);
                    listaidioma = idiomabl.IdiomaListaporPostulanteJson(postulante.pos_id);
                    listaofimatica = ofimaticabl.OfimaticaListaporPostulanteJson(postulante.pos_id);
                    listapostgrado = postgradobl.PostgradoListaporPostulanteJson(postulante.pos_id);                    
                    if (listaeducacionBasica.Count > 0) {
                        foreach (EducacionBasicaEntidad item in listaeducacionBasica)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionEducacionBasicaInsertarJson(item,fk_oferta_laboral);
                        }
                    }
                    if (listaeducacionSuperior.Count > 0) {
                        foreach(EducacionSuperiorEntidad item in listaeducacionSuperior)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionEducacionSuperiorInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listaexperiencia.Count > 0)
                    {
                        foreach (ExperienciaEntidad item in listaexperiencia)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionExperienciaInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listaidioma.Count > 0)
                    {
                        foreach (IdiomaEntidad item in listaidioma)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionIdiomaInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listaofimatica.Count > 0)
                    {
                        foreach (OfimaticaEntidad item in listaofimatica)
                        {
                            //var ofimaticaHerramienta = ofimaticaHerramientabl.EstOfimaticaIdObtenerJson(item.fk_ofimatica);
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionOfimaticaInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    if (listapostgrado.Count > 0)
                    {
                        foreach (PostgradoEntidad item in listapostgrado)
                        {
                            respuestaConsulta = postulantebl.PostulanteTablaPostulacionPostgradoInsertarJson(item, fk_oferta_laboral);
                        }
                    }
                    errormensaje = "Postulacion Correcta";
                }
                else {
                    errormensaje = "Error al Migrar";
                }
                
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + ", Llame Administrador";
            }
            return idPostulacion;
        }
        [HttpPost]
        public ActionResult PostulanteObtenerPorcentajeAvanceJson()
        {
            Decimal porcentaje = 0;
            PostulanteEntidad postulante = (PostulanteEntidad)Session["postulante"];
            PersonaEntidad persona = (PersonaEntidad)Session["per_full"];
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_full"];
            var educacionBasica = educacionbasicabl.EducacionBasicaListaporPostulanteJson(postulante.pos_id);
            var educacionSuperior = educacionsuperiorbl.EducacionSuperiorListaporPostulanteJson(postulante.pos_id);
            var idioma = idiomabl.IdiomaListaporPostulanteJson(postulante.pos_id);
            var ofimatica = ofimaticabl.OfimaticaListaporPostulanteJson(postulante.pos_id);
            var experienca = experienciabl.ExperienciaListaporPostulanteJson(postulante.pos_id);
            var postgrado = postgradobl.PostgradoListaporPostulanteJson(postulante.pos_id);
            if (educacionBasica.Count > 0) {
                porcentaje += 20;
            }
            if (educacionSuperior.Count > 0) {
                porcentaje += 10;
            }
            if (idioma.Count > 0) {
                porcentaje += 10;
            }
            if (ofimatica.Count > 0) {
                porcentaje += 10;
            }
            if (experienca.Count > 0) {
                porcentaje += 10;
            }
            if (postgrado.Count > 0) {
                porcentaje += 10;
            }
            if (!postulante.pos_cv.Equals(string.Empty))
            {
                porcentaje += (decimal)2.5;
            }
            if (postulante.pos_referido.Equals(true)) {
                porcentaje += (decimal)2.5;
            }
            if (postulante.pos_familia_amigos.Equals(true)) {
                porcentaje += (decimal)2.5;
            }
            if (postulante.pos_trabajo_pj.Equals(true)) {
                porcentaje += (decimal)2.5;
            }
            if (!persona.per_numdoc.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_apellido_pat.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_apellido_mat.Equals(string.Empty))
            {
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_nombre.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (postulante.fk_nacionalidad > 0) {
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_sexo.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_estado_civil.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_url_perfil.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_telefono.Equals(string.Empty)){
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_celular.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_brevete.Equals(false)) {
                porcentaje += (decimal)1.1;
            }
            if (!persona.per_fechanacimiento.Equals(string.Empty))
            {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_tipo_casa.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_tipo_calle.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_condicion_viv.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_direccion.Equals(string.Empty)) {
                porcentaje += (decimal)1.1;
            }
            if (persona.fk_ubigeo > 0) {
                porcentaje += (decimal)1.1;
            }
            if (!postulante.pos_foto.Equals(string.Empty)) {
                porcentaje += (decimal)1.3;
            }
            return Json(new { respuesta =true, mensaje="Porcentaje de Avance Perfil", data=porcentaje });
        }

    }
}