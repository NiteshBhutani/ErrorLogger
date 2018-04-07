namespace ErrorLoggerDBLayer
{
    using System;
    using System.Data.Entity;
    
    /// <summary>
    /// This class defines the DB Context/Model
    /// </summary>
    public class ErrorLoggerDBContext : DbContext
    {   /// <summary>
        /// Constructor
        /// </summary>
        public ErrorLoggerDBContext()
            : base("ErrorLoggerDB")
        {
            // Set the custom initializer
            Database.SetInitializer<ErrorLoggerDBContext>(new ErrorLoggerDBInitializer());
        }

        #region Properties used to build the DB

        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Error> Errors { get; set; }

        #endregion

        /// <summary>
        /// If you want to do anything custom, you can put it in here.. Otherwise, the method is a waste
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            // Configure UserId as FK for Users Table - One to One relationship b/w Login and User 
            modelBuilder.Entity<Login>()
                        .HasOptional(s => s.userDetails)
                        .WithRequired(ad => ad.loginDetails);

            // Configure Many to Many relationship b/w user and applications
            modelBuilder.Entity<User>()
              .HasMany<Application>(s => s.Applications)
              .WithMany(c => c.users)
              .Map(cs =>
              {
                  cs.ToTable("UserApplication");
              });

            // Configure One-to-Many relationship b/w Application and Error
            modelBuilder.Entity<Error>()
                    .HasRequired<Application>(s => s.application) // Student entity requires Standard 
                    .WithMany(s => s.errors); // Standard entity includes many Students entities

            base.OnModelCreating(modelBuilder);

        }
    }

}