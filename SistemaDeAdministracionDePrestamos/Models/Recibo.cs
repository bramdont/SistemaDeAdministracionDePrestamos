﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeAdministracionDePrestamos.Models
{
    public class Recibo
    {
        //Este modelo almacenara los recibos que se le entregaran
        // a los clientes a medida que pagan su prestamo
        public int Id { get; set; }
        [Required]
        [Display (Name ="Cliente")]
        public int ClienteId { get; set; }
        [Required]
        [Display (Name = "Monto a pagar")]
        public int MontoPago { get; set; }
        /* [Display (Name = "Total Prestado")]
        public int MontoTotal { get; set; } */
        [Display (Name = "Fecha de pago")]
        public DateTime FechaPago { get; set; }
        [Display(Name ="Prestamo")]
        public int PrestamoId { get; set; }
        public Cliente Cliente { get; set; }
        public Prestamo Prestamo { get; set; }
    }
}