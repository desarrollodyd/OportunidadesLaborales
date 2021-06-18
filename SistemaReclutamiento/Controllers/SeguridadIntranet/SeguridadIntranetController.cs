using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Entidades.SeguridadIntranet;

namespace SistemaReclutamiento.Controllers.SeguridadIntranet
{
    public class SeguridadIntranetController : Controller
    {
        // GET: SeguridadIntranet
        public ActionResult Index()
        {
            return View();
        }
        public List<dynamic> GetMetodos()
        {
            Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));
            var lista = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
                        .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                                .Where(f => f.Constructor.DeclaringType.Name == "SeguridadMenu")
                                                                .Select(c => (c.ConstructorArguments[0].Value))))
                                                          != "False"))
                        .Select(x => new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),



                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                                )),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
)),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return lista.ToList<dynamic>(); ;

        }

        public List<dynamic> MetodosLista_List()
        {
            Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));
            var lista = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
                        .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                                .Where(f => f.Constructor.DeclaringType.Name == "SeguridadMenu")
                                                                .Select(c => (c.ConstructorArguments[0].Value))))
                                                          != "False"))
                        .Select(x => new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                                )),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
)),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return lista.ToList<dynamic>();

        }

        public Metodo_atributos Metodo_Objeto(string control, string accion)
        {
            Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));
            var metodoactual = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        //.Where(m => (System.Attribute.GetCustomAttributes(typeof(seguridad), true).Length > 0))
                        //.Where(m => (String.Join(",", (m.GetCustomAttributesData()
                        //                                        .Where(f => f.Constructor.DeclaringType.Name == "seguridad")
                        //                                        .Select(c => (c.ConstructorArguments[0].Value))))
                        //                                  != "False"))
                        .Where(x => (x.DeclaringType.Name).ToUpper() == (control + "Controller").ToUpper())   ////correccion
                        .Where(x => (x.Name).ToUpper() == accion.ToUpper())
                        .Select(x => new Metodo_clase
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value)))),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                            )),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).FirstOrDefault();

            var atributosobjeto = metodoactual.AttributesMetodo;
            var atributosControladorobjeto = metodoactual.AttributesControlador;
            bool seguridad = true;///por defecto todos los metodos de  nuevalista  vienen con seguridad
            string modulo = "";
            string descripcion = "";
            bool atributoseguridaddefinido = false;
            bool atributomodulodefinido = false;
            bool atributodescripcion = false;

            foreach (var atributo in atributosobjeto)
            {
                if (atributo.AttributeType.Name == "autorizacion")
                {
                    seguridad = (bool)atributo.ConstructorArguments[0].Value;
                    atributoseguridaddefinido = true;
                }
                if (atributo.AttributeType.Name == "modulo")
                {
                    modulo = (string)atributo.ConstructorArguments[0].Value;
                    atributomodulodefinido = true;
                }
                if (atributo.AttributeType.Name == "descripcion")
                {
                    descripcion = (string)atributo.ConstructorArguments[0].Value;
                    atributodescripcion = true;
                }
            }
            ///atributos de controlador , si hubiera sido definido
            var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
            if (atributoscontroladorlinq != null)
            {
                var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (seguridadcontrolador != null)
                {
                    seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value
                                           ).SingleOrDefault();
                }
                var modulocontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (modulocontrolador != null)
                {
                    modulo = modulocontrolador;
                }
                var descripcioncontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "descripcion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (descripcioncontrolador != null)
                {
                    descripcion = descripcioncontrolador;
                }
            }
            ///atributos de metodo,  reemplazan atributos de controlador
            var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
            if (objetolinq != null)
            {
                var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (seguridaddelobjeto != null)
                {
                    seguridad = seguridaddelobjeto;
                }
                var modulodelobjeto = objetolinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (modulodelobjeto != null)
                {
                    modulo = modulodelobjeto;
                }
                var descripciondelobjeto = objetolinq.Where(x => x.AttributeType.Name == "descripcion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (descripciondelobjeto != null)
                {
                    descripcion = descripciondelobjeto;
                }
            }

            Metodo_atributos MetodoAtributos = new Metodo_atributos();
            MetodoAtributos.Controlador = control;
            MetodoAtributos.Metodo = accion;
            MetodoAtributos.seguridad = seguridad;
            MetodoAtributos.modulo = modulo;
            MetodoAtributos.descripcion = descripcion;


            return MetodoAtributos;
        }
    }
}