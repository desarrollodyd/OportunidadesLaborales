using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class ofertaLaboralEntidad
    {
        public int ola_id;
        public string ola_nombre;
        public string ola_requisitos;
        public string ola_funciones;
        public string ola_competencias;
        public string ola_condiciones_lab;
        public int ola_vacantes;
        public bool ola_enviar;
        public bool ola_enviado;
        public bool ola_publicado;
        public DateTime ola_fecha_pub;
        public string ola_estado_oferta;
        public int ola_duracion;
        public DateTime ola_fecha_fin;
        public DateTime ola_fecha_reg;
        public DateTime ola_fecha_act;
        public string ola_estado;
        public string ola_cod_empresa;
        public string ola_cod_unidad;
        public string ola_cod_sede;
        public string ola_cod_gerencia;
        public string ola_cod_area;
        public string ola_cod_puesto;
        public int fk_ubigeo;
        public int fk_usuario;
    }
}