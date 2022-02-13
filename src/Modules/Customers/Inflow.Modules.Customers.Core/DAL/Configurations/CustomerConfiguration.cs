using Inflow.Modules.Customers.Core.Domain.Entities;
using Inflow.Modules.Customers.Core.Domain.ValueObjects;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inflow.Modules.Customers.Core.DAL.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .HasConversion(x => x.Value, x => new Email(x));

        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .HasConversion(x => x.Value, x => new Name(x));

        builder.HasIndex(x => x.FullName);
        builder.Property(x => x.FullName)
            .HasMaxLength(100)
            .HasConversion(x => x.Value, x => new FullName(x));

        builder.HasIndex(x => x.Address);
        builder.Property(x => x.Address)
            .HasMaxLength(200)
            .HasConversion(x => x.Value, x => new Address(x));

        builder.HasIndex(x => x.Identity);
        builder.Property(x => x.Identity)
            .HasMaxLength(40)
            .HasConversion(x => x.ToString(), x => Identity.From(x));

        builder.HasIndex(x => x.Nationality);
        builder.Property(x => x.Nationality)
            .HasMaxLength(2)
            .HasConversion(x => x.Value, x => new Nationality(x));

        builder.HasIndex(x => x.Notes);
        builder.Property(x => x.Nationality)
            .HasMaxLength(500);
    }
}