namespace FireV2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StoreDb : DbContext
    {
        public StoreDb()
            : base("name=StoreDb")
        {
        }
        public virtual DbSet<IndOperationPoint> IndOperationPoints { get; set; }
        public virtual DbSet<PackPackInd> PackPackInds { get; set; }
        public virtual DbSet<PackPointTeam> PackPointTeams { get; set; }
        public virtual DbSet<DissType> DissTypes { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Archive> Archives { get; set; }
        public virtual DbSet<RengAthleteInPeriod> RengAthleteInPeriods { get; set; }
        public virtual DbSet<AgeRenge> AgeRenges { get; set; }
        public virtual DbSet<EditTable> EditTables { get; set; }
        public virtual DbSet<AthTypeMedical> AthTypeMedicals { get; set; }
        public virtual DbSet<FileUpload> FileUploads { get; set; }
        public virtual DbSet<MedicalItem> MedicalItems { get; set; }
        public virtual DbSet<MyAction> MyActions { get; set; }
        public virtual DbSet<RoleAction> RoleActions { get; set; }
        public virtual DbSet<UserAction> UserActions { get; set; }
        public virtual DbSet<Ostan> Ostans { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<BMI> BMIS { get; set; }
        public virtual DbSet<Athlete> Athletes { get; set; }
        public virtual DbSet<MAthleteByPeriod> MAthleteByPeriods { get; set; }
        public virtual DbSet<Medical> Medicals { get; set; }
        public virtual DbSet<MedicalTitleType> MedicalTitleTypes { get; set; }
        public virtual DbSet<Degree> Degrees { get; set; }
        public virtual DbSet<DissuasionsTitle> DissuasionsTitles { get; set; }
        public virtual DbSet<DissusionItemByPeroid> DissusionItemByPeroids { get; set; }
        public virtual DbSet<Dissusion> Dissusions { get; set; }
        public virtual DbSet<DocTypeAthelet> DocTypeAthelets { get; set; }
        public virtual DbSet<EmployeeStatu> EmployeeStatus { get; set; }
        public virtual DbSet<EmployeType> EmployeTypes { get; set; }
        public virtual DbSet<IndividualHistory> IndividualHistorys { get; set; }
        public virtual DbSet<IndividualOperationItem> IndividualOperationItems { get; set; }
        public virtual DbSet<IndivisualOperationHistoryItem> IndivisualOperationHistoryItems { get; set; }
        public virtual DbSet<IndOperationItemsByPeriod> IndOperationItemsByPeriods { get; set; }
        public virtual DbSet<InfoDocumentTitle> InfoDocumentTitles { get; set; }
        public virtual DbSet<InfoDocumentTitleType> InfoDocumentTitleTypes { get; set; }
        public virtual DbSet<MaritalStatu> MaritalStatus { get; set; }
        public virtual DbSet<MedicalExam> MedicalExams { get; set; }
        public virtual DbSet<MRecord> MRecords { get; set; }
        public virtual DbSet<Occupationalfield> Occupationalfields { get; set; }
        public virtual DbSet<PhReadinessItemByPeriod> PhReadinessItemByPeriods { get; set; }
        public virtual DbSet<PhRedinessHistoryItem> PhRedinessHistoryItems { get; set; }
        public virtual DbSet<phRItemsPoint> phRItemsPoints { get; set; }
        public virtual DbSet<PhysicalEducationItem> PhysicalEducationItems { get; set; }
        public virtual DbSet<PhysicalEducation> PhysicalEducations { get; set; }
        public virtual DbSet<PhysicalExamHistory> PhysicalExamHistories { get; set; }
        public virtual DbSet<PhysicalReadinesitem> PhysicalReadinesitems { get; set; }
        public virtual DbSet<PostSazmani> PostSazmanis { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Semat> Semats { get; set; }
        public virtual DbSet<Share> Shares { get; set; }
        public virtual DbSet<ShiftWork> ShiftWorks { get; set; }
        public virtual DbSet<SportScoreByPeriod> SportScoreByPeriods { get; set; }
        public virtual DbSet<SportsScoreTitle> SportsScoreTitles { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<TeamByPeriod> TeamByPeriods { get; set; }
        public virtual DbSet<TeamWorkPoint> TeamWorkPoints { get; set; }
        public virtual DbSet<TeamWorkTitle> TeamWorkTitles { get; set; }
        public virtual DbSet<TypeOfExam> TypeOfExams { get; set; }
        public virtual DbSet<TypeOfTest> TypeOfTests { get; set; }
        public virtual DbSet<Unjustified> Unjustifieds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgeRenge>()
                .HasMany(e => e.phRItemsPoints)
                .WithRequired(e => e.AgeRenge)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<AspNetRole>()
            //    .HasMany(e => e.AspNetUsers)
            //    .WithMany(e => e.AspNetRoles)
            //    .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            //modelBuilder.Entity<AspNetUser>()
            //    .HasMany(e => e.AspNetUserClaims)
            //    .WithRequired(e => e.AspNetUser)
            //    .HasForeignKey(e => e.UserId);

            //modelBuilder.Entity<AspNetUser>()
            //    .HasMany(e => e.AspNetUserLogins)
            //    .WithRequired(e => e.AspNetUser)
            //    .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Athlete>()
                .HasMany(e => e.Dissusions)
                .WithRequired(e => e.Athlete)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Athlete>()
                .HasMany(e => e.IndividualHistorys)
                .WithRequired(e => e.Athlete)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Athlete>()
                .HasMany(e => e.PhysicalExamHistories)
                .WithRequired(e => e.Athlete)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Degree>()
                .HasMany(e => e.Athletes)
                .WithRequired(e => e.Degree)
                .WillCascadeOnDelete(false);
            

            modelBuilder.Entity<EmployeeStatu>()
                .HasMany(e => e.Athletes)
                .WithRequired(e => e.EmployeeStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndividualHistory>()
                .HasMany(e => e.IndivisualOperationHistoryItems)
                .WithRequired(e => e.IndividualHistory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndividualOperationItem>()
                .HasMany(e => e.IndivisualOperationHistoryItems)
                .WithRequired(e => e.IndividualOperationItem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndividualOperationItem>()
                .HasMany(e => e.IndOperationItemsByPeriods)
                .WithRequired(e => e.IndividualOperationItem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InfoDocumentTitle>()
                .HasMany(e => e.InfoDocumentTitleTypes)
                .WithRequired(e => e.InfoDocumentTitle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MedicalItem>()
                .HasMany(e => e.MedicalTitleTypes)
                .WithRequired(e => e.MedicalItem)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MyAction>()
            //    .HasMany(e => e.RoleActions)
            //    .WithRequired(e => e.MyAction)
            //    .WillCascadeOnDelete(false);


            modelBuilder.Entity<Period>()
                .HasMany(e => e.Dissusions)
                .WithRequired(e => e.Period)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Period>()
                .HasMany(e => e.IndividualHistorys)
                .WithRequired(e => e.Period)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Period>()
                .HasMany(e => e.PhysicalExamHistories)
                .WithRequired(e => e.Period)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Period>()
                .HasMany(e => e.TeamByPeriods)
                .WithRequired(e => e.Period)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhysicalExamHistory>()
                .HasMany(e => e.PhRedinessHistoryItems)
                .WithRequired(e => e.PhysicalExamHistory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhysicalReadinesitem>()
                .HasMany(e => e.PhReadinessItemByPeriods)
                .WithRequired(e => e.PhysicalReadinesitem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhysicalReadinesitem>()
                .HasMany(e => e.PhRedinessHistoryItems)
                .WithRequired(e => e.PhysicalReadinesitem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Share>()
                .HasMany(e => e.Athletes)
                .WithRequired(e => e.Share)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ShiftWork>()
                .HasMany(e => e.Athletes)
                .WithRequired(e => e.ShiftWork)
                .WillCascadeOnDelete(false);

         
        }

        public System.Data.Entity.DbSet<FireV2.ViewModels.PhysicalHistoryReportVM> PhysicalHistoryReportVMs { get; set; }
    }
}
