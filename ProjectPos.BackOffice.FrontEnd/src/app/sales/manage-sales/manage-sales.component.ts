import { Currency } from './../../../proxy/enums/currency';
import { filter } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { SalesOrderDto } from 'src/proxy/interfaces/sales-order-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { CompanyService } from 'src/proxy/services/company.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';
import { SaleType } from 'src/proxy/enums/sale-type.enum';
import { OrderOption } from 'src/proxy/enums/order-option';
import { Table } from 'primeng/table';
import { OrderOption2 } from 'src/proxy/enums/order-option2';
import { Status } from 'src/proxy/enums/status';
import { SalesOrderStatus } from 'src/proxy/enums/sales-order-status';
import { Months } from 'src/proxy/enums/months';
import { ProofOfPaymentService } from 'src/proxy/services/proof-of-payment.service';
import { ProofOfPaymentDto } from 'src/proxy/interfaces/proof-of-payment-dto';
import { PaymentDto } from 'src/proxy/interfaces/payment-dto';
import { PaymentMethod } from 'src/proxy/enums/payment-method';
import { PaymentService } from 'src/proxy/services/payment.service';
import { ExchangeRateService } from 'src/proxy/services/exchange-rate.service';
import { GetExchangeRateDto } from 'src/proxy/interfaces/get-exchange-rate-dto';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { Category } from 'src/proxy/enums/category';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';

@Component({
  selector: 'app-manage-sales',
  templateUrl: './manage-sales.component.html',
  styleUrls: ['./manage-sales.component.scss']
})
export class ManageSalesComponent implements OnInit {

  order: SalesOrderDto = {} as SalesOrderDto;

  orders: SalesOrderDto[] = [];

  customers: CompanyDto[] = [];

  customer: CompanyDto = {} as CompanyDto;

  filteredOrders: SalesOrderDto[] = [];

  ordersByDate: SalesOrderDto[] = [];

  ordersByDateRange: SalesOrderDto[] = [];

  ordersByMonth: SalesOrderDto[] = [];

  ordersByUser: SalesOrderDto[] = [];

  ordersByCustomer: SalesOrderDto[] = [];

  ordersBySaleType: SalesOrderDto[] = [];

  proofOfPayments: ProofOfPaymentDto[] = [];

  proofOfPayment: ProofOfPaymentDto = {} as ProofOfPaymentDto;

  payment: PaymentDto = {} as PaymentDto

  user: UserDto = {} as UserDto;

  date = new Date();

  startDate = new Date();

  endDate = new Date();

  teller: UserDto = {} as UserDto;

  users: UserDto[] = [];

  paymentTypes: PaymentMethod[] = [];

  paymentType = PaymentMethod.Cash;

  saleOptions: OrderOption[] = [];

  saleOptions2: OrderOption2[] = [];

  statuses: SalesOrderStatus[] = [];

  rates: GetExchangeRateDto[] = [];

  rate: GetExchangeRateDto = {} as GetExchangeRateDto;

  totalAmount = 0;

  subTotal = 0;

  vat = 0;

  paidAmount = 0;

  change = 0;

  balance = 0;

  products: ProductInventoryDto[] = [];

  filteredProducts: ProductInventoryDto[] = [];

  product: ProductInventoryDto = {} as ProductInventoryDto;

  addedProduct: ProductInventoryDto = {} as ProductInventoryDto;

  months: Months[] = [];

  companies: CompanyDto[] = [];

  orderItems: SalesOrderItemDto[] = [];

  selectedOrderItems: SalesOrderItemDto[] = [];

  filteredCategories: SubCategoryDto[] = [];

  subCategories: SubCategoryDto[] = [];

  categories: Category[] = [];

  orderItem: SalesOrderItemDto = {} as SalesOrderItemDto;

  month = 0;

  confirm: any = {
    quantiy: 0
  };

  totalPrice = 0;

  invoiceValue = 0;

