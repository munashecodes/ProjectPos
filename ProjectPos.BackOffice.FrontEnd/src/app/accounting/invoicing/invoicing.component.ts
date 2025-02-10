import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { Currency } from 'src/proxy/enums/currency';
import { Months } from 'src/proxy/enums/months';
import { OrderOption } from 'src/proxy/enums/order-option';
import { OrderOption2 } from 'src/proxy/enums/order-option2';
import { PaymentMethod } from 'src/proxy/enums/payment-method';
import { SaleType } from 'src/proxy/enums/sale-type.enum';
import { SalesOrderStatus } from 'src/proxy/enums/sales-order-status';
import { Status } from 'src/proxy/enums/status';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { GetExchangeRateDto } from 'src/proxy/interfaces/get-exchange-rate-dto';
import { PaymentDto } from 'src/proxy/interfaces/payment-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { ProofOfPaymentDto } from 'src/proxy/interfaces/proof-of-payment-dto';
import { SalesOrderDto } from 'src/proxy/interfaces/sales-order-dto';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { CompanyService } from 'src/proxy/services/company.service';
import { ExchangeRateService } from 'src/proxy/services/exchange-rate.service';
import { PaymentService } from 'src/proxy/services/payment.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ProofOfPaymentService } from 'src/proxy/services/proof-of-payment.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';
import jspdf, { jsPDF } from "jspdf";
import jsPDFInvoiceTemplate, { OutputType } from "jspdf-invoice-template";
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-invoicing',
  templateUrl: './invoicing.component.html',
  styleUrls: ['./invoicing.component.scss']
})
export class InvoicingComponent implements OnInit {
  
  order: SalesOrderDto = {} as SalesOrderDto;

  newOder: SalesOrderDto = {} as SalesOrderDto;

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
  
