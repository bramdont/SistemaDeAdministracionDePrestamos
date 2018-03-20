namespace SistemaDeAdministracionDePrestamoes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrestyReci : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prestamoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        Monto = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId, cascadeDelete: true)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.Reciboes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cuota = c.Int(nullable: false),
                        MontoPago = c.Int(nullable: false),
                        FechaPago = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                        PrestamoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prestamoes", t => t.PrestamoId, cascadeDelete: true)
                .Index(t => t.PrestamoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reciboes", "PrestamoId", "dbo.Prestamoes");
            DropForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Reciboes", new[] { "PrestamoId" });
            DropIndex("dbo.Prestamoes", new[] { "ClienteId" });
            DropTable("dbo.Reciboes");
            DropTable("dbo.Prestamoes");
        }
    }
}
