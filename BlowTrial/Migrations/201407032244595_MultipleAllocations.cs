namespace BlowTrial.Migrations.TrialData
{
    using BlowTrial.Domain.Outcomes;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultipleAllocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AllocationBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GroupRepeats = c.Byte(nullable: false),
                        AllocationGroup = c.Int(nullable: false),
                        RandomisationCategory = c.Int(nullable: false),
                        RecordLastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BalancedAllocations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StudyCentreId = c.Int(nullable: false),
                        RandomisationCategory = c.Int(nullable: false),
                        IsEqualised = c.Boolean(nullable: false),
                        RecordLastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyCentres", t => t.StudyCentreId, cascadeDelete: true)
                .Index(t => t.StudyCentreId);
            
            AddColumn("dbo.Participants", "TrialArm", c => c.Int(nullable: false));
            AddColumn("dbo.Participants", "BcgPapuleAt28days", c => c.Boolean());
            AddColumn("dbo.Participants", "AllocationBlockId", c => c.Int());
            AddColumn("dbo.Participants", "UserMarkedFinished", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Participants", "AllocationBlockId");
            AddForeignKey("dbo.Participants", "AllocationBlockId", "dbo.AllocationBlocks", "Id");

            //RenameColumn("dbo.Participants", "BcgPapule", "BcgPapuleAtDischarge");
            Sql(string.Format("update Participants Set TrialArm = case when [IsInterventionArm]=0 then {0} else {1} end", (int)RandomisationArm.Control, (int)RandomisationArm.RussianBCG));

            DropColumn("dbo.Participants", "IsInterventionArm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Participants", "IsInterventionArm", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.BalancedAllocations", "StudyCentreId", "dbo.StudyCentres");
            DropForeignKey("dbo.Participants", "AllocationBlockId", "dbo.AllocationBlocks");
            DropIndex("dbo.BalancedAllocations", new[] { "StudyCentreId" });
            DropIndex("dbo.Participants", new[] { "AllocationBlockId" });
            DropColumn("dbo.Participants", "UserMarkedFinished");
            DropColumn("dbo.Participants", "AllocationBlockId");
            DropColumn("dbo.Participants", "BcgPapuleAt28days");
            DropColumn("dbo.Participants", "TrialArm");
            DropTable("dbo.BalancedAllocations");
            DropTable("dbo.AllocationBlocks");
        }
    }
}
