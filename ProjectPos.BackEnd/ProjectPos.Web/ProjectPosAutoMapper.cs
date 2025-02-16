using AutoMapper;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;

namespace ProjectPos.Web
{
    public class ProjectPosAutoMapper :  Profile
    {
        public ProjectPosAutoMapper()
        {
            //add expense mapping
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account!.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company!.Name));
            CreateMap<ExpenseDto, Expense>();

            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<AccountCategory, AccountCategoryDto>();
            CreateMap<AccountCategoryDto, AccountCategory>();

            CreateMap<JournalEntry, JournalEntryDto>();
            CreateMap<JournalEntryDto, JournalEntry>();

            CreateMap<JournalEntryLine, JournalEntryLineDto>();
            CreateMap<JournalEntryLineDto, JournalEntryLine>();

            CreateMap<StockMovementLog, StockMovementLogDto>();
            CreateMap<StockMovementLogDto, StockMovementLog>();

            //add stock movement mapping
            CreateMap<StockMovement, StockMovementDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
                .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.Product!.BarCode));
            CreateMap<StockMovementDto, StockMovement>();

            //add purchase order payments mapping
            CreateMap<PurchaceOrderPayment, PurchaceOrderPaymentsDto>();
            CreateMap<PurchaceOrderPaymentsDto, PurchaceOrderPayment>();

            CreateMap<StockTakeLog, StockTakeLogDto>();
            CreateMap<StockTakeLogDto, StockTakeLog>();

            CreateMap<EmployeeDetailsDto, EmployeeDetails>();
            CreateMap<EmployeeDetails, EmployeeDetailsDto>();
            
            CreateMap<SalaryStructure, SalaryStructureDto>()
                .ForMember(des => des.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name))
                .ForMember(des => des.EmployeeSurname, opt => opt.MapFrom(src => src.Employee!.Surname));
            CreateMap<SalaryStructureDto, SalaryStructure>();

            
            CreateMap<Attendance, AttendanceDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name));
            CreateMap<AttendanceDto, Attendance>();

            CreateMap<EmployeeDeduction, EmployeeDeductionDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name));
            CreateMap<EmployeeDeductionDto, EmployeeDeduction>();

            //add mapping for overtime
            CreateMap<OvertimeRecord, OvertimeRecordDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Employee!.SalaryStructure!.OvertimeRate));
            CreateMap<OvertimeRecordDto, OvertimeRecord>();

            CreateMap<PaySlip, PaySlipDto>()
                .ForMember(des => des.EmployeeName, opt => opt.MapFrom(src => src.Employee!.Name))
                .ForMember(des => des.EmployeeSurname, opt => opt.MapFrom(src => src.Employee!.Surname));
            CreateMap<PaySlipDto, PaySlip>();

            CreateMap<PayRollCycleDto, PayRollCycle>();
            CreateMap<PayRollCycle, PayRollCycleDto>();

            CreateMap<AccessLog, AccessLogDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName));
            CreateMap<AccessLogDto, AccessLog>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<CashUp, CashUpDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Creator!.UserName));
            CreateMap<CashUpDto, CashUp>();

            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();

            CreateMap<ContactPerson, ContactPersonDto>();
            CreateMap<ContactPersonDto, ContactPerson>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<ProductInventory, ProductDto>();

            CreateMap<ProductDto ,ProductInventory>();

            CreateMap<ExchangeRate, ExchangeRateDto>();
            CreateMap<ExchangeRateDto, ExchangeRate>();

            CreateMap<GoodsReceivedVoucher, GoodsReceivedVoucherDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier!.Name));
            CreateMap<GoodsReceivedVoucherDto, GoodsReceivedVoucher>();

            CreateMap<GoodsReceivedVoucherLine, GoodsReceivedVoucherLineDto>()
                .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.Product!.BarCode))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name));
            CreateMap<GoodsReceivedVoucherLineDto, GoodsReceivedVoucherLine>();

            CreateMap<Payment, PaymentDto>();
            CreateMap<PaymentDto, Payment>();

            CreateMap<PurchaceInvoice, PurchaceInvoiceDto>();
            CreateMap<PurchaceInvoiceDto, PurchaceInvoice>();

            CreateMap<PurchaceInvoiceLine, PurchaceInvoiceLineDto>();
            CreateMap<PurchaceInvoiceLineDto, PurchaceInvoiceLine>();


            CreateMap<ProductInventory, ProductInventoryDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory!.Name));
            CreateMap<ProductInventoryDto, ProductInventory>();

            CreateMap<ProductInventorySnapshot, ProductInventorySnapshotDto>();
            CreateMap<ProductInventorySnapshotDto, ProductInventorySnapshot>();

            CreateMap<InventorySnapShotLog, InventorySnapShotLogDto>();
            CreateMap<InventorySnapShotLogDto, InventorySnapShotLog>();

            CreateMap<ProductInventorySnapshot, ProductInventory>();
            CreateMap<ProductInventory, ProductInventorySnapshot>();

            CreateMap<ProductPrice, ProductPriceDto>()
                .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.Product!.BarCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product!.Name));
            CreateMap<ProductPriceDto, ProductPrice>();

            CreateMap<ProofOfPayment, ProofOfPaymentDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));
            CreateMap<ProofOfPaymentDto, ProofOfPayment>();

            CreateMap<PurchaceOrder, PurchaceOrderDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<PurchaceOrderDto, PurchaceOrder>();

            CreateMap<PurchaceOrderLine, PurchaceOrderLineDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product!.Name));
            CreateMap<PurchaceOrderLineDto, PurchaceOrderLine>()
                .ForMember(des => des.PurchaceOrder, opt => opt.Ignore());

            CreateMap<SalesOrder, SalesOrderDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name));
            CreateMap<SalesOrderDto, SalesOrder>()
                .ForMember(des => des.Customer, opt => opt.Ignore());

            CreateMap<SalesOrderItem, SalesOrderItemDto>()
                .ForMember(dest => dest.IsTaxable, opt => opt.MapFrom(src => src.Product!.IsTaxable));
            CreateMap<SalesOrderItemDto, SalesOrderItem>();

            CreateMap<StockMovement, StockMovementDto>();
            CreateMap<StockMovementDto, StockMovement>();

            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<SubCategoryDto, SubCategory>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.SupervisorCode, opt => opt.MapFrom(src => src.SupervisorCodeHash));
            CreateMap<UserDto, User>();
            CreateMap<UserSignInDto, User>();
        }
    }
}
