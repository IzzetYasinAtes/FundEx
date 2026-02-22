namespace FundEx.Persistence.EntityConfigurations;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FundConfiguration : IEntityTypeConfiguration<Fund>
{
    public void Configure(EntityTypeBuilder<Fund> builder)
    {
        builder.ToTable("Funds");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasOne(x => x.FundType).WithMany(x => x.Funds).HasForeignKey(x => x.FundTypeId);
        builder.HasOne(x => x.Founder).WithMany(x => x.Funds).HasForeignKey(x => x.FounderId).IsRequired(false);
    }
}
