using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using SistemaReclutamiento.Entidades;
using System.Net.Mail;

namespace SistemaReclutamiento.Controllers
{
    public class PersonaController : Controller
    {
        personaModel personabl = new personaModel();
        usuarioModel usuariobl = new usuarioModel();
        ubigeoModel ubigeobl = new ubigeoModel();
        postulanteModel postulantebl = new postulanteModel();
        personaSqlModel personasqlbl = new personaSqlModel();

        // GET: Vista Datos Personales
        public ActionResult DatosPersonalesVista()
        {
            if (Session["usu_full"] != null)
            {
                ViewBag.Message = "Bienvenido.";
                return View("~/Views/Persona/DatosPersonaVista.cshtml");
            }
            else
            {
                ViewBag.Message = "Login De Acceso";
                return View("~/Views/Login/Index.cshtml");
            }
        }

        // GET: Vista Educacion Basica
        public ActionResult EducacionBasicaVista()
        {
            ViewBag.Message = "Educacion Basica";
            return View();
        }

        // GET: Vista Educacion Basica
        public ActionResult EducacionSuperiorVista()
        {
            ViewBag.Message = "Educacion Superior";
            return View();
        }


        // GET: Vista PostGrado
        public ActionResult PostGradoVista()
        {
            ViewBag.Message = "PostGrado";
            return View();
        }

        // GET: Vista Ofimatica
        public ActionResult OfimaticaVista()
        {
            ViewBag.Message = "Ofimatica";
            return View();
        }

        // GET: Vista Idiomas
        public ActionResult IdiomasVista()
        {
            ViewBag.Message = "Idiomas";
            return View();
        }

        // GET: Vista Experiencia
        public ActionResult ExperienciaVista()
        {
            ViewBag.Message = "Experiencia";
            return View();
        }

