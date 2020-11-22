namespace WatchStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        ClientFirstName = c.String(nullable: false),
                        ClientLastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ClientID);
            
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        DealID = c.Int(nullable: false, identity: true),
                        ClientID = c.Int(nullable: false),
                        WatchID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DealID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.Watches", t => t.WatchID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.WatchID);
            
            CreateTable(
                "dbo.Watches",
                c => new
                    {
                        WatchID = c.Int(nullable: false, identity: true),
                        WatchName = c.String(nullable: false),
                        Brand = c.String(nullable: false),
                        WatchType = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        Resistant = c.Boolean(nullable: false),
                        price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WatchID);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        ManagerID = c.Int(nullable: false, identity: true),
                        ManagerName = c.String(nullable: false),
                        ManagerPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ManagerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deals", "WatchID", "dbo.Watches");
            DropForeignKey("dbo.Deals", "ClientID", "dbo.Clients");
            DropIndex("dbo.Deals", new[] { "WatchID" });
            DropIndex("dbo.Deals", new[] { "ClientID" });
            DropTable("dbo.Managers");
            DropTable("dbo.Watches");
            DropTable("dbo.Deals");
            DropTable("dbo.Clients");
        }
    }
}
