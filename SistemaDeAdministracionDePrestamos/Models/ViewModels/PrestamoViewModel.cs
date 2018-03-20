using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeAdministracionDePrestamos.Models.ViewModels
{
    public class PrestamoViewModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        public estatus Status { get; set; }
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public IEnumerable<SelectListItem> Estatus { get; set; }
        public enum estatus
        {
            Activo,
            Inactivo
        }


    }



}