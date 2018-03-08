using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeAdministracionDePrestamos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sistema de Administración de Préstamos";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Tu pagina de contacto.";

            return View();
        }
    }
}