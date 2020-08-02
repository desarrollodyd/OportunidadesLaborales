using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class SQLController : Controller
    {
        // GET: SQL
        SQLModel sqlbl = new SQLModel();
        PersonaModel personabl = new PersonaModel();
        CumUsuarioExcelModel cumusuexcelbl = new CumUsuarioExcelModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TMEMPRListarJson()
        {
            var errormensaje = "";
            var listaempresa = new List<TMEMPR>();
            var error = new claseError();
            bool response = false;
            try
            {
                var sqltupla = sqlbl.EmpresaListarJson();
                listaempresa = sqltupla.listaempresa;
                error = sqltupla.error;
                errormensaje = error.Value;
                if (errormensaje.Equals(string.Empty))
                {
                    response = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaempresa.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TTPUES_TRABListarJson(string CO_EMPR)
        {
            var errormensaje = "";
            var listapuesto = new List<TTPUES_TRAB>();
            var error = new claseError();
            bool response = false;
            try
            {
                var sqltupla = sqlbl.PuestoTrabajoObtenerPorEmpresaJson(CO_EMPR);
                listapuesto = sqltupla.listapuesto;
                error = sqltupla.error;
                errormensaje = error.Value;
                if (errormensaje.Equals(string.Empty))
                {
                    response = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listapuesto.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TTSEDEListarporEmpresaJson(string[] listaEmpresas)
        {
            string errormensaje = "";
            List<TTSEDE> lista = new List<TTSEDE>();
            var listasede = new List<dynamic>();
            claseError error = new claseError();
            bool response = false;
            string stringEmpresas = "";
            try
            {
                if (listaEmpresas.Count() > 0)
                {
                    stringEmpresas += "(";
                    foreach (var cod_emp in listaEmpresas)
                    {
                        stringEmpresas += @"'"+cod_emp+"',";
                    }
                    stringEmpresas = stringEmpresas.Substring(0, stringEmpresas.Length - 1);
                    stringEmpresas += ")";
                    var listaTupla = sqlbl.TTSEDEListarporEmpresaJson(stringEmpresas);
                    if (listaTupla.error.Key.Equals(string.Empty))
                    {
                        lista = listaTupla.listapuesto;
                        
                        var empresas = lista.GroupBy(z=>new { z.DE_NOMB,z.CO_EMPR}).Select(group=>new { group.Key.DE_NOMB,group.Key.CO_EMPR}).ToList();
                        foreach (var item in empresas)
                        {
                            var listaChildren = new List<dynamic>();
                            foreach (var itemL in lista)
                            {
                                if (item.CO_EMPR == itemL.CO_EMPR)
                                {
                                    listaChildren.Add(new
                                    {
                                        id = itemL.CO_SEDE,
                                        text = itemL.DE_SEDE
                                    });
                                }
                                
                            }

                            listasede.Add(new
                            {
                                id="",
                                text =  item.DE_NOMB,
                                children= listaChildren
                            });
                        }


                        errormensaje = "Listando Sedes";
                        response = true;
                    }
                    else
                    {
                        errormensaje = listaTupla.error.Value;
                    }
                }
                else
                {
                    errormensaje = "Datos Enviados Incorrectos";
                }
         
            }catch(Exception ex)
            {
                errormensaje = ex.Message + ",Llame Administrador";
            }
            return Json(new { data= listasede, mensaje=errormensaje, respuesta=response});
        }
        [HttpPost]
        public ActionResult PersonaListarFichasJson(string[] listaEmpresas, string[] listaSedes)
        {
            string errormensaje = "";
            bool response = false;
            string stringEmpresas = "";
            string stringSedes = "";
            List<PersonaEntidad> listaPersonasPostgres = new List<PersonaEntidad>();
            List<PersonaSqlEntidad> listaPersonasSQL = new List<PersonaSqlEntidad>();
            List<dynamic> lista = new List<dynamic>();
            List<CumUsuarioExcelEntidad> listaPostgresExcel = new List<CumUsuarioExcelEntidad>();

            try
            {
                
                if (listaEmpresas.Count() > 0 && listaSedes.Count() > 0)
                {
                    int mes_anterior = DateTime.Now.Month - 1;
                    mes_anterior = 6;
                    stringEmpresas += "(";
                    foreach (var cod_emp in listaEmpresas)
                    {
                        stringEmpresas += @"'" + cod_emp + "',";
                    }
                    stringEmpresas = stringEmpresas.Substring(0, stringEmpresas.Length - 1);
                    stringEmpresas += ")";

                    stringSedes += "(";
                    foreach(var cod_sede in listaSedes)
                    {
                        stringSedes += @"'" + cod_sede + "',";
                    }
                    stringSedes = stringSedes.Substring(0, stringSedes.Length - 1);
                    stringSedes += ")";

                    var listaPersonasSQLTupla = sqlbl.PersonaSQLObtenerDataEmpresaFichasJson(stringEmpresas, stringSedes, mes_anterior);
                    if (listaPersonasSQLTupla.error.Key.Equals(string.Empty))
                    {
                        listaPersonasSQL = listaPersonasSQLTupla.lista;
                    }
                    else
                    {
                        errormensaje += listaPersonasSQLTupla.error.Value;
                    }
                    var listaPersonasPostgresTupla = personabl.PersonaListarEmpleadosJson();
                    if (listaPersonasPostgresTupla.error.Key.Equals(string.Empty))
                    {
                        listaPersonasPostgres = listaPersonasPostgresTupla.listaPersonas;
                    }
                    else
                    {
                        errormensaje += listaPersonasPostgresTupla.error.Value;
                    }


                    if (listaPersonasSQL.Count > 0 && listaPersonasPostgres.Count > 0)
                    {
                        //listar de postgress Usuarios de Plantilla de Excel

                        var listaUsuariosExcelPostgresTupla = cumusuexcelbl.CumUsuarioExcelListarJson();
                        if (listaUsuariosExcelPostgresTupla.error.Key.Equals(string.Empty))
                        {
                            listaPostgresExcel = listaUsuariosExcelPostgresTupla.lista;
                        }
                        else
                        {
                            errormensaje += listaUsuariosExcelPostgresTupla.error.Value;
                        }

                        foreach (var m in listaPersonasSQL)
                        {

                            string correoCorporativo = "";

                            var contiene = listaPersonasPostgres.Where(x => x.per_numdoc.Equals(m.CO_TRAB)).FirstOrDefault();
                            if (contiene != null)
                            {
                                correoCorporativo = contiene.per_correoelectronico;
                            }

                            //Actualizar direccion de correo personal desde la tabla CumUsuarioExcel
                            var contieneUsuarioExcel = listaPostgresExcel.Where(x => x.cue_numdoc.Trim().Equals(m.CO_TRAB.Trim())).FirstOrDefault();
                            if (contieneUsuarioExcel != null)
                            {
                                m.NO_DIRE_MAI1 = contieneUsuarioExcel.cue_correo.Trim();
                            }
                            lista.Add(new {
                                id=m.CO_TRAB,
                                nombre=m.NO_APEL_PATE+" " + m.NO_APEL_MATE+", "+m.NO_TRAB,
                                id_empresa=m.CO_EMPR,
                                empresa =m.DE_NOMB,
                                id_sede=m.CO_SEDE,
                                sede=m.DE_SEDE,
                                correoPersonal=m.NO_DIRE_MAI1,
                                correoCorporativo
                            });
                        }
                    }
                    response = true;
                    errormensaje = "Listando Data";
                }
                else
                {
                    errormensaje = "Datos Enviados Incorrectos";
                }
     
            }
            catch(Exception ex)
            {
                errormensaje = ex.Message;
            }

            return Json(new { data = lista, respuesta=response,mensaje=errormensaje });
        }

    }
}