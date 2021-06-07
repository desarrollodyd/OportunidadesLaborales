using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.BoletasGDT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetPJBoletasGDTController : Controller
    {
        SQLModel sqlbl = new SQLModel();
        BolConfiguracionModel bolConfigBL = new BolConfiguracionModel();
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
                if (configuracionTupla.error.Value.Equals(string.Empty))
                {
                    configuracion = configuracionTupla.configuracion;
                    respuesta = true;
                    mensaje = "Obteniendo Registro";
                }
                else
                {
                    mensaje = configuracionTupla.error.Value;
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
                if (configuracionTupla.error.Value.Equals(string.Empty))
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
                if (configuracionTupla.error.Value.Equals(string.Empty))
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

            //string pathAplicacion = Server.MapPath("/") + Request.ApplicationPath + "\\Content\\intranetSGC\\jqueryztree\\css\\zTreeStyle\\img\\diy\\";
            try
            {
                var listaEmpresaTupla = sqlbl.EmpresaListarJson();
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);

                if (listaEmpresaTupla.error.Value.Equals(string.Empty)&&configuracionTupla.error.Value.Equals(string.Empty))
                {
                    listaempresa = listaEmpresaTupla.listaempresa;
                    configuracion = configuracionTupla.configuracion;
                    //Crear directorio principal
                    DirectoryInfo directorioPrincipal =Directory.CreateDirectory(configuracion.config_valor);
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

                        foreach (var mes in meses) {
                            string iconoPdf = "pdf_flat.png";
                            DirectoryInfo directorioMes = directorioAnio.CreateSubdirectory(mes);
                            DirectoryInfo directorioQuincena1 = directorioMes.CreateSubdirectory("Quincena1");
                            DirectoryInfo directorioQuincena2 = directorioMes.CreateSubdirectory("Quincena2");
                            List<dynamic> listaDirectorioQuincena = new List<dynamic>();

                            FileInfo[] filesQuincena1 = directorioQuincena1.GetFiles();
                            FileInfo[] filesQuincena2 = directorioQuincena2.GetFiles();

                            List<dynamic> listFilesQuincena1 =new List<dynamic>();
                            List<dynamic> listFilesQuincena2 =new List<dynamic>();
                            //var direccion = Server.MapPath("/") + Request.ApplicationPath;

                            foreach (var file in filesQuincena1) {
                                double mbytes = ConvertBytesToMegabytes(file.Length);
                                listFilesQuincena1.Add(new {
                                    name = file.Name + " \t \t "+mbytes+"Mb.",
                                    icon = direccion + "/Content/intranetSGC/jqueryztree/css/zTreeStyle/img/diy/" + iconoPdf,
                                });
                            }

                            foreach (var file in filesQuincena2)
                            {
                                double mbytes = ConvertBytesToMegabytes(file.Length);
                                listFilesQuincena2.Add(new
                                {
                                    name = file.Name + " \t \t " + mbytes + "Mb.",
                                    icon = direccion + "/Content/intranetSGC/jqueryztree/css/zTreeStyle/img/diy/" + iconoPdf,
                                });
                            }


                            listaDirectorioQuincena.Add(new {
                                name = directorioQuincena1.Name,
                                children=listFilesQuincena1
                            });
                            listaDirectorioQuincena.Add(new
                            {
                                name = directorioQuincena2.Name,
                                children = listFilesQuincena2
                            });

                            listaDirectorioMes.Add(new {
                                name = directorioMes.Name,
                                open = false,
                                children=listaDirectorioQuincena
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
        [HttpPost]
        public ActionResult BolProcesarPdf(DateTime fechaProcesoPdf,string empresa,string quincena,string nombreEmpresa)
        {
            string mensaje = "No se pudieron procesar las boletas";
            bool respuesta = false;
            List<PersonaSqlEntidad> listaPersonas = new List<PersonaSqlEntidad>();
            string tipoConfiguracion = "PATH";
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();

            try
            {
                DateTime fechaProceso = fechaProcesoPdf;
                var listaPersonasTupla = sqlbl.PersonaSQLObtenrListadoBoletasGDTJson(empresa, fechaProceso.Month, fechaProceso.Year);
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipoConfiguracion);
                
                if (listaPersonasTupla.error.Value.Equals(string.Empty))
                {
                    configuracion = configuracionTupla.configuracion;
                    listaPersonas = listaPersonasTupla.lista;

                    string[] arrayNombreEmpresa = nombreEmpresa.Split(' ');
                    //string nombreEmpresa = String.Join("", arrayNombreEmpresa);
                    string nombreDirectorio = empresa + "_" + String.Join("", arrayNombreEmpresa);
                    string mes = meses[fechaProceso.Month - 1];
                    string anio = Convert.ToString(fechaProceso.Year);
                    string pathPdf = Path.Combine(configuracion.config_valor, nombreDirectorio,anio,mes,quincena);

                    if (Directory.Exists(pathPdf))
                    {
                        //realizar la busqueda del pdf y realizar la division de este
                        string file = Directory.GetFiles(pathPdf, "*.pdf").FirstOrDefault();
                        if(file != null){
                            //pdf encontrado

                        }
                    }
                    mensaje = "Listando Data";
                    respuesta = true;
                }
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { data=listaPersonas,mensaje,respuesta });
        }
        static double ConvertBytesToMegabytes(long bytes)
        {
            return Math.Round(((bytes / 1024f) / 1024f), 2);
        }

        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return Math.Round((kilobytes / 1024f),2);
        }
    }
}