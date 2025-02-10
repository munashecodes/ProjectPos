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
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var date = DateTime.Today;

                // Fetch data in a single query to minimize database calls
                var salesData = await _context.SalesOrders!
                    .Where(x => x.CreationTime.Date == date)
                    .GroupBy(x => 1) // GroupBy(1) forces aggregation without grouping by a column
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
                    .SumAsync(c => c.Amount);

                //// Handle cases where no sales exist for the day
                //if (salesData == null)
                //{
                //    return new ServiceResponse<DayEndSalesSummaryDto>
                //    {
                //        Data = null,
                //        Message = "No sales data available for the selected day.",
                //        IsSuccess = false
                //    };
                //}

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
                    


                //create inventory snapshot
                var isSnapshotCreated = await CreateInventorySnapShotAsync();
                if (!isSnapshotCreated.IsSuccess)
                {
                    return new ServiceResponse<DayEndSalesSummaryDto>
                    {
                        Data = null,
                        Message = "Failed to create inventory snapshot",
                        IsSuccess = false
                    };
                }

                // Generate sales journals
                var isJournalCreated = await GenerateSalesJournalsAsync();
                if (!isJournalCreated)
                {
                    return new ServiceResponse<DayEndSalesSummaryDto>
                    {
                        Data = null,
                        Message = "Failed to generate sales journals",
                        IsSuccess = false
                    };
                }

                // Save changes
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return success response
                return new ServiceResponse<DayEndSalesSummaryDto>
                {
                    Message = "Day End Sales Summary created successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Day End Sales Summary: {Message}", ex.Message);

                return new ServiceResponse<DayEndSalesSummaryDto>
                {
                    Data = null,
                    Message = "An error occurred while creating Day End Sales Summary.",
                    IsSuccess = false
                };
            }
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
                    return new ServiceResponse<ProductInventorySnapshotDto>
                    {
                        IsSuccess = false,
                        Message = "Day has already been closed. Snapshot creation skipped.",
                        Time = DateTime.Now,
                    };
                }

                // Fetch all product inventories
                var inventories = await _context.ProductInventories!.ToListAsync();

                // Summarize inventory for serialization
                var summarizedInventory = inventories.Select(prod => new
                {
                    prod.Id,
                    prod.BarCode,
                    prod.Name,
                    prod.QuantityOnHand,
                    prod.Unit
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

                return new ServiceResponse<ProductInventorySnapshotDto>
                {
                    IsSuccess = true,
                    Message = "Snapshot created successfully.",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating inventory snapshot.");
                return new ServiceResponse<ProductInventorySnapshotDto>
                {
                    IsSuccess = false,
                    Message = $"Snapshot creation failed: {ex.Message}",
                    Time = DateTime.Now,
                };
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

                if (!sales.Any())
                    return true; // No sales to process

                // Prepare account IDs needed for lookup
                var requiredAccountIds = sales.SelectMany(s => new[]
                {
                    s.SaleType switch
                    {
                        SaleType.EcoCash => 24,
                        SaleType.Cash => 16,
                        SaleType.Credit => 18,
                        _ => 17
                    },
                    23, // Revenue account
                    25  // VAT account
                }).Distinct();

                // Fetch accounts in a single query
                var accounts = await _context.Accounts
                    .Where(a => requiredAccountIds.Contains(a.Id))
                    .ToDictionaryAsync(a => a.Id);

                if (accounts.Count != requiredAccountIds.Count())
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
                        Description = $"Created Sale",
                        CreatorId = sale.CreatorId,
                        IsDeleted = false,
                        JournalEntryLines = new List<JournalEntryLine>
                        {
                            new JournalEntryLine
                            {
                                AccountId = debitAccountId,
                                Amount = sale.PriceIncludingVat,
                                Type = JournalEntryType.Debit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 23,
                                Amount = sale.Price,
                                Type = JournalEntryType.Credit
                            },
                            new JournalEntryLine
                            {
                                AccountId = 25,
                                Amount = sale.Vat,
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

                // Update sales and accounts in bulk
                _context.SalesOrders!.UpdateRange(sales);
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
