using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class AtrasoRecibo
    {
        //Este modelo almacenara los recibos atrasados,
        //Cuando el cliente pague el atraso, su estatus
        // pasara a False
        [Required]
        public int Id { get; set; }
        [Required]
        [Display (Name = "Recibo")]
        public int ReciboId { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estatus { get; set; }
        public Recibo Recibo { get; set; }
    }
}