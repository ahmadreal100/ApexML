using System.Data.Entity;
using Apex.Core.Abstract;
using Apex.Core.Entities.AddressE;
using Apex.Core.Entities.UserE;
using Microsoft.AspNet.Identity.EntityFramework;
using Apex.Core.Entities.FrontE;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.ShopE;

namespace Apex.DAL.EF
{
    public class ApexContext : IdentityDbContext<User, Role, long, UserLogin, UserRole, UserClaim>
    {
        public ApexContext() : base("ApexContext")
        {

        }

        public static ApexContext Create()
        {
            return new ApexContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var user = modelBuilder.Entity<User>()
                .ToTable("AppUsers");

            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName).IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AppUserRoles");

            modelBuilder.Entity<UserLogin>()
                .HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey })
                .ToTable("AppUserLogins");

            modelBuilder.Entity<UserClaim>()
                .ToTable("AppUserClaims");

            var role = modelBuilder.Entity<Role>()
                .ToTable("AppRoles");

            role.Property(r => r.Name).IsRequired();
            role.HasMany(r => r.Users).WithRequired()
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<OperatorInfo>()
                .HasRequired(x => x.User)
                .WithRequiredDependent(x => x.OperatorInfo)
                .WillCascadeOnDelete(true);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ThemeSetting> ThemeSettings { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }

        public DbSet<Language> Languages { get; set; }

        //--------------------------------------------------------------------
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<MasterInfoTranslation> MasterInfoTranslations { get; set; }
        public DbSet<ThemeSettingTranslation> ThemeSettingTranslations { get; set; }
        public DbSet<AdvertTranslation> AdvertTranslations { get; set; }

        //--------------------------------------------------------------------
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderPicture> SliderPictures { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Advert> Adverts { get; set; }

        public DbSet<Visit> Visits { get; set; }
    }
}
