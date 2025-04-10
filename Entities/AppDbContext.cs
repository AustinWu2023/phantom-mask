using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace phantom_mask.Entities {
    public class AppDbContext : DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Mask> Masks { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
    }
}
