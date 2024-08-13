namespace TestePraticoMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pessoas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 30),
                        Sobrenome = c.String(nullable: false, maxLength: 30),
                        DataNascimento = c.DateTime(nullable: false),
                        EstadoCivil = c.String(nullable: false, maxLength: 20),
                        Cpf = c.String(),
                        Rg = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pessoas");
        }
    }
}
