using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.SeguridadIntranet
{
    public class SEG_PermisoDAL
    {
        string _conexion = string.Empty;
        public SEG_PermisoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public (bool respuesta, claseError error) GuardarPermiso(SEG_PermisoEntidad permiso)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"if NOT exists(select * from SEG_Permiso  WITH (UPDLOCK, HOLDLOCK) where WEB_PermNombre=@p0 AND [Web_PermControlador]=@p2) 
            INSERT INTO [dbo].[SEG_Permiso]
           ([WEB_PermNombre],[WEB_PermTipo],[WEB_PermControlador],[WEB_PermDescripcion],[WEB_PermEstado],[WEB_PermFechaRegistro], [WEB_ModuloNombre])
            VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)
                 else 
			  update [dbo].[SEG_Permiso]
			  SET
			        [WEB_ModuloNombre]=@p6,
                    [WEB_PermDescripcion]=@p3,
                    [WEB_PermTipo]=@p1
			    WHERE [WEB_PermNombre] = @p0 AND [Web_PermControlador]=@p2
                ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(permiso.WEB_PermNombre) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermNombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(permiso.WEB_PermTipo) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermTipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(permiso.WEB_PermControlador) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermControlador));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(permiso.WEB_PermDescripcion) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermDescripcion));
                    query.Parameters.AddWithValue("@p4", permiso.WEB_PermEstado);

                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(permiso.WEB_ModuloNombre) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_ModuloNombre));

                    query.Parameters.AddWithValue("@p5", DateTime.Now);
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (respuesta,error);
        }

        public (bool respuesta,claseError error) BorrarPermiso(string permisonombre, string controlador)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"
                    declare @id int;
                    set @id =(select  [WEB_PermID] from  [SEG_Permiso]   WHERE [WEB_PermNombre] = @p0 AND [Web_PermControlador]=@p1)
                    Delete from [SEG_Permiso] WHERE [WEB_PermNombre] = @p0 AND [Web_PermControlador]=@p1
                    Delete from [dbo].[SEG_PermisoRol] WHERE [WEB_PermID] = @id";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisonombre);
                    query.Parameters.AddWithValue("@p1", controlador);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (respuesta,error);
        }
        public (SEG_PermisoEntidad webPermisoRol,claseError error) GetPermisoId(string controlador, string permisonombre)
        {
            claseError error = new claseError();
            SEG_PermisoEntidad webPermisoRol = new SEG_PermisoEntidad();

            string consulta = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                                ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso] where WEB_PermNombre = @p0 and WEB_PermControlador=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisonombre);
                    query.Parameters.AddWithValue("@p1", controlador);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            webPermisoRol.WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]);
                            webPermisoRol.WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"].Trim());
                            webPermisoRol.WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"].Trim());
                            webPermisoRol.WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"].Trim());
                            webPermisoRol.WEB_PermFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PermFechaRegistro"]);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (webPermisoRol,error);
        }

        public (List<SEG_PermisoEntidad> lista,claseError error) GetPermisos()
        {
            claseError error = new claseError();
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                              ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso] order by WEB_PermControlador,WEB_PermNombre ASC";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulos.ManageNullStr(dr["WEB_PermTipo"].Trim()),
                                WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"].Trim()),
                                WEB_PermDescripcion = ManejoNulos.ManageNullStr(dr["WEB_PermDescripcion"].Trim()),
                                WEB_PermEstado = ManejoNulos.ManageNullStr(dr["WEB_PermEstado"].Trim()),
                                WEB_PermFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PermFechaRegistro"].Trim())
                            };

                            lista.Add(webPermiso);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (lista,error);
        }

        public (List<SEG_PermisoEntidad> lista,claseError error) GetPermisosActivos()
        {
            claseError error = new claseError();
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                              ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso]
                            where WEB_PermEstado = 1
                            order by WEB_PermControlador,WEB_PermNombre ASC";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulos.ManageNullStr(dr["WEB_PermTipo"].Trim()),
                                WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"].Trim()),
                                WEB_PermDescripcion = ManejoNulos.ManageNullStr(dr["WEB_PermDescripcion"].Trim()),
                                WEB_PermEstado = ManejoNulos.ManageNullStr(dr["WEB_PermEstado"].Trim()),
                                WEB_PermFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PermFechaRegistro"].Trim())
                            };

                            lista.Add(webPermiso);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (lista,error);
        }

        public (bool respuesta,claseError error) ActualizarEstadoPermiso(int web_PermId, int estado)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Permiso]
                        SET [WEB_PermEstado] =@p1
                       WHERE [WEB_PermID] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", estado);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (respuesta, error) ;
        }

        public (bool respuesta,claseError error) ActualizarDescripcionPermiso(int web_PermId, string descripcion)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Permiso]
                        SET [WEB_PermDescripcion] =@p1
                       WHERE [WEB_PermID] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", descripcion);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (respuesta,error);
        }

        public (bool respuesta,claseError error) ActualizarNombrePermiso(int web_PermId, string nombre)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Permiso]
                        SET [WEB_PermNombreR] =@p1
                       WHERE [WEB_PermID] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", nombre);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (respuesta,error);
        }
    }
}