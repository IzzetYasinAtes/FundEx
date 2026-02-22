namespace FundEx.Persistence.EntityConfigurations;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FundTypeConfiguration : IEntityTypeConfiguration<FundType>
{
    public void Configure(EntityTypeBuilder<FundType> builder)
    {
        builder.ToTable("FundTypes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new FundType { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Code = "YAT", Name = "Securities Investment Funds", Description = "Menkul Kiymet Yatirim Fonlari", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new FundType { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Code = "EMK", Name = "Pension Funds", Description = "Emeklilik Fonlari", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new FundType { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Code = "BYF", Name = "Exchange Traded Funds", Description = "Borsa Yatirim Fonlari", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new FundType { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Code = "GYF", Name = "Real Estate Investment Funds", Description = "Gayrimenkul Yatirim Fonlari", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new FundType { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Code = "GSYF", Name = "Venture Capital Investment Funds", Description = "Girisim Sermayesi Yatirim Fonlari", CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
