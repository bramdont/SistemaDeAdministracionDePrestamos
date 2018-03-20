namespace SistemaDeAdministracionDePrestamos.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<SistemaDeAdministracionDePrestamos.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SistemaDeAdministracionDePrestamos.Models.ApplicationDbContext context)
        {
            Cliente cliente = new Cliente();
            Prestamo prestamo = new Prestamo();
            Recibo recibo = new Recibo();

            //Clientes
            for (int i = 0; i < 100; i++)
            {
                cliente =
                    new Cliente { Nombre = string.Format("Cliente{0}", i), Cedula = string.Format("000-000000{0}", i), Direccion = string.Format("Direccion{0}", i), Telefono = string.Format("Telefono{0}", i) };

                context.Clientes.AddOrUpdate(r => r.Nombre, cliente);
                context.SaveChanges();
            }

            // Prestamos
            for (int i = 1; i <= 100; i++)
            {
                prestamo = new Prestamo { ClienteId = i, Monto = (i * 10), Fecha = DateTime.Today.AddDays(i), Estatus = true };

                context.Prestamos.AddOrUpdate(p => p.ClienteId, prestamo);
                context.SaveChanges();

                prestamo = context.Prestamos.OrderByDescending(p => p.Id).First();
                // Recibos
                for (int r = 1; r <= 13; r++)
                {
                    recibo = new Recibo { Cuota = r, MontoPago = (prestamo.Monto / 10), FechaPago = DateTime.Today.AddDays(7 * r), Estatus = true, PrestamoId = prestamo.Id };
                    context.Recibos.AddOrUpdate(a => a.Id, recibo);
                }
            }


        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //  to avoid creating duplicate seed data. E.g.
        //
        //    context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //
    }
}
}
