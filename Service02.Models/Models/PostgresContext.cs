using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Service02.Models.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgagent", "pgagent");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Event_pkey");

            entity.ToTable("event");

            entity.HasIndex(e => e.IpAddress, "idx_ipaddress");

            entity.HasIndex(e => e.UserId, "idx_userid");

            entity.HasIndex(e => new { e.UserId, e.IpAddress }, "unique_user_ipaddress").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.IpAddress)
                .HasConversion(
                    ip => IPAddress.Parse(ip),
                    ip => ip.ToString()
                )
                .HasColumnType("inet")
                .HasColumnName("ip_address");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