  changeValue = 0;

  paidValue = 0;

  value = 0;

  currencies: Currency[] = [];

  currency!: Currency;

  option = OrderOption.TODAY;

  option2 = OrderOption2.ALL;

  status!: SalesOrderStatus;

  saleTypes: SaleType[] = [];

  saleType!: SaleType;

  cols: any[] = [];

  rateModal = false;

  deleteProductsDialog = false;

  createModal = false;

  payModal = false;

  deleteModal = false;

  quantityModal = false;

  submitted = false;

  editModal = false;

  isDisabled = true;

  constructor(
    private salesService: SalesOrderService,
    private userService: AuthService,
    private companyService: CompanyService,
    private customerService: CompanyService,
    private popService: ProofOfPaymentService,
    private paymentService: PaymentService,
    private ratesService: ExchangeRateService,
    private productService: ProductInventoryService,
    private messageService: MessageService,
    private categoryService: SubCategoryService
  ){}

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    var todayDate = new Date();
    var dateString: string = todayDate.toISOString();
    console.log(dateString)

    this.userService.getAllUsers()
    .subscribe(res => {
      this.users = res.data;
      console.log(this.users)
    });

    this.categoryService.getAllList()
    .subscribe(res => {
      console.log(res);
      this.subCategories = res.data;
    });

    this.companyService.getAllList()
      .subscribe((res) => {
        console.log(res);
        this.companies = res.data;
    });

    this.productService.getAllList()
      .subscribe((res) => {
        console.log("prods");
        console.log(res);
        this.products = res.data;
      })

    this.salesService.getAllByDate(dateString)
    .subscribe(res => {
      this.ordersByDate = res.data;
      console.log(this.ordersByDate)
      this.orders = this.ordersByDate;
    });

    this.ratesService.getAllList()
        .subscribe(res => {
          this.rates = res.data;
          console.log(res.data);

          //this.rate = this.rates.find(x => x.currency === Currency.USD)!;

        });

    this.customerService.getAllList()
    .subscribe(res => {
      this.customers = res.data.filter((x: any) => x.isSupplier === false);
      console.log(this.customers)
    });

