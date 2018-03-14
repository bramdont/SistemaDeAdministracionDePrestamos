namespace SistemaDeAdministracionDePrestamos.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SistemaDeAdministracionDePrestamos.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SistemaDeAdministracionDePrestamos.Models.ApplicationDbContext context)
        {
            for (int i = 0; i < 100; i++)
            {
                context.Clientes.AddOrUpdate(r => r.Nombre,
            new Cliente { Nombre = string.Format("Cliente{0}", i), Cedula = string.Format("000-000000{0}", i), Direccion = string.Format("Direccion{0}", i), Telefono = string.Format("Telefono{0}", i) });
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
