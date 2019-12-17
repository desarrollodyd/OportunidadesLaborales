using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.Postulante;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.Postulante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.Postulante
{
    public class PostulanteFavoritosController : Controller
    {
        // GET: PostulanteFavoritos
        PostulanteFavoritosModel postulanteFavoritosbl = new PostulanteFavoritosModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostulanteFavoritosInsertaJson(int ola_id) {
            PostulanteEntidad postulante = (PostulanteEntidad)Session["postulante"];
            PostulanteFavoritosEntidad postulanteFavoritos = new PostulanteFavoritosEntidad();
            postulanteFavoritos.fk_oferta_laboral = ola_id;
            postulanteFavoritos.fk_postulante = postulante.pos_id;
            postulanteFavoritos.posfav_estado = "A";
            postulanteFavoritos.posfav_notificar = true;
            bool idPostulanteFavoritoInsertado = false;
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            try {
                var favoritosTupla = postulanteFavoritosbl.IntranetPostulanteFavoritosInsertarJson(postulanteFavoritos);
                error = favoritosTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    idPostulanteFavoritoInsertado = favoritosTupla.idIntranetPostulanteFavoritosInsertado;
                    if (idPostulanteFavoritoInsertado ==true)
                    {
                        errormensaje = "Agregado a Favoritos";
                        response = true;
                    }
                    else
                    {
                        errormensaje = "No se pudo Agregar a Favoritos";
                    }
                }
                else {
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje, data = idPostulanteFavoritoInsertado });
        }
    }
}