using Domain.Commons;
using Domain.Entities;
using Infrastructure.Interceptor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Infrastructure.ApplicationDbContext;

public partial class GreenWheelDbContext : DbContext, IGreenWheelDbContext
{
    private readonly UpdateTimestampInterceptor _updateInterceptor;
    public GreenWheelDbContext()
    {
    }

    public GreenWheelDbContext(DbContextOptions<GreenWheelDbContext> options, UpdateTimestampInterceptor updateTimestampInterceptor)
        : base(options)
    {
        _updateInterceptor = updateTimestampInterceptor;
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<CitizenIdentity> CitizenIdentities { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<DispatchRequest> DispatchRequests { get; set; }

    public virtual DbSet<DispatchRequestStaff> DispatchRequestStaffs { get; set; }

    public virtual DbSet<DispatchRequestVehicle> DispatchRequestVehicles { get; set; }

    public virtual DbSet<DriverLicense> DriverLicenses { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<ModelComponent> ModelComponents { get; set; }

    public virtual DbSet<ModelImage> ModelImages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RentalContract> RentalContracts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<StaffReport> StaffReports { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<StationFeedback> StationFeedbacks { get; set; }

    public virtual DbSet<SupportRequest> SupportRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleChecklist> VehicleChecklists { get; set; }

    public virtual DbSet<VehicleChecklistItem> VehicleChecklistItems { get; set; }

    public virtual DbSet<VehicleComponent> VehicleComponents { get; set; }

    public virtual DbSet<VehicleModel> VehicleModels { get; set; }

    public virtual DbSet<VehicleSegment> VehicleSegments { get; set; }
    public DbSet<T> Set<T>() where T : class, IEntity => base.Set<T>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_updateInterceptor);
    }
    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var result = Regex.Replace(
            name,
            @"([a-z0-9])([A-Z])",
            "$1_$2"
        );

        return result.ToLower();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__brands__3213E83F39FD01A6");

            entity.ToTable("brands");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FoundedYear).HasColumnName("founded_year");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<CitizenIdentity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__citizen___3213E83F5AA98353");

            entity.ToTable("citizen_identities");

            entity.HasIndex(e => e.UserId, "UQ__citizen___B9BE370E6D3C6B98").IsUnique();

            entity.HasIndex(e => e.Number, "UQ__citizen___FD291E41F15782F6").IsUnique();

            entity.HasIndex(e => e.UserId, "uq_citizen_identities_user_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.ImagePublicId)
                .HasMaxLength(255)
                .HasColumnName("image_public_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .HasColumnName("number");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.CitizenIdentity)
                .HasForeignKey<CitizenIdentity>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_citizen_users");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__deposits__3213E83FA62669D9");

            entity.ToTable("deposits");

            entity.HasIndex(e => e.InvoiceId, "UQ__deposits__F58DFD48A531CFB0").IsUnique();

            entity.HasIndex(e => e.InvoiceId, "uq_deposits_invoice_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.RefundedAt).HasColumnName("refunded_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Invoice).WithOne(p => p.Deposit)
                .HasForeignKey<Deposit>(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_deposits_invoices");
        });

        modelBuilder.Entity<DispatchRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dispatch__3213E83F4F8E6A7E");

            entity.ToTable("dispatch_requests");

            entity.HasIndex(e => e.ApprovedAdminId, "idx_dispatch_requests_approved_admin_id");

            entity.HasIndex(e => e.FromStationId, "idx_dispatch_requests_from_station_id");

            entity.HasIndex(e => e.RequestAdminId, "idx_dispatch_requests_request_admin_id");

            entity.HasIndex(e => e.ToStationId, "idx_dispatch_requests_to_station_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ApprovedAdminId).HasColumnName("approved_admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FromStationId).HasColumnName("from_station_id");
            entity.Property(e => e.RequestAdminId).HasColumnName("request_admin_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ToStationId).HasColumnName("to_station_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ApprovedAdmin).WithMany(p => p.DispatchRequestApprovedAdmins)
                .HasForeignKey(d => d.ApprovedAdminId)
                .HasConstraintName("fk_dispatch_requests_approved_admins");

            entity.HasOne(d => d.FromStation).WithMany(p => p.DispatchRequestFromStations)
                .HasForeignKey(d => d.FromStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_requests_from_stations");

            entity.HasOne(d => d.RequestAdmin).WithMany(p => p.DispatchRequestRequestAdmins)
                .HasForeignKey(d => d.RequestAdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_requests_request_admins");

            entity.HasOne(d => d.ToStation).WithMany(p => p.DispatchRequestToStations)
                .HasForeignKey(d => d.ToStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_requests_to_stations");
        });

        modelBuilder.Entity<DispatchRequestStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dispatch__3213E83FF6079550");

            entity.ToTable("dispatch_request_staffs");

            entity.HasIndex(e => e.DispatchRequestId, "idx_dispatch_request_staffs_dispatch_request_id");

            entity.HasIndex(e => e.StaffId, "idx_dispatch_request_staffs_staff_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DispatchRequestId).HasColumnName("dispatch_request_id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.DispatchRequest).WithMany(p => p.DispatchRequestStaffs)
                .HasForeignKey(d => d.DispatchRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_request_staffs_dispatch_requests");

            entity.HasOne(d => d.Staff).WithMany(p => p.DispatchRequestStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_request_staffs_staffs");
        });

        modelBuilder.Entity<DispatchRequestVehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dispatch__3213E83FF618BAA1");

            entity.ToTable("dispatch_request_vehicles");

            entity.HasIndex(e => e.DispatchRequestId, "idx_dispatch_request_vehicles_dispatch_request_id");

            entity.HasIndex(e => e.VehicleId, "idx_dispatch_request_vehicles_vehicle_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DispatchRequestId).HasColumnName("dispatch_request_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.DispatchRequest).WithMany(p => p.DispatchRequestVehicles)
                .HasForeignKey(d => d.DispatchRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_request_vehicles_dispatch_requests");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.DispatchRequestVehicles)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_request_vehicles_vehicles");
        });