    this.saleOptions = Object.values(OrderOption)
    this.saleOptions2 = Object.values(OrderOption2)
    this.statuses = Object.values(SalesOrderStatus)
    this.months = Object.values(Months)
    this.paymentTypes = Object.values(PaymentMethod);
    this.saleTypes = Object.values(SaleType)
    this.categories = Object.values(Category)
    this.currencies = Object.values(Currency);

  }

  confirmQuantity(){
    console.log(this.product)
    this.quantityModal = true

  }

  addItem(){
    if(this.confirm.quantity === 0){
      // this.nullQuantityModal = true;
    }
    else{
      this.orderItem.productId = this.product.id;
      this.orderItem.productName = this.product.name;
      this.orderItem.barCode = this.product.barCode;
      this.orderItem.quantity = this.confirm.quantity;
      this.orderItem.unitPrice = this.product.productPrice?.price;
      this.orderItem.price = this.orderItem.quantity! * this.orderItem.unitPrice!;

      console.log(this.orderItem)
      // this.selectedOrderItem.productId = this.product.id;
      // this.selectedOrderItem.quantity = this.confirm.quantity;
      // this.selectedOrderItem.unitPrice = this.product.productPrice?.price;
      // this.selectedOrderItem.price = this.orderItem.quantity! * this.orderItem.unitPrice!;

      if(!Array.isArray(this.order.salesOrderItems)){
        this.order.salesOrderItems = []
        this.order.salesOrderItems = [...this.order.salesOrderItems, this.orderItem];
        this.invoiceValue += this.orderItem.price;
        this.order.price! += this.orderItem.price;
      }
      else{
        this.order.salesOrderItems = [...this.order.salesOrderItems, this.orderItem];
        this.invoiceValue += this.orderItem.price;
      }

      console.log(this.order.salesOrderItems)

      // this.order.salesOrderItems = [...this.order.salesOrderItems!, this.orderItem];
      // this.selectedOrderItems.push(this.selectedOrderItem);
      this.value += 1

      this.quantityModal = false;
      this.submitted = true
    }
  }

  filterCategories(event: any){
    var department = event.value;
    this.filteredCategories = this.subCategories.filter(x => x.category === department);
  }

  filterItems(event: any){
    var category =  event.value;
    console.log(category)
    this.filteredProducts = this.products.filter(x => x.product?.subCategoryId === category.id)
    console.log(this.filteredProducts)
  }

  changeRate(event: any){
    this.currency = event.value;
    this.rate = this.rates.find(x => x.currency === this.currency)!;
  }

  resetRate(){
    this.totalPrice = 0;
    this.order.salesOrderItems!.forEach(item => {
        
      this.totalPrice += item.price!;

    });
    this.invoiceValue = this.totalPrice;
    this.order.price = this.totalPrice;
    this.order.balance = this.totalPrice;
    
    this.order.currency = Currency.USD;
    this.rateModal = false
  }

  confirmDeleteSelected() {
    this.deleteProductsDialog = false;
    this.selectedOrderItems.forEach(product => {
      var index = this.order.salesOrderItems!.findIndex(x => x === product);
      this.order.salesOrderItems?.splice(index, 1);

      this.value = this.value - 1;

      this.invoiceValue = this.invoiceValue - product.price!;
      this.order.price = this.invoiceValue;
      this.order.price! -= product.price!;
      console.log(this.invoiceValue);
    });

    this.messageService.add({ 
      severity: 'success', 
      summary: 'Successful', 
      detail: 'Products Deleted', 
      life: 3000 
    });
  }

  get(){
    if(this.option === OrderOption.TODAY){
      this.getToday();
    }
  }

  getToday(){
    
    this.filteredOrders = [];
    var todayDate = new Date();
    var dateString: string = todayDate.toISOString();
    console.log(dateString)

    this.salesService.getAllByDate(dateString)
    .subscribe(res => {
      this.filteredOrders = res.data;
      console.log(this.filteredOrders)
      this.orders = this.filteredOrders;
    });
  }

  getByDate(){
    this.filteredOrders = [];
    console.log(this.date)
    this.date.setDate(this.date.getDate());
    console.log(this.date)
    var dateString: string = new Date(this.date.getTime() + (2 * 60 * 60 * 1000)).toISOString();
    console.log(dateString)

    this.salesService.getAllByDate(dateString)
    .subscribe(res => {
      this.filteredOrders = res.data;
      console.log(this.filteredOrders)
      this.orders = this.filteredOrders;
    });
  }

  getByDateRange(){
    this.filteredOrders = [];
    var startDateString: string = this.startDate.toISOString();
    var endDateString: string = this.endDate.toISOString();

    console.log(endDateString)

    this.salesService.getAllByDateRange(startDateString, endDateString)
    .subscribe(res => {
      this.filteredOrders = res.data;
      console.log(this.filteredOrders)
      this.orders = this.filteredOrders;
    });
  }

  getByMonth(event: any){
    console.log(event.value)
    if(event.value === Months.JAN){
      this.month = 1
    }
    else if(event.value === Months.FEB){
      this.month = 2
    }
    else if(event.value === Months.MAR){
      this.month = 3
    }
    else if(event.value === Months.APR){
      this.month = 4
    }
    else if(event.value === Months.MAY){
      this.month = 5
    }
    else if(event.value === Months.JUN){
      this.month = 6
    }
    else if(event.value === Months.JUL){
      this.month = 7
    }
    else if(event.value === Months.AUG){
      this.month = 8
    }
    else if(event.value === Months.SEP){
      this.month = 9
    }
    else if(event.value === Months.OCT){
      this.month = 10
    }
    else if(event.value === Months.NOV){
      this.month = 11
    }
    else if(event.value === Months.DEC){
      this.month = 12
    }
    else{
      this.month = 0
    }

    console.log(this.month)
    

    this.salesService.getAllByMonth(this.month)
    .subscribe(res => {
      this.filteredOrders = res.data;
      this.orders = res.data;
      console.log(this.orders)
    });

    console.log(this.orders);
  }


  filterAll(){
    if(this.option2 === OrderOption2.ALL){
      this.orders = this.filteredOrders;
    }
  }

  filterByUser(){
    this.orders = this.filteredOrders.filter(x => x.creatorId === this.teller.id);
  }

  filterByCustomer(){
    this.orders = this.filteredOrders.filter(x => x.customerId === this.customer.id);
  }

  filterByStatus(){
    this.ordersBySaleType = this.filteredOrders
    this.orders = this.ordersBySaleType.filter(x => x.status === this.status);
  }

  view(order: SalesOrderDto){
    this.rate = this.rates.find(x => x.currency === order.currency)!;
    order.currencyBalance = this.rate === undefined ? order.balance! :  order.balance! * this.rate.exchangeRate!.baseToRate!;

    let total = 0;
    order.salesOrderItems?.forEach(item => {
      item.currencyUnitPrice = this.rate === undefined ? item.unitPrice! : (item.unitPrice! * this.rate.exchangeRate!.baseToRate!);
      item.currencyPrice = this.rate === undefined ? item.price! :  (item.price! * this.rate.exchangeRate!.baseToRate!);
      total += item.currencyPrice!;
    });

    this.subTotal = this.rate === undefined ? order.price! : order.price! * this.rate.exchangeRate?.baseToRate!;

    this.vat = this.rate === undefined ? order.vat! : order.vat! * this.rate.exchangeRate?.baseToRate!;

    this.totalAmount = this.rate === undefined ? order.priceIncludingVat! : order.priceIncludingVat! * this.rate.exchangeRate?.baseToRate!;

    this.balance = this.rate === undefined ? order.balance! : order.balance! * this.rate.exchangeRate?.baseToRate!;

    this.order = order;

    console.log(this.order)
    this.editModal = true;
  }

  quot(){
    this.order.saleType = SaleType.Cash;
    this.order.currency = Currency.USD;
    this.status = SalesOrderStatus.Quotation
    this.createModal = true;
  }

  creditSale(){
    this.order.saleType = SaleType.Credit
    this.order.currency = Currency.USD;
    this.status = SalesOrderStatus.NotPaid
    this.createModal = true;
  }

  getChange(event: any){
    var paid = event.value;
    this.changeValue = this.balance! - this.paidValue!;
  }

  pay(){
    this.changeValue = this.balance;
    this.payModal = true;
  }

  selectSaleType(){
    if(this.paymentType === PaymentMethod.POP){
      this.getPop()
    }
  }

  getPop(){
    this.popService.getByCustomer(this.order.customerId!)
    .subscribe(res => {
      this.proofOfPayments = res.data.filter((x : any) => x.usableAmount > 0);
    })
  }

  processPop(){
    console.log(this.proofOfPayment)
    console.log(this.order)
    if(this.order.balance! > this.proofOfPayment.usableAmount!){
      this.paidValue = this.proofOfPayment.usableAmount!
      this.proofOfPayment.usableAmount = 0;
      this.order.balance! -= this.proofOfPayment.usableAmount!;
      this.order.paidAmount! += this.proofOfPayment.usableAmount!;
      console.log(this.paidValue)
    }
    else if(this.order.balance! < this.proofOfPayment.usableAmount!){
      this.paidValue = this.order.balance!;
      this.proofOfPayment.usableAmount! -= this.order.balance!;
      this.order.paidAmount! += this.order.balance!;
      this.order.balance = 0;

      console.log(this.paidValue)
    }
  }

  savePayment(){
    this.rate = this.rates.find(x => x.currency === this.order.currency)!;

    this.payment.salesOrderId = this.order.id;
    this.payment.creationTime = new Date();
    this.payment.creatorId = this.user.id;
    this.payment.lastModifierUserId = this.user.id;
    this.payment.lastModificationTime = new Date();
    this.payment.totalPrice = this.order.price;
    this.payment.paidAmount = this.paidValue
    this.payment.uSDPaidAmount = this.rate === undefined ? this.paidValue : this.paidValue/this.rate.exchangeRate?.baseToRate!;
    this.payment.changeAmount = this.changeValue;
    this.payment.amount = this.order.price;
    this.payment.currency = this.order.currency;
    this.payment.orderDate = this.order.creationTime;
    // this.payment.exchangeRate = this.order.

    console.log(this.payment);

    this.paymentService.create(this.payment)
    .subscribe(res => {
      console.log(res.data);

      if(res.isSuccess){
        if(this.paymentType === PaymentMethod.POP){
          this.updatePop();
        }
        
        this.updateOrder();

        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message, 
          life: 3000
        });
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Success', 
          detail: res.message, 
          life: 3000
        });
      }
    },
    (error) => {
      this.messageService.add({
        severity:'error', 
        summary: 'Error', 
        detail: error.message, 
        life: 3000
      });
    });

    this.payModal = false;
  }

  updateOrder(){
    this.submitted = true;

    console.log(this.order)

    this.order.balance! -= this.payment.uSDPaidAmount!;
    this.order.lastModificationTime = new Date;
    this.order.lastModifierUserId = this.user.id;
    
    if (this.order.balance! <= 0){
      this.order.isPaid = true;
      this.order.status = SalesOrderStatus.Complete;
    }
    else if(this.order.balance! > 0){
      this.order.isPaid = false;
      this.order.status = SalesOrderStatus.Incomplete;
    }

    console.log(this.order)

    this.salesService.update(this.order)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message, 
          life: 3000
        });
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Success', 
          detail: res.message, 
          life: 3000
        });
      }
    },
    (error) => {
      this.messageService.add({
        severity:'error', 
        summary: 'Error', 
        detail: error.message, 
        life: 3000
      });
    });

    this.totalPrice = 0;

    this.invoiceValue = 0;
  
    this.changeValue = 0;
  
    this.paidValue = 0;
  
    this.value = 0;
  }

  updatePop(){
    this.proofOfPayment.lastModifierUserId = this.user.id;
    this.proofOfPayment.lastModificationTime = new Date();

    this.popService.update(this.proofOfPayment)
    .subscribe(res => {
      console.log(res.data)
    })
  }

  save(){
    this.submitted = true;

    console.log(this.order)
    
    this.order.customerId = this.order.customer!.id;
    this.order.creatorId = this.user.id
    this.order.creationTime = new Date();
    this.order.lastModificationTime = new Date();
    this.order.lastModifierUserId = this.user.id;
    this.order.isPaid = false;
    this.order.price = this.invoiceValue;
    this.order.balance = this.invoiceValue;
    this.order.creationTime = new Date(),
    this.order.status = this.status;

    console.log(this.order)

    this.salesService.create(this.order)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        console.log(res)

        res.data.customerName = this.order.customer?.name

        if(!Array.isArray(this.orders)){
          this.orders = []
          this.orders = [...this.orders, res.data];
        }
        else{
          this.orders = [...this.orders, res.data];
        }

        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message, 
          life: 3000
        });
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Success', 
          detail: res.message, 
          life: 3000
        });
      }
    },
    (error) => {
      this.messageService.add({
        severity:'error', 
        summary: 'Error', 
        detail: error.message, 
        life: 3000
      });
    });
    
    this.selectedOrderItems = [];
    this.orderItems = [];
    this.value = 0;

    this.hideDialog()
  }

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.payModal = false;
    this.deleteModal = false;
    this.order = {} as SalesOrderDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
