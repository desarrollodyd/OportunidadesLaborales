using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetFooterController : Controller
    {
        // GET: IntranetFooter
        IntranetFooterModel intranetFooterbl = new IntranetFooterModel();
        string PathActividadesIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        //RutaImagenes rutaImagenes = new RutaImagenes();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetFooterInsertarJson(IntranetFooterEntidad intranetFooter) {
            claseError error = new claseError();
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string errormensaje = "";
            bool response = false;
            try
            {
                intranetFooter.foot_estado = "A";
                //inserta imagen
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png")
                        {
                            var nombreArchivo = "Footer_" + DateTime.Now.ToString("yyyyMMddHHmmss")+extension;
                            rutaInsertar = Path.Combine(PathActividadesIntranet + "/Footer/", nombreArchivo);
                            if (!Directory.Exists(PathActividadesIntranet + "/Footer/")) {
                                System.IO.Directory.CreateDirectory(PathActividadesIntranet + "/Footer/");
                            }
                            file.SaveAs(rutaInsertar);
                            intranetFooter.foot_imagen = nombreArchivo;
                            if (intranetFooter.ruta_anterior != "" && intranetFooter.ruta_anterior != null) {
                                string rutaAnterior = Path.Combine(PathActividadesIntranet+"/Footer/" , intranetFooter.ruta_anterior);
                                if (System.IO.File.Exists(rutaAnterior))
                                {
                                    System.IO.File.Delete(rutaAnterior);
                                }
                            }
                        }
                        else
                        {
                            errormensaje = "Solo se aceptan formaton PNG ó JPG";
                            return Json(new { mensaje = errormensaje, respuesta = response });
                        }
                    }
                    else
                    {
                        errormensaje = "La imagen sobrepasa el tamaño maximo permitido (4mb)";
                        return Json(new { mensaje = errormensaje, respuesta = response });
                    }
                }
                else {
                    errormensaje = "Debe seleccionar una Imagen";
                    return Json(new { mensaje = errormensaje, respuesta = response });
                }
                var intranetFooterTupla = intranetFooterbl.IntranetFooterInsertarJson(intranetFooter);
                error = intranetFooterTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (intranetFooterTupla.idIntranetFooterInsertado > 0)
                    {
                        response = true;
                        errormensaje = "Insertado";
                    }
                    else {
                        errormensaje = "No se Pudo Insertar";
                    }
                }
                else {
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje = errormensaje, respuesta = response });
        }
        [HttpPost]
        public ActionResult IntranetFooterObtenerImagenes() {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            List<IntranetFooterEntidad> lista = new List<IntranetFooterEntidad>();
            try{
                var intranetFooterTupla = intranetFooterbl.IntranetFooterObtenerFootersJson();
                error = intranetFooterTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    lista = intranetFooterTupla.listaFooters;
                    errormensaje = "Listando Data";
                    response = true;
                }
                else {
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje = errormensaje, respuesta = response, data = lista.ToList() });
        }
    }
}