  props = {
    outputType: OutputType.Save,
    returnJsPDFDocObject: true,
    fileName: "Invoice 2021",
    orientationLandscape: false,
    compress: true,
    logo: {
        src: "https://raw.githubusercontent.com/edisonneza/jspdf-invoice-template/demo/images/logo.png",
        type: 'PNG', //optional, when src= data:uri (nodejs case)
        width: 53.33, //aspect ratio = width/height
        height: 26.66,
        margin: {
            top: 0, //negative or positive num, from the current position
            left: 0 //negative or positive num, from the current position
        }
    },
    stamp: {
        inAllPages: true, //by default = false, just in the last page
        src: "https://raw.githubusercontent.com/edisonneza/jspdf-invoice-template/demo/images/qr_code.jpg",
        type: 'JPG', //optional, when src= data:uri (nodejs case)
        width: 20, //aspect ratio = width/height
        height: 20,
        margin: {
            top: 0, //negative or positive num, from the current position
            left: 0 //negative or positive num, from the current position
        }
    },
    business: {
        name: "Business Name",
        address: "Albania, Tirane ish-Dogana, Durres 2001",
        phone: "(+355) 069 11 11 111",
        email: "email@example.com",
        email_1: "info@example.al",
        website: "www.example.al",
    },
    contact: {
        label: "Invoice issued for:",
        name: "Client Name",
        address: "Albania, Tirane, Astir",
        phone: "(+355) 069 22 22 222",
        email: "client@website.al",
        otherInfo: "www.website.al",
    },
    invoice: {
        label: "Invoice #: ",
        num: 19,
        invDate: "Payment Date: 01/01/2021 18:12",
        invGenDate: "Invoice Date: 02/02/2021 10:17",
        headerBorder: false,
        tableBodyBorder: false,
        header: [
          {
            title: "#", 
            style: { 
              width: 10 
            } 
          }, 
          { 
            title: "Title",
            style: {
              width: 30
            } 
          }, 
          { 
            title: "Description",
            style: {
              width: 80
            } 
          }, 
          { title: "Price"},
          { title: "Quantity"},
          { title: "Unit"},
          { title: "Total"}
        ],
        table: Array.from(Array(10), (item, index)=>([
            index + 1,
            "There are many variations ",
            "Lorem Ipsum is simply dummy text dummy text ",
            200.5,
            4.5,
            "m2",
            400.5
        ])),
        additionalRows: [{
            col1: 'Total:',
            col2: '145,250.50',
            col3: 'ALL',
            style: {
                fontSize: 14 //optional, default 12
            }
        },
        {
            col1: 'VAT:',
            col2: '20',
            col3: '%',
            style: {
                fontSize: 10 //optional, default 12
            }
        },
        {
            col1: 'SubTotal:',
            col2: '116,199.90',
            col3: 'ALL',
            style: {
                fontSize: 10 //optional, default 12
            }
        }],
        invDescLabel: "Invoice Note",
        invDesc: "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary.",
    },
    footer: {
        text: "The invoice is created on a computer and is valid without the signature and stamp.",
    },
    pageEnable: true,
    pageLabel: "Page ",
};

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
      console.log('sales orders', this.ordersByDate)
      this.orders = this.ordersByDate;
      this.filteredOrders = this.orders
    });

    this.ratesService.getAllList()
        .subscribe(res => {
          this.rates = res.data;
          console.log(res.data);

          this.rate = this.rates.find(x => x.currency === Currency.USD)!;

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
    this.date.setDate(this.date.getDate() + 1);
    console.log(this.date)
    var dateString: string = this.date.toISOString();
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
      this.filteredOrders = this.orders;
    }
  }

  filterByUser(){
    this.filteredOrders = this.orders.filter(x => x.creatorId === this.teller.id);
  }

  filterByCustomer(){
    this.filteredOrders = this.orders.filter(x => x.customerId === this.customer.id);
  }

  resetFilter(){
    this.filteredOrders = this.orders;
  }

  filterByStatus(){
    this.filteredOrders = this.orders
    this.filteredOrders = this.filteredOrders.filter(x => x.status === this.status);
  }

  view(order: SalesOrderDto){
    this.order = order;

    console.log(this.order)
    this.editModal = true;
  }

  getChange(event: any){
    var paid = event.value;
    this.changeValue = this.payment.amount! - this.order.price!;
  }

  printOrder(order: SalesOrderDto){
    this.order = {} as SalesOrderDto;
    this.order = {...order}
    this.newOder = order;
    this.printInvoice();
  }

  printInvoice(){
    let imgSrc = 'image-url';
    var staus = this.newOder.status === SalesOrderStatus.Complete ? "TAX INVOICE" : "QUOTATION";
    let pdf = new jspdf('p', 'mm', 'a4');

    //shearwate logo
    pdf.setFontSize(8);
    pdf.addImage('assets/demo/images/logos/FARM-LOGO.png\n', 'PNG', 5, 5, 25, 15);
    
    //shearwater address
    pdf.text('Andile Fresh Farm Produce | 6215 Mkhosana T/Ship'
      +'\n Magwaza Complex | Mkhosana Main\nVictoria Falls | Zimbabwe\n+263 779 577 216 | 779 972 253 | 777 788 719'
      +'\nVat No 0000000 | BP No 0000000 | TIN 2001463449\nandilefreshfarmproduce@gmail.com', 5, 25);

    pdf.setFontSize(12);
    pdf.text( staus.toUpperCase(), 100, 45, { align: 'center' });

    pdf.setFontSize(8);
    pdf.text('Invoice Date:            ' + new Date(this.newOder.creationTime!).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) 
        +  '\nInvoice No:               ' + this.newOder.id 
        +  '\nBuyer Name:         ' + this.newOder.customerName 
        +  '\nReceipt Number:  ' + this.newOder.id, 5, 50);
    pdf.text(this.newOder.customerName 
        +  '\n' + this.newOder.customer!.address!.street + ' ' + this.newOder.customer!.address!.addressLine1 
        +  '\n' + this.newOder.customer!.address!.city + ' ' + this.newOder.customer!.address!.country 
        +  '\n' + this.newOder.customer!.vatNumber, 205, 50 , { align: 'right'});

    pdf.line(5, 65, 205, 65, 'F');

    //table
    pdf.setFontSize(8);

    autoTable(pdf, {
      headStyles: { fillColor: [200, 200, 200], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
      bodyStyles: {lineWidth: 0.1 },
      footStyles: { fillColor: [200, 200, 200], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
      head: [
        [
          'Code', 
          'Product', 
          'Unit Cost', 
          'Qty', 
          'Unit', 
          'Disc %',
          'Total'
        ]
      ],
      body: this.newOder.salesOrderItems!.map(x => 
        [
          // new Date(x.dateOfActivity).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric'}), 
          x.barCode!,
          x.productName!, 
          x.unitPrice!.toFixed(2), 
          x.quantity!,
          x.unit!, 
          '0',
          (x.price!).toFixed(2)
        ],  
      ),
      foot: [
        [ '', '', '', '', '', 'Discount', '0.00'],
        [ '', '', '', '', '', 'Sub Total', this.newOder.price!.toFixed(2)],
        [ '', '', '', '', '', 'VAT', this.newOder.vat!.toFixed(2)],
        [ '', '', '', '', '', 'Invoice Total', (this.newOder.priceIncludingVat!).toFixed(2)],
      ],
      startY: 70,
      margin: 5,
      theme: 'striped',
      styles: {
        fontSize: 8,
        cellPadding: 2,
        overflow: 'linebreak',
      },
      columnStyles: {
        0: { cellWidth: 25 }, // Adjust the cell width for the date column if needed
        2: { halign: 'right', cellWidth: 15 },
        3: { halign: 'right', cellWidth: 15 },
        4: { halign: 'right' },
        5: { halign: 'right'},
        6: { halign: 'right', cellWidth: 20},
        7: { halign: 'right', cellWidth: 15 },
      },
    });

    const addFooter = (pdfInstance: jspdf) => {
      pdfInstance.setFont('Helvetica', 'normal');
      pdfInstance.setFontSize(8);
      pdfInstance.line(5, 260, 205, 260, 'F');
      pdfInstance.text('Notes: All bank charges including intermediary bank charges are the responsibility of the client.' 
        +' The correct and full amount as invoiced must be received into our account. ' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })
        +'\n\nAcc Name:      Andile Fresh Produce\nBank Name:    CABS\nBranch:           Victoria Falls'
        +'\nBranch Code:  \nSwift Code:     \nAccount No:    1133592821', 5, 265, { align: 'left', maxWidth: 205 });
    }

    addFooter(pdf);
    pdf.save(this.newOder.customerName + '-' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
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
    this.payment.paidAmount = this.rate === undefined ? this.paidValue : this.paidValue/this.rate.exchangeRate?.baseToRate!;
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

    this.order.balance! -= this.payment.paidAmount!;
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
