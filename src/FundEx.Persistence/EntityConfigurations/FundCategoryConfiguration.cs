namespace FundEx.Persistence.EntityConfigurations;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FundCategoryConfiguration : IEntityTypeConfiguration<FundCategory>
{
    public void Configure(EntityTypeBuilder<FundCategory> builder)
    {
        builder.ToTable("FundCategories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => new { x.FundTypeId, x.Code }).IsUnique();
        builder.HasOne(x => x.FundType).WithMany(x => x.Categories).HasForeignKey(x => x.FundTypeId);
    }
}