        modelBuilder.Entity<DriverLicense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__driver_l__3213E83F0C52E57F");

            entity.ToTable("driver_licenses");

            entity.HasIndex(e => e.UserId, "UQ__driver_l__B9BE370EC7CF60FE").IsUnique();

            entity.HasIndex(e => e.Number, "UQ__driver_l__FD291E41C4B31239").IsUnique();

            entity.HasIndex(e => e.UserId, "uq_driver_licenses_user_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Class).HasColumnName("class");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.ImagePublicId)
                .HasMaxLength(255)
                .HasColumnName("image_public_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .HasColumnName("number");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.DriverLicense)
                .HasForeignKey<DriverLicense>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_driver_users");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoices__3213E83FD78CB2FD");

            entity.ToTable("invoices");

            entity.HasIndex(e => e.ContractId, "idx_invoices_contract_id");


            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.PaidAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("paid_amount");
            entity.Property(e => e.PaidAt).HasColumnName("paid_at");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Tax)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tax");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

          

            entity.HasOne(d => d.Contract).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoices_contracts");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoice___3213E83FA6AF7666");

            entity.ToTable("invoice_items");

            entity.HasIndex(e => e.ChecklistItemId, "idx_invoice_items_checklist_item_id")
                .IsUnique()
                .HasFilter("([checklist_item_id] IS NOT NULL)");

            entity.HasIndex(e => e.InvoiceId, "idx_invoice_items_invoice_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ChecklistItemId).HasColumnName("checklist_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ChecklistItem).WithOne(p => p.InvoiceItem)
                .HasForeignKey<InvoiceItem>(d => d.ChecklistItemId)
                .HasConstraintName("fk_invoice_items_checklist_items");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_items_invoices");
        });

        modelBuilder.Entity<ModelComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__model_co__3213E83F3C4DADC3");

            entity.ToTable("model_components");

            entity.HasIndex(e => e.ComponentId, "idx_model_components_component_id");

            entity.HasIndex(e => e.ModelId, "idx_model_components_model_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ComponentId).HasColumnName("component_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Component).WithMany(p => p.ModelComponents)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_model_components_vehicle_components");

            entity.HasOne(d => d.Model).WithMany(p => p.ModelComponents)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_model_components_vehicle_models");
        });

        modelBuilder.Entity<ModelImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__model_im__3213E83FF3E97981");

            entity.ToTable("model_images");

            entity.HasIndex(e => e.Url, "UQ__model_im__DD7784175340E62A").IsUnique();

            entity.HasIndex(e => e.ModelId, "idx_model_images_model_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.PublicId)
                .HasMaxLength(255)
                .HasColumnName("public_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");

            entity.HasOne(d => d.Model).WithMany(p => p.ModelImages)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_model_images_vehicle_models");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__refresh___3213E83F8B43C8C7");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => e.UserId, "idx_refresh_tokens_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.IssuedAt).HasColumnName("issued_at");
            entity.Property(e => e.Token)
                .IsUnicode(false)
                .HasColumnName("token");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_refresh_users");
        });

        modelBuilder.Entity<RentalContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rental_c__3213E83F817349DC");

            entity.ToTable("rental_contracts");

            entity.HasIndex(e => e.CustomerId, "idx_rental_contracts_customer_id");

            entity.HasIndex(e => e.HandoverStaffId, "idx_rental_contracts_handover_staff_id");

            entity.HasIndex(e => e.ReturnStaffId, "idx_rental_contracts_return_staff_id");

            entity.HasIndex(e => e.VehicleId, "idx_rental_contracts_vehicle_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ActualEndDate).HasColumnName("actual_end_date");
            entity.Property(e => e.ActualStartDate).HasColumnName("actual_start_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.HandoverStaffId).HasColumnName("handover_staff_id");
            entity.Property(e => e.IsSignedByCustomer).HasColumnName("is_signed_by_customer");
            entity.Property(e => e.IsSignedByStaff).HasColumnName("is_signed_by_staff");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.ReturnStaffId).HasColumnName("return_staff_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentalContracts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rental_contracts_customers");

            entity.HasOne(d => d.HandoverStaff).WithMany(p => p.RentalContractHandoverStaffs)
                .HasForeignKey(d => d.HandoverStaffId)
                .HasConstraintName("fk_rental_contracts_handover_staffs");

            entity.HasOne(d => d.ReturnStaff).WithMany(p => p.RentalContractReturnStaffs)
                .HasForeignKey(d => d.ReturnStaffId)
                .HasConstraintName("fk_rental_contracts_return_staffs");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.RentalContracts)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("fk_rental_contracts_vehicles");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F94B3D944");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__staffs__B9BE370FBBD826E4");

            entity.ToTable("staffs");

            entity.HasIndex(e => e.StationId, "idx_staffs_station_id");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.StationId).HasColumnName("station_id");

            entity.HasOne(d => d.Station).WithMany(p => p.Staff)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_staff_stations");

            entity.HasOne(d => d.User).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_staff_users");
        });

        modelBuilder.Entity<StaffReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__staff_re__3213E83F5156E646");

            entity.ToTable("staff_reports");

            entity.HasIndex(e => e.AdminId, "idx_staff_reports_admin_id");

            entity.HasIndex(e => e.StaffId, "idx_staff_reports_staff_id");

            entity.HasIndex(e => e.SupportRequestId, "idx_staff_reports_support_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Reply).HasColumnName("reply");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SupportRequestId).HasColumnName("support_request_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Admin).WithMany(p => p.StaffReportAdmins)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("fk_staff_reports_admin");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffReportStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_staff_reports_staffs");

            entity.HasOne(d => d.SupportRequest).WithMany(p => p.StaffReports)
                .HasForeignKey(d => d.SupportRequestId)
                .HasConstraintName("fk_staff_reports_support");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__stations__3213E83FDE57DAC6");

            entity.ToTable("stations");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StationFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__station___3213E83F2B055C9C");

            entity.ToTable("station_feedbacks");

            entity.HasIndex(e => e.CustomerId, "idx_station_feedbacks_customer_id");

            entity.HasIndex(e => e.StationId, "idx_station_feedbacks_station_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.StationFeedbacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_feedback_users");

            entity.HasOne(d => d.Station).WithMany(p => p.StationFeedbacks)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_feedback_stations");
        });

        modelBuilder.Entity<SupportRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__support___3213E83F22E367C7");

            entity.ToTable("support_requests");

            entity.HasIndex(e => e.StaffId, "idx_support_requests_staff_id");

            entity.HasIndex(e => e.CustomerId, "idx_support_requests_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Reply).HasColumnName("reply");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.SupportRequests)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_support_requests_user");

            entity.HasOne(d => d.Staff).WithMany(p => p.SupportRequests)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("fk_support_requests_staff");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F083E0E83");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "idx_users_role_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AvatarPublicId)
                .HasMaxLength(255)
                .HasColumnName("avatar_public_id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(500)
                .HasColumnName("avatar_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IsGoogleLinked).HasColumnName("is_google_linked");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_roles");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicles__3213E83F1CEAB601");

            entity.ToTable("vehicles");

            entity.HasIndex(e => e.LicensePlate, "UQ__vehicles__F72CD56E11B6394E").IsUnique();

            entity.HasIndex(e => e.ModelId, "idx_vehicles_model_id");

            entity.HasIndex(e => e.StationId, "idx_vehicles_vehicles");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(15)
                .HasColumnName("license_plate");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Model).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicles_models");

            entity.HasOne(d => d.Station).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicles_stations");
        });

        modelBuilder.Entity<VehicleChecklist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicle___3213E83F5B95E7CD");

            entity.ToTable("vehicle_checklists");

            entity.HasIndex(e => e.ContractId, "idx_vehicle_checklists_contract_id");

            entity.HasIndex(e => e.CustomerId, "idx_vehicle_checklists_customer_id");

            entity.HasIndex(e => e.StaffId, "idx_vehicle_checklists_staff_id");

            entity.HasIndex(e => e.VehicleId, "idx_vehicle_checklists_vehicle_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.IsSignedByCustomer).HasColumnName("is_signed_by_customer");
            entity.Property(e => e.IsSignedByStaff).HasColumnName("is_signed_by_staff");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Contract).WithMany(p => p.VehicleChecklists)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("fk_vehicle_checklists_contracts");

            entity.HasOne(d => d.Customer).WithMany(p => p.VehicleChecklists)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_vehicle_checklists_users");

            entity.HasOne(d => d.Staff).WithMany(p => p.VehicleChecklists)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicle_checklists_staffs");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.VehicleChecklists)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicle_checklists_vehicles");
        });

        modelBuilder.Entity<VehicleChecklistItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicle___3213E83F906FB6F6");

            entity.ToTable("vehicle_checklist_items");

            entity.HasIndex(e => e.ChecklistId, "idx_vehicle_checklist_items_checklist_id");

            entity.HasIndex(e => e.ComponentId, "idx_vehicle_checklist_items_component_id");

            entity.HasIndex(e => e.ImageUrl, "idx_vehicle_checklist_items_image_url")
                .IsUnique()
                .HasFilter("([image_url] IS NOT NULL)");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ChecklistId).HasColumnName("checklist_id");
            entity.Property(e => e.ComponentId).HasColumnName("component_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ImagePublicId)
                .HasMaxLength(255)
                .HasColumnName("image_public_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Checklist).WithMany(p => p.VehicleChecklistItems)
                .HasForeignKey(d => d.ChecklistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicle_checklist_items_checklists");

            entity.HasOne(d => d.Component).WithMany(p => p.VehicleChecklistItems)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicle_checklist_items_components");
        });

        modelBuilder.Entity<VehicleComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicle___3213E83F39DCC37F");

            entity.ToTable("vehicle_components");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<VehicleModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicle___3213E83F7DCE480B");

            entity.ToTable("vehicle_models");

            entity.HasIndex(e => e.SegmentId, "idx_vehicle_models__segment_id");

            entity.HasIndex(e => e.BrandId, "idx_vehicle_models_brand_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BatteryCapacity)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("battery_capacity");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CostPerDay)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost_per_day");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DepositFee)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("deposit_fee");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EcoRangeKm)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("eco_range_km");
            entity.Property(e => e.MotorPower)
                .HasColumnType("decimal(5, 1)")
                .HasColumnName("motor_power");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfAirbags).HasColumnName("number_of_airbags");
            entity.Property(e => e.SeatingCapacity).HasColumnName("seating_capacity");
            entity.Property(e => e.SegmentId).HasColumnName("segment_id");
            entity.Property(e => e.SportRangeKm)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("sport_range_km");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Brand).WithMany(p => p.VehicleModels)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_model_brands");

            entity.HasOne(d => d.Segment).WithMany(p => p.VehicleModels)
                .HasForeignKey(d => d.SegmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_model_segments");
        });

        modelBuilder.Entity<VehicleSegment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicle___3213E83FB644C5D3");

            entity.ToTable("vehicle_segments");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("updated_at");
        });
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Đổi tên bảng
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            // Đổi tên cột
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
