
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.AppServices;
using ProjectPos.Services.BackgroundServices;
using ProjectPos.Services.Interfaces;
using Quartz;
using Quartz.Spi;

namespace ProjectPos.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("EndOfDayJob");

                q.AddJob<EndOfDayJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("EndOfDayTrigger")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(23, 0))); // 11:00 PM
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ProjectPosDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.Parse("8.0.19-mysql"));
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<ICompanyService, CompanyService>();
            builder.Services.AddTransient<IContactPersonService, ContactPersonService>();
            builder.Services.AddTransient<IExchangeRateService, ExchangeRateService>();
            builder.Services.AddTransient<IGoodsReceivedVoucherService, GoodsReceivedVoucherService>();
            builder.Services.AddTransient<IProductPriceService, ProductPriceService>();
            builder.Services.AddTransient<IProductInventoryService, ProductInventoryService>();
            builder.Services.AddTransient<IPurchaceInvoiceService, PurchaceInvoiceService>();
            builder.Services.AddTransient<IPurchaceOrderService, PurchaceOrderService>();
            builder.Services.AddTransient<IProofOfPaymentService, ProofOfPaymentService>();
            builder.Services.AddTransient<ISubCategoryService, SubCategoryService>();
            builder.Services.AddTransient<IEmployeeService, EmployeeService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<ISalesOrderService, SalesOrderService>();
            builder.Services.AddTransient<ICashUpService, CashUpService>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IProductInventorySnapShotService, ProductInventorySnapShotService>();
            builder.Services.AddTransient<IStockMovementService, StockMovementService>();
            builder.Services.AddTransient<IDayEndSalesSummaryService, DayEndSalesSummaryService>();
            builder.Services.AddTransient<IAccountCategoryService, AccountCategoryService>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IJournalEntryService, JournalEntryService>();
            builder.Services.AddTransient<IExpenseService, ExpenseService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            

            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}