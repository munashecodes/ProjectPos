using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectPos.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Street = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    LogInTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LogOutTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsLoggedIn = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    System = table.Column<int>(type: "int", nullable: true),
                    ComputerName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComputerUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComputerIpAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JwtToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountType = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Balance = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: true),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountCategories_AccountCategoryId",
                        column: x => x.AccountCategoryId,
                        principalTable: "AccountCategories",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CashUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    USDAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashUps", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    VatNumber = table.Column<int>(type: "int", nullable: false),
                    RegNumber = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSupplier = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasCreditFacility = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContactPersons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<int>(type: "int", nullable: true),
                    JobPosition = table.Column<int>(type: "int", nullable: true),
                    Phone1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactPersons_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactPersons_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DayEndSalesSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TotalSales = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalTransactions = table.Column<int>(type: "int", nullable: false),
                    AverageTransactionValue = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CashSales = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CashSalesReported = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CardSales = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreditSales = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MobilePayments = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalDiscounts = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalReturns = table.Column<int>(type: "int", nullable: false),
                    ReturnAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    VoidedTransactions = table.Column<int>(type: "int", nullable: false),
                    VoidedAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CancelledTransactions = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayEndSalesSummaries", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Surname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NatId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DOB = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    Cell = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", nullable: true),
                    JwtToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupervisorCodeHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemUsers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    BaseCurrency = table.Column<int>(type: "int", nullable: true),
                    BaseToRate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DateEffected = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiptAttachmentPath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    Payee = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_SystemUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "SystemUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InventorySnapShotLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Department = table.Column<int>(type: "int", nullable: false),
                    StartDay = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySnapShotLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventorySnapShotLogs_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventorySnapShotLogs_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventorySnapShotLogs_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BarCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Flag = table.Column<int>(type: "int", nullable: false),
                    PLUCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<int>(type: "int", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsWeighted = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    IsTaxable = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    IdealQuantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    QuantityOnHand = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    QuantityOnShelf = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    StockCount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    MarkUp = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInventories_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductInventories_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInventories_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInventories_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductInventorySnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Inventory = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<int>(type: "int", nullable: true),
                    SnapShotType = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventorySnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInventorySnapshots_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInventorySnapshots_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInventorySnapshots_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProofOfPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reference = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Bank = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BranchCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    UsableAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    BankingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofOfPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofOfPayments_Companies_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofOfPayments_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProofOfPayments_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProofOfPayments_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaceInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceNumber = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalValue = table.Column<double>(type: "double(12, 2)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaceInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaceInvoices_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaceInvoices_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaceInvoices_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaceOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceValue = table.Column<double>(type: "double(12, 2)", nullable: true),
                    IsReceived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsOpen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ETA = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaceOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaceOrders_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaceOrders_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaceOrders_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaceOrders_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaleType = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PriceIncludingVat = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Vat = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    IsPostedToJournal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Companies_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrders_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrders_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockMovementLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionType = table.Column<int>(type: "int", nullable: true),
                    IsssuedTo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAuthorised = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AuthorisedById = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovementLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovementLogs_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovementLogs_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovementLogs_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockTakeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsOpen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Department = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTakeLogs_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTakeLogs_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTakeLogs_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JournalEntryLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JournalEntryId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalTable: "JournalEntries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductInventoryId = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    MarkUp = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_ProductInventories_ProductInventoryId",
                        column: x => x.ProductInventoryId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductPrices_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPrices_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPrices_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaceInvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "double(12, 2)", nullable: false),
                    TotalPrice = table.Column<double>(type: "double(12, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaceInvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaceInvoiceLines_ProductInventories_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaceInvoiceLines_PurchaceInvoices_InvoiceNumber",
                        column: x => x.InvoiceNumber,
                        principalTable: "PurchaceInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GoodsReceivedVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(type: "int", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Transpoter = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    PaidAmount = table.Column<double>(type: "double(12, 2)", nullable: true),
                    UsdPaidAmount = table.Column<double>(type: "double(12, 2)", nullable: true),
                    AmountDue = table.Column<double>(type: "double(12, 2)", nullable: true),
                    Value = table.Column<double>(type: "double(12, 2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceivedVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVouchers_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVouchers_PurchaceOrders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "PurchaceOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVouchers_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVouchers_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVouchers_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaceOrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(type: "int", nullable: true),
                    ProductInventoryId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<double>(type: "double(12, 2)", nullable: true),
                    Price = table.Column<double>(type: "double(12, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaceOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaceOrderLines_ProductInventories_ProductInventoryId",
                        column: x => x.ProductInventoryId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaceOrderLines_PurchaceOrders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "PurchaceOrders",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SalesOrderId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    USDPaidAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PaidAmountAfterChange = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    USDPaidAmountAfterChange = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ChangeAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    VAT = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    MethodOfPay = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProofOfPaymentId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalesOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BarCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_ProductInventories_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_SalesOrders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "SalesOrders",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductInventoryId = table.Column<int>(type: "int", nullable: true),
                    BatchNumber = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: true),
                    IsssuedTo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAuthorised = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AuthorisedById = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_ProductInventories_ProductInventoryId",
                        column: x => x.ProductInventoryId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_StockMovementLogs_BatchNumber",
                        column: x => x.BatchNumber,
                        principalTable: "StockMovementLogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GoodsReceivedVoucherLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VoucherNumber = table.Column<int>(type: "int", nullable: true),
                    ProductInventoryId = table.Column<int>(type: "int", nullable: true),
                    OrderedQuantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    IssuedQuantity = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    QtyOnHand = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    IsIssued = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: true),
                    OrderPrice = table.Column<double>(type: "double(12, 2)", nullable: true),
                    UnitPrice = table.Column<double>(type: "double(12, 2)", nullable: true),
                    Price = table.Column<double>(type: "double(12, 2)", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceivedVoucherLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVoucherLines_GoodsReceivedVouchers_VoucherNumber",
                        column: x => x.VoucherNumber,
                        principalTable: "GoodsReceivedVouchers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoodsReceivedVoucherLines_ProductInventories_ProductInventor~",
                        column: x => x.ProductInventoryId,
                        principalTable: "ProductInventories",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaceOrderPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoodsReceivedVoucherId = table.Column<int>(type: "int", nullable: true),
                    OrderAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    USDPaidAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PaidAmountAfterChange = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    USDPaidAmountAfterChange = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ChangeAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    MethodOfPay = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaceOrderPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaceOrderPayments_GoodsReceivedVouchers_GoodsReceivedVou~",
                        column: x => x.GoodsReceivedVoucherId,
                        principalTable: "GoodsReceivedVouchers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaceOrderPayments_SystemUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaceOrderPayments_SystemUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaceOrderPayments_SystemUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AccountCategories",
                columns: new[] { "Id", "AccountType", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Used for recurring or day-to-day expenses", false, "Operating Expenses" },
                    { 2, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Used for payments that result in acquiring or improving long-term assets", false, "Capital Expenditures" },
                    { 3, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Used for services directly tied to inventory procurement or preparation", false, "Cost of Goods Sold" },
                    { 4, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "For payments that don’t fit into the above categories", false, "Other Expense Accounts" },
                    { 6, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "money that is readily available for use", false, "Cash" },
                    { 7, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "money owed to the company", false, "Accounts Receivable" },
                    { 8, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "goods for sale", false, "Inventory" },
                    { 9, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "money a business owes to its suppliers or creditors", false, "Accounts Payable" },
                    { 10, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "loans aquired by the company", false, "Short-term Loans" },
                    { 11, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "capital invested into the company", false, "Owner's Capital" },
                    { 12, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Retained Earnings", false, "Retained Earnings" },
                    { 13, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "amount of money a business earns from selling goods or services", false, "Sales Revenue" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCategoryId", "AccountType", "Balance", "Code", "CreationTime", "CreatorId", "DeleterId", "DeletionTime", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, 1, 5, 0.00m, 1001, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Electricity, water, and internet expenses", false, "Utilities" },
                    { 2, 1, 5, 0.00m, 1002, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Expenses related to repairing or maintaining assets", false, "Repairs and Maintenance" },
                    { 3, 1, 5, 0.00m, 1003, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Expenses for cleaning services provided to the shop", false, "Cleaning Services" },
                    { 4, 1, 5, 0.00m, 1004, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Payments made for security personnel or services", false, "Security Services" },
                    { 5, 1, 5, 0.00m, 1005, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Fees paid to consultants, lawyers, or accountants", false, "Professional Fees" },
                    { 6, 1, 5, 0.00m, 1006, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Expenses for promotions and advertisements", false, "Advertising and Marketing" },
                    { 7, 2, 5, 0.00m, 2007, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Renovations and upgrades to buildings", false, "Building Improvements" },
                    { 8, 2, 5, 0.00m, 2008, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Purchases of equipment used in operations", false, "Equipment Purchases" },
                    { 9, 2, 5, 0.00m, 2009, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Purchases of furniture or fixtures for the shop", false, "Furniture and Fixtures" },
                    { 10, 2, 5, 0.00m, 2010, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Expenses related to significant repairs or long-term maintenance", false, "Long-term Maintenance" },
                    { 11, 3, 5, 0.00m, 3011, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Costs related to transporting inventory", false, "Transportation Costs" },
                    { 12, 3, 5, 0.00m, 3012, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Costs incurred for packaging materials", false, "Packaging Costs" },
                    { 13, 3, 5, 0.00m, 3013, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Costs related to storing inventory", false, "Storage Costs" },
                    { 14, 3, 5, 0.00m, 3014, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Cost of raw materials used for goods", false, "Raw Materials" },
                    { 15, 3, 5, 0.00m, 3015, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Wages paid to employees directly involved in production", false, "Direct Labor" },
                    { 16, 6, 1, 0.00m, 6016, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Physical cash available in registers or safes", false, "Cash in Hand" },
                    { 17, 6, 1, 0.00m, 6017, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Operational bank account for business transactions", false, "Bank Account" },
                    { 18, 7, 1, 0.00m, 7018, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Amounts owed by customers", false, "Customer Debts" },
                    { 19, 9, 2, 0.00m, 9019, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Amounts owed to suppliers for goods or services", false, "Supplier Debts" },
                    { 20, 10, 2, 0.00m, 1020, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Short-term bank loans acquired for operations", false, "Bank Loan" },
                    { 21, 11, 3, 0.00m, 1121, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Capital contributed by the owner", false, "Owner's Equity" },
                    { 22, 12, 3, 0.00m, 1122, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Profits retained in the business for future use", false, "Retained Earnings" },
                    { 23, 12, 6, 0.00m, 1223, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Revenue from selling goods", false, "Product Sales" },
                    { 24, 6, 1, 0.00m, 6024, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Operational EcoCash account for business transactions", false, "EcoCash Account" },
                    { 25, 9, 2, 0.00m, 9025, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Amounts owed to suppliers for goods or services", false, "VAT Payable" },
                    { 26, 7, 1, 0.00m, 7026, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Amounts owed by customers", false, "VAT Receivable" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_UserId",
                table: "AccessLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountCategories_CreatorId",
                table: "AccountCategories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountCategories_DeleterId",
                table: "AccountCategories",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountCategoryId",
                table: "Accounts",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CreatorId",
                table: "Accounts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DeleterId",
                table: "Accounts",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_CashUps_CreatorId",
                table: "CashUps",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CashUps_DeleterId",
                table: "CashUps",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_CashUps_LastModifierUserId",
                table: "CashUps",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashUps_UserId",
                table: "CashUps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CreatorId",
                table: "Companies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_DeleterId",
                table: "Companies",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_AddressId",
                table: "ContactPersons",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_CompanyId",
                table: "ContactPersons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_CreatorId",
                table: "ContactPersons",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_DeleterId",
                table: "ContactPersons",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_DayEndSalesSummaries_CreatorId",
                table: "DayEndSalesSummaries",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DayEndSalesSummaries_DeleterId",
                table: "DayEndSalesSummaries",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_DayEndSalesSummaries_LastModifierUserId",
                table: "DayEndSalesSummaries",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AddressId",
                table: "Employees",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatorId",
                table: "Employees",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeleterId",
                table: "Employees",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LastModifierUserId",
                table: "Employees",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CreatorId",
                table: "ExchangeRates",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_DeleterId",
                table: "ExchangeRates",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_LastModifierUserId",
                table: "ExchangeRates",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ApprovedById",
                table: "Expenses",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CompanyId",
                table: "Expenses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CreatorId",
                table: "Expenses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_DeleterId",
                table: "Expenses",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_LastModifierUserId",
                table: "Expenses",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVoucherLines_ProductInventoryId",
                table: "GoodsReceivedVoucherLines",
                column: "ProductInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVoucherLines_VoucherNumber",
                table: "GoodsReceivedVoucherLines",
                column: "VoucherNumber");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVouchers_CreatorId",
                table: "GoodsReceivedVouchers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVouchers_DeleterId",
                table: "GoodsReceivedVouchers",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVouchers_LastModifierUserId",
                table: "GoodsReceivedVouchers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVouchers_OrderNumber",
                table: "GoodsReceivedVouchers",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedVouchers_SupplierId",
                table: "GoodsReceivedVouchers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySnapShotLogs_CreatorId",
                table: "InventorySnapShotLogs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySnapShotLogs_DeleterId",
                table: "InventorySnapShotLogs",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySnapShotLogs_LastModifierUserId",
                table: "InventorySnapShotLogs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_CreatorId",
                table: "JournalEntries",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_DeleterId",
                table: "JournalEntries",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId",
                table: "JournalEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_JournalEntryId",
                table: "JournalEntryLines",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatorId",
                table: "Payments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DeleterId",
                table: "Payments",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_LastModifierUserId",
                table: "Payments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SalesOrderId",
                table: "Payments",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_CreatorId",
                table: "ProductInventories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_DeleterId",
                table: "ProductInventories",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_LastModifierUserId",
                table: "ProductInventories",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_SubCategoryId",
                table: "ProductInventories",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventorySnapshots_CreatorId",
                table: "ProductInventorySnapshots",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventorySnapshots_DeleterId",
                table: "ProductInventorySnapshots",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventorySnapshots_LastModifierUserId",
                table: "ProductInventorySnapshots",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_CreatorId",
                table: "ProductPrices",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_DeleterId",
                table: "ProductPrices",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_LastModifierUserId",
                table: "ProductPrices",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductInventoryId",
                table: "ProductPrices",
                column: "ProductInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofOfPayments_CreatorId",
                table: "ProofOfPayments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofOfPayments_CustomerId",
                table: "ProofOfPayments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofOfPayments_DeleterId",
                table: "ProofOfPayments",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofOfPayments_LastModifierUserId",
                table: "ProofOfPayments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceInvoiceLines_InvoiceNumber",
                table: "PurchaceInvoiceLines",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceInvoiceLines_ProductId",
                table: "PurchaceInvoiceLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceInvoices_CreatorId",
                table: "PurchaceInvoices",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceInvoices_DeleterId",
                table: "PurchaceInvoices",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceInvoices_LastModifierUserId",
                table: "PurchaceInvoices",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrderLines_OrderNumber",
                table: "PurchaceOrderLines",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrderLines_ProductInventoryId",
                table: "PurchaceOrderLines",
                column: "ProductInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrderPayments_CreatorId",
                table: "PurchaceOrderPayments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrderPayments_DeleterId",
                table: "PurchaceOrderPayments",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrderPayments_GoodsReceivedVoucherId",
                table: "PurchaceOrderPayments",
                column: "GoodsReceivedVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrderPayments_LastModifierUserId",
                table: "PurchaceOrderPayments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrders_CreatorId",
                table: "PurchaceOrders",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrders_DeleterId",
                table: "PurchaceOrders",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrders_LastModifierUserId",
                table: "PurchaceOrders",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaceOrders_SupplierId",
                table: "PurchaceOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_OrderNumber",
                table: "SalesOrderItems",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_ProductId",
                table: "SalesOrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CreatorId",
                table: "SalesOrders",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerId",
                table: "SalesOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_DeleterId",
                table: "SalesOrders",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_LastModifierUserId",
                table: "SalesOrders",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementLogs_CreatorId",
                table: "StockMovementLogs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementLogs_DeleterId",
                table: "StockMovementLogs",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementLogs_LastModifierUserId",
                table: "StockMovementLogs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_BatchNumber",
                table: "StockMovements",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreatorId",
                table: "StockMovements",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_DeleterId",
                table: "StockMovements",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_LastModifierUserId",
                table: "StockMovements",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductInventoryId",
                table: "StockMovements",
                column: "ProductInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakeLogs_CreatorId",
                table: "StockTakeLogs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakeLogs_DeleterId",
                table: "StockTakeLogs",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakeLogs_LastModifierUserId",
                table: "StockTakeLogs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_EmployeeId",
                table: "SystemUsers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_SystemUsers_UserId",
                table: "AccessLogs",
                column: "UserId",
                principalTable: "SystemUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCategories_SystemUsers_CreatorId",
                table: "AccountCategories",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCategories_SystemUsers_DeleterId",
                table: "AccountCategories",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_SystemUsers_CreatorId",
                table: "Accounts",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_SystemUsers_DeleterId",
                table: "Accounts",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashUps_SystemUsers_CreatorId",
                table: "CashUps",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashUps_SystemUsers_DeleterId",
                table: "CashUps",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashUps_SystemUsers_LastModifierUserId",
                table: "CashUps",
                column: "LastModifierUserId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashUps_SystemUsers_UserId",
                table: "CashUps",
                column: "UserId",
                principalTable: "SystemUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_SystemUsers_CreatorId",
                table: "Companies",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_SystemUsers_DeleterId",
                table: "Companies",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPersons_SystemUsers_CreatorId",
                table: "ContactPersons",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPersons_SystemUsers_DeleterId",
                table: "ContactPersons",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DayEndSalesSummaries_SystemUsers_CreatorId",
                table: "DayEndSalesSummaries",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DayEndSalesSummaries_SystemUsers_DeleterId",
                table: "DayEndSalesSummaries",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DayEndSalesSummaries_SystemUsers_LastModifierUserId",
                table: "DayEndSalesSummaries",
                column: "LastModifierUserId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SystemUsers_CreatorId",
                table: "Employees",
                column: "CreatorId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SystemUsers_DeleterId",
                table: "Employees",
                column: "DeleterId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SystemUsers_LastModifierUserId",
                table: "Employees",
                column: "LastModifierUserId",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SystemUsers_CreatorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SystemUsers_DeleterId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SystemUsers_LastModifierUserId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.DropTable(
                name: "CashUps");

            migrationBuilder.DropTable(
                name: "ContactPersons");

            migrationBuilder.DropTable(
                name: "DayEndSalesSummaries");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "GoodsReceivedVoucherLines");

            migrationBuilder.DropTable(
                name: "InventorySnapShotLogs");

            migrationBuilder.DropTable(
                name: "JournalEntryLines");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ProductInventorySnapshots");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropTable(
                name: "ProofOfPayments");

            migrationBuilder.DropTable(
                name: "PurchaceInvoiceLines");

            migrationBuilder.DropTable(
                name: "PurchaceOrderLines");

            migrationBuilder.DropTable(
                name: "PurchaceOrderPayments");

            migrationBuilder.DropTable(
                name: "SalesOrderItems");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "StockTakeLogs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "PurchaceInvoices");

            migrationBuilder.DropTable(
                name: "GoodsReceivedVouchers");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropTable(
                name: "ProductInventories");

            migrationBuilder.DropTable(
                name: "StockMovementLogs");

            migrationBuilder.DropTable(
                name: "AccountCategories");

            migrationBuilder.DropTable(
                name: "PurchaceOrders");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "SystemUsers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
