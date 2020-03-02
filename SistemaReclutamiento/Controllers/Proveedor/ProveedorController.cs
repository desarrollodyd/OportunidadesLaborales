using OfficeOpenXml;
using OfficeOpenXml.Style;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.Proveedor;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.Proveedor;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();
        UbigeoModel ubigeobl = new UbigeoModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportePagosVista()
        {
            return View();
        }
        public ActionResult ProveedorCambiarPasswordVista()
        {
            return View();
        }
        #region Seccion de Acceso
        [HttpPost]
        public ActionResult ProveedorInsertarJson(UsuarioPersonaEntidad datos)
        {
            var errormensaje = "";
            //string nombre = datos.per_nombre + " " + datos.per_apellido_pat + " " + datos.per_apellido_mat;
            string usuario_envio = "";
            string contrasenia_envio = "";

            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();

            int respuestaPersonaInsertada = 0;
            int respuestaUsuarioInsertado = 0;
            bool respuestaConsulta = false;

            string contrasenia = "";
            string correo = datos.per_correoelectronico;
            var usuario_repetido = usuariobl.ProveedorUsuarioObtenerxRUC(datos.per_numdoc);
            if (usuario_repetido.usu_id == 0)
            {
                var persona_repetida_tupla = personabl.PersonaEmailObtenerJson(datos.per_correoelectronico);
                var persona_repetida = persona_repetida_tupla.persona;
                var error = persona_repetida_tupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (persona_repetida.per_id == 0)
                    {
                        //Seteando datos correspondiente a persona            
                        persona.per_numdoc = datos.per_numdoc;
                        persona.per_nombre = datos.per_numdoc;
                        persona.per_apellido_pat = datos.per_numdoc;
                        persona.per_apellido_mat = datos.per_numdoc;
                        persona.per_correoelectronico = datos.per_correoelectronico;
                        persona.per_estado = "A";
                        //persona.per_tipodoc = datos.per_tipodoc;
                        persona.per_fecha_reg = DateTime.Now;
                        respuestaPersonaInsertada = personabl.PersonaProveedorInsertarJson(persona);
                        if (respuestaPersonaInsertada != 0)
                        {
                            //Insercion de Usuario
                            contrasenia = GeneradorPassword.GenerarPassword(8);
                            usuario.usu_contrasenia = Seguridad.EncriptarSHA512(contrasenia);
                            usuario.usu_nombre = datos.per_numdoc;
                            usuario.fk_persona = respuestaPersonaInsertada;
                            usuario.usu_estado = "P";
                            usuario.usu_cambio_pass = true;
                            usuario.usu_clave_temp = Seguridad.EncriptarSHA512(usuario.usu_nombre);
                            usuario.usu_fecha_reg = DateTime.Now;
                            usuario.usu_tipo = "PROVEEDOR";
                            respuestaUsuarioInsertado = usuariobl.PostulanteUsuarioInsertarJson(usuario);

                            if (respuestaUsuarioInsertado == 0)
                            {
                                return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Usuario" });
                            }
                            respuestaConsulta = true;
                            //datos para cuerpo de correo
                            usuario_envio = usuario.usu_nombre;
                            contrasenia_envio = usuario.usu_contrasenia;
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
                else
                {
                    return Json(new { respuesta = respuestaConsulta, mensaje = error.Value });
                }

            }

            else
            {
                return Json(new { respuesta = respuestaConsulta, mensaje = "Ya hay un usuario registrado con el RUC: " + datos.per_numdoc });
            }
            if (respuestaConsulta)
            {
                /*LOGICA PARA ENVIO DE CORREO DE CONFIRMACION*/
                try
                {
                    //string cuerpo_correo = "";
                    Correo correo_enviar = new Correo();
                    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    //MailMessage message = new MailMessage("s3k.zimbra@gmail.com", persona.per_correoelectronico, "correo de confirmacion", cuerpo_correo);
                    correo_enviar.EnviarCorreo(
                        correo,
                        "Correo de Confirmacion Proveedores",
                        "Hola! : " + " \n " +
                        "Sus credenciales son las siguientes:\n Usuario : " + usuario_envio + "\n Contraseña : " + contrasenia
                        + "\n puede usar estas credenciales para acceder al sistema, donde se le pedira realizar un cambio de esta contraseña por su seguridad, \n" +
                        " o puede hacer click en el siguiente enlace y seguir los pasos indicados para cambiar su contraseña y completar su registro : " + basepath + "Login/ProveedorActivacion?id=" + usuario.usu_clave_temp
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
        public ActionResult CambiarPasswordVistaJson(string usu_password)
        {
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            bool respuestaConsulta = false;
            string errormensaje = "";
            string password_encriptado = Seguridad.EncriptarSHA512(usu_password);
            try
            {
                respuestaConsulta = usuariobl.ProveedorUsuarioEditarContraseniaJson(usuario.usu_id, password_encriptado);
                errormensaje = "Contraseña actualizada correctamente";
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + "";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        #endregion

        #region Seccion Permisos
        [HttpPost]
        public ActionResult ListarDataMenuJson()
        {
            var usuario = (UsuarioEntidad)Session["usu_proveedor"];
            var errormensaje = "";
            bool response = false;
            MenuModel menubl = new MenuModel();
            SubMenuModel submenubl = new SubMenuModel();
            List<MenuEntidad> listamenu = new List<MenuEntidad>();
            List<SubMenuEntidad> listasubmenu = new List<SubMenuEntidad>();
            claseError error = new claseError();
            try
            {
                var tuplalistamenu = menubl.MenuListarJson(usuario.usu_id);
                listamenu = tuplalistamenu.lista;
                error = tuplalistamenu.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (listamenu.Count > 0)
                    {
                        foreach (var m in listamenu)
                        {
                            listasubmenu = submenubl.SubMenuListarPorMenuJson(m.men_id, usuario.usu_id);
                            m.SubMenu = listasubmenu;
                        }
                    }
                    response = true;
                }
                else
                {
                    return Json(new { respuesta = false, mensaje = error.Value });
                }
            }
            catch (Exception ex)
            {
                return Json(new { respuesta = false, mensaje = ex.Message });
            }

            return Json(new { data = listamenu.ToList(), respuesta = response, mensaje = errormensaje });
        }
        public ActionResult UsuarioProveedorListarJson()
        {
            var errormensaje = "";
            UsuarioModel usuariobl = new UsuarioModel();
            var lista = new List<UsuarioEntidad>();
            try
            {
                lista = usuariobl.ProveedorListarUsuariosPorTipoJson();
                errormensaje = "Cargando Usuarios ...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        #endregion

        #region Seccion Reportes
        [HttpPost]
        public ActionResult MesaPartesListarCompaniasJson()
        {
            var errormensaje = "";
            MesaPartesCIAModel mesapartesbl = new MesaPartesCIAModel();
            var lista = new List<MesaPartesCIAEntidad>();
            try
            {
                lista = mesapartesbl.MesaPartesListarCompanias();
                errormensaje = "Cargando compañias ...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        public ActionResult ListarPagosporCompaniaJson(string cboCompania, string fecha_inicio, string fecha_final)
        {
            //yyyy - MM - dd HH':'mm':'ss
            bool respuesta = false;
            string cadena = "";
            var errormensaje = "";
            string nombretabla = "CP" + cboCompania + "CART";
            string nombretablapago = "CP" + cboCompania + "PAGO";
            string tipo_doc = "FT";
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            SQLModel sql = new SQLModel();
            var lista = new List<CPCARTEntidad>();
            //var listaDetracciones = new List<CPCARTEntidad>();
            string documento = "";
            try
            {
                var listatupla = sql.CPCARTListarPagosPorCompania(nombretabla, usuario.usu_nombre, tipo_doc, fecha_inicio, fecha_final);
                lista = listatupla.lista;
               var listaDetracciones = listatupla.lista.Where(x => x.CP_CNUMDOC.Trim().Substring(x.CP_CNUMDOC.Trim().Length-1, 1).Equals("D")).ToList();
                foreach (var detraccion in listaDetracciones)
                {
                    documento = detraccion.CP_CNUMDOC.Trim().Substring(0, detraccion.CP_CNUMDOC.Trim().Length - 2);
                    var objeto = lista.Where(x => x.CP_CNUMDOC.Trim().Equals(documento)).FirstOrDefault();
                    if (objeto != null)
                    {
                        objeto.CP_NSALDMN = objeto.CP_NIMPOMN - detraccion.CP_NIMPOMN;
                    }
                }
                cadena = listatupla.cadena;
                var errorlista = listatupla.error;
                if (errorlista.Key.Equals(string.Empty))
                {
                    if (lista.Count > 0)
                    {
                        foreach (var m in lista)
                        {
                            var subtotaltupla = sql.ObtenerSubtotalporNumeroDocumento(nombretablapago, m.CP_CNUMDOC, m.CP_CTIPDOC, usuario.usu_nombre);
                            m.subtotalSoles = subtotaltupla.subtotalSoles;
                            m.subtotalDolares = subtotaltupla.subtotalDolares;
                        }
                    }
                    errormensaje = "Cargando Data ...";
                    respuesta = true;
                }
                else
                {
                    errormensaje = errorlista.Value;
                    respuesta = false;
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje, cadena = cadena });
        }
        public ActionResult ListarPagosporNumeroDocumentoJson(string num_doc, string nombre_tabla, string fecha_compra,string cuenta)
        {
            //yyyy - MM - dd HH':'mm':'ss
            bool respuesta = false;
            var errormensaje = "";
            
            string anioconstancia = fecha_compra.Substring(0,2);
            string nombretablaconstancia = "CT"+nombre_tabla.Trim()+"COMD"+anioconstancia;
            string nombretabla = "CP" + nombre_tabla.Trim() + "PAGO";
            string tipo_doc = "FT";
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            SQLModel sql = new SQLModel();
            var lista = new List<CPPAGOEntidad>();
            try
            {
                if (cuenta.Trim().Equals("421203"))
                {
                    var listatupla = sql.CPPAGOListarPagosPorNumeroDocumentoDetraccion(nombretabla, usuario.usu_nombre, tipo_doc, num_doc.Trim(), nombretablaconstancia.Trim());
                    lista = listatupla.lista;
                    var errorlista = listatupla.error;
                    if (errorlista.Key.Equals(string.Empty))
                    {
                        errormensaje = "Cargando Data ...";
                        respuesta = true;
                    }
                    else
                    {
                        errormensaje = errorlista.Value;
                        respuesta = false;
                    }
                }
                else {
                    var listatupla = sql.CPPAGOListarPagosPorNumeroDocumento(nombretabla, usuario.usu_nombre, tipo_doc, num_doc.Trim(), nombretablaconstancia.Trim());
                    lista = listatupla.lista;
                    var errorlista = listatupla.error;
                    if (errorlista.Key.Equals(string.Empty))
                    {
                        errormensaje = "Cargando Data ...";
                        respuesta = true;
                    }
                    else
                    {
                        errormensaje = errorlista.Value;
                        respuesta = false;
                    }
                }
              

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje });
        }

        public void ReportePagosExportarExcel(string nombre_tabla, string fecha_inicio, string fecha_fin)
        {


            string nombretabla = "CP" + nombre_tabla.Trim() + "CART";
            string nombretablapago = "CP" + nombre_tabla.Trim() + "PAGO";
            string tipo_doc = "FT";
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            string nombreusuario = usuario.usu_nombre;
            DateTime fechahoy = DateTime.Now;
            string fechareporte = fechahoy.ToString("dd-MM-yyyy");
            string nombredocumento = "ReportePagos_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;

            SQLModel sql = new SQLModel();
            var listaPagosporCompania = new List<CPCARTEntidad>();
            decimal totalImporteSoles = 0,
                  totalImporteDolares = 0,
                  totalPagadoSoles = 0,
                  totalPagadoDolares = 0,
                  totalSaldoSoles = 0,
                  totalSaldoDolares = 0;

            //Variables para Detalle
            var listapagosporDocumento = new List<CPPAGOEntidad>();
            //Fin

            var listatuplapagosporcompaniatupla = sql.CPCARTListarPagosPorCompania(nombretabla, usuario.usu_nombre, tipo_doc, fecha_inicio, fecha_fin);
            listaPagosporCompania = listatuplapagosporcompaniatupla.lista;
            var errorlista = listatuplapagosporcompaniatupla.error;
            if (errorlista.Key.Equals(string.Empty))
            {
                if (listaPagosporCompania.Count > 0)
                {
                    foreach (var m in listaPagosporCompania)
                    {
                        var subtotaltupla = sql.ObtenerSubtotalporNumeroDocumento(nombretablapago, m.CP_CNUMDOC, m.CP_CTIPDOC, usuario.usu_nombre);
                        m.subtotalSoles = subtotaltupla.subtotalSoles;
                        m.subtotalDolares = subtotaltupla.subtotalDolares;
                        if (m.CP_CCODMON.Equals("MN"))
                        {
                            totalPagadoSoles += subtotaltupla.subtotalSoles;
                            totalImporteSoles += m.CP_NIMPOMN;
                        }
                        else
                        {
                            totalPagadoDolares += subtotaltupla.subtotalDolares;
                            totalImporteDolares += m.CP_NIMPOUS;
                        }
                    }
                }
            }
            totalSaldoSoles = totalImporteSoles - totalPagadoSoles;
            totalSaldoDolares = totalImporteDolares - totalPagadoDolares;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reporte");
            ws.Cells["B1"].Value = "Reporte de Pagos";
            ws.Cells["B1:C1"].Style.Font.Bold = true;

            ws.Cells["B1"].Style.Font.Size = 20;
            ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B1:J1"].Merge = true;
            ws.Cells["B1:J1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["B3"].Value = "Usuario";
            ws.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells["C3"].Value = nombreusuario;
            ws.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells["B3:C3"].Style.Font.Bold = true;

            ws.Cells["B4"].Value = "Fecha";
            ws.Cells["B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells["C4"].Value = fechareporte;
            ws.Cells["C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells["B4:C4"].Style.Font.Bold = true;

            ws.Cells["B6"].Value = "Tipo Anexo";
            ws.Cells["C6"].Value = "RUC";
            ws.Cells["D6"].Value = "Nro. Documento";
            ws.Cells["E6"].Value = "Fecha Documento";
            ws.Cells["F6"].Value = "Moneda";
            ws.Cells["G6"].Value = "Importe";
            ws.Cells["H6"].Value = "Monto Pagado";
            ws.Cells["I6"].Value = "Saldo";
            ws.Cells["J6"].Value = "Estado";

            ws.Cells["B6:J6"].Style.Font.Bold = true;
            ws.Cells["B6:J6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["B6:J6"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            ws.Cells["B6:J6"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["B6:J6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            int fila = 7, inicioGrupo = 0, finGrupo = 0;

            foreach (var item in listaPagosporCompania)
            {
                //Maestro
                DateTime fechadoc = ManejoNulos.ManageNullDate(item.CP_DFECDOC);
                string fecha = fechadoc.ToString("dd-MM-yyyy");
                string moneda = "", estado = "";
                decimal subtotalsoles = item.subtotalSoles;
                decimal subtotaldolares = item.subtotalDolares;
                decimal importe = 0, monto_pagado = 0, saldo = 0;
                if (item.CP_CCODMON.Equals("MN"))
                {
                    moneda = "Soles";
                    importe = item.CP_NIMPOMN;
                    monto_pagado = subtotalsoles;
                    saldo = importe - subtotalsoles;
                    if (subtotalsoles == importe)
                    {
                        estado = "PAGADO";
                    }
                    else if (subtotalsoles == 0)
                    {
                        estado = "PENDIENTE";
                    }
                    else if (subtotalsoles != 0 && subtotalsoles < importe)
                    {
                        estado = "PARCIAL";
                    }
                }
                else
                {
                    moneda = "Dolares";
                    importe = item.CP_NIMPOUS;
                    monto_pagado = subtotaldolares;
                    saldo = importe - subtotaldolares;
                    if (subtotaldolares == importe)
                    {
                        estado = "PAGADO";
                    }
                    else if (subtotaldolares == 0)
                    {
                        estado = "PENDIENTE";
                    }
                    else if (subtotaldolares != 0 && subtotaldolares < importe)
                    {
                        estado = "PARCIAL";
                    }
                }
                ws.Cells[string.Format("B{0}", fila)].Value = item.CP_CVANEXO;
                ws.Cells[string.Format("C{0}", fila)].Value = item.CP_CCODIGO;
                ws.Cells[string.Format("D{0}", fila)].Value = item.CP_CNUMDOC;
                ws.Cells[string.Format("E{0}", fila)].Value = fecha;
                ws.Cells[string.Format("F{0}", fila)].Value = moneda;
                ws.Cells[string.Format("G{0}", fila)].Value = importe;
                ws.Cells[string.Format("H{0}", fila)].Value = monto_pagado;
                ws.Cells[string.Format("I{0}", fila)].Value = saldo;
                ws.Cells[string.Format("J{0}", fila)].Value = estado;
                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                fila++;


                //Detalle
                string nombretablaconstancia = "CT" + nombre_tabla + "COMD" + item.CP_CFECCOM.Substring(0, 2);
                if (item.CP_CCUENTA.Trim().Equals("421203")) {
                    var listapagosporDocumentotupla = sql.CPPAGOListarPagosPorNumeroDocumentoDetraccion
                        (nombretablapago, usuario.usu_nombre, tipo_doc, item.CP_CNUMDOC.Trim(), nombretablaconstancia.Trim());
                    listapagosporDocumento = listapagosporDocumentotupla.lista;
                }
                else
                {
                    var listapagosporDocumentotupla = sql.CPPAGOListarPagosPorNumeroDocumento
                   (nombretablapago, usuario.usu_nombre, tipo_doc, item.CP_CNUMDOC.Trim(),nombretablaconstancia.Trim());
                    listapagosporDocumento = listapagosporDocumentotupla.lista;
                }
               
               
                if (listapagosporDocumento.Count > 0)
                {
                    inicioGrupo = fila;

                    //Cabeceras
                    ws.Cells[string.Format("C{0}", inicioGrupo)].Value = "Tipo Anexo";
                    ws.Cells[string.Format("D{0}", inicioGrupo)].Value = "RUC";
                    ws.Cells[string.Format("E{0}", inicioGrupo)].Value = "Tipo Documento";
                    ws.Cells[string.Format("F{0}", inicioGrupo)].Value = "Nro Documento";
                    ws.Cells[string.Format("G{0}", inicioGrupo)].Value = "Moneda";
                    ws.Cells[string.Format("H{0}", inicioGrupo)].Value = "Importe";
                    ws.Cells[string.Format("I{0}", inicioGrupo)].Value = "Fecha";
                    ws.Cells[string.Format("J{0}", inicioGrupo)].Value = "Glosa";
                    ws.Cells[string.Format("K{0}", inicioGrupo)].Value = "Nro Contancia";
                    ws.Cells[string.Format("C{0}:K{0}", inicioGrupo)].Style.Font.Bold = true;
                    ws.Cells[string.Format("C{0}:K{0}", inicioGrupo)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("C{0}:K{0}", inicioGrupo)].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                    ws.Cells[string.Format("C{0}:K{0}", inicioGrupo)].Style.Font.Color.SetColor(Color.White);
                    fila++;
                    //Datos
                    foreach (var detalle in listapagosporDocumento)
                    {
                        DateTime fechapagodoc = ManejoNulos.ManageNullDate(detalle.PG_DFECCOM);
                        string fechapago = fechapagodoc.ToString("dd-MM-yyyy");
                        string monedadetalle = "";
                        decimal importedetalle = 0;
                        if (detalle.PG_CCODMON.Equals("MN"))
                        {
                            monedadetalle = "Soles";
                            importedetalle = detalle.PG_NIMPOMN;
                        }
                        else
                        {
                            monedadetalle = "Dolares";
                            importedetalle = detalle.PG_NIMPOUS;
                        }
                        ws.Cells[string.Format("C{0}", fila)].Value = detalle.PG_CVANEXO;
                        ws.Cells[string.Format("D{0}", fila)].Value = detalle.PG_CCODIGO;
                        ws.Cells[string.Format("E{0}", fila)].Value = detalle.PG_CTIPDOC;
                        ws.Cells[string.Format("F{0}", fila)].Value = detalle.PG_CNUMDOC;
                        ws.Cells[string.Format("G{0}", fila)].Value = monedadetalle;
                        ws.Cells[string.Format("H{0}", fila)].Value = importedetalle;
                        ws.Cells[string.Format("I{0}", fila)].Value = fechapago;
                        ws.Cells[string.Format("J{0}", fila)].Value = detalle.PG_CGLOSA;
                        ws.Cells[string.Format("K{0}", fila)].Value = detalle.DNUMDOR;
                        fila++;
                    }
                    //Agregar una fila extra en el detalle para que no se confundan con el collapse
                    fila++;
                    finGrupo = fila - 1;
                    for (var i = inicioGrupo; i <= finGrupo; i++)
                    {
                        ws.Row(i).OutlineLevel = 1;
                        ws.Row(i).Collapsed = true;
                    }

                }
                else
                {
                    ws.Cells[string.Format("C{0}", fila)].Value = "No se encontro pagos para este documento";
                    ws.Cells[string.Format("C{0}:K{0}", fila)].Style.Font.Bold = true;
                    ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("C{0}:K{0}", fila)].Merge = true;
                    ws.Row(fila).OutlineLevel = 1;
                    ws.Row(fila).Collapsed = true;
                    //Agregar una fila extra en el detalle para que no se confundan con el collapse
                    fila++;
                    ws.Row(fila).OutlineLevel = 1;
                    ws.Row(fila).Collapsed = true;

                    fila++;
                }
                //Fin de Detalle
            }
            fila++;
            ws.Cells[string.Format("G{0}", fila)].Value = "Total en Pagos";
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Font.Bold = true;

            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Font.Size = 14;
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("G{0}:I{0}", fila)].Merge = true;
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            fila++;
            //Cabecera Total Final
            ws.Cells[string.Format("G{0}", fila)].Value = "Importe";
            ws.Cells[string.Format("H{0}", fila)].Value = "Monto Pagado";
            ws.Cells[string.Format("I{0}", fila)].Value = "Saldo";
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Font.Bold = true;
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Font.Color.SetColor(Color.White);
            ws.Cells[string.Format("G{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            fila++;
            //Soles
            ws.Cells[string.Format("F{0}", fila)].Value = "Total Soles";
            ws.Cells[string.Format("G{0}", fila)].Value = totalImporteSoles;
            ws.Cells[string.Format("H{0}", fila)].Value = totalPagadoSoles;
            ws.Cells[string.Format("I{0}", fila)].Value = totalSaldoSoles;
            ws.Cells[string.Format("F{0}:I{0}", fila)].Style.Font.Bold = true;
            ws.Cells[string.Format("F{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            fila++;
            //Dolares
            ws.Cells[string.Format("F{0}", fila)].Value = "Total Dolares";
            ws.Cells[string.Format("G{0}", fila)].Value = totalImporteDolares;
            ws.Cells[string.Format("H{0}", fila)].Value = totalPagadoDolares;
            ws.Cells[string.Format("I{0}", fila)].Value = totalSaldoDolares;
            ws.Cells[string.Format("F{0}:I{0}", fila)].Style.Font.Bold = true;
            ws.Cells[string.Format("F{0}:I{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", nombredocumento + ".xlsx"));

            //Response.AddHeader("content-disposition", "attachment: filename=" + nombredocumento+".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        #endregion
        //public ActionResult Index()
        //{
        //    string replace = "";
        //    ViewBag.controlleractionlist = ListarMetodosPorControlador("EducacionSuperior");
        //    //ViewBag.controllersnames = GetControllerNames();
        //    List<dynamic> lista = new List<dynamic>();
        //    foreach (var m in GetControllerNames())
        //    {
        //        lista.Add(m.Replace("Controller", replace));
        //    }
        //    ViewBag.controllersnames = lista;
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SubMenuListarPorMenuJson()
        //{
        //    var errormensaje = "";
        //    SubMenuModel submenubl = new SubMenuModel();
        //    MenuModel menubl = new MenuModel();
        //    var listaSubMenu = new List<SubMenuEntidad>();
        //    var listaMenu = new List<MenuEntidad>();
        //    try
        //    {
        //        var tuplalistaMenu = menubl.MenuListarJson();
        //        listaMenu = tuplalistaMenu.lista;
        //        foreach(var m in listaMenu)
        //        {
        //            var lista = submenubl.SubMenuListarPorMenuJson(m.men_id);
        //            foreach (var n in lista) {
        //                listaSubMenu.Add(n);
        //            }
        //            //listaSubMenu = submenubl.SubMenuListarPorMenuJson(m.men_id);
        //        }
        //        errormensaje = "Cargando Data...";
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }
        //    return Json(new { data = listaSubMenu.ToList(), dataMenu=listaMenu.ToList() , respuesta = true, mensaje = errormensaje });
        //}
        //[HttpPost]
        //public ActionResult ModuloListarJson() {
        //    var errormensaje = "";
        //    ModuloModel modulobl = new ModuloModel();
        //    var lista = new List<ModuloEntidad>();
        //    try
        //    {
        //        lista = modulobl.ModuloListarJson();
        //        errormensaje = "Cargando Data...";
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }
        //    return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        //}

        //public List<dynamic> ListarMetodosPorControlador(string controlador)
        //{
        //    List<dynamic> listametodos = new List<dynamic>();
        //    string nombrecontrolador = controlador+"Controller";
        //    Assembly asm = Assembly.GetExecutingAssembly();
        //    listametodos = asm.GetTypes()
        //                          .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
        //                          .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        //                          .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
        //                          .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
        //                          //.Where(x=>String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))!= "SeguridadMenu")
        //                          .Where(m => (String.Join(",", (m.GetCustomAttributesData()
        //                                                      .Where(f => f.Constructor.DeclaringType.Name == "SeguridadMenu")
        //                                                      .Select(c => (c.ConstructorArguments[0].Value))))
        //                                                != "False"))
        //                         .Where(m => m.DeclaringType.Name.Equals(nombrecontrolador))
        //                          .Select(x => new
        //                          {
        //                              Controller = x.DeclaringType.Name,
        //                              Action = x.Name,
        //                              ReturnType = x.ReturnType.Name,
        //                              Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
        //                          })
        //                          .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList<dynamic>();
        //    return listametodos;

        //}
        //private static List<Type> GetSubClasses<T>()
        //{
        //    List<Type> lista = new List<Type>();
        //    lista= Assembly.GetCallingAssembly().GetTypes().Where(
        //        type => type.IsSubclassOf(typeof(T))).ToList();
        //    return lista;
        //}

        //public List<string> GetControllerNames()
        //{
        //    List<string> controllerNames = new List<string>();
        //    GetSubClasses<Controller>().ForEach(
        //        type => controllerNames.Add(type.Name));
        //    return controllerNames;
        //}
    }
}