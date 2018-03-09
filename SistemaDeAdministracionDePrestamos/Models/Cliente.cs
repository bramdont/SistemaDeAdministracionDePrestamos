using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class Cliente
    {
        //Este modelo guardara la informacion de los clientes
        //a los que se les prestara dinero
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
}