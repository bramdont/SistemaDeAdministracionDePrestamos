using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class SolicitudPrestamo
    {
        //Este modelo almacenara los clientes que han solicitado un prestamo
        [Required]
        public int Id { get; set; }
        [Required]
        [Display (Name ="Cliente")]
        public int ClienteId { get; set; }
        [Display(Name = "Monto Solicitado")]
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estatus { get; set; }
        public Cliente Cliente { get; set; }
    }
}