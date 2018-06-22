using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vaistine.Areas.Cags.Models;
using Vaistine.Areas.Docs.Models;
using Vaistine.Areas.Goods.Models;
using Vaistine.Areas.Stores.Models;
using Vaistine.Models;

namespace Vaistine.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Cag> Cags { get; set; }
        public virtual DbSet<GoodCategory> Categories { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<DocHead> Docs { get; set; }
        public virtual DbSet<DocLine> DocLines { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Initialize();
        }

        public ApplicationDbContext()
            : base()
        {
            //Initialize();
        }

        private void Initialize()
        {
            if (!Stores.Any())
            {
                Stores.Add(new Store { Descr = "Главный склад", IsAccount=true });
                Stores.Add(new Store { Descr = "Склад 1", IsAccount = true});
                Stores.Add(new Store { Descr = "Склад 2", IsAccount=false });
                SaveChanges();
            }
            if (!Cags.Any())
            {
                Cags.Add(new Cag { Descr="АктивМед" });
                Cags.Add(new Cag { Descr = "Пакт-М" });
            }
            if (!Categories.Any())
            {
                Categories.Add(new GoodCategory { Descr = "Лекарства" });
                Categories.Add(new GoodCategory { Descr = "БАДы" });
            }
            if (!Units.Any())
            {
                Units.Add(new Unit { Descr = "Штука", Scale = 1.0, ShortDescr = "шт"});
                Units.Add(new Unit { Descr = "Упаковка", Scale = 1.0, ShortDescr = "уп" });
            }
            if (!Components.Any())
            {
                Components.Add(new Component { Descr = "Нафазолин"});
                Components.Add(new Component { Descr = "Амбазон"});
            }
            if (!Goods.Any())
            {
                Goods.Add(new Good { Descr = "Нафтизин" });
                Goods.Add(new Good { Descr = "Фарингосепт" });
            }

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Store>(s =>
          {
              s.HasOne(x => x.Owner)
              .WithMany(x => x.Stores)
              .HasForeignKey(x => x.OwnerId)
              .OnDelete(DeleteBehavior.Restrict);
          });

            builder.Entity<Cag>(c =>
            {
                c.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<GoodCategory>(g =>
            {
                g.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Unit>(u =>
            {
                u.HasOne(x => x.BaseUnit)
                .WithMany(x => x.ChildrenUnits)
                .HasForeignKey(x => x.BaseUnitId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Good>(g =>
            {
                g.HasOne(x => x.Category)
                .WithMany(x => x.Goods)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

                g.HasOne(x => x.Unit)
                .WithMany(x => x.Goods)
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

                g.HasOne(x => x.MainComponent)
                .WithMany(x => x.Goods)
                .HasForeignKey(x => x.MainComponentId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DocHead>(h =>
            {
                h.HasOne(x => x.FromStore)
                .WithMany(x => x.OutDocs)
                .HasForeignKey(x => x.FromStoreId)
                .OnDelete(DeleteBehavior.Restrict);

                h.HasOne(x => x.ToStore)
                .WithMany(x => x.InDocs)
                .HasForeignKey(x => x.ToStoreId)
                .OnDelete(DeleteBehavior.Restrict);

                h.HasOne(x => x.FromCag)
                .WithMany(x => x.OutDocs)
                .HasForeignKey(x => x.FromCagId)
                .OnDelete(DeleteBehavior.Restrict);

                h.HasOne(x => x.ToCag)
                .WithMany(x => x.InDocs)
                .HasForeignKey(x => x.ToCagId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<DocLine>(l =>
            {
                l.HasOne(x => x.DocHead)
                .WithMany(x => x.DocLines)
                .HasForeignKey(x => x.DocHeadId)
                .OnDelete(DeleteBehavior.Restrict);

                l.HasOne(x => x.Good)
                .WithMany(x => x.DocLines)
                .HasForeignKey(x => x.GoodId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
