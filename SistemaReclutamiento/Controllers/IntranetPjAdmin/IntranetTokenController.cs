using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models.IntranetPJ;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetTokenController : Controller
    {
        // GET: IntranetToken
        UsuarioModel usuariobl = new UsuarioModel();
        IntranetSistemasModel sistemasbl = new IntranetSistemasModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetTokenListarUsuariosJson()
        {
            string errormensaje = "";
            bool response = false;
            List<UsuarioPersonaEntidad> listaUsuarios = new List<UsuarioPersonaEntidad>();
            List<IntranetSistemasEntidad> listaSistemas = new List<IntranetSistemasEntidad>();
            //var listaIAS = new List<dynamic>();
            List<dynamic> listaTotal = new List<dynamic>();
            try
            {
                //Listar Sistemas
                var sistemasTupla = sistemasbl.IntranetSistemasListarJson();
                if (sistemasTupla.error.Key.Equals(string.Empty))
                {
                    listaSistemas = sistemasTupla.intranetSistemasLista;
                }
                else {
                    errormensaje += sistemasTupla.error.Value;
                }

                if (listaSistemas.Count > 0) {
                    foreach (var sistema in listaSistemas) {
                        var client = new RestClient(sistema.sist_ruta);
                        var request = new RestRequest(Method.POST);
                        IRestResponse responseAPI = client.Execute(request);
                        dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(responseAPI.Content);
                        listaTotal.Add(json);
                    }
                }
                //var client = new RestClient("http://127.0.0.1/IAS/Usuario/UsuarioListadoJson");

                //var request = new RestRequest(Method.POST);
                //IRestResponse responseAPI = client.Execute(request);
                //JObject json = JObject.Parse(responseAPI.Content);
                //var result = json["result"];
                //dynamic jsonObj = JsonConvert.DeserializeObject(responseAPI.Content);

                //foreach (var obj in jsonObj.data) {
                //    string NombreEmpleado = obj.NombreEmpleado;
                //    int UsuarioID = obj.UsuarioID;
                //    int EmpleadoID = obj.EmpleadoID;
                //    string UsuarioNombre = obj.UsuarioNombre;
                //    string Estado = obj.Estado;
                //    listaIAS.Add(new
                //    {
                //        NombreEmpleado,
                //        UsuarioID,
                //        EmpleadoID,
                //        UsuarioNombre,
                //        Estado
                //    });
                //}

                var listaTupla = usuariobl.IntranetListarUsuariosTokenJson();
                if (listaTupla.error.Key.Equals(string.Empty))
                {
                    listaUsuarios = listaTupla.listaUsuarios;
                    errormensaje = "Listando Usuarios Mesa de Partes";
                    response = true;
                }
                else
                {
                    errormensaje = listaTupla.error.Value;
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { data_total= listaTotal,data = listaUsuarios.ToList(), respuesta = response, mensaje = errormensaje });
        }
    }
}