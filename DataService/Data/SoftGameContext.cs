using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataService.Entity;

namespace DataService.Data
{
    public partial class SoftGameContext : DbContext
    {
        public SoftGameContext()
        {
        }

        public SoftGameContext(DbContextOptions<SoftGameContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountRole> AccountRoles { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductUser> ProductUsers { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<ServerSoft> ServerSofts { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionType> TransactionTypes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            var constr = "";
            if (File.Exists(filePath))
            {
                constr = File.ReadLines(filePath).First();
                Console.WriteLine($"First line: {constr}");
                File.AppendAllText(filePath, Environment.NewLine + "This is a new line of text.");
            }
            else
            {
                Console.WriteLine("File not found in project directory.");
                File.WriteAllText(filePath, "This is the first line of the new file." + Environment.NewLine);
                File.AppendAllText(filePath, "This is a new line of text.");
            }

            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=185.126.6.10,2525;Initial Catalog=SoftGame;Persist Security Info=True;User ID=sa;Password=Aaq1234567;Connect Timeout=120");
                optionsBuilder.UseSqlServer(constr);
                // 185.126.11.237 navid New : *****
                // 185.126.6.10 panahi New : *****
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.CountFailedLimit).HasDefaultValueSql("((3))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Accounts_AccountRole");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Product");
            });

            modelBuilder.Entity<ProductUser>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductUsers)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductUser_Product");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.LimitUserActive).HasDefaultValueSql("((100))");
            });

            modelBuilder.Entity<ServerSoft>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Settell).HasDefaultValueSql("((1))");

                entity.Property(e => e.SiteId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Accounts");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_Transaction_Currency");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Transaction_Product");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_TransactionType");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ProductId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_User_Accounts");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_User_Product");

                entity.HasOne(d => d.Server)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Server");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
