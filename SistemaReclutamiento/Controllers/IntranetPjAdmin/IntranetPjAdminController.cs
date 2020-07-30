using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Entidades.FichaCumplimiento;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;

using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    public class IntranetPjAdminController : Controller
    {
        IntranetUsuarioModel usuarioIntranetbl = new IntranetUsuarioModel();
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetAccesoModel usuarioAccesobl = new IntranetAccesoModel();
        IntranetDetalleElementoModel detalleelementobl = new IntranetDetalleElementoModel();
        IntranetDetalleElementoModalModel detalleelementomodalbl = new IntranetDetalleElementoModalModel();
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();

        CumUsuarioModel cumusubl = new CumUsuarioModel();
        CumEnvioModel cumenviobl = new CumEnvioModel();
        CumEnvioDetModel cumenviodetbl = new CumEnvioDetModel();
        CumUsuarioExcelModel cumusuexcelbl = new CumUsuarioExcelModel();

        SQLModel sqlbl = new SQLModel();

        IntranetFichaModel fichabl = new IntranetFichaModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        claseError error = new claseError();
        RutaImagenes rutaImagenes = new RutaImagenes();
        // GET: IntranetPjAdmin
        public ActionResult Index()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJAdminIndex.cshtml");
        }

        public ActionResult PanelMenus()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJMenus.cshtml");
        }
        public ActionResult PanelActividades()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJActividades.cshtml");
        }
        public ActionResult PanelComentarios()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJComentarios.cshtml");
        }
        public ActionResult PanelFooter()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFooter.cshtml");
        }
        public ActionResult PanelArchivos()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJArchivos.cshtml");
        }
        public ActionResult PanelFichas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFichas.cshtml");
        }

        public ActionResult FichaFormulario(string id)
        {
            var envio_id = Seguridad.Desencriptar(id);
            ViewBag.envioid = envio_id;
            return View("~/Views/IntranetPJAdmin/IntranetPJFichaFormulario.cshtml");
        }
        public ActionResult PanelSubidaExcel()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJSubidaExcel.cshtml");
        }
        [HttpPost]
        public ActionResult IntranetFichasEmpleadoListarJson(DateTime desde,DateTime hasta , int estado)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_envio> listaEnvios = new List<cum_envio>();
            try
            {
                string tipo = "EMPLEADO";
                var envioTupla = fichabl.IntranetFichaListarJson(tipo, desde, hasta);
                error = envioTupla.error;
                listaEnvios = envioTupla.intranetFichaLista.Where(x=>x.env_estado.Equals(estado.Trim())).ToList();
                if (error.Key.Equals(string.Empty))
                {
                    var txtids = new List<dynamic>();
                    foreach (var p in listaEnvios)
                    {
                        var itemarray = p.cus_dni.Split('|');
                        txtids.Add(itemarray[0]);
                    }

                    var ids = "";
                    ids = "  in(" + "'" + String.Join("','", txtids) + "'" + ")";
                    var lista = sqlbl.PersonaSQLJson(ids);
                    var listadopersonas = lista.lista;

                    foreach (var p in listaEnvios)
                    {
                        var itemarray = p.cus_dni.Split('|');
                        var persona = listadopersonas.Select(x=>new {x.CO_TRAB,x.NO_TRAB,x.NO_APEL_PATE,x.NO_APEL_MATE }).Where(z=>z.CO_TRAB== itemarray[0]).ToList();
                        if (persona.Count > 0)
                        {
                            p.per_nombre = persona[0].NO_TRAB;
                            p.per_apellido_pat = persona[0].NO_APEL_PATE;
                            p.per_apellido_mat= persona[0].NO_APEL_MATE;
                        }
                        
                    }

                    mensaje = "Listando Fichas";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Fichas";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaEnvios.ToList(), respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetFichasPostulanteListarJson(DateTime desde, DateTime hasta, int estado)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_envio> listaEnvios = new List<cum_envio>();
            try
            {
                string tipo = "POSTULANTE";
                var envioTupla = fichabl.IntranetFichaPostListarJson(desde, hasta);
                error = envioTupla.error;
                listaEnvios = envioTupla.intranetFichaLista.Where(x=>x.env_estado.Equals(estado.Trim())).ToList();
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Fichas";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Fichas";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaEnvios.ToList(), respuesta, mensaje, mensajeconsola = mensajeConsola });
        }


        [HttpPost]
        public ActionResult EnviarJson(string[] listaEmpleados)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_usuario> listausuarios = new List<cum_usuario>();
            CumUsuarioEntidad cumusuario = new CumUsuarioEntidad();
            CumEnvioEntidad cumenvio = new CumEnvioEntidad();
            CumEnvioDetalleEntidad cumenviodet = new CumEnvioDetalleEntidad();
            var correopersonal = "";
            var correocorporativo = "";
            var clave = "";
            int idcumusu = 0;
            try
            {
               
                foreach (var item in listaEmpleados)
                {
                    var itemarray = item.Split('|');


                    var usuarioTupla = fichabl.IntranetUsuarioListarJson(itemarray[0],"EMPLEADO");
                    correopersonal = itemarray[2].Trim();
                    correocorporativo = itemarray[1].Trim(); ;
                    var cumusuarioExiste = usuarioTupla.intranetCumusuarioLista;
                    int existe = usuarioTupla.intranetCumusuarioLista.Count;

                    string path = Path.GetRandomFileName();
                    path = path.Replace(".", "");
                    clave = path.Substring(0, 8).Trim();
                
                    if (existe > 0)
                    {

                        cumusuario.cus_correo = correopersonal;
                        cumusuario.cus_id = cumusuarioExiste[0].cus_id;
                        var usuaupdate = cumusubl.CumUsuarioEditarcorreoJson(cumusuario);
                        idcumusu = cumusuarioExiste[0].cus_id;
                    }
                    else
                    {
                        cumusuario.cus_estado = "A";
                        cumusuario.cus_firma = "";
                        cumusuario.cus_dni = itemarray[0].ToString();
                        cumusuario.cus_correo = correopersonal;
                        cumusuario.cus_tipo = "EMPLEADO";
                        cumusuario.cus_fecha_reg = DateTime.Now;
                        cumusuario.cus_fecha_act = DateTime.Now;
                        cumusuario.cus_clave = clave;
                        var usuainsert = cumusubl.CumUsuarioInsertarsinfkuserJson(cumusuario);
                        idcumusu = usuainsert.idInsertado;
                    }

                    if (idcumusu > 0)
                    {
                        cumenvio.env_fecha_reg = DateTime.Now;
                        cumenvio.env_fecha_act = DateTime.Now;
                        cumenvio.env_estado = "1";
                        cumenvio.fk_cuestionario = 1;
                        cumenvio.fk_usuario = idcumusu;
                        var envio = cumenviobl.CumEnvioInsertarJson(cumenvio);

                        if (envio.idInsertado > 0)
                        {
                            cumenviodet.end_fecha_reg = DateTime.Now;
                            cumenviodet.end_fecha_act = DateTime.Now;
                            cumenviodet.end_estado = "1";
                            cumenviodet.end_dni = item.Trim();
                            cumenviodet.fk_envio = envio.idInsertado;
                            cumenviodet.end_correo_corp = correocorporativo;
                            cumenviodet.end_correo_pers = correopersonal;
                            var enviodet = cumenviodetbl.CumEnvioDetalleInsertarJson(cumenviodet);

                            //////envio correo aqui/////
                            Correo correo_enviar = new Correo();
                            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                            string nombre = "";

                            cumenvio.env_fecha_act = DateTime.Now;
                            cumenvio.env_estado = "1";
                            cumenvio.env_id = envio.idInsertado;
                            string correo = (correopersonal == "" ? correocorporativo : correopersonal);
                            string encriptado = Seguridad.Encriptar(envio.idInsertado.ToString());
                            var estado = cumenviobl.CumEnvioEditarJson(cumenvio);
                            correo_enviar.EnviarCorreo(
                             correo,
                             "Link de Ficha Sintomatológica",
                             "Hola! : " + nombre + " \n " +
                             "Tu clave es la que necesitaras para guardar tu ficha : " + clave + " \n " +
                             "Ingrese al siguiente Link y complete el formulario"
                             + "\n solo se puede editar el mismo dia de envio, \n" +
                             " Link Ficha Sintomatológica : " + basepath + "IntranetPJAdmin/FichaFormulario?id=" + encriptado
                             );

                        }

                    }
                }

                mensaje = "Fichas Registradas";
                respuesta = true;

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult ReEnviarJson(int envioID)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            CumEnvioEntidad envio = new CumEnvioEntidad();
            try
            {
                var envioTupla = cumenviobl.CumEnvioIdObtenerJson(envioID);
                error = envioTupla.error;
                envio = envioTupla.cumEnvio;
                if (envio.fk_usuario > 0)
                {
                    int cususuario = envio.fk_usuario;
                    var cum_usua = cumusubl.CumUsuarioIdObtenerJson(cususuario);
                    var entidadusuario = cum_usua.cumUsuario;
                    var clave = entidadusuario.cus_clave.Trim();
                    var correo = entidadusuario.cus_correo.Trim();
                    var nombre = "";
                    var id = envioID.ToString();
                    var encriptado = Seguridad.Encriptar(id);
                    Correo correo_enviar = new Correo();
                    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    //MailMessage message = new MailMessage("s3k.zimbra@gmail.com", persona.per_correoelectronico, "correo de confirmacion", cuerpo_correo);
                    envio.env_fecha_act = DateTime.Now;
                    envio.env_estado = "3";
                    envio.env_id = envioID;
                    var estado = cumenviobl.CumEnvioEditarJson(envio);
                    correo_enviar.EnviarCorreo(
                        correo,
                        "Link de Ficha Sintomatológica",
                        "Hola! : " + nombre + " \n " +
                        "Tu clave es la que necesitaras para guardar tu ficha : " + clave + " \n " +
                        "Ingrese al siguiente Link y complete el formulario"
                        + "\n solo se puede editar el mismo dia de envio, \n" +
                        " Link Ficha Sintomatológica : " + basepath + "IntranetPJAdmin/FichaFormulario?id=" + encriptado
                        );

                    mensaje = "Se reenvio Ficha";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No Se pudo reenviar Ficha";
                    respuesta = false;
                }
                

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult listaPostulantes()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<UsuarioEntidadPostulante> lista = new List<UsuarioEntidadPostulante>();
            try
            {

                var envioTupla = usuariobl.PostulantesListarJson();
                error = envioTupla.error;
                lista = envioTupla.lista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Postulantes";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Postulantes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult EnviarPJson(string[] listaPostulantes)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_usuario> listausuarios = new List<cum_usuario>();
            CumUsuarioEntidad cumusuario = new CumUsuarioEntidad();
            CumEnvioEntidad cumenvio = new CumEnvioEntidad();
            CumEnvioDetalleEntidad cumenviodet = new CumEnvioDetalleEntidad();
            var correopersonal = "";
            var dni = "";
            var clave = "";
            int idcumusu = 0;
            try
            {

                foreach (var item in listaPostulantes)
                {
                    var itemarray = item.Split('|');

                    var usuarioTupla = fichabl.IntranetUsuarioListarJson(itemarray[1],"POSTULANTE");
                    dni = itemarray[1].Trim();
                    correopersonal = itemarray[2].Trim();
                    var cumusuarioExiste = usuarioTupla.intranetCumusuarioLista;
                    int existe = usuarioTupla.intranetCumusuarioLista.Count;

                    string path = Path.GetRandomFileName();
                    path = path.Replace(".", "");
                    clave = path.Substring(0, 8).Trim();

                    if (existe > 0)
                    {
                       
                        cumusuario.cus_correo = correopersonal;
                        cumusuario.cus_id = cumusuarioExiste[0].cus_id;
                        var usuaupdate = cumusubl.CumUsuarioEditarcorreoJson(cumusuario);
                        idcumusu = cumusuarioExiste[0].cus_id;
                    }
                    else
                    {
                        cumusuario.cus_estado = "A";
                        cumusuario.cus_firma = "";
                        cumusuario.cus_dni = dni;
                        cumusuario.cus_correo = correopersonal;
                        cumusuario.cus_tipo = "POSTULANTE";
                        cumusuario.cus_fecha_reg = DateTime.Now;
                        cumusuario.cus_fecha_act = DateTime.Now;
                        cumusuario.cus_clave = clave;
                        cumusuario.fk_usuario = Convert.ToInt32(itemarray[0]);
                        var usuainsert = cumusubl.CumUsuarioPostInsertarsinfkuserJson(cumusuario);
                        idcumusu = usuainsert.idInsertado;
                    }

                    if (idcumusu > 0)
                    {
                        cumenvio.env_fecha_reg = DateTime.Now;
                        cumenvio.env_fecha_act = DateTime.Now;
                        cumenvio.env_estado = "1";
                        cumenvio.fk_cuestionario = 1;
                        cumenvio.fk_usuario = idcumusu;
                        var envio = cumenviobl.CumEnvioInsertarJson(cumenvio);

                        if (envio.idInsertado > 0)
                        {
                            cumenviodet.end_fecha_reg = DateTime.Now;
                            cumenviodet.end_fecha_act = DateTime.Now;
                            cumenviodet.end_estado = "1";
                            cumenviodet.end_dni = item;
                            cumenviodet.fk_envio = envio.idInsertado;
                            cumenviodet.end_correo_pers = correopersonal;
                            var enviodet = cumenviodetbl.CumEnvioDetalleInsertarJson(cumenviodet);

                            //////envio correo aqui/////
                            Correo correo_enviar = new Correo();
                            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                            string nombre = "";
                            
                            cumenvio.env_fecha_act = DateTime.Now;
                            cumenvio.env_estado = "1";
                            cumenvio.env_id = envio.idInsertado;

                            string encriptado = Seguridad.Encriptar(envio.idInsertado.ToString());
                            var estado = cumenviobl.CumEnvioEditarJson(cumenvio);
                            correo_enviar.EnviarCorreo(
                             correopersonal,
                             "Link de Ficha Sintomatológica",
                             "Hola! : " + nombre + " \n " +
                             "Tu clave es la que necesitaras para guardar tu ficha : " + clave + " \n " +
                             "Ingrese al siguiente Link y complete el formulario"
                             + "\n solo se puede editar el mismo dia de envio, \n" +
                             " Link Ficha Sintomatológica : " + basepath + "IntranetPJAdmin/FichaFormulario?id=" + encriptado
                             );
                        }

                    }
                }

                mensaje = "Fichas Registradas";
                respuesta = true;

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        public ActionResult PanelSecciones(int menu_id=1)
        {
            //List<IntranetMenuEntidad> intranetMenu = new List<IntranetMenuEntidad>();
            //claseError error = new claseError();
            //string mensajeerrorBD = "";
            //string mensaje = "";
            //try
            //{
            //    var menuTupla = intranetMenubl.IntranetMenuListarJson();
            //    error = menuTupla.error;
            //    if (error.Key.Equals(string.Empty))
            //    {
            //        intranetMenu = menuTupla.intranetMenuLista;
            //        ViewBag.Menu = intranetMenu;
            //    }
            //    else
            //    {
            //        mensajeerrorBD += "Error en Menus: " + error.Value + "\n";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    mensaje = ex.Message;
            //}
            
            return View("~/Views/IntranetPJAdmin/IntranetPJSecciones1.cshtml");
        }
        public ActionResult PanelConfiguracionToken()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJToken.cshtml");
        }
        public ActionResult PanelSistemas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJSistemas.cshtml");
        }

        #region Region Modelado de Excel para Usuarios de Ficha Sintomatologica
        [HttpPost]
        public ActionResult MostrarExcelModeloJson()
        {
            string errormensaje = "";
            bool response = false;
            string base64String = "";
            string pathCarpetaArchivoExcel = Server.MapPath("/") + Request.ApplicationPath + "CumplimientoFiles/ExcelModelo";
            try
            {
                string pathRutaArchivoExcel = @"" + pathCarpetaArchivoExcel + "/CORREO_PERSONAL_MODELO.xlsx";
                if (System.IO.File.Exists(pathRutaArchivoExcel))
                {
                    byte[] imagebytes = System.IO.File.ReadAllBytes(pathRutaArchivoExcel);
                    base64String = Convert.ToBase64String(imagebytes);
                }
                else
                {
                    base64String = string.Empty;
                }
                errormensaje = "Obteniendo Archivo";
                response = true;
            }
            catch(Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje=errormensaje,respuesta=response,data=base64String});
        }
        [HttpPost]
        public ActionResult SubirExcelJson()
        {
            bool response = false;
            string errormensaje = "";

            List<PersonaSqlEntidad> listaDocumentosExcel = new List<PersonaSqlEntidad>();

            List<object> listaRespuesta = new List<object>();
            HttpPostedFileBase file = Request.Files["file"];

            string documento = "";
            string cabeceraDocumento = "";
            string cabeceraCorreo = "";

            List<PersonaSqlEntidad> listaSQL = new List<PersonaSqlEntidad>();
            List<CumUsuarioExcelEntidad> listaPostgres = new List<CumUsuarioExcelEntidad>();

            ExcelPackage ExcelResultado = new ExcelPackage();

            try
            {
                using (var package = new ExcelPackage(file.InputStream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count
                    cabeceraDocumento = worksheet.Cells[1, 1].Value?.ToString().Trim();
                    cabeceraCorreo = worksheet.Cells[1, 2].Value?.ToString().Trim();
                    if(cabeceraDocumento.Equals("COD_TRAB.")&&cabeceraCorreo.Equals("CORREO ELECTRÓNICO"))
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {

                            listaDocumentosExcel.Add(new PersonaSqlEntidad {
                               CO_TRAB = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                               NO_DIRE_MAI1= worksheet.Cells[row, 2].Value?.ToString().Trim()
                        });
                            //for (int col = 1; col <= colCount; col++)
                            //{
                            //    listaDocumentos.Add(worksheet.Cells[row, col].Value?.ToString().Trim());
                            //}
                        }
                        if (listaDocumentosExcel.Count > 0)
                        {
                            ExcelResultado.Workbook.Worksheets.Add("Resultado");

                            ExcelWorksheet WSResultado = ExcelResultado.Workbook.Worksheets[1];

                            int rowResultado = 1;

                            //listar de sql
                            var listaPersonasSQLTupla = sqlbl.PersonaSQLListarDocumentosJson();
                            if (listaPersonasSQLTupla.error.Key.Equals(string.Empty))
                            {
                                listaSQL = listaPersonasSQLTupla.lista;
                            }
                            else
                            {
                                errormensaje = listaPersonasSQLTupla.error.Value;
                            }
                            //listar de postgress
                            var listaUsuariosExcelPostgresTupla = cumusuexcelbl.CumUsuarioExcelListarJson();
                            if (listaUsuariosExcelPostgresTupla.error.Key.Equals(string.Empty))
                            {
                                listaPostgres = listaUsuariosExcelPostgresTupla.lista;
                            }
                            else
                            {
                                errormensaje = listaUsuariosExcelPostgresTupla.error.Value;
                            }
                            foreach(var registro in listaDocumentosExcel)
                            {
                                var contieneSQL = listaSQL.Where(x => x.CO_TRAB.Equals(registro.CO_TRAB)).FirstOrDefault();
                                if (contieneSQL != null)
                                {
                                    CumUsuarioExcelEntidad cumUsuarioExcelET = new CumUsuarioExcelEntidad();
                                    var contienePostgres = listaPostgres.Where(x => x.cue_numdoc.Equals(registro.CO_TRAB)).FirstOrDefault();
                                    if (contienePostgres != null)
                                    {
                                        //Existe, entonces editar
                                        if (!contienePostgres.cue_correo.Equals(registro.NO_DIRE_MAI1))
                                        {
                                            cumUsuarioExcelET.cue_id = contienePostgres.cue_id;
                                            cumUsuarioExcelET.cue_numdoc = contienePostgres.cue_numdoc;
                                            cumUsuarioExcelET.cue_correo = registro.NO_DIRE_MAI1;
                                            cumUsuarioExcelET.cue_fecha_act = DateTime.Now;
                                            var cumUSuarioTupla = cumusuexcelbl.CumUsuarioExcelEditarJson(cumUsuarioExcelET);
                                            if (!cumUSuarioTupla.error.Key.Equals(string.Empty))
                                            {
                                                //Agregar a excel
                                                WSResultado.Cells[rowResultado, 1].Value = cumUsuarioExcelET.cue_numdoc;
                                                WSResultado.Cells[rowResultado, 2].Value = cumUsuarioExcelET.cue_correo;
                                                WSResultado.Cells[rowResultado, 3].Value = "Error Al Editar";
                                                rowResultado++;
                                                listaRespuesta.Add(new
                                                {
                                                    documento = contienePostgres.cue_numdoc,
                                                    correo = contienePostgres.cue_correo,
                                                    motivo = "Error Al Editar"
                                                });

                                            }
                                        }
                                       
                                    }
                                    else
                                    {
                                        //No Existe, entonces insertar
                                        cumUsuarioExcelET.cue_numdoc = registro.CO_TRAB;
                                        cumUsuarioExcelET.cue_correo = registro.NO_DIRE_MAI1;
                                        cumUsuarioExcelET.cue_fecha_reg = DateTime.Now;
                                        var cumUSuarioTupla = cumusuexcelbl.CumUsuarioExcelInsertarJson(cumUsuarioExcelET);
                                        if (!cumUSuarioTupla.error.Key.Equals(string.Empty))
                                        {
                                            WSResultado.Cells[rowResultado, 1].Value = cumUsuarioExcelET.cue_numdoc;
                                            WSResultado.Cells[rowResultado, 2].Value = cumUsuarioExcelET.cue_correo;
                                            WSResultado.Cells[rowResultado, 3].Value = "Error Al Insertar";
                                            rowResultado++;
                                            listaRespuesta.Add(new
                                            {
                                                documento = cumUsuarioExcelET.cue_numdoc,
                                                correo = cumUsuarioExcelET.cue_correo,
                                                motivo = "Error Al Insertar"
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    WSResultado.Cells[rowResultado, 1].Value = registro.CO_TRAB;
                                    WSResultado.Cells[rowResultado, 2].Value = registro.NO_DIRE_MAI1;
                                    WSResultado.Cells[rowResultado, 3].Value = "No se Encuentra en OFIPLAN";
                                    rowResultado++;
                                    listaRespuesta.Add(new
                                    {
                                        documento = registro.CO_TRAB,
                                        correo = registro.NO_DIRE_MAI1,
                                        motivo = "No se Encuentra en OFIPLAN"
                                    });
                                   
                                }
                                
                            }
                        }
                        response = true;
                        errormensaje = "Listando";
                    }
                    else
                    {
                        errormensaje = "Error en formato de Documento (Cabecera)";
                    }
                    
                } 
            }catch(Exception ex)
            {
                errormensaje = ex.Message;
            }
            byte[] imagebytes = ExcelResultado.GetAsByteArray();
            string base64String = Convert.ToBase64String(imagebytes);
            return Json(new {
                respuesta = response,
                mensaje = errormensaje,
                data = listaRespuesta.ToList(),
                base64=base64String
            });
       

        }
        #endregion

        #region seccion1

        public ActionResult PanelSecciones1()
        {
            return View("~/Views/IntranetPjAdmin/IntranetPJSecciones1.cshtml");
        }
        #endregion


        #region Region Acceso a Mantenimiento Intranet PJ
        public ActionResult Login()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJAdminLogin.cshtml");
        }
        [HttpPost]
        public ActionResult IntranetPJAdminValidarCredencialesJson(string usu_login, string usu_password)
        {
            bool respuesta = false;
            string errormensaje = "";
            string mensajeConsola = "";
            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();
            claseError error = new claseError();
            string pendiente = "";
            try
            {
                var usuarioTupla = usuarioAccesobl.UsuarioIntranetSGCValidarCredenciales(usu_login.ToLower());
                error = usuarioTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    usuario = usuarioTupla.intranetUsuarioSGCEncontrado;
                    if (usuario.usu_id > 0)
                    {
                        if (usuario.usu_estado == "A")
                        {
                            if (usuario.usu_tipo == "EMPLEADO")
                            {
                                if (usuario.usu_contrasenia == Seguridad.EncriptarSHA512(usu_password.Trim()))
                                {
                                    Session["usuSGC_full"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                                    persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                                    Session["perSGC_full"] = persona;
                                    respuesta = true;
                                    errormensaje = "Bienvenido, " + usuario.usu_nombre;
                                }
                                else
                                {
                                    errormensaje = "Contraseña no Coincide";
                                }
                            }
                            else
                            {
                                errormensaje = "Usuario no Pertenece a CPJ";
                            }
                        }
                        else
                        {
                            errormensaje = "Usuario no se Encuentra Activo";
                        }
                    }
                    else
                    {
                        errormensaje = "Usuario no Encontrado";
                    }
                }
                else
                {
                    errormensaje = "Ha ocurrido un problema";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
                mensajeConsola = error.Value;
            }

            return Json(new { mensajeconsola = mensajeConsola, respuesta = respuesta, mensaje = errormensaje, estado = pendiente/*, usuario=usuario*/ });
        }
        [HttpPost]
        public ActionResult IntranetPJAdminCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                Session["usuSGC_full"] = null;
                Session["perSGC_full"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        #endregion

        #region Edicion de Hash para imagenes de Detalle Elemento y Detalle elemento modal
        public ActionResult IntranetEditarHashDetallesJson() {
            bool response = false;
            string errormensaje = "";
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
            List<IntranetDetalleElementoEntidad> listaDetalleElementoDevuelta = new List<IntranetDetalleElementoEntidad>();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModalDevuelta = new List<IntranetDetalleElementoModalEntidad>();
            int totalDetalles=0, totaldetallesEditados=0, totaldetallemodal=0, totaldetallemodalEditado = 0;
            try
            {
                //Detalleelemento
                var listadetelemtupla = detalleelementobl.IntranetDetalleElementoListarJson();
                if (listadetelemtupla.error.Key.Equals(string.Empty))
                {
                    listaDetalleElemento = listadetelemtupla.intranetDetalleElementoLista.Where(x => x.detel_extension != "").ToList();
                    totalDetalles = listaDetalleElemento.Count;
                    if (listaDetalleElemento.Count > 0) {
                        
                        foreach (var detalle in listaDetalleElemento) {
                            detalle.detel_hash= rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, detalle.detel_nombre+"."+detalle.detel_extension);
                            var detalleElementoEditado = detalleelementobl.IntranetDetalleElementoEditarHashJson(detalle);
                            if (detalleElementoEditado.error.Key.Equals(string.Empty))
                            {
                                totaldetallesEditados++;
                            }
                            else {
                                errormensaje += detalleElementoEditado.error.Value;
                            }
                        }
                    }
                    errormensaje += "Detalle Elemento,";
                }
                else
                {
                    errormensaje += listadetelemtupla.error.Value;
                }
                //DetalleElementoModal
                var listadetelemodTupla = detalleelementomodalbl.IntranetDetalleElementoModalListarJson();
                if (listadetelemodTupla.error.Key.Equals(string.Empty))
                {
                    listaDetalleElementoModal = listadetelemodTupla.intranetDetalleElementoModalLista.Where(x => x.detelm_extension != "").ToList();
                    totaldetallemodal = listaDetalleElementoModal.Count;
                    if (listaDetalleElementoModal.Count > 0)
                    {
                        foreach (var detallemodal in listaDetalleElementoModal)
                        {
                            detallemodal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, detallemodal.detelm_nombre + "." + detallemodal.detelm_extension);
                            var detalleElementoModalEditado = detalleelementomodalbl.IntranetDetalleElementoModalEditarHashJson(detallemodal);
                            if (detalleElementoModalEditado.error.Key.Equals(string.Empty))
                            {
                                totaldetallemodalEditado++;
                            }
                            else
                            {
                                errormensaje += detalleElementoModalEditado.error.Value;
                            }
                        }
                    }
                    errormensaje += " Detalle Elemento Modal,";
                }
                else {
                    errormensaje += listadetelemodTupla.error.Value;
                }
                response = true;
                errormensaje += " Editados";

            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            //DetalleElemento
            //DetalleElementoModal
            return Json(new {
                totaldetalleElemento =totalDetalles,
                totaldetalleElementoEditado =totaldetallesEditados,
                totaldetalleElementoModal= totaldetallemodal,
                totalDetalleElementoModalEditados= totaldetallemodalEditado,
                respuesta =response,
                mensaje =errormensaje},JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}