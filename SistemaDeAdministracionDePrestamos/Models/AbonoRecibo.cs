using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class AbonoRecibo
    {
        //Este modelo almacenara los abonos que le 
        // hace un cliente a un recibo, pero sin 
        // terminar de pagarlo
        [Required]
        public int Id { get; set; }
        [Required]
        [Display (Name ="Recibo")]
        public int ReciboId { get; set; }
        [Required]
        [Display (Name ="Monto abonado")]
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        [Required]
        public bool Estatus { get; set; }
        public Recibo Recibo { get; set; }
    }
}