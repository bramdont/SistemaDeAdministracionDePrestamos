using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models.ViewModels
{
    public class PrestPendViewModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        [Display(Name = "Monto a prestar")]
        public int Monto { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public Status Estatus { get; set; }

        public enum Status
        {
            Pendiente,
            Realizado
        }
    }

}