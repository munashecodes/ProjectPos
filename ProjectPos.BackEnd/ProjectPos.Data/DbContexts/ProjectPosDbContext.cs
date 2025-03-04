using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Data.DbContexts
{
    public class ProjectPosDbContext : IdentityDbContext
    {
        public ProjectPosDbContext() { }
        public ProjectPosDbContext(DbContextOptions<ProjectPosDbContext> options) : base(options) { }

        //create tables
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<PayRollCycle> PayRollCycles { get; set; }
        public virtual DbSet<PaySlip> PaySlips { get; set; }
        public virtual DbSet<SalaryStructure> SalaryStructures { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<OvertimeRecord> OvertimeRecords { get; set; }
        public virtual DbSet<EmployeeDeduction> EmployeeDeductions { get; set; }
        public virtual DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public virtual DbSet<AccountCategory> AccountCategories { get; set; }
        public virtual DbSet<JournalEntry> JournalEntries { get; set; }
        public virtual DbSet<JournalEntryLine> JournalEntryLines { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<User>? SystemUsers { get; set; }
        public virtual DbSet<AccessLog>? AccessLogs { get; set; }
        public virtual DbSet<CashUp>? CashUps { get; set; }
        public virtual DbSet<DayEndSalesSummary>? DayEndSalesSummaries { get; set; }
        public virtual DbSet<Employee>? Employees { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        
        public virtual DbSet<FinancialAccountSnapShot>? FinancialAccountSnapShots { get; set; }
        public virtual DbSet<ProductInventory>? ProductInventories { get; set; }
        public virtual DbSet<ProductPrice>? ProductPrices { get; set; }
        public virtual DbSet<ProofOfPayment>? ProofOfPayments { get; set; }
        public virtual DbSet<PurchaceInvoice>? PurchaceInvoices { get; set; }
        public virtual DbSet<PurchaceInvoiceLine>? PurchaceInvoiceLines { get; set; }
        public virtual DbSet<Address>? Addresses { get; set; }
        public virtual DbSet<Company>? Companies { get; set; }
        public virtual DbSet<ContactPerson>? ContactPersons { get; set; }
        public virtual DbSet<GoodsReceivedVoucher>? GoodsReceivedVouchers { get; set; }
        public virtual DbSet<PurchaceOrderPayment> PurchaceOrderPayments { get; set; }
        public virtual DbSet<GoodsReceivedVoucherLine>? GoodsReceivedVoucherLines { get; set; }
        public virtual DbSet<ProductInventorySnapshot>? ProductInventorySnapshots { get; set; }
        public virtual DbSet<InventorySnapShotLog>? InventorySnapShotLogs { get; set; }
        public virtual DbSet<PurchaceOrder>? PurchaceOrders { get; set; }
        public virtual DbSet<PurchaceOrderLine>? PurchaceOrderLines { get; set; }
        public virtual DbSet<SalesOrder>? SalesOrders { get; set; }
        public virtual DbSet<SalesOrderItem>? SalesOrderItems { get; set; }
        public virtual DbSet<StockMovement>? StockMovements { get; set; }
        public virtual DbSet<StockMovementLog>? StockMovementLogs { get; set; }
        public virtual DbSet<StockTakeLog>? StockTakeLogs { get; set; }
        public virtual DbSet<SubCategory>? SubCategories { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Seed Data for Expense Categories
            modelBuilder.Entity<AccountCategory>().HasData(
                new AccountCategory 
                { 
                    Id = 1, 
                    Name = "Operating Expenses", 
                    Description = "Used for recurring or day-to-day expenses",
                    AccountType = AccountType.Expense
                },
                new AccountCategory 
                { 
                    Id = 2, 
                    Name = "Capital Expenditures", 
                    Description = "Used for payments that result in acquiring or improving long-term assets",
                    AccountType = AccountType.Expense
                },
                new AccountCategory 
                { 
                    Id = 3, 
                    Name = "Cost of Goods Sold", 
                    Description = "Used for services directly tied to inventory procurement or preparation",
                    AccountType = AccountType.Expense
                },
                new AccountCategory
                {
                    Id = 4,
                    Name = "Other Expense Accounts",
                    Description = "For payments that don’t fit into the above categories",
                    AccountType = AccountType.Expense
                },
                new AccountCategory
                {
                    Id = 6,
                    Name = "Cash",
                    Description = "money that is readily available for use",
                    AccountType = AccountType.Assets
                },
                new AccountCategory
                {
                    Id = 7,
                    Name = "Accounts Receivable",
                    Description = "money owed to the company",
                    AccountType = AccountType.Assets
                },
                new AccountCategory
                {
                    Id = 8,
                    Name = "Inventory",
                    Description = "goods for sale",
                    AccountType = AccountType.Assets
                },
                new AccountCategory
                {
                    Id = 9,
                    Name = "Accounts Payable",
                    Description = "money a business owes to its suppliers or creditors",
                    AccountType = AccountType.Liability
                },
                new AccountCategory
                {
                    Id = 10,
                    Name = "Short-term Loans",
                    Description = "loans aquired by the company",
                    AccountType = AccountType.Liability
                },
                new AccountCategory
                {
                    Id = 11,
                    Name = "Owner's Capital",
                    Description = "capital invested into the company",
                    AccountType = AccountType.Equity
                },
                new AccountCategory
                {
                    Id = 12,
                    Name = "Retained Earnings",
                    Description = "Retained Earnings",
                    AccountType = AccountType.Equity
                },
                new AccountCategory
                {
                    Id = 13,
                    Name = "Sales Revenue",
                    Description = "amount of money a business earns from selling goods or services",
                    AccountType = AccountType.Income
                }
            );

            // Seed Data for Accounts
            modelBuilder.Entity<Account>().HasData(
    // Operating Expenses Accounts
    new Account
    {
        Id = 1,
        Code = 1001, // Category 1 (Operating Expenses) + Account ID 1
        Name = "Utilities",
        Description = "Electricity, water, and internet expenses",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 1
    },
    new Account
    {
        Id = 2,
        Code = 1002, // Category 1 (Operating Expenses) + Account ID 2
        Name = "Repairs and Maintenance",
        Description = "Expenses related to repairing or maintaining assets",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 1
    },
    new Account
    {
        Id = 3,
        Code = 1003,
        Name = "Cleaning Services",
        Description = "Expenses for cleaning services provided to the shop",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 1
    },
    new Account
    {
        Id = 4,
        Code = 1004,
        Name = "Security Services",
        Description = "Payments made for security personnel or services",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 1
    },
    new Account
    {
        Id = 5,
        Code = 1005,
        Name = "Professional Fees",
        Description = "Fees paid to consultants, lawyers, or accountants",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 1
    },
    new Account
    {
        Id = 6,
        Code = 1006,
        Name = "Advertising and Marketing",
        Description = "Expenses for promotions and advertisements",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 1
    },

    // Capital Expenditures Accounts
    new Account
    {
        Id = 7,
        Code = 2007, // Category 2 (Capital Expenditures) + Account ID 7
        Name = "Building Improvements",
        Description = "Renovations and upgrades to buildings",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 2
    },
    new Account
    {
        Id = 8,
        Code = 2008, // Category 2 (Capital Expenditures) + Account ID 8
        Name = "Equipment Purchases",
        Description = "Purchases of equipment used in operations",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 2
    },
    new Account
    {
        Id = 9,
        Code = 2009, // Category 2 (Capital Expenditures) + Account ID 9
        Name = "Furniture and Fixtures",
        Description = "Purchases of furniture or fixtures for the shop",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 2
    },
    new Account
    {
        Id = 10,
        Code = 2010,
        Name = "Long-term Maintenance",
        Description = "Expenses related to significant repairs or long-term maintenance",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 2
    },

    // Cost of Goods Sold Accounts
    new Account
    {
        Id = 11,
        Code = 3011, // Category 3 (Cost of Goods Sold) + Account ID 11
        Name = "Transportation Costs",
        Description = "Costs related to transporting inventory",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 3
    },
    new Account
    {
        Id = 12,
        Code = 3012,
        Name = "Packaging Costs",
        Description = "Costs incurred for packaging materials",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 3
    },
    new Account
    {
        Id = 13,
        Code = 3013,
        Name = "Storage Costs",
        Description = "Costs related to storing inventory",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 3
    },
    new Account
    {
        Id = 14,
        Code = 3014,
        Name = "Raw Materials",
        Description = "Cost of raw materials used for goods",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 3
    },
    new Account
    {
        Id = 15,
        Code = 3015,
        Name = "Direct Labor",
        Description = "Wages paid to employees directly involved in production",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 3
    },

    // Cash Accounts
    new Account
    {
        Id = 16,
        Code = 6016, // Category 6 (Cash) + Account ID 16
        Name = "Cash in Hand",
        Description = "Physical cash available in registers or safes",
        Balance = 0.00m,
        AccountType = AccountType.Assets,
        AccountCategoryId = 6
    },
    new Account
    {
        Id = 17,
        Code = 6017, // Category 6 (Cash) + Account ID 17
        Name = "Bank Account",
        Description = "Operational bank account for business transactions",
        Balance = 0.00m,
        AccountType = AccountType.Assets,
        AccountCategoryId = 6
    },
    new Account
    {
        Id = 24,
        Code = 6024, // Category 6 (Cash) + Account ID 17
        Name = "EcoCash Account",
        Description = "Operational EcoCash account for business transactions",
        Balance = 0.00m,
        AccountType = AccountType.Assets,
        AccountCategoryId = 6
    },

    // Accounts Receivable
    new Account
    {
        Id = 18,
        Code = 7018, // Category 7 (Accounts Receivable) + Account ID 18
        Name = "Customer Debts",
        Description = "Amounts owed by customers",
        Balance = 0.00m,
        AccountType = AccountType.Assets,
        AccountCategoryId = 7
    },
    new Account
    {
        Id = 26,
        Code = 7026, // Category 7 (Accounts Receivable) + Account ID 18
        Name = "VAT Receivable",
        Description = "Amounts owed by customers",
        Balance = 0.00m,
        AccountType = AccountType.Assets,
        AccountCategoryId = 7
    },

    // Accounts Payable
    new Account
    {
        Id = 19,
        Code = 9019, // Category 9 (Accounts Payable) + Account ID 19
        Name = "Supplier Debts",
        Description = "Amounts owed to suppliers for goods or services",
        Balance = 0.00m,
        AccountType = AccountType.Liability,
        AccountCategoryId = 9
    },
    new Account
    {
        Id = 25,
        Code = 9025, // Category 9 (Accounts Payable) + Account ID 19
        Name = "VAT Payable",
        Description = "Amounts owed to suppliers for goods or services",
        Balance = 0.00m,
        AccountType = AccountType.Liability,
        AccountCategoryId = 9
    },


    // Short-term Loans
    new Account
    {
        Id = 20,
        Code = 1020, // Category 10 (Short-term Loans) + Account ID 20
        Name = "Bank Loan",
        Description = "Short-term bank loans acquired for operations",
        Balance = 0.00m,
        AccountType = AccountType.Liability,
        AccountCategoryId = 10
    },

    // Equity Accounts
    new Account
    {
        Id = 21,
        Code = 1121, // Category 11 (Owner's Capital) + Account ID 21
        Name = "Owner's Equity",
        Description = "Capital contributed by the owner",
        Balance = 0.00m,
        AccountType = AccountType.Equity,
        AccountCategoryId = 11
    },
    new Account
    {
        Id = 22,
        Code = 1122, // Category 11 (Retained Earnings) + Account ID 22
        Name = "Retained Earnings",
        Description = "Profits retained in the business for future use",
        Balance = 0.00m,
        AccountType = AccountType.Equity,
        AccountCategoryId = 12
    },

    // Sales Revenue
    new Account
    {
        Id = 23,
        Code = 1223, // Category 12 (Sales Revenue) + Account ID 23
        Name = "Product Sales",
        Description = "Revenue from selling goods",
        Balance = 0.00m,
        AccountType = AccountType.Income,
        AccountCategoryId = 13
    },
    new Account
    {
        Id = 27,
        Code = 1227, 
        Name = "Inventory",
        Description = "Goods For sale",
        Balance = 0.00m,
        AccountType = AccountType.Assets,
        AccountCategoryId = 8
    },

    //Expense Accounts
    new Account
    {
        Id = 28,
        Code = 1228, 
        Name = "Cost Of Goods Sold",
        Description = "Cost Of Goods Sold",
        Balance = 0.00m,
        AccountType = AccountType.Expense,
        AccountCategoryId = 3
    }
);

            

            // Configure Audited Entities
            ConfigureAuditedEntity<Account>(modelBuilder);
            ConfigureAuditedEntity<AccountCategory>(modelBuilder);
            ConfigureAuditedEntity<CashUp>(modelBuilder);
            ConfigureAuditedEntity<DayEndSalesSummary>(modelBuilder);
            ConfigureAuditedEntity<Company>(modelBuilder);
            ConfigureAuditedEntity<ContactPerson>(modelBuilder);
            ConfigureAuditedEntity<Employee>(modelBuilder);
            ConfigureAuditedEntity<ExchangeRate>(modelBuilder);
            ConfigureAuditedEntity<Expense>(modelBuilder);
            ConfigureAuditedEntity<GoodsReceivedVoucher>(modelBuilder);
            ConfigureAuditedEntity<InventorySnapShotLog>(modelBuilder);
            ConfigureAuditedEntity<JournalEntry>(modelBuilder);
            ConfigureAuditedEntity<Payment>(modelBuilder);
            ConfigureAuditedEntity<ProductInventory>(modelBuilder);
            ConfigureAuditedEntity<ProductInventorySnapshot>(modelBuilder);
            ConfigureAuditedEntity<ProductPrice>(modelBuilder);
            ConfigureAuditedEntity<PurchaceInvoice>(modelBuilder);
            ConfigureAuditedEntity<ProofOfPayment>(modelBuilder);
            ConfigureAuditedEntity<PurchaceOrder>(modelBuilder);
            ConfigureAuditedEntity<SalesOrder>(modelBuilder);
            ConfigureAuditedEntity<PurchaceOrderPayment>(modelBuilder);
            ConfigureAuditedEntity<StockMovement>(modelBuilder);
            ConfigureAuditedEntity<StockMovementLog>(modelBuilder);
            ConfigureAuditedEntity<StockTakeLog>(modelBuilder);

            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
            modelBuilder.Ignore<IdentityRole>();
            modelBuilder.Ignore<IdentityUser<string>>();

        }


        private static void ConfigureAuditedEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : AuditedAggregateRoot<int>
        {
            modelBuilder.Entity<TEntity>(entity =>
            {
                // Configure the relationship for Creator
                entity.HasOne<User>("Creator")
                      .WithMany()
                      .HasForeignKey("CreatorId")
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure the relationship for Deleter
                entity.HasOne<User>("Deleter")
                      .WithMany()
                      .HasForeignKey("DeleterId")
                      .OnDelete(DeleteBehavior.Restrict);

                // If the entity is of type FullAuditedAggregateRoot, configure LastModifierUser
                if (typeof(FullAuditedAggregateRoot<int>).IsAssignableFrom(typeof(TEntity)))
                {
                    entity.HasOne<User>("LastModifierUser")
                          .WithMany()
                          .HasForeignKey("LastModifierUserId")
                          .OnDelete(DeleteBehavior.Restrict);
                }
            });
        }

    }
}
