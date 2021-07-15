using iTextSharp.text;
using iTextSharp.text.pdf;
using Rotativa;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.FichaCumplimiento;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class FichaSintomatologicaController : Controller
    {
        CumUsuarioModel cumUsuariobl = new CumUsuarioModel();
        CumUsuPreguntaModel cumUsuPreguntabl = new CumUsuPreguntaModel();
        CumUsuRespuestaModel cumUsuRespuestabl = new CumUsuRespuestaModel();
        SQLModel sqlBL = new SQLModel();
        CumEnvioDetModel envDetModelbl = new CumEnvioDetModel();
        CumEnvioModel cumEnviobl = new CumEnvioModel();
        CumEnvioDetModel cumEnvioDetbl = new CumEnvioDetModel();
        // GET: FichaSintomatologica
        public ActionResult FormularioFichaVista()
        {
            return View();
        }
        public ActionResult FormularioFichaVistaReporte(int env_id=0)
        {
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            CumEnvioDetalleEntidad cumEnvioDet = new CumEnvioDetalleEntidad();
            CumEnvioEntidad cumEnvio = new CumEnvioEntidad();
            try
            {
                //Obtener Envio

                var envioDetTupla = envDetModelbl.CumEnvioDetalleObtenerxEnvioJson(env_id);
                if (envioDetTupla.error.Respuesta)
                {
                    cumEnvioDet = envioDetTupla.cumEnvioDet;
                    //llenar Envio;
                    var envioTupla = cumEnviobl.CumEnvioIdObtenerJson(env_id);
                    if (envioTupla.error.Respuesta)
                    {
                        cumEnvio = envioTupla.cumEnvio;
                        if (cumEnvio.env_id != 0)
                        {
                            //cumEnvio.CumUsuario = cumUsuario;
                            cumEnvioDet.CumEnvio = cumEnvio;
                        }
                    }
                }
                //obtener usuario por fk_envio
                var usuarioTuplaClave = cumUsuariobl.CumUsuarioIdObtenerJson(cumEnvio.fk_usuario);
                if (usuarioTuplaClave.error.Respuesta)
                {
                    //cumUsuario = usuarioTupla.cumUsuario;
                    if (usuarioTuplaClave.cumUsuario.cus_id != 0)
                    {
                        //Se encontro, buscar su data de acuerdo al tipo EMPLEADO o POSTULANTE
                        if (usuarioTuplaClave.cumUsuario.cus_tipo.ToUpper().Equals("POSTULANTE"))
                        {
                            //postgres
                            var usuarioTuplaPostgres = cumUsuariobl.CumUsuarioIdObtenerDataCompletaJson(usuarioTuplaClave.cumUsuario.cus_id);
                            if (usuarioTuplaPostgres.error.Respuesta)
                            {
                                cumUsuario = usuarioTuplaPostgres.cumUsuario;
                            }
                        }
                        else if (usuarioTuplaClave.cumUsuario.cus_tipo.ToUpper().Equals("EMPLEADO"))
                        {
                            //sql
                            cumUsuario = usuarioTuplaClave.cumUsuario;
                            DateTime fecha_act = Convert.ToDateTime(cumEnvioDet.end_fecha_act);
                            int mes_actual = fecha_act.Month;
                            int anio = fecha_act.Year;
                            var personaSQLTupla = sqlBL.PersonaSQLObtenerInformacionPuestoTrabajoJson(cumUsuario.cus_dni,mes_actual,anio);
                            if (personaSQLTupla.error.Respuesta)
                            {
                                PersonaSqlEntidad persona = new PersonaSqlEntidad();
                                if (personaSQLTupla.persona.CO_TRAB==null)
                                {
                                    if (mes_actual == 1)
                                    {
                                        mes_actual = 12;
                                        anio = anio - 1;
                                    }
                                    else
                                    {
                                        mes_actual = mes_actual - 1;
                                    }
                                    var personaSQLTupla2 = sqlBL.PersonaSQLObtenerInformacionPuestoTrabajoJson(cumUsuario.cus_dni, mes_actual,anio);
                                    if (personaSQLTupla2.error.Respuesta)
                                    {
                                        persona = personaSQLTupla2.persona;
                                    }
                                }
                                else
                                {
                                    persona = personaSQLTupla.persona;
                                }
                                //PersonaSqlEntidad persona = personaSQLTupla.persona;
                                cumUsuario.nombre = persona.NO_TRAB;
                                cumUsuario.apellido_pat = persona.NO_APEL_PATE;
                                cumUsuario.apellido_mat = persona.NO_APEL_MATE;
                                cumUsuario.empresa = persona.DE_NOMB;
                                cumUsuario.sede = persona.DE_SEDE;
                                cumUsuario.celular = persona.NU_TLF1;
                                cumUsuario.direccion = persona.NO_DIRE_TRAB;
                                cumUsuario.ruc = persona.NU_RUCS;
                            }
                        }
                        //Listar Preguntas y Respuestas
                        var cumPreguntaTupla = cumUsuPreguntabl.CumUsuPreguntaListarxUsuarioJson(cumUsuario.cus_id, env_id);
                        if (cumPreguntaTupla.error.Respuesta)
                        {
                            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
                            foreach (var preg in cumPreguntaTupla.lista)
                            {
                                CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
                                pregunta = preg;
                                var cumRespuestaTupla = cumUsuRespuestabl.CumUsuRespuestaListarxUsuPreguntaJson(pregunta.upr_id);
                                if (cumRespuestaTupla.error.Respuesta)
                                {
                                    pregunta.CumUsuRespuesta = cumRespuestaTupla.lista.OrderBy(x=>x.ure_orden).ToList();
                                }
                                listaPreguntas.Add(pregunta);
                            }
                            cumUsuario.CumUsuPregunta = listaPreguntas.OrderBy(x=>x.fk_pregunta).ToList();


                        }
                        cumEnvioDet.CumEnvio.CumUsuario = cumUsuario;
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }
            ViewBag.cumEnvioDet = cumEnvioDet;
            return View();
        }
        public void DownloadFdfReporte(int env_id)
        {
            List<byte[]> viewDatas = new List<byte[]>();
            ActionAsPdf view = new ActionAsPdf("FormularioFichaVistaReporte", new { env_id});
            byte[] viewData = view.BuildFile(ControllerContext);
            //viewDatas.Add(viewData);
            //byte[] combinedViewData = combineViewData(viewDatas);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=FichaReporte_" + DateTime.Now+".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(viewData.ToArray());
            Response.End();
        }
        public void DownloadPdfReporteMultile(string env_ids)
        {

            List<ActionAsPdf> lista = new List<ActionAsPdf>();
            List<byte[]> viewDatas = new List<byte[]>();
            string[] arrayEnvios = env_ids.Split(',');
            foreach(var m in arrayEnvios)
            {
                ActionAsPdf view = new ActionAsPdf("FormularioFichaVistaReporte", new { env_id=Convert.ToInt32(m)});
                lista.Add(view);
                byte[] viewData = view.BuildFile(ControllerContext);
                //Add them to array
                viewDatas.Add(viewData);
            }

            //Combine them
            byte[] combinedViewData = combineViewData(viewDatas);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=FichaReporteMultiple_"+DateTime.Now+".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(combinedViewData.ToArray());
            Response.End();
            //return combinedViewData;
          
        }
        private static byte[] combineViewData(List<byte[]> viewData)
        {
            byte[] combinedViewData = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Document document = new Document(PageSize.A4, 50, 50, 25, 25))
                {
                    using (PdfCopy copy = new PdfCopy(document, ms))
                    {
                        document.Open();

                        foreach (byte[] arr in viewData)
                        {
                            using (MemoryStream viewStream = new MemoryStream(arr))
                            {
                                using (PdfReader reader = new PdfReader(viewStream))
                                {
                                    int n = reader.NumberOfPages;
                                    for (int page = 0; page < n;)
                                    {
                                        copy.AddPage(copy.GetImportedPage(reader, ++page));
                                    }
                                }
                            }
                        }
                    }
                }
                combinedViewData = ms.ToArray();
            }
            return combinedViewData;
        }
    }
}