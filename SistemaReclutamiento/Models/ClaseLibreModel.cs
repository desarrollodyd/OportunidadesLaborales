using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class ClaseLibreModel
    {
    }
    public class ReporteOfertaLaboral
    {
        public string ola_nombre { get; set; }
        public string ola_cod_empresa { get; set; }
        public string ola_cod_cargo { get; set; }
        public int fk_ubigeo { get; set; }
        public string busqueda { get; set; }
        public string ubi_pais_id { get; set; }
        public string ubi_departamento_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_distrito_id { get; set; }
        public string ola_rango_fecha { get; set; }
        public DateTime ola_fecha_ini { get; set; }
        public int pos_id { get; set; }
        public UbigeoEntidad ubigeo { get; set; }
        public ReporteOfertaLaboral()
        {
            this.ubigeo = new UbigeoEntidad();
        }
    }
    public class postulacionEntidad
    {
        public int ppo_id { get; set; }
        public string ppo_tipo_direcion { get; set; }
        public string ppo_direccion { get; set; }
        public string ppo_tipo_calle { get; set; }
        public string ppo_numero_casa { get; set; }
        public string ppo_tipo_casa { get; set; }
        public string ppo_celular { get; set; }
        public string ppo_estado_civil { get; set; }
        public bool ppo_brevete { get; set; }
        public string ppo_num_brevete { get; set; }
        public bool ppo_referido { get; set; }
        public string ppo_nombre_referido { get; set; }
        public string ppo_cv { get; set; }
        public string ppo_foto { get; set; }
        public string ppo_situacion { get; set; }
        public DateTime ppo_fecha_reg { get; set; }
        public DateTime ppo_fecha_act { get; set; }
        public string ppo_estado { get; set; }
        public int fk_postulante { get; set; }
        public int fk_oferta_laboral { get; set; }
    }
    public class claseError {
        public bool Respuesta { get; set; }
        public string Mensaje { get; set; }
        public claseError() {
            Respuesta = true;
            Mensaje = string.Empty;
        }
    }
    public class TMEMPR {
        public string CO_EMPR { get; set; }
        public string DE_NOMB { get; set; }
        public string DE_NOMB_CORT { get; set; }
        public string NO_DEPA { get; set; }
        public string NO_PROV { get; set; }
        public string NU_RUCS { get; set; }
        public string NO_REPR_LEGA { get; set; }
        public string NO_PAIS { get; set; }
        public string CO_GIRO { get; set; }
        public string DE_DIRE { get; set; }
        public string CO_UBIC_GEOG { get; set; }
        public string CO_PAIS { get; set; }
        public string DE_CODI_POST { get; set; }
        public string NU_TLF1 { get; set; }
        public string NU_TLF2 { get; set; }
        public string NU_FAXS { get; set; }
        public string DE_DIRE_WEBS { get; set; }
        public string DE_DIRE_MAIL { get; set; }
        public string CO_MONE_NACI { get; set; }
        public string NU_CERT_INSC { get; set; }
        public string NU_REGI_PUBL { get; set; }
        public string NU_LICE_MUNI { get; set; }
        public string NO_GERE_GRAL { get; set; }
        public string ST_PLAN { get; set; }
        public string ST_CONT { get; set; }
        public string ST_TESO { get; set; }
        public string ST_LOGI { get; set; }
        public string ST_VENT { get; set; }
        public string ST_PROD { get; set; }
        public string ST_ACTI { get; set; }
        public string ST_PRES { get; set; }
        public string ST_SIS1 { get; set; }
        public string ST_SIS2 { get; set; }
        public string ST_SIS3 { get; set; }
        public string ST_SIS4 { get; set; }
        public string ST_SIS5 { get; set; }
        public int NV_INFO_OSER { get; set; }
        public string NV_QUIE_OSER { get; set; }
        public string DE_RUTA_LOGO { get; set; }
        public string TI_DOID_REPR { get; set; }
        public string NU_DOID_REPR { get; set; }
        public string TI_DOID_GERE { get; set; }
        public string NU_DOID_GERE { get; set; }
        public string TI_SITU { get; set; }
        public string CO_REGI_LABO { get; set; }
        public DateTime FE_INSC { get; set; }
        public string CO_USUA_CREA { get; set; }
        public DateTime FE_USUA_CREA { get; set; }
        public string CO_USUA_MODI { get; set; }
        public DateTime FE_USUA_MODI { get; set; }
        public string ST_RERS { get; set; }
        public string DE_RUTA_BBVA { get; set; }
        public string de_ruta_exce { get; set; }
        public string ST_SCHU { get; set; }
        public string IP_SERV_SMTP { get; set; }
        public string NO_PORT { get; set; }
        public string ST_SSLS { get; set; }
        public string DE_DIRE_MAIL_WAPI { get; set; }
        public string NO_CLAV_MAIL_WAPI { get; set; }
        public string DE_DIRE_MAIL_ATEN { get; set; }
        public string ST_ORGA_EMPR { get; set; }
        public string ST_ORGA_PUES { get; set; }
    }
    public class TTPUES_TRAB {
        public string CO_EMPR { get; set; }
        public string CO_PUES_TRAB { get; set; }
        public string DE_PUES_TRAB { get; set; }
    }
    public class SEG_Usuario
    {
        public string NombreEmpleado { get; set; }
        public int UsuarioID { get; set; }
        public int EmpleadoID { get; set; }
        public string UsuarioNombre { get; set; }
        public string Estado { get; set; }
        public string Token { get; set; }
        public string DOI { get; set; }
        public string TokenPostgres { get; set; }
    }
    public class TTSEDE{
        public string CO_EMPR { get; set; }
        public string DE_NOMB { get; set; }
        public string CO_SEDE { get; set; }
        public string DE_SEDE { get; set; }
        public string ST_DOMI_FISC { get; set; }
        public string TI_SEDE_RTPS { get; set; }
        public string NU_RUCS_SEDE { get; set; }
        public string NO_DIRE_SEDE { get; set; }
        public string NU_CASA { get; set; }
        public string NU_INTE { get; set; }
        public string NO_ZONA { get; set; }

        public string CO_TIPO_CASA { get; set; }
        public string CO_TIPO_VIAS { get; set; }
        public string CO_TIPO_ZONA { get; set; }
        public string DE_REFE { get; set; }
        public string NU_FRUC { get; set; }
        public string ST_CRIE { get; set; }
        public double IM_CRIE { get; set; }
        public string CO_UBIC_GEOG { get; set; }
        public string CO_USUA_CREA { get; set; }
        public DateTime FE_USUA_CREA { get; set; }
        public string CO_USUA_MODI { get; set; }
        public DateTime FE_USUA_MODI { get; set; }
    }

}