namespace FundEx.Persistence.EntityConfigurations;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FundTitleConfiguration : IEntityTypeConfiguration<FundTitle>
{
    public void Configure(EntityTypeBuilder<FundTitle> builder)
    {
        builder.ToTable("FundTitles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => new { x.FundTypeId, x.Name }).IsUnique();
        builder.HasOne(x => x.FundType).WithMany(x => x.Titles).HasForeignKey(x => x.FundTypeId);
    }
}
