using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeAdministracionDePrestamos.Models.ViewModels
{
    public class PrestamoViewModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public int Monto { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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