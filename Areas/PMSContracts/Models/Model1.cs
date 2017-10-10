namespace WebApp.Areas.PMSContracts.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=TechSpecsModel")
        {
        }

        public virtual DbSet<TechSpecsModel> CONTRACTS_CONDITIONS_TECHSPECS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TechSpecsModel>()
                .Property(e => e.TechSpecCode)
                .IsUnicode(false);

            modelBuilder.Entity<TechSpecsModel>()
                .Property(e => e.CreationBy)
                .IsUnicode(false);

            modelBuilder.Entity<TechSpecsModel>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TechSpecsModel>()
                .Property(e => e.OriginalLanguage)
                .IsUnicode(false);
        }
    }
}