        // GET: Vista InformacionAdicional
        public ActionResult InformacionAdicionalVista()
        {
            ViewBag.Message = "Informacion Adicional";
            return View();
        }
        public ActionResult CambiarPasswordVista()
        {
            ViewBag.Message = "Cambio de Contraseña";
            return View();
        }
        public ActionResult PersonaIndexVista()
        {
            return View();
        }
        public ActionResult PersonaEditarVista(string id)
        {
            var errormensaje = "";
            int idPersona = Convert.ToInt32(Seguridad.Desencriptar(id));
            var persona = new personaEntidad();
            var ubigeo = new ubigeoEntidad();
            try
            {

                persona = personabl.PersonaIdObtenerJson(idPersona);
                if (persona.fk_ubigeo != 0) {
                    ubigeo = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            ViewBag.Ubigeo = ubigeo;
            ViewBag.Persona = persona;
            ViewBag.errormensaje = errormensaje;
            return View();
        }
        [HttpPost]
        public ActionResult PersonaInsertarJson(usuarioPersonaEntidad datos)
        {
            var errormensaje = "";
            string nombre = datos.per_nombre + " " + datos.per_apellido_pat + " " + datos.per_apellido_mat;
            string usuario_envio = "";
            string contrasenia_envio = "";
            postulanteEntidad postulante = new postulanteEntidad();
            usuarioEntidad usuario = new usuarioEntidad();
            personaEntidad persona = new personaEntidad();
            personaSqlEntidad personasql = new personaSqlEntidad();
            int respuestaPersonaInsertada = 0;
            int respuestaUsuarioInsertado = 0;
            bool respuestaConsulta = false;

            string contrasenia = "";
            string correo = datos.per_correoelectronico;
            string busqueda = datos.busqueda;
            if (busqueda == "nuevo")
            {
                var usuario_repetido = usuariobl.UsuarioObtenerxCorreo(correo);
                if (usuario_repetido.usu_id == 0)
                {
                    //Seteando datos correspondiente a persona            
                    persona.per_numdoc = datos.per_numdoc;
                    persona.per_nombre = datos.per_nombre;
                    persona.per_apellido_pat = datos.per_apellido_pat;
                    persona.per_apellido_mat = datos.per_apellido_mat;
                    persona.per_correoelectronico = datos.per_correoelectronico;
                    persona.per_estado = "P";
                    persona.per_tipodoc = datos.per_tipodoc;
                    persona.per_fecha_reg = DateTime.Now;
                    respuestaPersonaInsertada = personabl.PersonaInsertarJson(persona);
                    if (respuestaPersonaInsertada != 0)
                    {
                        //Insercion de Usuario
                        contrasenia = GeneradorPassword.GenerarPassword(8);
                        usuario.usu_contrasenia = Seguridad.EncriptarSHA512(contrasenia);
                        usuario.usu_nombre = datos.per_correoelectronico;
                        usuario.fk_persona = respuestaPersonaInsertada;
                        usuario.usu_estado = "P";
                        usuario.usu_cambio_pass = true;
                        usuario.usu_clave_temp = Seguridad.EncriptarSHA512(usuario.usu_nombre);
                        usuario.usu_fecha_reg = DateTime.Now;
                        respuestaUsuarioInsertado = usuariobl.UsuarioInsertarJson(usuario);

                        if (respuestaUsuarioInsertado == 0)
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Usuario" });
                        }

                        //datos para cuerpo de correo
                        usuario_envio = usuario.usu_nombre;
                        contrasenia_envio = usuario.usu_contrasenia;
                        //Insercion de Postulante
                        postulante.fk_usuario = respuestaUsuarioInsertado;
                        postulante.pos_fecha_reg = DateTime.Now;
                        postulante.pos_estado = "A";
                        respuestaConsulta = postulantebl.PostulanteInsertarJson(postulante);

                        if (!respuestaConsulta)
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Postulante" });
                        }
                    }
                    else
                    {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar a la Persona" });
                    }
                }
                else
                {
                    return Json(new { respuesta = respuestaConsulta, mensaje = "Ya hay un usuario registrado con el correo: " + datos.per_correoelectronico });
                }
            }
            else if (busqueda == "postgres")
            {
                var usuario_repetido = usuariobl.UsuarioObtenerxCorreo(correo);
                if (usuario_repetido.usu_id == 0)
                {
                    //Insercion de Usuario                   
                    persona = personabl.PersonaDniObtenerJson(datos.per_numdoc);
                    contrasenia = GeneradorPassword.GenerarPassword(8);
                    usuario.usu_contrasenia = Seguridad.EncriptarSHA512(contrasenia);
                    usuario.usu_nombre = correo;
                    usuario.fk_persona = persona.per_id;
                    usuario.usu_estado = "P";
                    usuario.usu_cambio_pass = true;
                    usuario.usu_clave_temp = Seguridad.EncriptarSHA512(usuario.usu_nombre);
                    usuario.usu_fecha_reg = DateTime.Now;
                    respuestaUsuarioInsertado = usuariobl.UsuarioInsertarJson(usuario);

                    if (respuestaUsuarioInsertado == 0)
                    {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Usuario" });
                    }

                    //datos para cuerpo de correo
                    usuario_envio = usuario.usu_nombre;
                    contrasenia_envio = usuario.usu_contrasenia;
                    //Insercion de Postulante
                    postulante.fk_usuario = respuestaUsuarioInsertado;
                    postulante.pos_fecha_reg = DateTime.Now;
                    postulante.pos_estado = "A";
                    respuestaConsulta = postulantebl.PostulanteInsertarJson(postulante);
                    errormensaje = "Registrado con éxito";

                    if (!respuestaConsulta)
                    {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Postulante" });
                    }
                }
                else
                {
                    return Json(new { respuesta = respuestaConsulta, mensaje = "Ya hay un usuario registrado con el correo: " + datos.per_correoelectronico });
                }

            }
            else if (busqueda == "sql")
            {
                var usuario_repetido = usuariobl.UsuarioObtenerxCorreo(correo);
                if (usuario_repetido.usu_id == 0)
                {
                    personasql = personasqlbl.PersonaDniObtenerJson(datos.per_numdoc);
                    persona.per_numdoc = personasql.CO_TRAB;
                    persona.per_nombre = personasql.NO_TRAB;
                    persona.per_apellido_pat = personasql.NO_APEL_PATE;
                    persona.per_apellido_mat = personasql.NO_APEL_MATE;
                    persona.per_correoelectronico = correo;
                    persona.per_estado = "P";
                    persona.per_tipodoc = datos.per_tipodoc;
                    persona.per_fecha_reg = DateTime.Now;
                    respuestaPersonaInsertada = personabl.PersonaInsertarJson(persona);
                    if (respuestaPersonaInsertada != 0)
                    {
                        //Insercion de Usuario
                        contrasenia = GeneradorPassword.GenerarPassword(8);
                        usuario.usu_contrasenia = Seguridad.EncriptarSHA512(contrasenia);
                        usuario.usu_nombre = datos.per_correoelectronico;
                        usuario.fk_persona = respuestaPersonaInsertada;
                        usuario.usu_estado = "P";
                        usuario.usu_cambio_pass = true;
                        usuario.usu_clave_temp = Seguridad.EncriptarSHA512(usuario.usu_nombre);
                        usuario.usu_fecha_reg = DateTime.Now;
                        respuestaUsuarioInsertado = usuariobl.UsuarioInsertarJson(usuario);

                        if (respuestaUsuarioInsertado == 0)
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Usuario" });
                        }

                        //datos para cuerpo de correo
                        usuario_envio = usuario.usu_nombre;
                        contrasenia_envio = usuario.usu_contrasenia;
                        //Insercion de Postulante
                        postulante.fk_usuario = respuestaUsuarioInsertado;
                        postulante.pos_fecha_reg = DateTime.Now;
                        postulante.pos_estado = "A";
                        respuestaConsulta = postulantebl.PostulanteInsertarJson(postulante);
                        errormensaje = "Registrado con éxito";

                        if (!respuestaConsulta)
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Postulante" });
                        }
                    }
                    else
                    {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar a la Persona" });
                    }

                }
            }
            if (respuestaConsulta)
            {
                /*LOGICA PARA ENVIO DE CORREO DE CONFIRMACION*/
                try
                {
                    //string cuerpo_correo = "";
                    Correo correo_enviar = new Correo();
                    //MailMessage message = new MailMessage("s3k.zimbra@gmail.com", persona.per_correoelectronico, "correo de confirmacion", cuerpo_correo);
                    correo_enviar.EnviarCorreo(
                        correo,
                        "Correo de Confirmacion",
                        "Hola! : " + nombre + " \n " +
                        "Sus credenciales son las siguientes:\n Usuario : " + usuario_envio + "\n Contraseña : " + contrasenia
                        + "\n puede usar estas credenciales para acceder al sistema, donde se le pedira realizar un cambio de esta contraseña por su seguridad, \n" +
                        " o puede hacer click en el siguiente enlace y seguir los pasos indicados para cambiar su contraseña y completar su registro : http://localhost:63576/Login/Activacion?id=" + usuario.usu_clave_temp
                        );
                    errormensaje = "Verifique su Correo ,Se le ha enviado su Usuario y Contraseña para activar su Registro, Gracias.";
                }
                catch (Exception ex)
                {
                    errormensaje = ex.Message;
                }
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PersonaEditarJson(usuarioPersonaEntidad data)
        {
            var errormensaje = "";
            bool respuestaConsulta = true;
            ubigeoEntidad ubigeo = new ubigeoEntidad();
            personaEntidad persona = new personaEntidad();
            postulanteEntidad postulante = new postulanteEntidad();
            usuarioEntidad usuario = (usuarioEntidad)Session["usu_full"];
            ubigeo = ubigeobl.UbigeoIdObtenerJson(data.ubi_pais_id, data.ubi_departamento_id, data.ubi_provincia_id, data.ubi_distrito_id);
            
            //Seteando datos correspondiente a persona            
            persona.per_nombre = data.per_nombre;
            persona.per_apellido_pat = data.per_apellido_pat;
            persona.per_direccion = data.pos_direccion + data.pos_numero_casa;
            persona.per_fechanacimiento = data.per_fechanacimiento;
            persona.per_apellido_mat = data.per_apellido_mat;
            persona.per_telefono = data.per_telefono;
            persona.per_celular = data.pos_celular;
            persona.per_tipodoc = data.per_tipodoc;
            persona.per_numdoc = data.per_numdoc;
            persona.fk_ubigeo = ubigeo.ubi_id;
            persona.per_sexo = data.per_sexo;
            persona.per_id = data.per_id;
            persona.per_fecha_act = DateTime.Now;
            //Seteando datos correspondiente a postulante
            var nacionalidad = ubigeobl.UbigeoIdObtenerJson(data.ubi_pais_id, "0", "0", "0");
            postulante.fk_nacionalidad = nacionalidad.ubi_id;
            postulante.pos_tipo_direccion = data.pos_tipo_direccion;
            postulante.pos_direccion = data.pos_direccion;
            postulante.pos_tipo_calle = data.pos_tipo_calle;
            postulante.pos_numero_casa = data.pos_numero_casa;
            postulante.pos_tipo_casa = data.pos_tipo_casa;
            postulante.pos_celular = data.pos_celular;
            postulante.pos_estado_civil = data.pos_estado_civil;
            postulante.pos_brevete = data.pos_brevete;
            postulante.pos_num_brevete = data.pos_num_brevete;
            postulante.pos_id = data.pos_id;
            postulante.pos_fecha_act = DateTime.Now;
            //persona.fk_ubigeo = ubigeo.ubi_id;           
            try
            {
                respuestaConsulta = personabl.PersonaEditarJson(persona);
                if (respuestaConsulta) {
                    respuestaConsulta = postulantebl.PostulanteEditarJson(postulante);
                    errormensaje = "Se registro Correctamente";
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            if (respuestaConsulta)
            {
                Session.Remove("per_full");
                Session.Remove("ubigeo");
                Session.Remove("postulante");
                Session["per_full"] = personabl.PersonaIdObtenerJson(data.per_id);
                persona = personabl.PersonaIdObtenerJson(data.per_id);
                Session["ubigeo"] = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                Session["postulante"] = postulantebl.PostulanteIdObtenerporUsuarioJson(usuario.usu_id);

            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult PersonaDniObtenerJson(string per_numdoc)
        {
            //string per_numdoc = Convert.ToString(Request.Params["per_numdoc"]);
            var errormensaje = "";
            bool respuestaConsulta = true;
            string _encontrado = "";
            personaEntidad persona = new personaEntidad();
            personaSqlEntidad personasql = new personaSqlEntidad();
            try
            {
                persona = personabl.PersonaDniObtenerJson(per_numdoc);
                if (persona.per_id != 0)
                {
                    _encontrado = "postgres";
                    errormensaje = "Encontrado en POSTGRES";
                }
                else
                {
                    personasql = personasqlbl.PersonaDniObtenerJson(per_numdoc);
                    if (personasql.CO_TRAB != "" && personasql.CO_TRAB != null)
                    {
                        persona.per_nombre = personasql.NO_TRAB;
                        persona.per_apellido_pat = personasql.NO_APEL_PATE;
                        persona.per_apellido_mat = personasql.NO_APEL_MATE;
                        persona.per_correoelectronico = personasql.NO_DIRE_MAI1;
                        _encontrado = "sql";
                        errormensaje = "Encontrado en SQL SERVER";
                        //  respuestaConsulta = true;
                    }
                    else
                    {
                        errormensaje = "Usuario Nuevo";
                        _encontrado = "nuevo";
                    }
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { data = persona, respuesta = respuestaConsulta, mensaje = errormensaje, encontrado = _encontrado });
        }
        [HttpPost]
        public ActionResult CambiarPasswordVistaJson(string usu_password)
        {
            usuarioEntidad usuario = (usuarioEntidad)Session["usu_full"];
            bool respuestaConsulta = false;
            string errormensaje = "";
            string password_encriptado = Seguridad.EncriptarSHA512(usu_password);
            try
            {
                respuestaConsulta = usuariobl.UsuarioEditarContraseniaJson(usuario.usu_id, password_encriptado);
                errormensaje = "Contraseña actualizada correctamente";
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + "";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

    }
}