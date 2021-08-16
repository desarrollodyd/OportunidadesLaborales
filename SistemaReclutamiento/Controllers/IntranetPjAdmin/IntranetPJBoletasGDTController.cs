using iTextSharp.text;
using iTextSharp.text.pdf;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.BoletasGDT;
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

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion]
    public class IntranetPJBoletasGDTController : Controller
    {
        SQLModel sqlbl = new SQLModel();
        BolConfiguracionModel bolConfigBL = new BolConfiguracionModel();
        BolEmpleadoBoletaModel empleadoBoletaBL = new BolEmpleadoBoletaModel();
        BolBitacoraModel bitacoraBL = new BolBitacoraModel();

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
            catch(Exception ex)
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
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta,idInsertado });
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
            return Json(new { mensaje, respuesta,idInsertado });
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
            try
            {
                var listaEmpresaTupla = sqlbl.EmpresaListarJson();
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);

                if (listaEmpresaTupla.error.Mensaje.Equals(string.Empty)&&configuracionTupla.error.Mensaje.Equals(string.Empty))
                {
                    listaempresa = listaEmpresaTupla.listaempresa;
                    configuracion = configuracionTupla.configuracion;
                    //Crear directorio principal
                    DirectoryInfo directorioPrincipal =Directory.CreateDirectory(Path.Combine(configuracion.config_valor+directorioHijo));
                    //Creacion de arbol de directorios
                    foreach (var empresa in listaempresa)
                    {
                       
                        string[] arrayNombreEmpresa = empresa.DE_NOMB.Split(' ');
                        string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                        string nombreDirectorio = empresa.CO_EMPR + "_" + nombreEmpresa;

                        DirectoryInfo directorioEmpresa =directorioPrincipal.CreateSubdirectory(nombreDirectorio);
                        DirectoryInfo directorioAnio = directorioEmpresa.CreateSubdirectory(Convert.ToString(anio));

                        List<dynamic> listaDirectorioAnio = new List<dynamic>();
                        List<dynamic> listaDirectorioMes = new List<dynamic>();
                        int nroMes = 1;
                        foreach (var mes in meses) {
                            string iconoPdf = "pdf_flat.png";
                            DirectoryInfo directorioMes = directorioAnio.CreateSubdirectory(nroMes.ToString().PadLeft(2, '0')+"_"+mes);
                            nroMes++;
                            FileInfo[] filesMes = directorioMes.GetFiles();

                            List<dynamic> listFilesMes =new List<dynamic>();
                            //var direccion = Server.MapPath("/") + Request.ApplicationPath;

                            foreach (var file in filesMes) {
                                double mbytes = ConvertBytesToMegabytes(file.Length);
                                listFilesMes.Add(new {
                                    name = file.Name + " \t \t "+mbytes+"Mb.",
                                    icon = direccion + "/Content/intranetSGC/jqueryztree/css/zTreeStyle/img/diy/" + iconoPdf,
                                });
                            }

                            listaDirectorioMes.Add(new {
                                name = directorioMes.Name,
                                open = false,
                                children=listFilesMes
                            });
                        }
                        listaDirectorioAnio.Add(new {
                            name = directorioAnio.Name,
                            children = listaDirectorioMes,
                            open = false
                        });
                        listaDirectorioEmpresa.Add(new {
                            name=directorioEmpresa.Name,
                            open=false,
                            children=listaDirectorioAnio
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
            return Json(new { mensaje, respuesta,data=listaDirectorioEmpresa });
        }

        public ActionResult BolListarPdfJson(DateTime fechaListar,string empresaListar,string nombreEmpresaListar)
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
            return Json(new { mensaje,respuesta,data=listaBoletas });
        }
        public ActionResult BolListarporEmpleadoJson(DateTime fechaProcesoInicio,DateTime fechaProcesoFin, string empresaListar, string nombreEmpresaListar,string empleado)
        {
            string mensaje = "No se pudieron listar las boletas";
            bool respuesta = false;
            List<BolEmpleadoBoletaEntidad> listaBoletas = new List<BolEmpleadoBoletaEntidad>();
            try
            {
                int mesInicio = fechaProcesoInicio.Month;
                int anioInicio = fechaProcesoInicio.Year;
                int mesFin = fechaProcesoFin.Month;
                int anioFin= fechaProcesoFin.Year;

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
                var listaBoletasTupla = empleadoBoletaBL.BoolEmpleadoBoletaListarxEmpleadoEmpresaFechasJson(empresaListar,empleado,stringAnio);

                if (listaBoletasTupla.error.Mensaje.Equals(string.Empty))
                {
                    listaBoletas = listaBoletasTupla.lista;
                    foreach(var anio in _anios)
                    {
                        if (anio == anioInicio)
                        {
                            for (int i = 1; i < mesInicio ; i++)
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
                            for (int i = mesFin+1; i <= 12; i++)
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
        public ActionResult BolProcesarPdf2(DateTime fechaProcesoPdf, string empresa, string nombreEmpresa, string connectionId)
        {
            string mensaje = "No se pudieron procesar las boletas";
            bool respuesta = false;
            string mensajeConsola = "";
            int totalRegistrosBD = 0;
            int totalHojas = 0;
            string tipoConfiguracion = "PATH";
            string directorioProceso = "BOLETASPROCESADAS";
            string directorioaProcesar = "BOLETASAPROCESAR";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            List<PersonaSqlEntidad> listaPersonas = new List<PersonaSqlEntidad>();
            List<BolEmpleadoBoletaEntidad> listaInsertar = new List<BolEmpleadoBoletaEntidad>();
            List<BolEmpleadoBoletaEntidad> listaEliminar = new List<BolEmpleadoBoletaEntidad>();
            try
            {
                DateTime fechaProceso = fechaProcesoPdf;
                int mes = fechaProceso.Month;
                string carpetaMes = mes.ToString().PadLeft(2, '0') + "_" + meses[mes - 1];
                string anio = Convert.ToString(fechaProceso.Year);
                ProgressBarFunction.SendProgressBoletas("Iniciando ...", 0, false, connectionId);
                Thread.Sleep(1000);
                var listaPersonasTupla = sqlbl.PersonaSQLObtenrListadoBoletasGDTJson(empresa, fechaProceso.Month, fechaProceso.Year);
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                var listaEliminarTupla = empleadoBoletaBL.BoolEmpleadoBoletaListarJson(empresa, anio, mes.ToString());
                string[] arrayNombreEmpresa = nombreEmpresa.Split(' ');
                string nombreDirectorioEmpresa = empresa + "_" + String.Join("", arrayNombreEmpresa);
                if (listaPersonasTupla.error.Mensaje.Equals(string.Empty))
                {
                    configuracion = configuracionTupla.configuracion;
                    listaPersonas = listaPersonasTupla.lista;
                    string pathPdf = Path.Combine(configuracion.config_valor, directorioaProcesar, nombreDirectorioEmpresa, anio, carpetaMes);

                    DirectoryInfo directorioRoot = Directory.CreateDirectory(Path.Combine(configuracion.config_valor, directorioProceso, nombreDirectorioEmpresa));

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
                                        string[] subdirectoryEntries = Directory.GetDirectories(Path.Combine(configuracion.config_valor, directorioaProcesar));
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
                                        empleado.emp_co_trab = item.CO_TRAB;
                                        empleado.emp_co_empr = item.CO_EMPR;
                                        empleado.emp_anio = anio;
                                        empleado.emp_periodo = Convert.ToString(mes);
                                        empleado.emp_ruta_pdf = filename;
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
                                        ProgressBarFunction.SendProgressBoletas("Creando Pdf ... " +empleado.emp_ruta_pdf ,porcentaje, false, connectionId);
                                    }
                                    //llenado en base de datos

                                    string consulta = "";
                                    int totalInsertados = 0;
                                    foreach (var empleado in listaInsertar)
                                    {
                                        consulta += String.Format("('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}'),",
                                            empleado.emp_co_trab,
                                            empleado.emp_co_empr,
                                            empleado.emp_anio,
                                            empleado.emp_periodo,
                                            empleado.emp_ruta_pdf,
                                            empleado.emp_enviado,
                                            empleado.emp_descargado,
                                            empleado.emp_fecha_reg.ToString("yyyy-MM-dd HH:mm:ss"),
                                            empleado.emp_no_trab,
                                            empleado.emp_apel_pat,
                                            empleado.emp_apel_mat,
                                            empleado.emp_direc_mail,
                                            empleado.emp_nro_cel,
                                            empleado.emp_tipo_doc
                                            );
                                    }
                                    consulta = consulta.TrimEnd(',');

                                    var totalInsertadosTupla = empleadoBoletaBL.BoolEmpleadoBoletaInsertarMasivoJson(consulta);
                                    if (totalInsertadosTupla.error.Mensaje.Equals(string.Empty))
                                    {
                                        totalInsertados = totalInsertadosTupla.totalInsertados;
                                    }
                                    if (totalInsertados == listaPersonas.Count)
                                    {
                                        mensaje = "PDFs procesados";
                                        mensajeConsola = "PDFs procesados---> TotalRegistrosBD:[" + totalRegistrosBD + "]" + "; ---- Total Hojas PDF:[" + totalHojas+"]"; 
                                        respuesta = true;
                                        ProgressBarFunction.SendProgressBoletas("Registros Insertados ...", porcentaje,false, connectionId);
                                        Thread.Sleep(1000);
                                        ProgressBarFunction.SendProgressBoletas("Proceso Terminado ...",100, true, connectionId);
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
;            }
            if (!respuesta)
            {
                ProgressBarFunction.SendProgressBoletas(mensaje, 99, false, connectionId);
                Thread.Sleep(2000);
                ProgressBarFunction.SendProgressBoletas(mensaje, 100, true, connectionId);
            }
            return Json(new { data = listaInsertar, mensaje, respuesta,mensajeConsola });
        }
        [HttpPost]
        public ActionResult EnviarBoletasEmailJson(List<BolEmpleadoBoletaEntidad> listaBoletas) {
            string mensaje = "";
            bool respuesta = false;
            string remitente = ConfigurationManager.AppSettings["user_boletasgdt"].ToString();
            string password = ConfigurationManager.AppSettings["password_boletasgdt"].ToString();
            string direccionesEnvio= ConfigurationManager.AppSettings["user_envio_boletas_dt"].ToString();
            try
            {
                if (listaBoletas.Count > 0)
                {
                    UsuarioEntidad usuario=(UsuarioEntidad)Session["usuSGC_full"];
                    string mes = "";
                    foreach (var boleta in listaBoletas)
                    {
                        //string direccionesEnvio = boleta.emp_direc_mail;
                        int periodo = Convert.ToInt32(boleta.emp_periodo)-1;
                        mes = meses[periodo];
                        //string direccionesEnvio = "diego.canchari@gladcon.com";
                        string nombreEmpleado = boleta.emp_no_trab + " " + boleta.emp_apel_pat + " " + boleta.emp_apel_mat;
                        string cuerpoMensaje = ("Buenos dias, se ha creado su boleta <br>" +
                             " <br>Mes : "+mes+" <br>Año : "+boleta.emp_anio +"<br>Cod. Trabajador :"+boleta.emp_co_trab+
                             "<br>Empresa: "+boleta.nombreEmpresa+
                             " <br>Puede visualizarla en:"+
                             " <h3><a href='http://181.65.130.36:2222/ExtranetPJ/IntranetPJ/Login'><strong>Link de Intranet Gladcon</strong></a></h3>" +
                             "<br>");
                        string asunto = "Boleta creada, Trabajador: " + nombreEmpleado ;
                        Task.Run(() =>
                        {
                            Task oResp = EnviarCorreoAsync(usuario.usu_id,remitente, password, direccionesEnvio, asunto, cuerpoMensaje);
                        })/*.ContinueWith(t =>
                        {
                            if (t.IsCompleted)
                            {
                                var editadoTupla = empleadoBoletaBL.BoolEmpleadoBoletaEditarEnvioJson(boleta.emp_ruta_pdf, DateTime.Now);
                            }
                        })*//*.GetAwaiter().GetResult()*/;
                    }
                    mensaje = "Envio Iniciado";
                    respuesta = true;
                }
                else {
                    mensaje = "No se encontro registros a enviar";
                }
              
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta });
        }
        public async Task<bool> EnviarCorreoAsync(int usu_id,string remitente, string password, string destinatarios,string asunto, string body = "")
        {
            bool respuesta = false;
            SmtpClient cliente;
            MailMessage email;
            BolBitacoraEntidad bitacora = new BolBitacoraEntidad();
            try
            {
                cliente = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(remitente, password)
                };
                email = new MailMessage(remitente, destinatarios.Trim(), asunto, body)
                {
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.Default
                };
                await cliente.SendMailAsync(email).ContinueWith(t=> {
                    bitacora.btc_fecha_reg = DateTime.Now;
                    bitacora.btc_accion = "Envio Correo";
                    bitacora.btc_usuario_id = usu_id;
                    bitacora.btc_ruta_pdf = "ENVIADO - "+"remitente: " + remitente+"; destinatario: "+destinatarios;

                    var insertadoTupla = bitacoraBL.BitacoraInsertarJson(bitacora);
                }) ;
               
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                bitacora.btc_fecha_reg = DateTime.Now;
                bitacora.btc_accion = "Envio Correo";
                bitacora.btc_usuario_id = usu_id;
                bitacora.btc_ruta_pdf = "NO SE PUDO ENVIAR - "+"remitente: " + remitente + "; destinatario: " + destinatarios;
                var insertadoTupla = bitacoraBL.BitacoraInsertarJson(bitacora);
            }
            return respuesta;
        }
        static double ConvertBytesToMegabytes(long bytes)
        {
            return Math.Round(((bytes / 1024f) / 1024f), 2);
        }

        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return Math.Round((kilobytes / 1024f), 2);
        }
        [HttpPost]
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
                    string pathArchivo = Path.Combine(configuracionTupla.configuracion.config_valor,directorioProceso, nombreDirectorioEmpresa, nombreDirectorioEmpleado, nombreArchivo);
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
            return Json(new { data,mensaje,respuesta,fileName});
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
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new {respuesta,mensaje });
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
                    listaBitacoras = listaTupla.lista.OrderByDescending(x=>x.btc_fecha_reg).ToList();
                    mensaje = "Listando Registros";
                    respuesta = true;
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listaBitacoras });
        }
        [HttpPost]
        public JsonResult LongRunningProcess(string connectionId)
        {
            int itemsCount = 100;
            int limit = 0;
            bool hide = false;
            for (int i = 0; i <= itemsCount; i++)
            {
                //simulating some task
                Thread.Sleep(100);
                //calling a function that calculattes percentaje and sends the data to the client
                if (limit == itemsCount)
                {
                    hide = true;
                }
                ProgressBarFunction.SendProgress("Process in progress...", i, itemsCount, hide, connectionId);
                limit++;
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}