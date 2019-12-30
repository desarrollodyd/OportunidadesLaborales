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
    public class IntranetDetalleElementoModalController : Controller
    {
        // GET: IntranetDetalleElementoModal
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        claseError error = new claseError();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IntranetDetalleElementoModalListarxElementoModalJson(int fk_elemento_modal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetDetalleElementoModalEntidad> listaElementos = new List<IntranetDetalleElementoModalEntidad>();
            claseError error = new claseError();
            try
            {
                var ElementoTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(fk_elemento_modal);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.intranetDetalleElementoModalListaxElementoID;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Detalle Elemento Modal";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Detalles";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaElementos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoModalInsertarJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetDetalleElementoModalInsertado = 0;
            int tamanioMaximo = 0;
            string extension = "";
            string rutaInsertar = "";

            IntranetSeccionElementoEntidad intranetSeccionElemento = new IntranetSeccionElementoEntidad();
            claseError error = new claseError();

            //verificar si es imagen o abre un modal si fk_seccion_elemento==0 es imagen
            if (intranetDetalleElementoModal.fk_seccion_elemento == 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                tamanioMaximo = 4194304;
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png"||extension==".jpeg")
                        {
                            string nombreArchivo = ("ElementoModal_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                            var nombre = (nombreArchivo + extension);
                            rutaInsertar = Path.Combine(pathArchivosIntranet + "/", nombre);
                            if (!Directory.Exists(pathArchivosIntranet + "/"))
                            {
                                System.IO.Directory.CreateDirectory(pathArchivosIntranet + "/");
                            }
                            file.SaveAs(rutaInsertar);
                            intranetDetalleElementoModal.detelm_nombre = nombreArchivo;
                            //intranetDetalleElementoModal.detelm_extension = (extension == ".jpg" ? "jpg" : "png");
                            if (extension == ".jpg")
                            {
                                intranetDetalleElementoModal.detelm_extension = "jpg";
                            }
                            else if (extension == ".png")
                            {
                                intranetDetalleElementoModal.detelm_extension = "png";
                            }
                            else
                            {
                                intranetDetalleElementoModal.detelm_extension = "jpeg";
                            }
                        }
                        else
                        {
                            mensaje = "Solo se admiten archivos jpg o png.";
                            respuesta = false;
                            return Json(new { respuesta = respuesta, mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                    }

                }
                else
                {
                    mensaje = "Error al Subir el Archivo.";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            //Lista de Texto
            else if (intranetDetalleElementoModal.fk_seccion_elemento == 1)
            {
                try
                {
                    var totalDetallesTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalObtenerTotalRegistrosxElementoJson(intranetDetalleElementoModal.fk_elemento_modal);
                    if (totalDetallesTupla.error.Key.Equals(string.Empty))
                    {
                        intranetDetalleElementoModal.detelm_orden = totalDetallesTupla.intranetDetalleElementoModalTotal + 1;
                        //Insertar Imagen si esque hubiera
                        if (intranetDetalleElementoModal.detelm_nombre != "" && intranetDetalleElementoModal.detelm_nombre != null)
                        {

                            HttpPostedFileBase file = Request.Files[0];
                            tamanioMaximo = 4194304;
                            if (file.ContentLength > 0 && file != null)
                            {
                                if (file.ContentLength <= tamanioMaximo)
                                {
                                    extension = Path.GetExtension(file.FileName);
                                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                                    {
                                        string nombreArchivo = ("ElementoModal_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                        var nombre = (nombreArchivo + extension);
                                        rutaInsertar = Path.Combine(pathArchivosIntranet + "/", nombre);

                                        if (!Directory.Exists(pathArchivosIntranet + "/"))
                                        {
                                            System.IO.Directory.CreateDirectory(pathArchivosIntranet + "/");
                                        }

                                        file.SaveAs(rutaInsertar);
                                        intranetDetalleElementoModal.detelm_nombre = nombreArchivo;

                                        if (extension == ".jpg")
                                        {
                                            intranetDetalleElementoModal.detelm_extension = "jpg";
                                        }
                                        else if (extension == ".png")
                                        {
                                            intranetDetalleElementoModal.detelm_extension = "png";
                                        }
                                        else
                                        {
                                            intranetDetalleElementoModal.detelm_extension = "jpeg";
                                        }
                                    }
                                    else
                                    {
                                        mensaje = "Solo se admiten archivos jpg o png.";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    }
                                }
                                else
                                {
                                    mensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                                }

                            }
                            else
                            {
                                mensaje = "Error al Subir el Archivo.";
                                respuesta = false;
                                return Json(new { respuesta = respuesta, mensaje = mensaje });
                            }
                        }

                    }
                    else
                    {
                        mensaje = totalDetallesTupla.error.Value;
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                    }
             
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            try
            {

                var DetalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalInsertarJson(intranetDetalleElementoModal);
                error = DetalleElementoModalTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetDetalleElementoModalInsertado = DetalleElementoModalTupla.idIntranetDetalleElementoModalInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar el Detalle";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetDetalleElementoInsertado = idIntranetDetalleElementoModalInsertado, mensajeconsola = mensajeConsola });
        }
    }
}