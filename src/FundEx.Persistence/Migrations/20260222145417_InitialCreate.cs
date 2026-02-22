using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FundEx.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Founders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FundTypeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FundTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Founders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Founders_FundTypes_FundTypeId",
                        column: x => x.FundTypeId,
                        principalTable: "FundTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FundTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundCategories_FundTypes_FundTypeId",
                        column: x => x.FundTypeId,
                        principalTable: "FundTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupCode = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FundTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundTitles_FundTypes_FundTypeId",
                        column: x => x.FundTypeId,
                        principalTable: "FundTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FundTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FounderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funds_Founders_FounderId",
                        column: x => x.FounderId,
                        principalTable: "Founders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Funds_FundTypes_FundTypeId",
                        column: x => x.FundTypeId,
                        principalTable: "FundTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundDailyData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShareCount = table.Column<long>(type: "bigint", nullable: false),
                    InvestorCount = table.Column<int>(type: "int", nullable: false),
                    PortfolioSize = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ExchangeBulletinPrice = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    Stock = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    GovernmentBond = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    FxDenominatedGovernmentDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    TreasuryBill = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    CommercialPaper = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    CorporateBond = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    AssetBackedSecurities = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    GovernmentExternalDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    CorporateExternalDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    GovernmentLeaseCertificates = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    GovernmentLeaseCertificatesTl = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    GovernmentLeaseCertificatesFx = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    LeaseCertificatesNonInvestment = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    CorporateLeaseCertificates = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ReverseRepo = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Repo = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    BistCommittedTradingSale = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    BistCommittedTradingPurchase = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    TakasbankMoneyMarket = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    BorsaIstanbulMoneyMarket = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    DepositFx = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    DepositTl = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Deposit = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    DepositGold = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ParticipationAccountFx = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ParticipationAccountTl = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ParticipationAccount = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ParticipationAccountGold = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    PreciousMetals = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    PreciousMetalsEtf = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    PreciousMetalsGovernmentDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    PreciousMetalsLeaseCertificates = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ForeignEtf = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ForeignStock = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    EtfParticipationShares = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ForeignPartnershipDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ForeignDebtInstruments = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ForeignSecurities = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    InvestmentFundShares = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    FuturesCashCollateral = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Other = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ExchangeTradedFund = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    OtherPartnership = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Eurobond = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    FundGovernmentDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    RealEstateTrading = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    VentureCapitalInstitutionDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    VentureCapitalInvestment = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    RealEstateInstitutionDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    RealEstateInvestment = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    GovernmentDebtInstruments = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    LeasingTransactionDebt = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    PrivateSectorNonInvestment = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Bond = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundDailyData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundDailyData_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FundTypes",
                columns: new[] { "Id", "Code", "CreatedDate", "Description", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "YAT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Menkul Kiymet Yatirim Fonlari", "Securities Investment Funds", null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "EMK", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Emeklilik Fonlari", "Pension Funds", null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "BYF", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Borsa Yatirim Fonlari", "Exchange Traded Funds", null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "GYF", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gayrimenkul Yatirim Fonlari", "Real Estate Investment Funds", null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "GSYF", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Girisim Sermayesi Yatirim Fonlari", "Venture Capital Investment Funds", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Founders_Code_FundTypeCode",
                table: "Founders",
                columns: new[] { "Code", "FundTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Founders_FundTypeId",
                table: "Founders",
                column: "FundTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FundCategories_FundTypeId_Code",
                table: "FundCategories",
                columns: new[] { "FundTypeId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FundDailyData_Date",
                table: "FundDailyData",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_FundDailyData_FundId_Date",
                table: "FundDailyData",
                columns: new[] { "FundId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funds_Code",
                table: "Funds",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funds_FounderId",
                table: "Funds",
                column: "FounderId");

            migrationBuilder.CreateIndex(
                name: "IX_Funds_FundTypeId",
                table: "Funds",
                column: "FundTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FundTitles_FundTypeId_Name",
                table: "FundTitles",
                columns: new[] { "FundTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FundTypes_Code",
                table: "FundTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundCategories");

            migrationBuilder.DropTable(
                name: "FundDailyData");

            migrationBuilder.DropTable(
                name: "FundTitles");

            migrationBuilder.DropTable(
                name: "Funds");

            migrationBuilder.DropTable(
                name: "Founders");

            migrationBuilder.DropTable(
                name: "FundTypes");
        }
    }
}
