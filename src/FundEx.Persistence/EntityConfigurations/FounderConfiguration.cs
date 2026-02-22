namespace FundEx.Persistence.EntityConfigurations;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FounderConfiguration : IEntityTypeConfiguration<Founder>
{
    public void Configure(EntityTypeBuilder<Founder> builder)
    {
        builder.ToTable("Founders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(500).IsRequired();
        builder.Property(x => x.FundTypeCode).HasMaxLength(10).IsRequired();
        builder.HasIndex(x => new { x.Code, x.FundTypeCode }).IsUnique();
        builder.HasOne(x => x.FundType).WithMany(x => x.Founders).HasForeignKey(x => x.FundTypeId);
    }
}
