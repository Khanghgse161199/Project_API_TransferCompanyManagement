using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entities;

public partial class TransferCompanyContext : DbContext
{
    public TransferCompanyContext()
    {
    }

    public TransferCompanyContext(DbContextOptions<TransferCompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<CategoryBlock> CategoryBlocks { get; set; }

    public virtual DbSet<CategoryContainer> CategoryContainers { get; set; }

    public virtual DbSet<Container> Containers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<MainOrder> MainOrders { get; set; }

    public virtual DbSet<OrderShipping> OrderShippings { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TransitCar> TransitCars { get; set; }

    public virtual DbSet<WorkMapping> WorkMappings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=TransferCompany;User Id=test;Password=Test;TrustServerCertificate=True;Trusted_Connection=true;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Password).HasMaxLength(400);
            entity.Property(e => e.RoleId).HasMaxLength(400);
            entity.Property(e => e.Username).HasMaxLength(400);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Block>(entity =>
        {
            entity.ToTable("Block");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.CategoryBlockId).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);

            entity.HasOne(d => d.CategoryBlock).WithMany(p => p.Blocks)
                .HasForeignKey(d => d.CategoryBlockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Block_CategoryBlock1");
        });

        modelBuilder.Entity<CategoryBlock>(entity =>
        {
            entity.ToTable("CategoryBlock");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<CategoryContainer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CategoryTrans");

            entity.ToTable("CategoryContainer");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Container>(entity =>
        {
            entity.ToTable("Container");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.CategoryTransId).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.CategoryTrans).WithMany(p => p.Containers)
                .HasForeignKey(d => d.CategoryTransId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Container_CategoryContainer1");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AccId).HasMaxLength(400);
            entity.Property(e => e.Address).HasMaxLength(700);
            entity.Property(e => e.CitizenId).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(400);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.Acc).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Account");
        });

        modelBuilder.Entity<MainOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Order");

            entity.ToTable("MainOrder");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.DateTimeDone).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Reciver).HasMaxLength(200);
            entity.Property(e => e.ReciverAddress).HasMaxLength(200);
            entity.Property(e => e.ReciverCitizenId).HasMaxLength(200);
            entity.Property(e => e.ReciverEmail).HasMaxLength(200);
            entity.Property(e => e.ReciverPhone).HasMaxLength(200);
            entity.Property(e => e.Sender).HasMaxLength(200);
            entity.Property(e => e.Total).HasColumnType("money");
        });

        modelBuilder.Entity<OrderShipping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderDetail");

            entity.ToTable("OrderShipping");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.BlockId).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.DateTimeRecive).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.MainOrderId).HasMaxLength(400);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.WorkMappingId).HasMaxLength(400);

            entity.HasOne(d => d.Block).WithMany(p => p.OrderShippings)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderShipping_Block1");

            entity.HasOne(d => d.MainOrder).WithMany(p => p.OrderShippings)
                .HasForeignKey(d => d.MainOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderShipping_MainOrder1");

            entity.HasOne(d => d.WorkMapping).WithMany(p => p.OrderShippings)
                .HasForeignKey(d => d.WorkMappingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderShipping_WorkMapping1");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Rating");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.ImgUrl).HasMaxLength(700);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Reciver).HasMaxLength(400);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.ToTable("Token");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AccId).HasMaxLength(400);
            entity.Property(e => e.AccessToken).HasMaxLength(400);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EmailToken).HasMaxLength(400);
            entity.Property(e => e.EtcreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("ETCreatedDate");
            entity.Property(e => e.EtisActive).HasColumnName("ETisActive");

            entity.HasOne(d => d.Acc).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Account");
        });

        modelBuilder.Entity<TransitCar>(entity =>
        {
            entity.ToTable("TransitCar");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Brand).HasMaxLength(200);
            entity.Property(e => e.DateRegister).HasColumnType("date");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.OriginCompany).HasMaxLength(200);
            entity.Property(e => e.OutOfDate).HasColumnType("date");
        });

        modelBuilder.Entity<WorkMapping>(entity =>
        {
            entity.ToTable("WorkMapping");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.ContainerId).HasMaxLength(400);
            entity.Property(e => e.DateTimeCreate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasMaxLength(400);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.RatingId).HasMaxLength(400);
            entity.Property(e => e.TransitCarId).HasMaxLength(400);

            entity.HasOne(d => d.Container).WithMany(p => p.WorkMappings)
                .HasForeignKey(d => d.ContainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkMapping_Container1");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkMappings)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_WorkMapping_Employee1");

            entity.HasOne(d => d.Rating).WithMany(p => p.WorkMappings)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkMapping_Rating");

            entity.HasOne(d => d.TransitCar).WithMany(p => p.WorkMappings)
                .HasForeignKey(d => d.TransitCarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkMapping_TransitCar");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
