using BusX.Data.Models;
using BusX.Data.Base;
using BusX.Data.Extensions;
using BusX.Data.Interfaces;
using BusX.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;

namespace BusX.Data.Context
{
    public class BusXDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public BusXDbContext() : base() { }

        public BusXDbContext(DbContextOptions<BusXDbContext> options, IHttpContextAccessor _httpContextAccessor = null) : base(options)
        {
            if (_httpContextAccessor != null)
                httpContextAccessor = _httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(AppContext.BaseDirectory, "busx.sqlite");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        #region GEN Models
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<InProcessJourneySeat> InProcessJourneySeats { get; set; }
        #endregion GEN Models

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("BusX.Data")); //for custom entity config

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                    entityType.AddSoftDeleteQueryFilter();
            }

            // Decimal tipler
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,4)");
            }

            // DateTime tipler (SQLite uyumlu)
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                if (Database.IsSqlite())
                    property.SetColumnType("TEXT");  // SQLite
                else
                    property.SetColumnType("timestamptz");  // PostgreSQL
            }

            // Tablo ve kolon isimlerini snake_case yap
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().CamelCaseToUnderscore());

                foreach (var property in entity.GetProperties())
                    property.SetColumnName(property.Name.CamelCaseToUnderscore());

                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToLower(CultureInfo.InvariantCulture));

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName().CamelCaseToUnderscore());

                foreach (var key in entity.GetForeignKeys())
                {
                    key.PrincipalKey.SetName(key.PrincipalKey.GetName().CamelCaseToUnderscore());
                    key.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDatetimeToUTC();
            AddAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public virtual void SaveChanges(bool isAudit = false, long? userID = null)
        {
            ConvertDatetimeToUTC();
            if (isAudit) OnBeforeSaveChanges(userID);
            base.SaveChanges();
        }

        private void ConvertDatetimeToUTC()
        {
            var entities = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                var properties = entity.GetType().GetProperties();

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(DateTime) && property.CanWrite)
                    {
                        var dateTimeValue = (DateTime)property.GetValue(entity);
                        if (dateTimeValue.Kind == DateTimeKind.Local || dateTimeValue.Kind == DateTimeKind.Unspecified)
                        {
                            DateTime standardizedDate = new DateTime(dateTimeValue.Ticks - (dateTimeValue.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc)
                                .AddMilliseconds(dateTimeValue.Millisecond);
                            property.SetValue(entity, standardizedDate);
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime?) && property.CanWrite)
                    {
                        var dateTimeValue = (DateTime?)property.GetValue(entity);
                        if (dateTimeValue.HasValue && (dateTimeValue.Value.Kind == DateTimeKind.Local || dateTimeValue.Value.Kind == DateTimeKind.Unspecified))
                        {
                            DateTime standardizedDate = new DateTime(dateTimeValue.Value.Ticks - (dateTimeValue.Value.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc)
                                .AddMilliseconds(dateTimeValue.Value.Millisecond);
                            property.SetValue(entity, standardizedDate);
                        }
                    }
                }
            }
        }

        private void AddAuditInfo()
        {
            var entities = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var utcNow = DateTime.UtcNow;
            var user = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "0";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreateDate = utcNow;
                    entity.Entity.CreateUserID = Convert.ToInt32(user);
                }
                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.ModifyDate = utcNow;
                    entity.Entity.ModifyUserID = Convert.ToInt32(user);
                }
            }
        }

        private void OnBeforeSaveChanges(long? userID)
        {
            ChangeTracker.DetectChanges();
            // TODO: audit işlemleri
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;
                // audit logic buraya eklenebilir
            }
        }
    }
}
