using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System.Data.SqlClient;

namespace SistemaReclutamiento.Models
{
    public class personaModel
    {
        string _conexion;
        public personaModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        internal personaEntidad PersonaIdObtenerJson(int idPersona)
        {
            personaEntidad persona = new personaEntidad();
            string consulta = @"SELECT [personaId]
                                                  ,[personaNroDocumento]
                                                  ,[tipoDocumentoId]
                                                  ,[personaNombre]
                                                  ,[personaApellidoPaterno]
                                                  ,[personaApellidoMaterno]
                                                  ,[personaEmail]
                                                  ,[personaEstado]
                                                  ,[personaFechaNacimiento]
                                                  ,[personaDireccion]
                                                  ,[personaContacto1]
                                                  ,[personaContacto2]
                                                  ,[personaTelefono]
                                                  ,[personaSexo]
                                                  ,[personaEstadoCivil]
                                              FROM [dbo].[Persona]
                                            where personaId=@p0";         
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", idPersona);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                persona.personaId = ManejoNulos.ManageNullInteger(dr["personaId"]);
                                persona.tipoDocumentoId = ManejoNulos.ManageNullInteger(dr["tipoDocumentoId"]);
                                persona.personaNroDocumento = ManejoNulos.ManageNullStr(dr["personaNroDocumento"]);
                                persona.personaNombre = ManejoNulos.ManageNullStr(dr["personaNombre"]);
                                persona.personaApellidoPaterno = ManejoNulos.ManageNullStr(dr["personaApellidoPaterno"]);
                                persona.personaApellidoMaterno = ManejoNulos.ManageNullStr(dr["personaApellidoMaterno"]);
                                persona.personaEmail = ManejoNulos.ManageNullStr(dr["personaEmail"]);
                                persona.personaEstado = ManejoNulos.ManageNullInteger(dr["personaId"]);
                                persona.personaFechaNacimiento = ManejoNulos.ManageNullDate(dr["personaFechaNacimiento"]);
                                persona.personaDireccion = ManejoNulos.ManageNullStr(dr["personaDireccion"]);
                                persona.personaContacto1 = ManejoNulos.ManageNullStr(dr["personaContacto1"]);
                                persona.personaContacto2 = ManejoNulos.ManageNullStr(dr["personaContacto2"]);
                                persona.personaTelefono = ManejoNulos.ManageNullStr(dr["personaTelefono"]);
                                persona.personaSexo = ManejoNulos.ManageNullStr(dr["personaSexo"]);
                                persona.personaEstadoCivil = ManejoNulos.ManageNullStr(dr["personaEstadoCivil"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return persona;
        }
        
        public int PersonaInsertarJson(personaEntidad persona)
        {
            int idPersonaInsertada=0;
            //bool response = false;
            string consulta = @"INSERT INTO [dbo].[Persona]
                                   ([personaNroDocumento]
                                    ,[personaNombre]
                                    ,[personaApellidoPaterno]
                                    ,[personaApellidoMaterno]
                                    ,[personaEmail]                                
                                    ,[personaEstado]
                                    ,[tipoDocumentoId])
                             VALUES
                                   (@p0,@p1,@p2,@p3,@p4,@p5,@p6) 
                                SELECT SCOPE_IDENTITY()";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", persona.personaNroDocumento);
                    query.Parameters.AddWithValue("@p1", persona.personaNombre);
                    query.Parameters.AddWithValue("@p2", persona.personaApellidoPaterno);
                    query.Parameters.AddWithValue("@p3", persona.personaApellidoMaterno);
                    query.Parameters.AddWithValue("@p4", persona.personaEmail);
                    query.Parameters.AddWithValue("@p5", persona.personaEstado);
                    query.Parameters.AddWithValue("@p6", persona.tipoDocumentoId);

                    idPersonaInsertada = Int32.Parse(query.ExecuteScalar().ToString());
                  
                    //response = true;
                }
            }
            catch (Exception ex)
            {
            }
            return idPersonaInsertada;
        }
        public bool PersonaEditarJson(personaEntidad persona)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[Persona]
                                SET 
                             [personaNroDocumento]=@p1
                            ,[personaNombre]=@p2
                            ,[personaApellidoPaterno]=@p3
                            ,[personaApellidoMaterno]=@p4
                            ,[personaEmail]=@p5
                            ,[personaEstado]=@p6
                            ,[personaFechaNacimiento]=@p7
                            ,[personaDireccion]=@p8
                            ,[personaContacto1]=@p9
                            ,[personaContacto2]=@p10
                            ,[personaTelefono]=@p11
                            ,[personaSexo]=@p12
                            ,[personaEstadoCivil]=@p13                          
                               WHERE personaId= @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", persona.personaId);
                    query.Parameters.AddWithValue("@p1", persona.personaNroDocumento);
                    query.Parameters.AddWithValue("@p2", persona.personaNombre);
                    query.Parameters.AddWithValue("@p3", persona.personaApellidoPaterno);
                    query.Parameters.AddWithValue("@p4", persona.personaApellidoMaterno);
                    query.Parameters.AddWithValue("@p5", persona.personaEmail);
                    query.Parameters.AddWithValue("@p6", persona.personaEstado);
                    query.Parameters.AddWithValue("@p7", persona.personaFechaNacimiento);
                    query.Parameters.AddWithValue("@p8", persona.personaDireccion);
                    query.Parameters.AddWithValue("@p9", persona.personaContacto1);
                    query.Parameters.AddWithValue("@p10", persona.personaContacto2);
                    query.Parameters.AddWithValue("@p11", persona.personaTelefono);
                    query.Parameters.AddWithValue("@p12", persona.personaSexo);
                    query.Parameters.AddWithValue("@p13", persona.personaEstadoCivil);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }
        internal personaEntidad PersonaDniEmailObtenerJson(string personaEmail, string personaNroDocumento)
        {
            personaEntidad persona = new personaEntidad();
            string consulta = @"SELECT [personaId]
                                                  ,[personaNroDocumento]                                             
                                                  ,[personaEmail]                                        
                                              FROM [dbo].[Persona]
                                            where personaNroDocumento=@p0 OR personaEmail=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", personaNroDocumento);
                    query.Parameters.AddWithValue("@p1", personaEmail);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                persona.personaId = ManejoNulos.ManageNullInteger(dr["personaId"]);
                                persona.personaNroDocumento = ManejoNulos.ManageNullStr(dr["personaNroDocumento"]);
                                persona.personaEmail = ManejoNulos.ManageNullStr(dr["personaEmail"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return persona;
        }
    }
}