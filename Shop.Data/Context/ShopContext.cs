using Azure;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities.Ticket;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.Order;
using Shop.Domain.Models.Permissions;
using Shop.Domain.Models.Product;
using Shop.Domain.Models.Wallet;

namespace Shop.Data.Context
{
    public class ShopContext:DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            
        }

        #region account

        public DbSet<User> Users { get; set; }

        #endregion

        #region ticket

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }

        #endregion

        #region product

        public DbSet<Product> Products { get; set; } 
        public DbSet<ProductCategory> ProductCategories { get; set; } 
        public DbSet<ProductDiscount> ProductDiscounts { get; set; } 
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }

        #endregion

        #region reole permission

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion

        #region order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        #endregion

        #region wallet

        public DbSet<Wallet> Wallets { get; set; }

        #endregion

        #region on model creating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region Seed Data

   
            modelBuilder.Entity<User>().HasData(new User()
            {
                Email = "998877123azaz@gmail.com@gmail.com",
                Password = "96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12", // 111111
                IsAdmin = true,
                FullName = "admin",
                ImageName = "admin",
                IsBan = false,
                Mobile = "093779077",
                CreateDate = DateTime.Now,
                EmailActiveCode = Guid.NewGuid().ToString("N"),
                IsEmailActive = true,
                TicketMessages = null,
                Tickets = null,
                UserRoles = null,
                Wallet = null,
                Id = 1
            });

          
            #endregion

            base.OnModelCreating(modelBuilder);
        }
        #endregion

    }
}
