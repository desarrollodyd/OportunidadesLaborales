using iTextSharp.text;
using iTextSharp.text.pdf;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.BoletasGDT;
using SistemaReclutamiento.Models.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion]
    public class IntranetPJBoletasGDTController : Controller
    {
        SQLModel sqlbl = new SQLModel();
        BolConfiguracionModel bolConfigBL = new BolConfiguracionModel();
        BolEmpleadoBoletaModel empleadoBoletaBL = new BolEmpleadoBoletaModel();
        BolBitacoraModel bitacoraBL = new BolBitacoraModel();
        BolEmpresaModel bolEmpresaBL = new BolEmpresaModel();
        BolDetCertEmpresaModel bolDetCertEmpresaBL = new BolDetCertEmpresaModel();
        private SEG_UsuarioEmpresaDAL usuarioEmpresaDAL = new SEG_UsuarioEmpresaDAL();
        private BolEmailRemitenteModel emailRemitenteDAL = new BolEmailRemitenteModel();

        public string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre" };

        #region Configuracion Boletas
        [HttpPost]
        public ActionResult BolConfiguracionObtenerxTipoJson(string tipo)
        {
            string mensaje = "";
            bool respuesta = false;
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();

            try
            {
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipo);
                if (configuracionTupla.error.Mensaje.Equals(string.Empty))
                {
                    configuracion = configuracionTupla.configuracion;
                    respuesta = true;
                    mensaje = "Obteniendo Registro";
                }
                else
                {
                    mensaje = configuracionTupla.error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = configuracion });
        }
        [HttpPost]
        public ActionResult BolConfiguracionInsertarJson(BolConfiguracionEntidad configuracion)
        {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            int idInsertado = 0;
            try
            {
                var configuracionTupla = bolConfigBL.BoolConfiguracionInsertarJson(configuracion);
                if (configuracionTupla.error.Mensaje.Equals(string.Empty))
                {
                    idInsertado = configuracionTupla.idInsertado;
                    if (idInsertado != 0)
                    {
                        mensaje = "Registro insertado";
                        respuesta = true;
                    }
                }
            } catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, idInsertado });
        }
        [HttpPost]
        public ActionResult BolConfiguracionEditarJson(BolConfiguracionEntidad configuracion)
        {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            int idInsertado = 0;
            try
            {
                var configuracionTupla = bolConfigBL.BoolConfiguracionEditarJson(configuracion);
                if (configuracionTupla.error.Mensaje.Equals(string.Empty))
                {
                    respuesta = configuracionTupla.editado;
                    if (respuesta)
                    {
                        idInsertado = configuracion.config_id;
                        mensaje = "Registro insertado";
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, idInsertado });
        }
        #endregion
        [HttpPost]
        public ActionResult CrearDirectorioBoletasGDTJson(string anioCreacion)
        {
            bool respuesta = false;
            string mensaje = "";
            string tipoConfiguracion = "PATH";
            List<TMEMPR> listaempresa = new List<TMEMPR>();
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            claseError error = new claseError();
            string anio = anioCreacion;
            List<dynamic> listaDirectorioEmpresa = new List<dynamic>();
            string direccion = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            string directorioHijo = "BOLETASAPROCESAR";
            List<SEG_UsuarioEmpresaEntidad> listaUsuarioEmpresa = new List<SEG_UsuarioEmpresaEntidad>();
            List<BolEmpresaEntidad> listaEmpresasPostgres = new List<BolEmpresaEntidad>();
            try
            {
                var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                //var listaEmpresaTupla = sqlbl.EmpresaListarJson();
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                var listaEmpresasTuplaPostgres = bolEmpresaBL.BolEmpresaListarPorUsuarioJson(usuarioId);

                if (listaEmpresasTuplaPostgres.error.Respuesta && configuracionTupla.error.Respuesta)
                {
                    listaEmpresasPostgres = listaEmpresasTuplaPostgres.lista;

                    configuracion = configuracionTupla.configuracion;
                    //Sincronizar empresas hacia postgress

                    //bool sincronizado = SincronizarEmpresas(listaempresa,configuracion.config_valor);

                    //Crear directorio principal
                    DirectoryInfo directorioPrincipal = Directory.CreateDirectory(Path.Combine(configuracion.config_valor + directorioHijo));
                    //Creacion de arbol de directorios
                    foreach (var empresa in listaEmpresasPostgres)
                    {

                        string[] arrayNombreEmpresa = empresa.emp_nomb.Split(' ');
                        string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                        string nombreDirectorio = empresa.emp_co_ofisis + "_" + nombreEmpresa;

                        DirectoryInfo directorioEmpresa = directorioPrincipal.CreateSubdirectory(nombreDirectorio);
                        DirectoryInfo directorioAnio = directorioEmpresa.CreateSubdirectory(Convert.ToString(anio));

                        List<dynamic> listaDirectorioAnio = new List<dynamic>();
                        List<dynamic> listaDirectorioMes = new List<dynamic>();
                        int nroMes = 1;
                        foreach (var mes in meses) {
                            string iconoPdf = "pdf_flat.png";
                            DirectoryInfo directorioMes = directorioAnio.CreateSubdirectory(nroMes.ToString().PadLeft(2, '0') + "_" + mes);
                            nroMes++;
                            FileInfo[] filesMes = directorioMes.GetFiles();

                            List<dynamic> listFilesMes = new List<dynamic>();
                            //var direccion = Server.MapPath("/") + Request.ApplicationPath;

                            foreach (var file in filesMes) {
                                double mbytes = ConvertBytesToMegabytes(file.Length);
                                listFilesMes.Add(new {
                                    name = file.Name + " \t \t " + mbytes + "Mb.",
                                    icon = direccion + "/Content/intranetSGC/jqueryztree/css/zTreeStyle/img/diy/" + iconoPdf,
                                });
                            }

                            listaDirectorioMes.Add(new {
                                name = directorioMes.Name,
                                open = false,
                                children = listFilesMes
                            });
                        }
                        listaDirectorioAnio.Add(new {
                            name = directorioAnio.Name,
                            children = listaDirectorioMes,
                            open = false
                        });
                        listaDirectorioEmpresa.Add(new {
                            name = directorioEmpresa.Name,
                            open = false,
                            children = listaDirectorioAnio
                        });
                    }

                    mensaje = "Directorios creados";
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listaDirectorioEmpresa });
        }
        public ActionResult BolListarPdfJson(DateTime fechaListar, string empresaListar, string nombreEmpresaListar)
        {
            string mensaje = "No se pudieron listar las boletas";
            bool respuesta = false;
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            try
            {
                string mes = Convert.ToString(fechaListar.Month);
                string anio = Convert.ToString(fechaListar.Year);
                var listaBoletasTupla = empleadoBoletaBL.BoolEmpleadoBoletaListarJson(empresaListar, anio, mes);
                if (listaBoletasTupla.error.Mensaje.Equals(string.Empty))
                {
                    listaBoletas = listaBoletasTupla.lista;
                    mensaje = "Listando registros";
                    respuesta = true;
                }
                else
                {
                    mensaje = listaBoletasTupla.error.Mensaje;
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listaBoletas });
        }
        [autorizacion(false)]
        public ActionResult BolListarporEmpleadoJson(DateTime fechaProcesoInicio, DateTime fechaProcesoFin, string empresaListar, string nombreEmpresaListar, string empleado)
        {
            string mensaje = "No se pudieron listar las boletas";
            bool respuesta = false;
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            try
            {
                int mesInicio = fechaProcesoInicio.Month;
                int anioInicio = fechaProcesoInicio.Year;
                int mesFin = fechaProcesoFin.Month;
                int anioFin = fechaProcesoFin.Year;

                //string stringPeriodo = "";
                //List<string> periodos = new List<string>();
                //for (int i = 1; i <= mesFin; i++)
                //{
                //    periodos.Add("'" + i + "'");
                //}
                //stringPeriodo = String.Join(",", periodos);

                string stringAnio = "";
                List<string> anios = new List<string>();
                List<int> _anios = new List<int>();
                for (int i = anioInicio; i <= anioFin; i++)
                {
                    anios.Add("'" + i + "'");
                    _anios.Add(i);
                }
                stringAnio = String.Join(",", anios);
                var listaBoletasTupla = empleadoBoletaBL.BoolEmpleadoBoletaListarxEmpleadoEmpresaFechasJson(empresaListar, empleado, stringAnio);

                if (listaBoletasTupla.error.Mensaje.Equals(string.Empty))
                {
                    listaBoletas = listaBoletasTupla.lista;
                    foreach (var anio in _anios)
                    {
                        if (anio == anioInicio)
                        {
                            for (int i = 1; i < mesInicio; i++)
                            {
                                var boleta = listaBoletas.Where(x => x.emp_anio.Equals(anio.ToString()) && x.emp_periodo.Equals(i.ToString())).FirstOrDefault();
                                if (boleta != null)
                                {
                                    listaBoletas.Remove(boleta);
                                }
                            }
                        }
                        if (anio == anioFin)
                        {
                            for (int i = mesFin + 1; i <= 12; i++)
                            {
                                var boleta = listaBoletas.Where(x => x.emp_anio.Equals(anio.ToString()) && x.emp_periodo.Equals(i.ToString())).FirstOrDefault();
                                if (boleta != null)
                                {
                                    listaBoletas.Remove(boleta);
                                }
                            }
                        }
                    }
                    mensaje = "Listando registros";
                    respuesta = true;
                }
                else
                {
                    mensaje = listaBoletasTupla.error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listaBoletas });
        }
        [HttpPost]
        [autorizacion(false)]
        public ActionResult VisualizarPdfIntranetAdminJson(BolEmpleadoBoletaEntidad empleado)
        {
            string mensaje = "";
            bool respuesta = false;
            string directorioProceso = "BOLETASPROCESADAS";
            string tipoConfiguracion = "PATH";
            string fileName = "";
            String data = "";
            try
            {
                string[] arrayNombreEmpresa = empleado.nombreEmpresa.Split(' ');
                string nombreDirectorioEmpresa = empleado.emp_co_empr + "_" + String.Join("", arrayNombreEmpresa);
                string nombreDirectorioEmpleado = empleado.emp_co_empr + "_" + empleado.emp_co_trab;
                string nombreArchivo = empleado.emp_ruta_pdf;
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                if (configuracionTupla.error.Mensaje.Equals(string.Empty))
                {
                    string pathArchivo = Path.Combine(configuracionTupla.configuracion.config_valor, directorioProceso, nombreDirectorioEmpresa, nombreDirectorioEmpleado, nombreArchivo);
                    if (System.IO.File.Exists(pathArchivo))
                    {
                        Byte[] bytes = System.IO.File.ReadAllBytes(pathArchivo);
                        String file = Convert.ToBase64String(bytes);
                        data = file;
                        fileName = nombreArchivo;
                        respuesta = true;
                        mensaje = "Existe el archivo";
                    }
                    else {
                        mensaje = "No se encontro el archivo";
                    }
                }
            }
            catch (Exception ex) {

            }
            return Json(new { data, mensaje, respuesta, fileName });
        }
        [autorizacion(false)]
        [HttpPost]
        public ActionResult GuardarBitacoraJson(BolBitacoraEntidad bitacora)
        {
            string mensaje = "No se pudo insertar";
            bool respuesta = false;
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usuIntranet_full"];
            int idInsertado = 0;
            try
            {
                bitacora.btc_fecha_reg = DateTime.Now;
                bitacora.btc_estado = 1;
                bitacora.btc_usuario_id = usuario.usu_id;
                var insertadoTupla = bitacoraBL.BitacoraInsertarJson(bitacora);
                if (insertadoTupla.error.Respuesta)
                {
                    mensaje = "Registrado";
                    respuesta = true;
                }
                else
                {
                    mensaje = insertadoTupla.error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [autorizacion(false)]
        [HttpPost]
        public ActionResult GuardarBitacoraSGCJson(BolBitacoraEntidad bitacora)
        {
            string mensaje = "No se pudo insertar";
            bool respuesta = false;
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usuSGC_full"];
            int idInsertado = 0;
            try
            {
                bitacora.btc_fecha_reg = DateTime.Now;
                bitacora.btc_estado = 1;
                bitacora.btc_usuario_id = usuario.usu_id;
                var insertadoTupla = bitacoraBL.BitacoraInsertarJson(bitacora);
                if (insertadoTupla.error.Respuesta)
                {
                    mensaje = "Registrado";
                    respuesta = true;
                }
                else
                {
                    mensaje = insertadoTupla.error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult BitacoraListarFiltrosJson(DateTime fechaInicio, DateTime fechaFin)
        {
            string mensaje = "No se pudieron listar los datos";
            bool respuesta = false;
            List<BolBitacoraEntidad> listaBitacoras = new List<BolBitacoraEntidad>();
            try
            {
                var listaTupla = bitacoraBL.BitacoraListarFiltrosJson(fechaInicio, fechaFin);
                if (listaTupla.error.Respuesta)
                {
                    listaBitacoras = listaTupla.lista.OrderByDescending(x => x.btc_fecha_reg).ToList();
                    mensaje = "Listando Registros";
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listaBitacoras });
        }
        [HttpPost]
        public ActionResult BolEmpresaListarJson()
        {
            string mensaje = "No se pudo insertar";
            bool respuesta = false;
            List<BolEmpresaEntidad> listaEmpresas = new List<BolEmpresaEntidad>();
            try
            {
                var listaEmpresasTupla = bolEmpresaBL.BolEmpresaListarJson();
                if (listaEmpresasTupla.error.Respuesta)
                {
                    listaEmpresas = listaEmpresasTupla.lista;
                    respuesta = true;
                    mensaje = "Listando Registros";
                }
                else
                {
                    mensaje = "No se pudo listar los registros";
                }
            } catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje, data = listaEmpresas });
        }
        [HttpPost]
        public ActionResult BolEmpresaIdObtenerJson(int emp_id)
        {
            string mensaje = "No se pudo insertar";
            bool respuesta = false;
            BolEmpresaEntidad empresa = new BolEmpresaEntidad();
            string directorioFirma = "DIRECTORIOCERTIFICADOS";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            string tipoConfiguracion = "PATH";
            string rutaAnterior = "";
            string rutaInsertar = "";
            try
            {

                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                configuracion = configuracionTupla.configuracion;

                var empresaTupla = bolEmpresaBL.BolEmpresaIdObtenerJson(emp_id);
                if (empresaTupla.error.Respuesta)
                {
                    empresa = empresaTupla.empresa;
                    string[] arrayNombreEmpresa = empresa.emp_nomb.Split(' ');
                    string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                    string directorioEmpresa = empresa.emp_co_ofisis + "_" + nombreEmpresa;
                    if (empresa.emp_firma_img != "")
                    {
                        rutaInsertar = Path.Combine(configuracion.config_valor, directorioFirma, directorioEmpresa);
                        rutaAnterior = Path.Combine(rutaInsertar, empresa.emp_firma_img == null ? "" : empresa.emp_firma_img);
                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(rutaAnterior);
                            empresa.emp_firma_img_base64 = Convert.ToBase64String(imageArray);
                        }
                    }
                    respuesta = true;
                    mensaje = "Obteniendo Registro";
                }
                else
                {
                    mensaje = "No se pudo obtener el registro";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                respuesta, mensaje, data = empresa
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { respuesta, mensaje, data = empresa });
        }
        public ActionResult BolEmpresaEditarJson(BolEmpresaEntidad empresa, HttpPostedFileBase file)
        {
            //HttpPostedFileBase file;
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            bool respuesta = false;
            string mensaje = "No se pudo editar el registro";
            string directorioFirma = "DIRECTORIOCERTIFICADOS";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            string tipoConfiguracion = "PATH";
            string rutaAnterior = "";
            try
            {
                string[] arrayNombreEmpresa = empresa.emp_nomb.Split(' ');
                string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                string directorioEmpresa = empresa.emp_co_ofisis + "_" + nombreEmpresa;
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                configuracion = configuracionTupla.configuracion;
                //if (empresa.emp_firma_img != null)
                //{
                //    file = Request.Files[0];
                //}
                //else
                //{
                //    file = null;
                //}
                if (file != null)
                {
                    if (file.ContentLength > 0 && file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                        {
                            rutaInsertar = Path.Combine(configuracion.config_valor, directorioFirma, directorioEmpresa);
                            if (Directory.Exists(rutaInsertar))
                            {
                                string nombreArchivo = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                rutaAnterior = Path.Combine(rutaInsertar, empresa.emp_firma_img == null ? "" : empresa.emp_firma_img);
                                if (System.IO.File.Exists(rutaAnterior))
                                {
                                    System.IO.File.Delete(rutaAnterior);
                                }
                                file.SaveAs(Path.Combine(rutaInsertar, nombreArchivo));
                                empresa.emp_firma_img = nombreArchivo;
                            }
                            else
                            {
                                mensaje = "No se encuentra el directorio a Insertar, Sincronize y Cree directorios nuevamente.";
                                return Json(new { mensaje, respuesta });
                            }
                        }
                        else
                        {
                            mensaje = "Solo se aceptan formaton .jpg,.png,.jpeg";
                            return Json(new { mensaje, respuesta });
                        }
                    }
                    else
                    {
                        mensaje = "Solo se permiten archivos de hasta 4Mb";
                        return Json(new { mensaje, respuesta });
                    }
                }
                //No se edito el archivo
                var respuestaTupla = bolEmpresaBL.BolEmpresaEditarJson(empresa);
                respuesta = respuestaTupla.editado;
                if (respuesta)
                {
                    mensaje = "registro editado";
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult BolEmpresaSincronizarJson()
        {
            string mensaje = "No se pudo sincronizar";
            bool respuesta = false;
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            string tipoConfiguracion = "PATH";
            try
            {

                List<TMEMPR> listaEmpresas = new List<TMEMPR>();
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                var listaEmpresasSQLTupla = sqlbl.EmpresaListarJson();
                configuracion = configuracionTupla.configuracion;
                if (listaEmpresasSQLTupla.error.Respuesta)
                {
                    respuesta = SincronizarEmpresas(listaEmpresasSQLTupla.listaempresa, configuracion.config_valor);
                    mensaje = "Se realizo la sincronización";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult BolDetCertEmpresaInsertarJson(BolDetCertEmpresaEntidad detalle)
        {
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            bool respuesta = false;
            string mensaje = "No se pudo insertar el registro";
            string directorioFirma = "DIRECTORIOCERTIFICADOS";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            string tipoConfiguracion = "PATH";
            try
            {
                string[] arrayNombreEmpresa = detalle.emp_nomb.Split(' ');
                string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                string directorioEmpresa = detalle.emp_co_ofisis + "_" + nombreEmpresa;
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                configuracion = configuracionTupla.configuracion;
                if (file != null)
                {
                    if (file.ContentLength > 0 && file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".pfx")
                        {
                            rutaInsertar = Path.Combine(configuracion.config_valor, directorioFirma, directorioEmpresa, "CERTIFICADOS");
                            if (Directory.Exists(rutaInsertar))
                            {
                                string nombreArchivo = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                file.SaveAs(Path.Combine(rutaInsertar, nombreArchivo));
                                detalle.det_nomb_cert = Path.GetFileNameWithoutExtension(file.FileName);
                                detalle.det_ruta_cert = nombreArchivo;
                            }
                            else
                            {
                                mensaje = "No se encuentra el directorio a Insertar, Sincronize y Cree directorios nuevamente.";
                                return Json(new { mensaje, respuesta });
                            }
                        }
                        else
                        {
                            mensaje = "Solo se aceptan formaton .pfx";
                            return Json(new { mensaje, respuesta });
                        }
                    }
                    else
                    {
                        mensaje = "Solo se permiten archivos de hasta 4Mb";
                        return Json(new { mensaje, respuesta });
                    }
                }
                else
                {
                    mensaje = "Debe seleccionar una archivo";
                    return Json(new { mensaje, respuesta });
                }
                var totalDetallesTupla = bolDetCertEmpresaBL.BolDetCertEmpresaListarxEmpresaIdJson(detalle.det_empr_id);
                if (totalDetallesTupla.lista.Count > 0)
                {
                    detalle.det_en_uso = 0;
                }
                else
                {
                    detalle.det_en_uso = 1;
                }
                detalle.det_estado_cert = 1;
                detalle.det_fecha_reg = DateTime.Now;
                var IdInsertadoTupla = bolDetCertEmpresaBL.BolDetCertEmpresaInsertarJson(detalle);
                if (IdInsertadoTupla.error.Respuesta)
                {
                    mensaje = "Registro Insertado con éxito";
                    respuesta = true;
                }
                else
                {
                    mensaje = IdInsertadoTupla.error.Mensaje;
                }
            } catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [HttpPost]
        public ActionResult BolDetCertEmpresaEditarUsoJson(BolDetCertEmpresaEntidad detalle)
        {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try
            {
                var respuestaTupla = bolDetCertEmpresaBL.BolDetCertEmpresaQuitarUsoJson(detalle);
                var respuestaEdicionTupla = bolDetCertEmpresaBL.BolDetCertEmpresaEditarUsoJson(detalle);
                respuesta = respuestaEdicionTupla.error.Respuesta;
                if (respuesta)
                {
                    mensaje = "Registro Editado";
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult BolDetCertEmpresaEliminarJson(BolDetCertEmpresaEntidad detalle)
        {
            string mensaje = "No se pudo eliminar el registro";
            bool respuesta = false;
            string directorioFirma = "DIRECTORIOCERTIFICADOS";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            string tipoConfiguracion = "PATH";
            string rutaEliminar = "";
            try
            {
                string[] arrayNombreEmpresa = detalle.emp_nomb.Split(' ');
                string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                string directorioEmpresa = detalle.emp_co_ofisis + "_" + nombreEmpresa;

                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                configuracion = configuracionTupla.configuracion;

                rutaEliminar = Path.Combine(configuracion.config_valor, directorioFirma, directorioEmpresa, "CERTIFICADOS", detalle.det_ruta_cert);

                if (System.IO.File.Exists(rutaEliminar))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(rutaEliminar);
                }


                var respuestaTupla = bolDetCertEmpresaBL.BolDetCertEmpresaEliminarJson(detalle);
                respuesta = respuestaTupla.error.Respuesta;
                if (respuesta)
                {
                    mensaje = "Registro Eliminado";
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult BolProcesarPdf(HttpPostedFileBase archivoProceso, DateTime fechaProcesoPdf, string empresa, string nombreEmpresa, string connectionId = "")
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string archivoDescomprimido = string.Empty;
            //Varialbes necesarias
            int totalRegistrosBD = 0;
            int totalHojas = 0;
            const string TIPO_CONFIGURACION = "PATH";
            const string DIRECTORIO_PROCESO = "BOLETASPROCESADAS";
            const string DIRECTORIO_A_PROCESAR = "BOLETASAPROCESAR";
            const string DIRECTORIO_RAIZ_CERTIFICADOS = "DIRECTORIOCERTIFICADOS";
            const string DIRECTORIO_CERTIFICADO_EMPRESA = "CERTIFICADOS";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            List<PersonaSqlEntidad> listaPersonas = new List<PersonaSqlEntidad>();
            List<BolEmpleadoBoletaEntidad> listaInsertar = new List<BolEmpleadoBoletaEntidad>();
            List<BolEmpleadoBoletaEntidad> listaEliminar = new List<BolEmpleadoBoletaEntidad>();
            BolDetCertEmpresaEntidad detalleCertificadoEmpresa = new BolDetCertEmpresaEntidad();
            try
            {
                //Iniciar Signalr
                ProgressBarFunction.SendProgressBoletas("Iniciando ...", 0, false, connectionId);
                Thread.Sleep(1000);

                DateTime fechaProceso = fechaProcesoPdf;
                int mes = fechaProceso.Month;
                string carpetaMes = mes.ToString().PadLeft(2, '0') + "_" + meses[mes - 1];

                string anio = Convert.ToString(fechaProceso.Year);

                //Nombre Directorio de la Empresa
                string[] arrayNombreEmpresa = nombreEmpresa.Split(' ');
                string nombreDirectorioEmpresa = empresa + "_" + String.Join("", arrayNombreEmpresa);


                if (archivoProceso == null)
                {
                    mensaje = "Archivo comprimido obligatorio";
                    mensajeConsola = "Archivo comprimido obligatorio";
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                //
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(TIPO_CONFIGURACION);
                if (!configuracionTupla.error.Respuesta && configuracionTupla.configuracion.config_id == 0)
                {
                    mensaje = "No se pudo encontrar el registro de configuracion en intranet.bol_configuracion";
                    mensajeConsola = configuracionTupla.error.Mensaje;
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                configuracion = configuracionTupla.configuracion;
                //ejemplo ruta C:\BoletasGDT\BOLETASAPROCESAR\01_IBERPERUSAC\2021\01_Enero
                string pathDirectorioInsercionTemporal = Path.Combine(configuracion.config_valor, DIRECTORIO_A_PROCESAR, nombreDirectorioEmpresa, anio, carpetaMes);
                //string pathDirectorioInsercionTemporal = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/";
                if (!Directory.Exists(pathDirectorioInsercionTemporal))
                {
                    // Try to create the directory.  
                    //DirectoryInfo di = Directory.CreateDirectory(pathDirectorioInsercionTemporal);
                    mensaje = "No se pudo encontrar el directorio de proceso";
                    mensajeConsola = "Directorio de proceso" + pathDirectorioInsercionTemporal;
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                //Creacion de variables
                var listaPersonasTupla = sqlbl.PersonaSQLObtenrListadoBoletasGDTJson(empresa, fechaProceso.Month, fechaProceso.Year);
                var listaEliminarTupla = empleadoBoletaBL.BoolEmpleadoBoletaListarJson(empresa, anio, mes.ToString());
                var empresaPostgres = bolEmpresaBL.BolEmpresaObtenerxOfisisIdJson(empresa);
                //Obtener certificado para firmar
                detalleCertificadoEmpresa = empresaPostgres.empresa.DetalleCerts.Where(x => x.det_en_uso == 1).FirstOrDefault();

                string nombreArchivo = Path.GetFileName(archivoProceso.FileName);
                string pathInsercionArchivoTemporal = Path.Combine(pathDirectorioInsercionTemporal, nombreArchivo);
                string extensionArchivo = Path.GetExtension(archivoProceso.FileName);
                //Verificar que sea .rar
                if (!extensionArchivo.ToLower().Equals(".rar"))
                {
                    mensaje = "El archivo debe tener formato .rar";
                    mensajeConsola = "Extension de Archivo: " + extensionArchivo;
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                //Eliminar Archivos dentro
                LimpiarDirectorio(pathDirectorioInsercionTemporal);
                //Insertar Archivo
                archivoProceso.SaveAs(pathInsercionArchivoTemporal);
                archivoDescomprimido = DescomprimirArchivo(nombreArchivo, extensionArchivo, pathDirectorioInsercionTemporal);
                if (archivoDescomprimido.Equals(string.Empty))
                {
                    mensaje = "Error al Extraer Archivo, el archivo comprimido debe ser un documento .pdf";
                    mensajeConsola = "Error al descomprimir archivo";
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                //realizar el mismo proceso de creacion de boletas
                if (listaPersonasTupla.error.Mensaje.Equals(string.Empty) && detalleCertificadoEmpresa != null)
                {
                    configuracion = configuracionTupla.configuracion;
                    listaPersonas = listaPersonasTupla.lista;
                    string pathPdf = Path.Combine(configuracion.config_valor, DIRECTORIO_A_PROCESAR, nombreDirectorioEmpresa, anio, carpetaMes);

                    DirectoryInfo directorioRoot = Directory.CreateDirectory(Path.Combine(configuracion.config_valor, DIRECTORIO_PROCESO, nombreDirectorioEmpresa));

                    if (Directory.Exists(pathPdf))
                    {
                        //realizar la busqueda del pdf y realizar la division de este
                        string file = Directory.GetFiles(pathPdf, "*.pdf").FirstOrDefault();
                        if (file != null)
                        {
                            //pdf encontrado
                            using (PdfReader reader = new PdfReader(Path.Combine(pathPdf, file)))
                            {
                                totalRegistrosBD = listaPersonas.Count;
                                totalHojas = reader.NumberOfPages;
                                if (reader.NumberOfPages == listaPersonas.Count)
                                {
                                    int totalRegistrosInsertar = listaPersonas.Count;//100%
                                    decimal totalElementos = totalRegistrosInsertar;
                                    decimal limit = 0;
                                    decimal porcentaje = 0;
                                    //eliminar la data y carpetas
                                    if (listaEliminarTupla.error.Mensaje.Equals(string.Empty))
                                    {
                                        string[] subdirectoryEntries = Directory.GetDirectories(Path.Combine(configuracion.config_valor, DIRECTORIO_A_PROCESAR));
                                        if (subdirectoryEntries.Length > 0 && listaEliminarTupla.lista.Count > 0)
                                        {
                                            int totalRegistrosEliminar = listaEliminarTupla.lista.Count;//100%
                                            totalElementos = totalRegistrosInsertar + totalRegistrosEliminar;
                                            //eliminar pdfs
                                            foreach (var empleado in listaEliminarTupla.lista)
                                            {
                                                string myfile = Directory.GetFiles(Path.Combine(configuracion.config_valor), empleado.emp_ruta_pdf, SearchOption.AllDirectories).FirstOrDefault();
                                                if (myfile != null)
                                                {
                                                    System.IO.File.Delete(myfile);
                                                }
                                                porcentaje = (limit * 100 / totalElementos);
                                                porcentaje = Math.Round(porcentaje, 2);
                                                //porcentaje = decimal.Round(((limit * 100) / totalElementos), 2);

                                                ProgressBarFunction.SendProgressBoletas("Limpiando Archivos ... " + empleado.emp_ruta_pdf, porcentaje, false, connectionId);
                                                limit++;
                                            }
                                        }
                                        //eliminar de BD
                                        ProgressBarFunction.SendProgressBoletas("Limpiando Base de Datos ...", 30, false, connectionId);
                                        var eliminadoTupla = empleadoBoletaBL.BoolEmpleadoBoletaEliminarMasivoJson(empresa, anio, mes.ToString());
                                    }
                                    //
                                    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
                                    {

                                        BolEmpleadoBoletaEntidad empleado = new BolEmpleadoBoletaEntidad();

                                        var item = listaPersonas.ElementAt(pagenumber - 1);
                                        string directorioEmpleado = item.CO_EMPR + "_" + item.CO_TRAB;
                                        string filename = item.CO_TRAB + "_" + item.CO_EMPR + "_" + anio + "_" + mes + ".pdf";

                                        DirectoryInfo subdirectorioEmpleado = directorioRoot.CreateSubdirectory(directorioEmpleado);

                                        Document document = new Document();
                                        PdfCopy copy = new PdfCopy(document, new FileStream(Path.Combine(subdirectorioEmpleado.FullName, filename), FileMode.Create));
                                        document.Open();

                                        copy.AddPage(copy.GetImportedPage(reader, pagenumber));
                                        document.Close();

                                        //Firmar Pdf Aqui
                                        var rutaCertificado = Path.Combine(
                                                configuracion.config_valor,
                                                DIRECTORIO_RAIZ_CERTIFICADOS,
                                                nombreDirectorioEmpresa,
                                                DIRECTORIO_CERTIFICADO_EMPRESA,
                                                detalleCertificadoEmpresa.det_ruta_cert
                                                );
                                        var rutaImagen = Path.Combine(
                                                configuracion.config_valor,
                                                DIRECTORIO_RAIZ_CERTIFICADOS,
                                                nombreDirectorioEmpresa
                                                );
                                        var certificado = new Certificado(
                                            rutaCertificado,
                                            detalleCertificadoEmpresa.det_pass_cert
                                            );
                                        var firmante = new Firmante(certificado);
                                        string secondFileName = item.CO_TRAB + "_" + item.CO_EMPR + "_" + anio + "_" + mes + "_signed.pdf";
                                        if (System.IO.File.Exists(rutaCertificado))
                                        {
                                            firmante.Firmar(
                                                Path.Combine(subdirectorioEmpleado.FullName, filename),
                                                Path.Combine(subdirectorioEmpleado.FullName, secondFileName),
                                                empresaPostgres.empresa,
                                                rutaImagen
                                                );
                                            System.IO.File.Delete(Path.Combine(subdirectorioEmpleado.FullName, filename));
                                        }
                                        //Termino de Firma
                                        empleado.emp_co_trab = item.CO_TRAB;
                                        empleado.emp_co_empr = item.CO_EMPR;
                                        empleado.emp_anio = anio;
                                        empleado.emp_periodo = Convert.ToString(mes);
                                        empleado.emp_ruta_pdf = secondFileName;
                                        empleado.emp_no_trab = item.NO_TRAB;
                                        empleado.emp_apel_pat = item.NO_APEL_PATE;
                                        empleado.emp_apel_mat = item.NO_APEL_MATE;
                                        empleado.emp_direc_mail = item.NO_DIRE_MAI1;
                                        empleado.emp_nro_cel = item.NU_TLF1;
                                        empleado.emp_tipo_doc = item.TI_DOCU_IDEN;
                                        listaInsertar.Add(empleado);

                                        //porcentaje = decimal.Round(((limit * 100) / totalElementos), 2);
                                        porcentaje = (limit * 100 / totalElementos);
                                        porcentaje = Math.Round(porcentaje, 2);
                                        limit++;
                                        ProgressBarFunction.SendProgressBoletas("Creando Pdf ... " + empleado.emp_ruta_pdf, porcentaje, false, connectionId);
                                    }
                                    //llenado en base de datos

                                    string consulta = "";
                                    int totalInsertados = 0;
                                    List<string> listaStringInsertar = new List<string>();
                                    foreach (var empleado in listaInsertar)
                                    {
                                        listaStringInsertar.Add(
                                            String.Format("('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}')",
                                            empleado.emp_co_trab,
                                            empleado.emp_co_empr,
                                            empleado.emp_anio,
                                            empleado.emp_periodo,
                                            empleado.emp_ruta_pdf,
                                            empleado.emp_enviado,
                                            empleado.emp_descargado,
                                            empleado.emp_fecha_reg.ToString("yyyy-MM-dd HH:mm:ss"),
                                            empleado.emp_no_trab.Replace("'", @"''"),
                                            empleado.emp_apel_pat.Replace("'", @"''"),
                                            empleado.emp_apel_mat.Replace("'", @"''"),
                                            empleado.emp_direc_mail.Replace("'", @"''"),
                                            empleado.emp_nro_cel,
                                            empleado.emp_tipo_doc
                                            ));
                                    }
                                    consulta = String.Join(",", listaStringInsertar);

                                    var totalInsertadosTupla = empleadoBoletaBL.BoolEmpleadoBoletaInsertarMasivoJson(consulta);
                                    if (totalInsertadosTupla.error.Mensaje.Equals(string.Empty))
                                    {
                                        totalInsertados = totalInsertadosTupla.totalInsertados;
                                    }
                                    if (totalInsertados == listaPersonas.Count)
                                    {
                                        mensaje = "PDFs procesados";
                                        mensajeConsola = "PDFs procesados---> TotalRegistrosBD:[" + totalRegistrosBD + "]" + "; ---- Total Hojas PDF:[" + totalHojas + "]";
                                        respuesta = true;
                                        ProgressBarFunction.SendProgressBoletas("Registros Insertados ...", porcentaje, false, connectionId);
                                        Thread.Sleep(1000);
                                        ProgressBarFunction.SendProgressBoletas("Proceso Terminado ...", 100, true, connectionId);
                                        Thread.Sleep(1000);
                                    }
                                }
                                else
                                {
                                    mensaje = "Inconsistencia entre pdf y total de trabajadores";
                                    mensajeConsola = "Inconsistencia entre pdf y total de trabajadores---> TotalRegistrosBD:[" + totalRegistrosBD + "]" + "; ---- Total Hojas PDF:[" + totalHojas + "]";
                                }
                            }

                        }
                        else
                        {
                            mensaje = "No se encontro el archivo pdf, subir el pdf a su carpeta correspondiente";
                            mensajeConsola = "No se encontro el archivo pdf, subir el pdf a su carpeta correspondiente ---> TotalRegistrosBD:[" + totalRegistrosBD + "]" + "; ---- Total Hojas PDF:[" + totalHojas + "]";

                        }
                    }
                    else
                    {
                        mensaje = "No se encuentra el directorio, crearlo en el menú de creación de directorios";
                        mensajeConsola = "No se encuentra el directorio, crearlo en el menú de creación de directorios --->TotalRegistrosBD:[" + totalRegistrosBD + "]" + "; ---- Total Hojas PDF:[" + totalHojas + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                mensajeConsola = ex.Message + "--->TotalRegistrosBD:[" + totalRegistrosBD + "]" + "; ---- Total Hojas PDF:[" + totalHojas + "]";
            }
            if (!respuesta)
            {
                ProgressBarFunction.SendProgressBoletas(mensaje, 99, false, connectionId);
                Thread.Sleep(2000);
                ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
            }
            return Json(new { data = listaInsertar, mensaje, mensajeConsola, respuesta });
        }
        [HttpPost]
        public ActionResult BolEmpresaListarPorUsuarioJson()
        {
            string mensaje = "No se pudo insertar";
            bool respuesta = false;
            List<BolEmpresaEntidad> listaEmpresas = new List<BolEmpresaEntidad>();
            string usuario_sgc = ConfigurationManager.AppSettings["usuario_sgc"].ToString();
            try
            {
                var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                var usuarionNombre = Convert.ToString(Session["UsuarioNombre"]);
                if (usuarionNombre == usuario_sgc)
                {
                    var listaEmpresasTupla = bolEmpresaBL.BolEmpresaListarJson();
                    if (listaEmpresasTupla.error.Respuesta)
                    {
                        listaEmpresas = listaEmpresasTupla.lista;
                        respuesta = true;
                        mensaje = "Listando Registros";
                    }
                    else
                    {
                        mensaje = "No se pudo listar los registros";
                    }
                }
                else
                {
                    var listaEmpresasTupla = bolEmpresaBL.BolEmpresaListarPorUsuarioJson(usuarioId);
                    if (listaEmpresasTupla.error.Respuesta)
                    {
                        listaEmpresas = listaEmpresasTupla.lista;
                        respuesta = true;
                        mensaje = "Listando Registros";
                    }
                    else
                    {
                        mensaje = "No se pudo listar los registros";
                    }
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje, data = listaEmpresas });
        }


        public ActionResult BolProcesarPdfVista()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJBolProcesarPdf.cshtml");
        }
        public ActionResult BolEnvioBoletasVista()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJBolEnvioBoletas.cshtml");
        }
        public ActionResult BolConfiguracionDirectorioVista()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJBolConfiguracionDirectorios.cshtml");
        }
        public ActionResult BolBitacoraVista()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJBolBitacora.cshtml");
        }
        #region Region Email Remitente
        public ActionResult BolEmailRemitenteVista()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJBolEmailRemitente.cshtml");
        }
        [HttpPost]
        public ActionResult BolEmailRemitenteListadoJson()
        {
            string mensaje = "No se pudieron listar los registros";
            bool respuesta = false;
            List<BolEmailRemitenteEntidad> lista = new List<BolEmailRemitenteEntidad>();
            try
            {
                lista = emailRemitenteDAL.BolEmailRemitenteListarJson();
                respuesta = true;
                mensaje = "Listando Registros";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje, data = lista });
        }
        [HttpPost]
        public ActionResult BolEmailRemitenteIdObtenerJson(int email_id)
        {
            string mensaje = "No se pudo obtener el registro";
            bool respuesta = false;
            BolEmailRemitenteEntidad remitente = new BolEmailRemitenteEntidad();
            try
            {
                remitente = emailRemitenteDAL.BolEmailRemitenteIdObtenerJson(email_id);
                remitente.email_password = Seguridad.Desencriptar(remitente.email_password);
                respuesta = true;
                mensaje = "Obteniendo Registro";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje, data = remitente });
        }
        [HttpPost]
        public ActionResult BolEmailRemitenteInsertarJson(BolEmailRemitenteEntidad email)
        {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            int idInsertado = 0;
            try
            {
                email.email_password = email.email_password != null ? Seguridad.Encriptar(email.email_password) : "";
                idInsertado = emailRemitenteDAL.BolEmailRemitenteInsertarJson(email);
                if (idInsertado != 0)
                {
                    respuesta = true;
                    mensaje = "Registro Insertado";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje, data = idInsertado });
        }
        [HttpPost]
        public ActionResult BolEmailRemitenteEditarJson(BolEmailRemitenteEntidad email)
        {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try
            {
                email.email_password = email.email_password != null ? Seguridad.Encriptar(email.email_password) : "";
                respuesta = emailRemitenteDAL.BolEmailRemitenteEditarJson(email);
                if (respuesta)
                {
                    respuesta = true;
                    mensaje = "Registro Editado";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult BolEmailRemitenteEliminarJson(int email_id)
        {
            string mensaje = "No se pudo eliminar el registro";
            bool respuesta = false;
            try
            {
                respuesta = emailRemitenteDAL.BolEmailRemitenteEliminarJson(email_id);
                if (respuesta)
                {
                    respuesta = true;
                    mensaje = "Registro Eliminado";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        #endregion
        [autorizacion(false)]
        [HttpPost]
        public ActionResult BolEnviarCorreosHub(List<BolEmpleadoBoletaEntidad> listaBoletas, string connectionId)
        {
            string mensaje = "";
            bool respuesta = false;
            string remitente = ConfigurationManager.AppSettings["user_boletasgdt"].ToString();
            string password = ConfigurationManager.AppSettings["password_boletasgdt"].ToString();
            //string direccionesEnvio = ConfigurationManager.AppSettings["user_envio_boletas_dt"].ToString();
            BolEmailRemitenteEntidad remitenteUsar = new BolEmailRemitenteEntidad();
            try
            {
                var basePath = "http://" + Request.Url.Authority;
                EnvioCorreosFunction.SendProgressBoletas("Iniciando Proceso", 0, false, connectionId);
                Thread.Sleep(1000);


                if (listaBoletas.Count == 0)
                {
                    EnvioCorreosFunction.SendProgressBoletas("Proceso Terminado", 100, true, connectionId);
                    Thread.Sleep(1000);
                    mensaje = "No se seleccionaron boletas a enviar";
                    return Json(new { mensaje, respuesta });
                }


                UsuarioEntidad usuario = (UsuarioEntidad)Session["usuSGC_full"];
                string mes = "";

                decimal totalElementos = listaBoletas.Count;
                decimal limit = 1;
                decimal porcentaje = 0;

                foreach (var boleta in listaBoletas)
                {
                    string mensajeSignalr = "No se pudo enviar el correo a : ";
                    //string direccionesEnvio = "bvqr09@gmail.com bvqr09@gmail.com|||";

                    string direccionesEnvio = boleta.emp_direc_mail;
                    int periodo = Convert.ToInt32(boleta.emp_periodo) - 1;
                    mes = meses[periodo];
                    string nombreEmpleado = boleta.emp_no_trab + " " + boleta.emp_apel_pat + " " + boleta.emp_apel_mat;

                    string cuerpoMensaje = $@"
                        <div style='background: rgb(250,251,63);
                                    background: radial-gradient(circle, rgba(250,251,63,1) 0%, rgba(255,162,0,1) 100%);width: 100%;'>
                            <table style='max-width: 500px; display: table;margin:0 auto; padding: 25px;'>
                                <tbody>
                                <tr>
                                    <td colspan='2'>
                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                            <h3>{boleta.nombreEmpresa}</h3>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2'>
                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                <p style=' margin-top: 0px;
                                                margin-bottom: 0px;'>Buenos dias, se ha creado su boleta</p>
                                                    <p style=' margin-top: 10px;
                                                    margin-bottom: 0px;'><strong>Nombre:</strong> {nombreEmpleado}</p>
                                                <p style=' margin-top: 10px;
                                                margin-bottom: 0px;'><strong>Codigo:</strong> {boleta.emp_co_trab}</p>
                                                <p style=' margin-top: 10px;
                                                margin-bottom: 0px;'><strong>Mes:</strong> {mes}</p>
                                                <p style=' margin-top: 10px;
                                                margin-bottom: 0px;'><strong>Año:</strong> {boleta.emp_anio}</p>
                          
                                                <p style=' margin-top: 10px;
                                                margin-bottom: 0px;'>Puede visualizarla aquí: 
                                                    <a style='color: #213f7e; 
                                                    font-weight: bold; text-decoration: none;'
                                                        href='{basePath}/ExtranetPJ/IntranetPJ/Login'>
                                                    Link Intranet
                                                    </a>
                                                <br>
                                                </p>
                                                <p style=' margin-top: 0px;
                                                margin-bottom: 0px;'>Gracias. </p>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2'>
                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 15px; color: #000000;'>
                                            <p style='font-size: 13px;'><strong>GLADCON GROUP</strong></p>
                                        </div>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    ";

                    string asunto = "Boleta creada, Trabajador: " + nombreEmpleado;

                    porcentaje = (limit * 100 / totalElementos);
                    porcentaje = Math.Round(porcentaje, 2);
                    limit++;

                    remitenteUsar = ObtenerRemitenteUsar();
                    if (remitenteUsar.email_id != 0)
                    {
                        bool respuestaenvio = EnviarEmailBoleta(usuario.usu_id, remitenteUsar, direccionesEnvio, asunto, cuerpoMensaje);
                        if (respuesta)
                        {
                            mensajeSignalr = "Correo Enviado a : ";
                            var editadoTupla = empleadoBoletaBL.BoolEmpleadoBoletaEditarEnvioJson(boleta.emp_ruta_pdf, DateTime.Now);
                            remitenteUsar.email_ultimo_envio = DateTime.Now;
                            var remitenteEditado = emailRemitenteDAL.BolEmailRemitenteAumentarCantidadEnviosJson(remitenteUsar);
                            //Thread.Sleep(1000);
                        }
                        EnvioCorreosFunction.SendProgressBoletas(mensajeSignalr + direccionesEnvio, porcentaje, false, connectionId);
                    }
                    else
                    {
                        mensajeSignalr += "; no se encontro remitente disponible.";
                        EnvioCorreosFunction.SendProgressBoletas(mensajeSignalr + direccionesEnvio, porcentaje, false, connectionId);
                        Thread.Sleep(1000);
                    }
                }

                mensaje = "Envio Iniciado";
                respuesta = true;

                EnvioCorreosFunction.SendProgressBoletas("Proceso Terminado", 100, true, connectionId);
                Thread.Sleep(1000);

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [autorizacion(false)]
        public BolEmailRemitenteEntidad ObtenerRemitenteUsar()
        {
            List<BolEmailRemitenteEntidad> listaRemitentes = new List<BolEmailRemitenteEntidad>();
            BolEmailRemitenteEntidad remitente = new BolEmailRemitenteEntidad();
            try
            {
                listaRemitentes = emailRemitenteDAL.BolEmailRemitenteListarJson().Where(x => x.email_estado == 1).Where(x => x.email_cantidad_envios < x.email_limite).ToList();
                int MaximaCantidad = listaRemitentes.Max(obj => obj.email_cantidad_envios);//obtener el numero maximo de cantidad de envios de la lista
                //obteniendo registro con menos registros restantes
                var remitenteUso = listaRemitentes.Where(x => x.email_cantidad_envios == MaximaCantidad).FirstOrDefault();
                if (remitenteUso != null)
                {
                    remitente = remitenteUso;
                }
            }
            catch (Exception ex)
            {
                remitente = new BolEmailRemitenteEntidad();
            }
            return remitente;
        }
        public bool EnviarEmailBoleta(int usu_id, BolEmailRemitenteEntidad remitente, string destinatarios, string asunto, string body = "")
        {
            bool respuesta = false;
            BolBitacoraEntidad bitacora = new BolBitacoraEntidad();
            SmtpClient cliente;
            MailMessage email;
            try
            {
                string PasswordRemitente = Seguridad.Desencriptar(remitente.email_password);
                string ServidorSMTP = remitente.email_smtp.Trim().ToLower();
                int Puerto = Convert.ToInt32(remitente.email_puerto);
                bool SSL = Convert.ToBoolean(remitente.email_ssl);
                cliente = new SmtpClient(ServidorSMTP, Puerto)
                {
                    EnableSsl = SSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(remitente.email_direccion, PasswordRemitente)
                };
                email = new MailMessage(remitente.email_direccion, destinatarios.Trim(), asunto, body)
                {
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.Default
                };
                cliente.Send(email);
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                bitacora.btc_fecha_reg = DateTime.Now;
                bitacora.btc_accion = "Envio Correo";
                bitacora.btc_usuario_id = usu_id;
                bitacora.btc_ruta_pdf = "NO SE PUDO ENVIAR - " + "remitente: " + remitente + "; destinatario: " + destinatarios;
                var insertadoTupla = bitacoraBL.BitacoraInsertarJson(bitacora);
            }
            return respuesta;
        }
        #region MetodosExtra
        [autorizacion(false)]
        public static string ObtenerDocumentoDesdePDF(string RutaPdf)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string returnedText = string.Empty;
            string pdfText = string.Empty;
            List<string> pdfTextSplited = new List<string>();
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();
            string pattern = @"([A-Z]{1,3}-[0-9]{1,})\w+";
            try
            {
                using (PdfReader reader = new PdfReader(RutaPdf))
                {
                    iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                    pdfText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, 1, strategy);
                    pdfTextSplited = System.Text.Encoding.UTF8.GetString(System.Text.ASCIIEncoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(pdfText))).Split(' ').ToList();
                }
                foreach (var splited in pdfTextSplited)
                {
                    string textSplited = splited.Replace("\n\n", " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace(lineSeparator, " ").Replace(paragraphSeparator, " ");
                    List<string> subListSplited = textSplited.Split(' ').ToList();
                    foreach (var subList in subListSplited)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(subList.Trim(), pattern))
                        {
                            returnedText = subList.Trim();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnedText = string.Empty;
            }
            return returnedText;
        }
        [autorizacion(false)]
        public bool LimpiarDirectorio(string PathDirectorio)
        {
            bool respuesta = false;
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(PathDirectorio);
                if (di == null)
                {
                    return respuesta;
                }
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }
        [autorizacion(false)]
        static double ConvertBytesToMegabytes(long bytes)
        {
            return Math.Round(((bytes / 1024f) / 1024f), 2);
        }
        [autorizacion(false)]
        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return Math.Round((kilobytes / 1024f), 2);
        }
        [autorizacion(false)]
        public bool SincronizarEmpresas(List<TMEMPR> listaEmpresas, string directorioRaizArchivosFirma)
        {
            bool respuesta = true;
            List<BolEmpresaEntidad> listaEmpresasPostgres = new List<BolEmpresaEntidad>();
            string directorioFirma = "DIRECTORIOCERTIFICADOS";
            try
            {

                var listaEmpresasPostgresTupla = bolEmpresaBL.BolEmpresaListarJson();
                if (listaEmpresasPostgresTupla.error.Respuesta)
                {
                    listaEmpresasPostgres = listaEmpresasPostgresTupla.lista;
                    foreach (var empresaOfisis in listaEmpresas)
                    {
                        var empresaConsulta = listaEmpresasPostgres.Where(x => x.emp_co_ofisis.Equals(empresaOfisis.CO_EMPR)).FirstOrDefault();
                        if (empresaConsulta == null)
                        {
                            //insertar empresa
                            BolEmpresaEntidad empresaInsertar = new BolEmpresaEntidad();
                            empresaInsertar.emp_nomb = empresaOfisis.DE_NOMB;
                            empresaInsertar.emp_nomb_corto = empresaOfisis.DE_NOMB_CORT;
                            empresaInsertar.emp_pais = empresaOfisis.NO_PAIS;
                            empresaInsertar.emp_prov = empresaOfisis.NO_PROV;
                            empresaInsertar.emp_depa = empresaOfisis.NO_DEPA;
                            empresaInsertar.emp_rucs = empresaOfisis.NU_RUCS;
                            empresaInsertar.emp_co_ofisis = empresaOfisis.CO_EMPR;
                            empresaInsertar.emp_nom_rep_legal = empresaOfisis.NO_REPR_LEGA;
                            var InsertadoTupla = bolEmpresaBL.BolEmpresaInsertarJson(empresaInsertar);
                            respuesta = InsertadoTupla.error.Respuesta;
                        }
                        if (!respuesta)
                        {
                            break;
                        }
                    }
                }
                //crear directorios
                DirectoryInfo directorioPrincipal = Directory.CreateDirectory(Path.Combine(directorioRaizArchivosFirma) + directorioFirma);
                foreach (var empresa in listaEmpresas)
                {
                    string[] arrayNombreEmpresa = empresa.DE_NOMB.Split(' ');
                    string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                    string nombreDirectorio = empresa.CO_EMPR + "_" + nombreEmpresa;

                    DirectoryInfo directorioEmpresa = directorioPrincipal.CreateSubdirectory(nombreDirectorio);
                    DirectoryInfo directorioCertificados = directorioEmpresa.CreateSubdirectory("CERTIFICADOS");
                }

            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }
        [autorizacion(false)]
        public string DescomprimirArchivo(string nombreArchivo, string extensionArchivo, string pathInsercionArchivoTemporal)
        {
            string mensaje = string.Empty;
            try
            {
                if (extensionArchivo.ToLower().Equals(".rar"))
                {
                    using (var archive = RarArchive.Open(Path.Combine(pathInsercionArchivoTemporal, nombreArchivo)))
                    {
                        if (archive.Entries.Count == 1)
                        {
                            var entry = archive.Entries.Where(x => !x.IsDirectory).FirstOrDefault();
                            if (entry == null)
                            {
                                return string.Empty;
                            }
                            string FileExtension = Path.GetExtension(entry.Key);
                            if (!FileExtension.ToLower().Equals(".pdf"))
                            {
                                return string.Empty;
                            }
                            entry.WriteToDirectory(Path.Combine(pathInsercionArchivoTemporal), new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                            return entry.Key;
                        }
                        return string.Empty;
                    }
                }
                //else if (extensionArchivo.ToLower().Equals(".zip"))
                //{
                //    using (var archive = ZipArchive.Open(Path.Combine(pathInsercionArchivoTemporal, nombreArchivo)))
                //    {
                //        if (archive.Entries.Count == 1)
                //        {
                //            var entry = archive.Entries.Where(x => !x.IsDirectory).FirstOrDefault();
                //            if (entry == null)
                //            {
                //                return string.Empty;
                //            }
                //            string FileExtension = Path.GetExtension(entry.Key);
                //            if (!FileExtension.ToLower().Equals(".pdf"))
                //            {
                //                return string.Empty;
                //            }
                //            entry.WriteToDirectory(Path.Combine(pathInsercionArchivoTemporal), new ExtractionOptions()
                //            {
                //                ExtractFullPath = true,
                //                Overwrite = true
                //            });
                //            return entry.Key;
                //            //foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                //            //{
                //            //    entry.WriteToDirectory(Path.Combine(pathInsercionArchivoTemporal), new ExtractionOptions()
                //            //    {
                //            //        ExtractFullPath = true,
                //            //        Overwrite = true
                //            //    });
                //            //    return entry.Key;
                //            //}
                //        }
                //        return string.Empty;
                //    }
                //}
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                mensaje = string.Empty;
            }
            return mensaje;
        }
        #endregion
        [autorizacion(false)]
        [HttpPost]
        public ActionResult BolProcesarPdfV2(HttpPostedFileBase archivoProceso, DateTime fechaProcesoPdf, string empresa, string nombreEmpresa, string connectionId = "")
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string ArchivoDescomprimido = string.Empty;
            //Varialbes necesarias
            int totalRegistrosBDOFISIS = 0;
            int totalHojasPDF = 0;
            const string TIPO_CONFIGURACION = "PATH";
            const string DIRECTORIO_PROCESO = "BOLETASPROCESADAS";
            const string DIRECTORIO_A_PROCESAR = "BOLETASAPROCESAR";
            const string DIRECTORIO_RAIZ_CERTIFICADOS = "DIRECTORIOCERTIFICADOS";
            const string DIRECTORIO_CERTIFICADO_EMPRESA = "CERTIFICADOS";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            List<PersonaSqlEntidad> ListaPersonasOFISIS = new List<PersonaSqlEntidad>();
            List<BolEmpleadoBoletaEntidad> ListaRespuesta = new List<BolEmpleadoBoletaEntidad>();
            List<BolEmpleadoBoletaEntidad> ListaEliminarPOSTGRES = new List<BolEmpleadoBoletaEntidad>();
            BolEmpresaEntidad empresaProceso = new BolEmpresaEntidad();
            BolDetCertEmpresaEntidad detalleCertificado = new BolDetCertEmpresaEntidad();
            string directorioRaizAProcesar = string.Empty;
            string mensajeErrorMetodo = string.Empty;
            try
            {
                //Iniciar Signalr
                ProgressBarFunction.SendProgressBoletas("Iniciando ...", 0, false, connectionId);
                Thread.Sleep(1000);
                //Declarar variables
                int NumeroMes = fechaProcesoPdf.Month;
                string Anio = fechaProcesoPdf.Year.ToString();

                string CarpetaMes = $@"{ NumeroMes.ToString().PadLeft(2, '0')}_{meses[NumeroMes - 1]}";
                string NombreDirectorioEmpresa = $@"{empresa}_{String.Join(string.Empty, nombreEmpresa.Split(' '))}";

                //Validar Data Necesaria y directorios raiz para proceso

                if (archivoProceso == null)
                {
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    mensaje = "Archivo a procesar no encontrado";
                    mensajeConsola = "archivo proceso==null";
                    return Json(new { mensaje, respuesta });
                }
                mensaje = VerificarDataNecesaria(archivoProceso.FileName, fechaProcesoPdf, empresa, nombreEmpresa, out directorioRaizAProcesar, out empresaProceso);
                detalleCertificado = empresaProceso.DetalleCerts.Where(x=>x.det_en_uso==1).FirstOrDefault();
                if (!mensaje.Equals(string.Empty))
                {
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    mensajeConsola = "Error em metodo VerificarDataNecesaria : " + mensaje;
                    return Json(new { mensaje, respuesta });
                }
                //Descomprimir archivo

                string NombreArchivoComprimido = Path.GetFileName(archivoProceso.FileName);
                string ExtensionArchivoComprimido = Path.GetExtension(archivoProceso.FileName);
                //Verificar que sera .rar
                if (!ExtensionArchivoComprimido.ToLower().Equals(".rar"))
                {
                    mensaje = "El archivo debe tener formato .rar";
                    mensajeConsola = "Extension de Archivo: " + ExtensionArchivoComprimido;
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                //Obtener path de carpeta donde se insertara el archivo comprimido para luego extraer pdf de procesof -
                //ejemplo ruta C:\BoletasGDT\BOLETASAPROCESAR\01_IBERPERUSAC\2021\01_Enero
                string PathDirectorioProceso = Path.Combine(directorioRaizAProcesar, DIRECTORIO_A_PROCESAR, NombreDirectorioEmpresa, Anio, CarpetaMes);

                //Limpiar Directorio
                LimpiarDirectorio(PathDirectorioProceso);

                archivoProceso.SaveAs(Path.Combine(PathDirectorioProceso,NombreArchivoComprimido));
                ArchivoDescomprimido = DescomprimirArchivo(NombreArchivoComprimido, ExtensionArchivoComprimido, PathDirectorioProceso);
                //Devuelve el nombre del archivo descomprimido
                if (ArchivoDescomprimido.Equals(string.Empty))
                {
                    mensaje = "Error al Extraer Archivo, el archivo comprimido debe ser un documento .pdf";
                    mensajeConsola = "Error al descomprimir archivo";
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }

                //consultas a BD
                var ListaPersonasTuplaOFISIS = sqlbl.PersonaSQLObtenrListadoBoletasGDTJson(empresa, fechaProcesoPdf.Month, fechaProcesoPdf.Year);
                var ListaEliminarTuplaPOSTGRES = empleadoBoletaBL.BoolEmpleadoBoletaListarJson(empresa, Anio, NumeroMes.ToString());

                if (ListaPersonasTuplaOFISIS.error.Respuesta == false)
                {
                    mensaje = "Error a conectarse a OFISIS";
                    mensajeConsola = ListaPersonasTuplaOFISIS.error.Mensaje;
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }
                if (ListaEliminarTuplaPOSTGRES.error.Respuesta == false)
                {
                    mensaje = "Error a conectarse a Mesa de Partes";
                    mensajeConsola = ListaEliminarTuplaPOSTGRES.error.Mensaje;
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }

                ListaPersonasOFISIS = ListaPersonasTuplaOFISIS.lista;
                ListaEliminarPOSTGRES = ListaEliminarTuplaPOSTGRES.lista;
                //ejemplo : C:\BoletasGDT\BOLETASPROCESADAS\15_CORPORACIONPJENTRETENIMIENTOS.A.C
                string PathDirectorioEmpresaBoletasProcesadas = Path.Combine(directorioRaizAProcesar, DIRECTORIO_PROCESO, NombreDirectorioEmpresa);
                DirectoryInfo directorioRoot = Directory.CreateDirectory(PathDirectorioEmpresaBoletasProcesadas);
                //Obtener pdf del directorio BOLETASAPROCESAR
                totalRegistrosBDOFISIS = ListaPersonasOFISIS.Count();
                using (PdfReader reader = new PdfReader(Path.Combine(PathDirectorioProceso, ArchivoDescomprimido)))
                {
                    totalHojasPDF = reader.NumberOfPages;
                }
                if (totalHojasPDF != totalRegistrosBDOFISIS)
                {
                    mensaje = "Inconsistencia entre pdf y total de trabajadores";
                    mensajeConsola = "Inconsistencia entre pdf y total de trabajadores---> TotalRegistrosBD:[" + totalRegistrosBDOFISIS + "]" + "; ---- Total Hojas PDF:[" + totalHojasPDF + "]";
                    ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
                    return Json(new { mensaje, mensajeConsola, respuesta });
                }

                decimal totalElementos = totalRegistrosBDOFISIS;
                decimal limit = 0;
                decimal porcentaje = 0;
                //Eliminar data y carpetas del la fecha de proceso

                int totalRegistrosEliminar = ListaEliminarPOSTGRES.Count;
                totalElementos = totalRegistrosBDOFISIS + totalRegistrosEliminar;

                //eliminar pdfs
                foreach (var empleado in ListaEliminarPOSTGRES)
                {
                    string ArchivoEliminar = Directory.GetFiles(directorioRaizAProcesar, empleado.emp_ruta_pdf, SearchOption.AllDirectories).FirstOrDefault();
                    if (ArchivoEliminar != null)
                    {
                        System.IO.File.Delete(ArchivoEliminar);
                    }
                    porcentaje = (limit * 100 / totalElementos);
                    porcentaje = Math.Round(porcentaje, 2);

                    ProgressBarFunction.SendProgressBoletas("Limpiando Archivos ... " + empleado.emp_ruta_pdf, porcentaje, false, connectionId);
                    limit++;
                }
                //Eliminar de BD
                ProgressBarFunction.SendProgressBoletas("Limpiando Base de Datos ...", 30, false, connectionId);
                var eliminadoTupla = empleadoBoletaBL.BoolEmpleadoBoletaEliminarMasivoJson(empresa, Anio, NumeroMes.ToString());

                int TotalInsertados = 0;
                //Procesar PDF Grande
                using (PdfReader reader = new PdfReader(Path.Combine(PathDirectorioProceso, ArchivoDescomprimido)))
                {
                    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
                    {
                        BolEmpleadoBoletaEntidad empleado = new BolEmpleadoBoletaEntidad();
                        string NroDocumento = ObtenerNroDocumentDesdePDF(reader,pagenumber);
                        var item = ListaPersonasOFISIS.Where(x=>x.CO_TRAB.ToLower().Trim().Equals(NroDocumento.Trim())).FirstOrDefault();
                        if (item != null)
                        {
                            string directorioEmpleado = item.CO_EMPR + "_" + item.CO_TRAB;
                            string filename = item.CO_TRAB + "_" + item.CO_EMPR + "_" + Anio + "_" + NumeroMes.ToString() + ".pdf";
                            DirectoryInfo subdirectorioEmpleado = directorioRoot.CreateSubdirectory(directorioEmpleado);
                            Document document = new Document();
                            PdfCopy copy = new PdfCopy(document, new FileStream(Path.Combine(subdirectorioEmpleado.FullName, filename), FileMode.Create));
                            document.Open();
                            copy.AddPage(copy.GetImportedPage(reader, pagenumber));
                            document.Close();

                            //FirmarPDF
                            var rutaCertificado = Path.Combine(
                                               directorioRaizAProcesar,
                                               DIRECTORIO_RAIZ_CERTIFICADOS,
                                               NombreDirectorioEmpresa,
                                               DIRECTORIO_CERTIFICADO_EMPRESA,
                                               detalleCertificado.det_ruta_cert
                                               );
                            var rutaImagen = Path.Combine(
                                    directorioRaizAProcesar,
                                    DIRECTORIO_RAIZ_CERTIFICADOS,
                                    NombreDirectorioEmpresa
                                    );
                            var certificado = new Certificado(
                                rutaCertificado,
                                detalleCertificado.det_pass_cert
                                );
                            var firmante = new Firmante(certificado);
                            string secondFileName = item.CO_TRAB + "_" + item.CO_EMPR + "_" + Anio + "_" + NumeroMes + "_signed.pdf";
                            if (System.IO.File.Exists(rutaCertificado))
                            {
                                firmante.Firmar(
                                    Path.Combine(subdirectorioEmpleado.FullName, filename),
                                    Path.Combine(subdirectorioEmpleado.FullName, secondFileName),
                                    empresaProceso,
                                    rutaImagen
                                    );
                                System.IO.File.Delete(Path.Combine(subdirectorioEmpleado.FullName, filename));
                            }
                            //Termino la firma
                            empleado.emp_co_trab = item.CO_TRAB;
                            empleado.emp_co_empr = item.CO_EMPR;
                            empleado.emp_anio = Anio;
                            empleado.emp_periodo = Convert.ToString(NumeroMes);
                            empleado.emp_ruta_pdf = secondFileName;
                            empleado.emp_no_trab = item.NO_TRAB;
                            empleado.emp_apel_pat = item.NO_APEL_PATE;
                            empleado.emp_apel_mat = item.NO_APEL_MATE;
                            empleado.emp_direc_mail = item.NO_DIRE_MAI1;
                            empleado.emp_nro_cel = item.NU_TLF1;
                            empleado.emp_tipo_doc = item.TI_DOCU_IDEN;
                            empleado.emp_fecha_reg = DateTime.Now;
                            empleado.emp_enviado = 0;
                            empleado.emp_descargado = 0;
                            //Insertar a BD
                            bool insertado= empleadoBoletaBL.BoolEmpleadoBoletaInsertarJson(empleado);
                            empleado.boletaCreada = insertado;
                            ListaRespuesta.Add(empleado);
                            porcentaje = (limit * 100 / totalElementos);
                            porcentaje = Math.Round(porcentaje, 2);
                            limit++;
                            if (insertado)
                            {
                                TotalInsertados++;
                                ProgressBarFunction.SendProgressBoletas("Creando Pdf ... " + empleado.emp_ruta_pdf, porcentaje, false, connectionId);
                            }
                            else
                            {
                                ProgressBarFunction.SendProgressBoletas("No se pudo crear ... " + empleado.emp_ruta_pdf, porcentaje, false, connectionId);
                            }
                            
                        }
                    }
                }
                
                ProgressBarFunction.SendProgressBoletas("Proceso Terminado", 100, true, connectionId);
                respuesta = true;
                mensaje = "Proceso Terminado";
            }
            catch (Exception ex)
            {
                mensaje = "Error";
                mensajeConsola = ex.Message;
            }

            return Json(new { data = ListaRespuesta, mensaje, mensajeConsola, respuesta });
        }
        [autorizacion(false)]
        public string VerificarDataNecesaria(
            string nombreArchivo,
            DateTime fechaProcesoPdf,
            string codEmpresa,
            string nombreEmpresa,
            out string directorioRaizAProcesar,
            out BolEmpresaEntidad empresaProceso)
        {
            const string TIPO_CONFIGURACION = "PATH";
            const string DIRECTORIO_A_PROCESAR = "BOLETASAPROCESAR";
            string mensaje = string.Empty;
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            BolEmpresaEntidad empresaRetorno = new BolEmpresaEntidad();
            string directorioBase = string.Empty;
            string mensajeError = string.Empty;
            try
            {

                int numeroMes = fechaProcesoPdf.Month;

                string carpetaMes = numeroMes.ToString().PadLeft(2, '0') + "_" + meses[numeroMes - 1];

                string anio = Convert.ToString(fechaProcesoPdf.Year);

                //Nombre Directorio de la Empresa
                string nombreDirectorioEmpresa = $@"{codEmpresa}_{String.Join(string.Empty, nombreEmpresa.Split(' '))}";

                if (nombreArchivo.Equals(string.Empty))
                {
                    mensaje = "Archivo Comprimido Obligatorio";
                }
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(TIPO_CONFIGURACION);
                if (!configuracionTupla.error.Respuesta && configuracionTupla.configuracion.config_id == 0)
                {
                    mensaje = "No se pudo encontrar el registro de configuracion en intranet.bol_configuracion";
                }
                configuracion = configuracionTupla.configuracion;

                string pathDirectorioInsercionArchivoComprimido = Path.Combine(configuracion.config_valor, DIRECTORIO_A_PROCESAR, nombreDirectorioEmpresa, anio, carpetaMes);
                if (!Directory.Exists(pathDirectorioInsercionArchivoComprimido))
                {
                    mensaje = "No se pudo encontrar el directorio de proceso";
                }

                var empresaPostgres = bolEmpresaBL.BolEmpresaObtenerxOfisisIdJson(codEmpresa);
                if (empresaPostgres.empresa.DetalleCerts.Where(x => x.det_en_uso == 1).FirstOrDefault() == null)
                {
                    mensaje = "No existe certificado a usar para esta empresa";
                }
                directorioBase = configuracion.config_valor;
                empresaRetorno = empresaPostgres.empresa;
            }
            catch (Exception ex)
            {
                mensaje = string.Empty;
                empresaRetorno = new BolEmpresaEntidad();
            }
            directorioRaizAProcesar = directorioBase;
            empresaProceso = empresaRetorno;
            return mensaje;
        }
        [autorizacion(false)]
        public static string ObtenerNroDocumentDesdePDF(PdfReader reader, int pagina)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string pattern = @"^([0-9]{8,20})$";
            try
            {
                Rectangle mediabox = reader.GetPageSize(pagina);

                Rectangle rect = new Rectangle(mediabox.Left + 45, mediabox.Top - 160, mediabox.Left + 100, mediabox.Top - 110);


                iTextSharp.text.pdf.parser.RenderFilter[] filter = { new iTextSharp.text.pdf.parser.RegionTextRenderFilter(rect) };
                iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string lineSeparator = ((char)0x2028).ToString();
                string paragraphSeparator = ((char)0x2029).ToString();

                strategy = new iTextSharp.text.pdf.parser.FilteredTextRenderListener(new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy(), filter);
                //sb.AppendLine(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                string texto = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);

                List<string> listasplited = texto.Split(' ').ToList();
                List<string> listaSplitedFinal = new List<string>();
                foreach (var splited in listasplited)
                {
                    string text = splited.Replace("\n\n", " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace(lineSeparator, " ").Replace(paragraphSeparator, " ");
                    List<string> subListaSplited = text.Split(' ').ToList();
                    foreach (var subLista in subListaSplited)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(subLista.Trim(), pattern))
                        {
                            return subLista.Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }
}