using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class Prestamo
    {
        //Este modelo almacenara los prestamos que se han realizado
        [Required]
        public int Id { get; set; }
        [Required]
        [Display (Name ="Cliente")]
        public int ClienteId { get; set; }
        [Required]
        [Display (Name = "Monto prestado")]
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estatus { get; set; }
        public Cliente Cliente { get; set; }
    }
}