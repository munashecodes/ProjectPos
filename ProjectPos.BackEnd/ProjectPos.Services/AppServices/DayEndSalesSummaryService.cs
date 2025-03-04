using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.AppServices
{
    public class DayEndSalesSummaryService : IDayEndSalesSummaryService
    {
        //intialize the DbContext and the mapper and Ilogger
        private readonly IMapper _mapper;
        private readonly ILogger<DayEndSalesSummaryService> _logger;
        private readonly ProjectPosDbContext _context;

        public DayEndSalesSummaryService(
            ProjectPosDbContext context, 
            IMapper mapper, 
            ILogger<DayEndSalesSummaryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<DayEndSalesSummaryDto>> CreateDayEndSalesSummaryAsync()
{
    int maxRetries = 5;
    int currentRetry = 0;
    TimeSpan delay = TimeSpan.FromSeconds(2);

    while (currentRetry < maxRetries)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var date = DateTime.Today;

                // Check if day end has already been run
                var existingDayEnd = await _context.DayEndSalesSummaries!
                    .FirstOrDefaultAsync(x => x.CreationTime.Date == date);

                if (existingDayEnd != null)
                {
                    return ServiceResponse<DayEndSalesSummaryDto>.Failure("Day end has already been processed for today");
                }

                // Fetch data in a single query to minimize database calls
                var salesData = await _context.SalesOrders!
                    .Where(x => x.CreationTime.Date == date)
                    .GroupBy(x => 1)
                    .Select(g => new
                    {
                        TotalSales = g.Sum(x => x.Price),
                        TotalTransactions = g.Count(),
                        CashSales = g.Where(x => x.SaleType == SaleType.Cash).Sum(x => x.Price),
                        CardSales = g.Where(x => x.SaleType == SaleType.CreditCard).Sum(x => x.Price),
                        MobilePayments = g.Where(x => x.SaleType == SaleType.EcoCash).Sum(x => x.Price),
                        CreditSales = g.Where(x => x.SaleType == SaleType.Credit).Sum(x => x.Price)
                    })
                    .FirstOrDefaultAsync();

                // Fetch cash-ups for the day
                var cashSalesReported = await _context.CashUps!
                    .Where(c => c.CreationTime.Date == date)
                    .SumAsync(c => c.USDAmount);

        if (salesData != null)
        {
            // Calculate average transaction value safely
            var averageTransactionValue = salesData.TotalTransactions > 0
            ? salesData.TotalSales / salesData.TotalTransactions
            : 0;

            // Create DTO
            var dayEndSalesSummaryDto = new DayEndSalesSummaryDto
            {
                TotalSales = salesData.TotalSales ?? 0,
                TotalTransactions = salesData.TotalTransactions,
                AverageTransactionValue = averageTransactionValue ?? 0,
                CashSales = salesData.CashSales ?? 0,
                CashSalesReported = cashSalesReported ?? 0,
                CardSales = salesData.CardSales ?? 0,
                MobilePayments = salesData.MobilePayments ?? 0,
                CreditSales = salesData.CreditSales ?? 0,
                TotalDiscounts = 0, // Add logic if applicable
                TotalReturns = 0,  // Add logic if applicable
                ReturnAmount = 0,  // Add logic if applicable
                VoidedTransactions = 0, // Add logic if applicable
                VoidedAmount = 0,      // Add logic if applicable
                CancelledTransactions = 0 // Add logic if applicable
            };

            // Add to database
            var dayEndEntity = _mapper.Map<DayEndSalesSummary>(dayEndSalesSummaryDto);
            _context.DayEndSalesSummaries!.Add(dayEndEntity);
        }

                // Create inventory snapshot with retry
                var isSnapshotCreated = await CreateInventorySnapShotAsync();
                if (!isSnapshotCreated.IsSuccess)
                {
                    _logger.LogError($"Failed to create inventory snapshot at {DateTime.UtcNow}: {isSnapshotCreated.Message}");
                    throw new Exception("Failed to create inventory snapshot: " + isSnapshotCreated.Message);
                }

                // Generate sales journals with retry
                var isJournalCreated = await GenerateSalesJournalsAsync();
                if (!isJournalCreated)
                {
                    _logger.LogError($"Failed to generate sales journals at {DateTime.UtcNow}");
                    throw new Exception("Failed to generate sales journals");
                }

                // Generate expense journals with retry
                var isExpenseJournalCreated = await GenerateExpenseJournalsAsync();
                if (!isExpenseJournalCreated)
                {
                    _logger.LogError($"Failed to generate expense journals at {DateTime.UtcNow}");
                    throw new Exception("Failed to generate expense journals");
                }

                // Generate GRV journals with retry
                var isGRVJournalCreated = await GenerateGRVJournalsAsync();
                if (!isGRVJournalCreated)
                {
                    _logger.LogError($"Failed to generate GRV journals at {DateTime.UtcNow}");
                    throw new Exception("Failed to generate GRV journals");
                }

                // Capture account snapshot with retry
                var accountSnapshot = await CaptureAccountSnapshotAsync();
                if (!accountSnapshot.IsSuccess)
                {
                    _logger.LogError($"Failed to capture account snapshot at {DateTime.UtcNow}: {accountSnapshot.Message}");
                    throw new Exception("Failed to capture account snapshot: " + accountSnapshot.Message);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ServiceResponse<DayEndSalesSummaryDto>.Success(null, "Day End Sales Summary created successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw; // Rethrow to be caught by outer try-catch for retry logic
            }
        }
        catch (Exception ex)
        {
            currentRetry++;
            _logger.LogWarning(ex, 
                $"Attempt {currentRetry} of {maxRetries} failed for Day End Sales Summary creation: {ex.Message}", 
                currentRetry, 
                maxRetries, 
                ex.Message);

            if (currentRetry == maxRetries)
            {
                _logger.LogError(ex, "All retry attempts failed for Day End Sales Summary creation");
                return ServiceResponse<DayEndSalesSummaryDto>.Failure(
                    $"Failed to create Day End Sales Summary after {maxRetries} attempts: {ex.Message}");
            }

            // Exponential backoff
            await Task.Delay(delay);
            delay *= 2; // Double the delay for next retry
        }
    }

    // This should never be reached due to the return in the final retry
    _logger.LogError("Unexpected error in retry logic");
    return ServiceResponse<DayEndSalesSummaryDto>.Failure("Unexpected error in retry logic");
}


        public Task<ServiceResponse<DayEndSalesSummaryDto>> GetDayByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<DayEndSalesSummaryDto>> GetDayByDateRangeAsync(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<DayEndSalesSummaryDto>> GetDayByMonthAsync(int month)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<DayEndSalesSummaryDto>> UpdateDayEndSalesSummaryAsync(DayEndSalesSummaryDto dayEndSalesSummaryDto)
        {
            throw new NotImplementedException();
        }

                public async Task<ServiceResponse<ProductInventorySnapshotDto>> CreateInventorySnapShotAsync()
        {
            try
            {
                // Check if a snapshot has already been created today
                var existingSnapshot = await _context.InventorySnapShotLogs!
                    .FirstOrDefaultAsync(x => x.CreationTime.Date == DateTime.Today);

                if (existingSnapshot != null)
                {
                    return ServiceResponse<ProductInventorySnapshotDto>.Success(null,"Day has already been closed. Snapshot creation skipped.");
                }

                // Fetch all product inventories
                var inventories = await _context.ProductInventories!.ToListAsync();

                // Summarize inventory for serialization
                var summarizedInventory = inventories.Select(prod => new
                {
                    prod.Id,
                    prod.Name,
                    prod.QuantityOnHand,
                });

                // Create and add inventory snapshot
                var snapshot = new ProductInventorySnapshot
                {
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    SnapShotType = SnapShotEnum.CloseDay,
                    LastModificationTime = DateTime.Now,
                    Inventory = JsonSerializer.Serialize(summarizedInventory)
                };

                await _context.ProductInventorySnapshots!.AddAsync(snapshot);

                // Determine start day for the log
                var lastSnapshotLog = await _context.InventorySnapShotLogs!
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync();

                var startDay = lastSnapshotLog?.CreationTime ?? DateTime.Today;

                // Create and add snapshot log
                var log = new InventorySnapShotLog
                {
                    StartDay = startDay,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                };

                await _context.InventorySnapShotLogs!.AddAsync(log);

                // Save changes
                await _context.SaveChangesAsync();

                return ServiceResponse<ProductInventorySnapshotDto>.Success(null,"Snapshot created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating inventory snapshot.");
                return ServiceResponse<ProductInventorySnapshotDto>.Failure($"Snapshot creation failed: {ex.Message}");
            }
        }

        //Create a metthod to capture the account snapshot
        private async Task<ServiceResponse<bool>> CaptureAccountSnapshotAsync()
        {
            try
            {
                //get all financial accounts
                var accounts = await _context.Accounts.ToListAsync();

                //create account snapshots
                var accountSnapshots = new List<FinancialAccountSnapShot>();

                foreach (var account in accounts)
                {
                    var accountSnapshot = new FinancialAccountSnapShot
                    {
                        FinancialAccountId = account.Id,
                        ClosingBalance = account.Balance,
                        SnapShotDate = DateTime.Now
                    };

                    accountSnapshots.Add(accountSnapshot);
                }

                //add account snapshots to db
                await _context.FinancialAccountSnapShots.AddRangeAsync(accountSnapshots);
                //save changes
                await _context.SaveChangesAsync();

                return ServiceResponse<bool>.Success(true,"Account Snapshots Captured Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error capturing account snapshots");
                return ServiceResponse<bool>.Failure("Error capturing account snapshots");
            }
        }


        public async Task<bool> GenerateSalesJournalsAsync()
        {
            try
            {

                // Fetch sales orders that need processing
                var sales = await _context.SalesOrders!
                    .Where(x => !x.IsPostedToJournal)
                    .ToListAsync();

                var salesItems = await _context.SalesOrderItems!
                    .Include(x => x.Product)
                    .Include(x => x.SalesOrder)
                    .Where(x => !x.IsPostedToJournal)
                    .ToListAsync();

                var payments = await _context.Payments!
                    .Where(x => !x.IsPostedToJournal)
                    .ToListAsync();

                if (!sales.Any())
                    return true; // No sales to process

                // Prepare account IDs needed for lookup
                var requiredAccountIds = sales.SelectMany(s => new[]
                {
                    s.SaleType switch
                    {
                        SaleType.EcoCash => 24,
                        SaleType.Cash => 16,
                        // SaleType.Credit => 18,
                        _ => 17
                    },
                    18,
                    23, // Revenue account
                    25,  // VAT account
                    27, // Cost of sales
                    28  // Inventory account
                }).Distinct().ToList();

                // Fetch accounts in a single query
                var accounts = await _context.Accounts
                    .Where(a => requiredAccountIds.Contains(a.Id))
                    .ToDictionaryAsync(a => a.Id);

                if (accounts.Count != requiredAccountIds.Count)
                    throw new Exception("Missing accounts for journal processing.");

                var journalEntries = new List<JournalEntry>();

                foreach (var sale in sales)
                {
                    // Determine the debit account based on SaleType
                    var debitAccountId = sale.SaleType switch
                    {
                        SaleType.EcoCash => 24,
                        SaleType.Cash => 16,
                        SaleType.Credit => 18,
                        _ => 17
                    };

                    var journalEntry = new JournalEntry
                    {
                        CreationTime = DateTime.UtcNow,
                        Description = $"Created Sale receipt number {sale.Id}",
                        CreatorId = sale.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = 18,
                                Amount = sale.PriceIncludingVat == null ? 0 :(decimal) sale.PriceIncludingVat,
                                Type = JournalEntryType.Debit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 23,
                                Amount = sale.Price == null ? 0 : (decimal)sale.Price,
                                Type = JournalEntryType.Credit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 25,
                                Amount = sale.Vat == null ? 0 : (decimal) sale.Vat,
                                Type = JournalEntryType.Credit
                            }
                        }
                    };

                    journalEntries.Add(journalEntry);

                    // Mark sale as posted
                    sale.IsPostedToJournal = true;

                    // Update account balances
                    foreach (var line in journalEntry.JournalEntryLines!)
                    {
                        if (!accounts.TryGetValue((int)line.AccountId!, out var account))
                            throw new Exception($"Account with ID {line.AccountId} not found.");

                        account.Balance += AdjustAccountBalance(
                            (AccountType)account.AccountType!,
                            (JournalEntryType)line.Type!,
                            (decimal)line.Amount!
                        );
                    }
                }

                foreach (var item in salesItems)
                {
                    var costOfSale = (item.Product!.Cost == null ? 0 : item.Product!.Cost) * (item.Quantity! == null ? 0 : item.Quantity!);
                    var vat = item.Product!.IsTaxable == true ? (costOfSale * 0.15m) : 0;
                    costOfSale += vat;
                    var journalEntry = new JournalEntry
                    {
                        CreationTime = DateTime.UtcNow,
                        Description = $"Created COGS",
                        CreatorId = item.SalesOrder!.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = 28,
                                Amount = (decimal)(costOfSale),
                                Type = JournalEntryType.Debit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 27,
                                Amount = (decimal)(costOfSale),
                                Type = JournalEntryType.Credit
                            }
                        }
                    };

                    journalEntries.Add(journalEntry);

                    // Mark sale as posted
                    item.IsPostedToJournal = true;

                    // Update account balances
                    foreach (var line in journalEntry.JournalEntryLines!)
                    {
                        if (!accounts.TryGetValue((int)line.AccountId!, out var account))
                            throw new Exception($"Account with ID {line.AccountId} not found.");

                        account.Balance += AdjustAccountBalance(
                            (AccountType)account.AccountType!,
                            (JournalEntryType)line.Type!,
                            (decimal)line.Amount!
                        );
                    }
                }

                foreach (var payment in payments)
                {
                    // Determine the debit account based on SaleType
                    var debitAccountId = payment.MethodOfPay switch
                    {
                        SaleType.EcoCash => 24,
                        SaleType.Cash => 16,
                        SaleType.Credit => 18,
                        _ => 17
                    };

                    var journalEntry = new JournalEntry
                    {
                        CreationTime = DateTime.UtcNow,
                        Description = $"Payment for sale receipt n# {payment.SalesOrderId}",
                        CreatorId = payment.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = debitAccountId,
                                Amount = payment.USDPaidAmountAfterChange! == null ? 0 :  (decimal)payment.USDPaidAmountAfterChange,
                                Type = JournalEntryType.Debit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 18,
                                Amount = payment.USDPaidAmountAfterChange! == null ? 0 :  (decimal)payment.USDPaidAmountAfterChange,
                                Type = JournalEntryType.Credit
                            }
                        }
                    };

                    journalEntries.Add(journalEntry);

                    // Mark sale as posted
                    payment.IsPostedToJournal = true;

                    // Update account balances
                    foreach (var line in journalEntry.JournalEntryLines!)
                    {
                        if (!accounts.TryGetValue((int)line.AccountId!, out var account))
                            throw new Exception($"Account with ID {line.AccountId} not found.");

                        account.Balance += AdjustAccountBalance(
                            (AccountType)account.AccountType!,
                            (JournalEntryType)line.Type!,
                            (decimal)line.Amount!
                        );
                    }
                }

                // Update sales and accounts in bulk
                _context.SalesOrders!.UpdateRange(sales);
                _context.SalesOrderItems!.UpdateRange(salesItems);
                _context.Payments!.UpdateRange(payments);
                _context.Accounts.UpdateRange(accounts.Values);
                await _context.JournalEntries.AddRangeAsync(journalEntries);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales journals: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> GenerateExpenseJournalsAsync()
        {
            try
            {

                // Fetch sales orders that need processing
                var expenses = await _context.Expenses!
                    .Where(x => !x.IsPostedToJournal)
                    .ToListAsync();

                if (!expenses.Any())
                    return true; // No sales to process

                // Prepare account IDs needed for lookup
                var requiredAccountIds = expenses.SelectMany(s => new[]
                {
                    s.AccountId,
                    16,
                    17,
                    24,
                    23
                }).Distinct();

                // Fetch accounts in a single query
                var accounts = await _context.Accounts
                    .Where(a => requiredAccountIds.Contains(a.Id))
                    .ToDictionaryAsync(a => a.Id);

                if (accounts.Count != requiredAccountIds.Count())
                    throw new Exception("Missing accounts for journal processing.");

                var journalEntries = new List<JournalEntry>();

                foreach (var expense in expenses)
                {
                    // Determine the debit account based on SaleType
                    var creditAccountId = expense.PaymentMethod switch
                    {
                        PaymentMethod.EcoCash => 24,
                        PaymentMethod.Cash => 16,
                        PaymentMethod.BankDeposit => 17,
                        _ => 17
                    };

                    var journalEntry = new JournalEntry
                    {
                        CreationTime = DateTime.UtcNow,
                        Description = $"Created Expense N# {expense.Id}",
                        CreatorId = expense.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = creditAccountId,
                                Amount = expense.Amount == null ? 0 : expense.Amount,
                                Type = JournalEntryType.Credit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 23,
                                Amount = expense.Amount == null ? 0 : expense.Amount,
                                Type = JournalEntryType.Debit
                            }
                        }
                    };

                    journalEntries.Add(journalEntry);

                    // Mark sale as posted
                    expense.IsPostedToJournal = true;

                    // Update account balances
                    foreach (var line in journalEntry.JournalEntryLines!)
                    {
                        if (!accounts.TryGetValue((int)line.AccountId!, out var account))
                            throw new Exception($"Account with ID {line.AccountId} not found.");

                        account.Balance += AdjustAccountBalance(
                            (AccountType)account.AccountType!,
                            (JournalEntryType)line.Type!,
                            (decimal)line.Amount!
                        );
                    }
                }

                // Update sales and accounts in bulk
                _context.Expenses!.UpdateRange(expenses);
                _context.Accounts.UpdateRange(accounts.Values);
                await _context.JournalEntries.AddRangeAsync(journalEntries);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales journals: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> GenerateGRVJournalsAsync()
        {
            try
            {

                // Fetch sales orders that need processing
                var grvs = await _context.GoodsReceivedVouchers!
                    .Where(x => !x.IsPostedToJournal && x.IsApproved)
                    .ToListAsync();

                var payments = await _context.PurchaceOrderPayments!
                    .Where(x => !x.IsPostedToJournal)
                    .ToListAsync();

                if (!grvs.Any())
                    return true; // No grvs to process

                // Prepare account IDs needed for lookup
                var requiredAccountIds = payments.SelectMany(s => new[]
                {
                     s.MethodOfPay switch
                    {
                        SaleType.EcoCash => 24,
                        SaleType.Cash => 16,
                        SaleType.CreditCard => 18,
                        _ => 17
                    },
                    23, // Revenue account
                    25,  // VAT account
                    27, // Cost of sales
                    28,  // Inventory account
                    19
                }).Distinct();

                // Union with two new values (19 and 27) added
                requiredAccountIds = requiredAccountIds.Union(new[] { 19, 27 });

                // Fetch accounts in a single query
                var accounts = await _context.Accounts
                    .Where(a => requiredAccountIds.Contains(a.Id))
                    .ToDictionaryAsync(a => a.Id);

                if (accounts.Count != requiredAccountIds.Count())
                    throw new Exception("Missing accounts for journal processing.");

                var journalEntries = new List<JournalEntry>();

                foreach (var grv in grvs)
                {

                    var journalEntry = new JournalEntry
                    {
                        CreationTime = DateTime.UtcNow,
                        Description = $"Created created grv n3 {grv.Id}",
                        CreatorId = grv.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = 19,
                                Amount = grv.Value == null ? 0 : (decimal)grv.Value!,
                                Type = JournalEntryType.Credit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 27,
                                Amount = grv.Value == null ? 0 : (decimal)grv.Value!,
                                Type = JournalEntryType.Debit
                            }
                        }
                    };

                    journalEntries.Add(journalEntry);

                    // Mark sale as posted
                    grv.IsPostedToJournal = true;

                    // Update account balances
                    foreach (var line in journalEntry.JournalEntryLines!)
                    {
                        if (!accounts.TryGetValue((int)line.AccountId!, out var account))
                            throw new Exception($"Account with ID {line.AccountId} not found.");

                        account.Balance += AdjustAccountBalance(
                            (AccountType)account.AccountType!,
                            (JournalEntryType)line.Type!,
                            (decimal)line.Amount!
                        );
                    }
                }

                foreach (var payment in payments)
                {

                    // Determine the debit account based on SaleType
                    var debitAccountId = payment.MethodOfPay switch
                    {
                        SaleType.EcoCash => 24,
                        SaleType.Cash => 16,
                        SaleType.Credit => 18,
                        _ => 17
                    };

                    var journalEntry = new JournalEntry
                    {
                        CreationTime = DateTime.UtcNow,
                        Description = $"payment for GRV n# {payment.GoodsReceivedVoucherId}",
                        CreatorId = payment.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = debitAccountId,
                                Amount = payment.USDPaidAmount == null ? 0 : (decimal)payment.USDPaidAmount!,
                                Type = JournalEntryType.Credit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 19,
                                Amount = payment.USDPaidAmount == null ? 0 :(decimal) payment.USDPaidAmount !,
                                Type = JournalEntryType.Debit
                            }
                        }
                    };

                    journalEntries.Add(journalEntry);

                    // Mark sale as posted
                    payment.IsPostedToJournal = true;

                    // Update account balances
                    foreach (var line in journalEntry.JournalEntryLines!)
                    {
                        if (!accounts.TryGetValue((int)line.AccountId!, out var account))
                            throw new Exception($"Account with ID {line.AccountId} not found.");

                        account.Balance += AdjustAccountBalance(
                            (AccountType)account.AccountType!,
                            (JournalEntryType)line.Type!,
                            (decimal)line.Amount!
                        );
                    }
                }

                // Update sales and accounts in bulk
                _context.GoodsReceivedVouchers!.UpdateRange(grvs);
                _context.PurchaceOrderPayments!.UpdateRange(payments);
                _context.Accounts.UpdateRange(accounts.Values);
                await _context.JournalEntries.AddRangeAsync(journalEntries);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales journals: {Message}", ex.Message);
                return false;
            }
        }

        // Helper method to adjust account balance
        static decimal AdjustAccountBalance(AccountType accountType, JournalEntryType entryType, decimal amount)
        {
            return accountType switch
            {
                AccountType.Assets or AccountType.Expense => entryType == JournalEntryType.Debit ? amount : -amount,
                AccountType.Liability or AccountType.Equity or AccountType.Revenue or AccountType.Income => entryType == JournalEntryType.Credit ? amount : -amount,
                _ => throw new Exception($"Unhandled account type: {accountType}")
            };
        }

        
    }
}
