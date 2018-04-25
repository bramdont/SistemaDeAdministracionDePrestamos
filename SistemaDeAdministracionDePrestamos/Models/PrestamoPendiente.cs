using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class PrestamoPendiente
    {
        public int Id { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estatus { get; set; }
    }
}