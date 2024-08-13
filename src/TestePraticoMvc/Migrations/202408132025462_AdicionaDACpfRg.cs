namespace TestePraticoMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionaDACpfRg : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pessoas", "Cpf", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.Pessoas", "Rg", c => c.String(nullable: false, maxLength: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoas", "Rg", c => c.String());
            AlterColumn("dbo.Pessoas", "Cpf", c => c.String());
        }
    }
}
