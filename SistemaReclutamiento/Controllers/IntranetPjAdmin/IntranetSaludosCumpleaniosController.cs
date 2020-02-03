using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetSaludosCumpleaniosController : Controller
    {
        // GET: IntranetSaludosCumpleanios
        IntranetSaludoCumpleaniosModel intranetSaludoCumpleaniobl = new IntranetSaludoCumpleaniosModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSaludoCumpleanioEntidad> listaComentarios = new List<IntranetSaludoCumpleanioEntidad>();
            claseError error = new claseError();
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioListarJson();
                error = saludoTupla.error;
                listaComentarios = saludoTupla.intranetSaludoCumpleanioLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Comentarios";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Comentarios";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaComentarios.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioEditarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioEditarJson(intranetSaludoCumpleanio);
                error = saludoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = saludoTupla.intranetSaludoCumpleanioEditado;
                    errormensaje = "Se Editó Correctamente";
                }
                else
                {
                    mensajeConsola = error.Value;
                    errormensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioInsertarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {

            RutaImagenes rutaImagenes = new RutaImagenes();
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            int intranetSaludoCumpleaniosInsertado=0;
            claseError error = new claseError();
            Correo correo_enviar = new Correo();
            string asunto_correo = "";
            string cuerpo_correo = "";
            PersonaEntidad persona = (PersonaEntidad)Session["perIntranet_full"];
            try
            {
                intranetSaludoCumpleanio.sld_fecha_envio = DateTime.Now;
                intranetSaludoCumpleanio.sld_estado = "A";
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioInsertarJson(intranetSaludoCumpleanio);
                error = saludoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetSaludoCumpleaniosInsertado = saludoTupla.idIntranetSaludoCumpleanioInsertado;
                    respuestaConsulta = true;
                    errormensaje = "Se Guardó Correctamente";
                    try
                    {
                        ////envio de correo
                
                        string persona_que_saluda = persona.per_nombre.ToUpper() + " " + persona.per_apellido_pat.ToUpper() + " " + persona.per_apellido_mat.ToUpper();
                 

                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("s3kzimbra@gmail.com");
                        mail.To.Add(intranetSaludoCumpleanio.direccion_envio);
                        mail.Subject = "CPJ | TE DESEA UN FELIZ CUMPLEAÑOS";

                        // Creamos la vista para clientes que
                        // sólo pueden acceder a texto plano...

                        string text = "Hola, Corporacion PJ y  " +
                                      "te desean con mucho cariño un Feliz Cumpleaños!!.\n" +
                                      "Ademas! " + persona_que_saluda +
                                      "\n desea entregarte el siguiente mensaje de cumpleaños: \n" +
                                      intranetSaludoCumpleanio.sld_cuerpo;

                        AlternateView plainView =
                        AlternateView.CreateAlternateViewFromString(text,
                                                    Encoding.UTF8,
                                                    MediaTypeNames.Text.Plain);


                        // Ahora creamos la vista para clientes que 
                        // pueden mostrar contenido HTML...
                        string background = "\"background-image: url('cid:imagen_fondo'); width: 100%; height: 100vh; padding-top:50px;padding-left:50px;\"";
                        string html = @"<div style="+background+" ><div style="+"\"text-align:center;\""+"><h1>¡Corporacion PJ Te desea Un Feliz Cumpleaños!</h1></div></br>" +
                            "<h2>Ademas queremos entregarte un mensaje de " + persona_que_saluda+" para ti :<h2></br>" +
                            "<h1>"+intranetSaludoCumpleanio.sld_cuerpo+"<h1>"+
                                      "<div>";

                        AlternateView htmlView =
                            AlternateView.CreateAlternateViewFromString(html,
                                                    Encoding.UTF8,
                                                    MediaTypeNames.Text.Html);

                        // Creamos el recurso a incrustar. Observad
                        // que el ID que le asignamos (arbitrario) está
                        // referenciado desde el código HTML como origen
                        var direccion = Server.MapPath("/") + Request.ApplicationPath;
                        

                        LinkedResource img_fondo =
                           new LinkedResource(direccion+"/Content/intranet/images/faces/cumpleanios.jpg",
                                               MediaTypeNames.Image.Jpeg);
                        img_fondo.ContentId = "imagen_fondo";
                        img_fondo.ContentType.Name = "Fondo";

                        // Lo incrustamos en la vista HTML...

                       
                        
                        htmlView.LinkedResources.Add(img_fondo);

                        // Por último, vinculamos ambas vistas al mensaje...

                        mail.AlternateViews.Add(plainView);
                        mail.AlternateViews.Add(htmlView);

                        // Y lo enviamos a través del servidor SMTP...

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", Int32.Parse("587"))
                        {
                            EnableSsl = Boolean.Parse("true"),
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("s3kzimbra@gmail.com", "Sistemas.123")
                        };
                        smtp.Send(mail);
                    }
                    catch (Exception ex) {
                        errormensaje += ex.Message;
                    }
                }
                else
                {
                    mensajeConsola += error.Value;
                    errormensaje = "Error, no se Puede Insertar";
                }
            }
            catch (Exception exp)
            {
                errormensaje += exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioEliminarJson(int sld_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            string mensajeConsola = "";
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioEliminarJson(sld_id);
                error = saludoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = saludoTupla.intranetSaludoCumpleanioEliminado;
                    errormensaje = "Saludo Eliminado";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        public ActionResult IntranetMenuSaludoCumpleanioVariosJson(int[] listaComentariosEliminar) {
            string errormensaje = "";
            string mensajeConsola = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            try
            {
                for (int i = 0; i <= listaComentariosEliminar.Length - 1; i++)
                {
                    var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioEliminarJson(listaComentariosEliminar[i]);
                    error = saludoTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        respuestaConsulta = saludoTupla.intranetSaludoCumpleanioEliminado;
                        errormensaje = "Comentarios Eliminados";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Eliminar";
                        mensajeConsola = error.Value;
                    }
                }
                respuestaConsulta = true;
            }
            catch (Exception ex)
            {
                errormensaje = "Error, no se Puede Eliminar, "+ex.Message;
                respuestaConsulta = false;
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
    }
}