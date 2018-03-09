using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class Pago
    {
        //Este modelo almacenara los recibos que han sido pagado  en su totalidad
        //y la fecha en la que se realizo el pago.
        [Required]
        public int Id { get; set; }
        [Required]
        [Display (Name ="Recibo")]
        public int ReciboId { get; set; }
        public DateTime Fecha { get; set; }
        public Recibo Recibo { get; set; }
    }
}