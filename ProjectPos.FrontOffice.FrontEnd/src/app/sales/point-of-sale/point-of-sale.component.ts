import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import jspdf from 'jspdf';
import autoTable from 'jspdf-autotable';
import { ConfirmationService, MessageService, SelectItem } from 'primeng/api';
import { DataView } from 'primeng/dataview';
import { Table } from 'primeng/table';
import { environment } from 'src/environments/environment';
import { Currency } from 'src/proxy/enums/currency';
import { SaleType } from 'src/proxy/enums/sale-type.enum';
import { SalesOrderStatus } from 'src/proxy/enums/sales-order-status';
import { Status } from 'src/proxy/enums/status';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { GetExchangeRateDto } from 'src/proxy/interfaces/get-exchange-rate-dto';
import { PaymentDto } from 'src/proxy/interfaces/payment-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { SalesOrderDto } from 'src/proxy/interfaces/sales-order-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { CompanyService } from 'src/proxy/services/company.service';
import { ExchangeRateService } from 'src/proxy/services/exchange-rate.service';
import { PaymentService } from 'src/proxy/services/payment.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ProductPriceService } from 'src/proxy/services/product-price.service';
import { RateService } from 'src/proxy/services/rate.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';
import { UserService } from 'src/proxy/services/user.service';
import { SalesOrderItemDto } from './../../../proxy/interfaces/sales-order-item-dto';
import { PrintReceiptDto } from 'src/proxy/interfaces/print-receipt-dto';

// const url = sessionStorage.getItem('urls');

@Component({
  selector: 'app-point-of-sale',
  templateUrl: './point-of-sale.component.html',
  styleUrls: ['./point-of-sale.component.scss']
})
export class PointOfSaleComponent implements OnInit {

  sortOptions: SelectItem[] = [];

    sortOrder: number = 0;

    sortField: string = '';

    sourceCities: any[] = [];

    targetCities: any[] = [];

    orderCities: any[] = [];

    rates: GetExchangeRateDto[] = [];

    orderItems: SalesOrderItemDto[] = [];

    orderItem: SalesOrderItemDto = {} as SalesOrderItemDto;

    products: ProductInventoryDto[] = [];

    filteredProducts: ProductInventoryDto[] = [];

    reconItems: SalesOrderItemDto[] = [];

    fruits: SalesOrderItemDto[] = [];

    beverages: SalesOrderItemDto[] = [];

    spices: SalesOrderItemDto[] = [];

    sweets: SalesOrderItemDto[] = [];

    vegetables: SalesOrderItemDto[] = [];

    cereals: SalesOrderItemDto[] = [];

    unallocated :  SalesOrderItemDto[] = [];

    displayProducts: ProductInventoryDto[] = [];

    product: ProductInventoryDto = {} as ProductInventoryDto;

    addedProduct: ProductInventoryDto = {} as ProductInventoryDto;

    companies: CompanyDto[] = [];

    priceIncludingVat = 0;

    price = 0;

    vat = 0;

    newOder: SalesOrderDto = {
      id: 0,
      saleType: SaleType.Cash,
      price: 0,
      vat: 0,
      priceIncludingVat: 0,
      balance: 0,
      paidAmount: 0,
      currency: Currency.USD,
      isPaid: false,
      customerId: 0,
      customer: {
        id: 0,
      } as CompanyDto,
    } as SalesOrderDto;

    order: SalesOrderDto = {
      id: 0,
      saleType: SaleType.Cash,
      price: 0,
      vat: 0,
      priceIncludingVat: 0,
      balance: 0,
      paidAmount: 0,
      currency: Currency.USD,
      isPaid: false,
      customerId: 0,
      customer: {
        id: 0,
      } as CompanyDto,
    } as SalesOrderDto;

    selectedOrder: SalesOrderDto = {
      id: 0,
      saleType: SaleType.Cash,
      price: 0,
      vat: 0,
      priceIncludingVat: 0,
      balance: 0,
      paidAmount: 0,
      currency: Currency.USD,
      isPaid: false,
      customerId: 0,
      customer: {
        id: 0,
      } as CompanyDto,
    } as SalesOrderDto;

    selectedOrderItems: SalesOrderItemDto[] = [];

    selectedOrderItem: SalesOrderItemDto = {} as SalesOrderItemDto;

    user: UserDto = {} as UserDto;

    currentUser!: string

    payment: PaymentDto = {} as PaymentDto;

    createModal = false;

    editModal = false;

    deleteModal = false;
    
    quantityModal = false;

    trolleyModal = false;

    payModal = false;

    outOfStockModal = false;

    deleteProductsDialog = false;

    rateModal = false;

    isReturn = false;

    submitted = false;

    nullQuantityModal = false;

    confirm: any = {
      quantity: undefined
    };

    currentValue: string = '';

    currentValue2: string = '';

    totalPrice = 0;

    invoiceValue = 0;

    orderNumber = 0;

    paidValue?: number;

    changeValue = 0;

    cols: any[] = [];

    supervisorCode = "";

    value = 0;

    interval: any;

    image: any;

    searchText: string = '';

    voidModal = false;

    returnModal = false;

    returnApproveModal = false;

    saleType!: SaleType[];

    currencies: Currency[] = [];

    currency!: Currency;

    orderPrice = 0;

    orderPriceIncludingVat = 0;

    isQuotation = false;

    statuses: SalesOrderStatus[] = [];

    rate: GetExchangeRateDto = {} as GetExchangeRateDto;

    url = "";

    rowIndex = 0;

    isRemoved = false;

    receiptNumber = 0;

    barcodeComponents = {
      flag: '',
      plu: '',
      weight: 0
    }

    constructor(
      private productService: ProductInventoryService,
      private confirmationService: ConfirmationService,
      private salesOrderService: SalesOrderService,
      private companyService: CompanyService,
      private messageService: MessageService,
      private rateService: RateService,
      private paymentService: PaymentService,
      private sanitizer: DomSanitizer,
      private ratesService: ExchangeRateService,
      private productPriceService: ProductPriceService,
    private userService: UserService) { }

    ngOnInit() {

      this.isReturn = false;
      this.url = environment.imageUrl;
      console.log( this.url)
        this.productService.getAllList()
        .subscribe((res) => {
          console.log(res);

          

          this.products = res.data;
          this.filteredProducts = this.products
        })

        this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}')
        console.log(this.user)

        this.companyService
        .getAllList()
        .subscribe((res) => {
          console.log(res);
          this.companies = res.data;
        });

        this.ratesService.getAllList()
        .subscribe(res => {
          this.rates = res.data;
          console.log(res.data)
        });

        this.saleType = Object.values(SaleType);
        this.currencies = Object.values(Currency);
        this.statuses = Object.values(SalesOrderStatus)

        this.sortOptions = [
            { label: 'Price High to Low', value: '!price' },
            { label: 'Price Low to High', value: 'price' }
        ];
    }

    // Method to handle number button click
    onButtonClick(value: string) {
      this.currentValue += value;
      this.confirm.quantity = Number(this.currentValue);
    }

    // Method to clear all input
    clear() {
      this.currentValue = '';
      this.confirm.quantity = undefined
    }

    // Method to delete the last character
    keyPadDelete() {
      this.currentValue = this.currentValue.slice(0, -1);
      this.confirm.quantity = Number(this.currentValue);
    }

    // Method to handle number button click
    onButtonClick2(value: string) {
      this.currentValue2 += value;
      this.paidValue = Number(this.currentValue2);
      this.changeValue = this.paidValue - this.priceIncludingVat;
    }

    // Method to clear all input
    clear2() {
      this.currentValue2 = '';
      this.paidValue = undefined;
      this.changeValue = 0;
    }

    // Method to delete the last character
    keyPadDelete2() {
      this.currentValue2 = this.currentValue2.slice(0, -1);
      this.paidValue = Number(this.currentValue2);
      this.changeValue = this.paidValue - this.priceIncludingVat;
    }

    //stringToImage
    stringToImage(base64String: string){
      if (!base64String) {
        return null;
      }

      const image = new Image();
      image.src = base64String;
      return image;
    }

    onVoidApprove(){
      this.userService.verifySupervisor(this.supervisorCode)
      .subscribe(res => {
        if(res.data){
          
          this.supervisorCode = '';
          this.confirmationService.confirm({
            message: "Are you sure you want to delete this product?",
            header: "Confirm",
            icon: "pi pi-exclamation-triangle",
            accept: () => {
              this.confirmDeleteSelected();
              this.voidModal = false;
            },
            reject: () => {
            }
          });
        }
        else{
          // this.messageService.add({
          //   severity:'error', 
          //   summary: 'Error', 
          //   detail: 'Not Authorised', 
          //   life: 3000
          // });
          this.voidModal = false;
          this.supervisorCode = '';
          this.confirmationService.confirm({
            message: "Not Authorised",
            header: "ERROR",
            icon: "pi pi-exclamation-triangle",
            accept: () => {
            },
          });
        }
      })
    }

    onReturnApprove(){
      this.userService.verifySupervisor(this.supervisorCode)
      .subscribe(res => {
        if(res.data){
          
          this.supervisorCode = '';
          this.confirmationService.confirm({
            message: "Are you sure you want to process a return?",
            header: "Confirm",
            icon: "pi pi-exclamation-triangle",
            accept: () => {
              this.returnModal = true;
              this.isReturn = true;
              this.returnApproveModal = false;
            },
            reject: () => {
            }
          });
        }
        else{
          // this.messageService.add({
          //   severity:'error', 
          //   summary: 'Error', 
          //   detail: 'Not Authorised', 
          //   life: 3000
          // });
          this.returnApproveModal = false;
          this.supervisorCode = '';
          this.confirmationService.confirm({
            message: "Not Authorised",
            header: "ERROR",
            icon: "pi pi-exclamation-triangle",
            accept: () => {
            },
          });
        }
      })
    }

    onRemoveFromCart(){
      this.supervisorCode = '';
      this.voidModal = true;
    }

    confirmAddToCart(){
      if(this.confirm.quantity === 0){
        this.nullQuantityModal = true;
      }
      else if(Number(this.confirm.quantity) > this.product.quantityOnHand!){
        //confirmation dialog
        this.confirmationService.confirm({
          message: `Quantity exceeds available stock! Only ${this.product.quantityOnHand} available`,
          accept: () => {
          }
        });
      }
      else{
        this.rowIndex += 1;
        this.rate = this.rates.find(x => x.currency === this.currency)!;
        let eRate = this.rate === undefined ? 1 : this.rate.exchangeRate?.baseToRate;
        this.orderItem.rowIndex = this.rowIndex;
        this.orderItem.productId = this.product.id;
        this.orderItem.productName = this.product.name;
        this.orderItem.barCode = this.product.barCode;
        this.orderItem.quantity = Number(this.confirm.quantity);
        this.orderItem.vatPercentage! = this.product.product?.isTaxable === true ? 0.15 : 0;
        this.orderItem.unitPrice = (this.product.productPrice?.price!)/(1 + this.orderItem.vatPercentage!);
        this.orderItem.currencyUnitPrice = (this.product.productPrice?.price!  * eRate!)/(1 + this.orderItem.vatPercentage!);
        this.orderItem.price = this.orderItem.quantity! * this.orderItem.unitPrice!;
        this.orderItem.currencyPrice = (this.orderItem.quantity! * this.orderItem.currencyUnitPrice!);
        this.orderItem.vat = this.orderItem.price! * this.orderItem.vatPercentage!;

        if(!Array.isArray(this.newOder.salesOrderItems)){
          this.newOder.salesOrderItems = []
        }
        this.newOder.salesOrderItems.push(this.orderItem);
        this.newOder.price! = 0;
        this.newOder.vat! = 0;
        this.newOder.salesOrderItems.forEach(item => {
          this.newOder.price! += item.price!;
          this.newOder.vat! += item.vat!;
        })
        this.newOder.priceIncludingVat! = this.newOder.price! + this.newOder.vat!;
        
        this.priceIncludingVat! = (Number(this.newOder.priceIncludingVat!.toFixed(2))*eRate!);
        this.price = this.newOder.price!*eRate!;
        this.vat = this.newOder.vat!*eRate!;
        // this.selectedOrderItems.push(this.selectedOrderItem);
        console.log(this.orderItems)
        this.value += 1

        this.quantityModal = false;
        this.submitted = true;
        this.confirm.quantity = undefined;
        this.orderItem = {} as SalesOrderItemDto;
        
      this.currentValue = '';
      }
    }
    
    onClear(){
      this.searchText = '';
    }

    // Method to filter by name or barcode
    filterProducts(searchText: string) {
      const searchLowerCase = searchText.toLowerCase();

      this.filteredProducts = this.products.filter(product => 
          product.name!.toLowerCase().includes(searchLowerCase));
    }

    onBarCodeScan(event: any){
      // Cast the event to a KeyboardEvent
      const keyboardEvent = event as KeyboardEvent;

      // Get the input element and its value
      const inputElement = keyboardEvent.target as HTMLInputElement;
      const barcode = inputElement.value;

      
      // Reset the input field for the next scan
      (event.target as HTMLInputElement).value = '';
      
      // Check if the barcode is the correct length
      // if (barcode.length !== 13) {
      //   console.error("Invalid barcode length");
      //   return null;
      // }

      // Extract components based on the provided structure
      const flag = barcode.substring(0, 2);        // F1F2 (flag for weighted product)
      const plu = barcode.substring(2, 6);         // CCCC (PLU code)
      const additionalInfo = barcode.substring(6, 7); // X (Additional info, possibly type or packaging)
      const weightString = barcode.substring(7, 12);  // XXXXX (Weight in kilograms)
      const checkDigit = barcode.substring(12);    // CD (Check digit)

      // Convert weight string to a number with a decimal place (assuming last 4 digits are the fractional part)
      const weight = parseFloat(`${weightString.substring(0, 2)}.${weightString.substring(2)}`);

      // Return the parsed components as an object
      this.barcodeComponents = {
        flag: flag,
        plu: plu,
        weight: weight,
      };
      this.getProduct(barcode);
    }

    getProduct(barcode: string){ 

      let foundProduct = {} as ProductInventoryDto;
    
      // Find the product using the scanned barcode
      if(this.barcodeComponents.flag === '20'){
        //foundProduct = this.products.find(product => product.plu === this.barcodeComponents.plu)!;
        this.productService.getByPlu(this.barcodeComponents.plu)
        .subscribe(res => {
          if(res.isSuccess){
            foundProduct = res.data;
            
            if(foundProduct.status === Status.OutOfStock){
              this.outOfStockModal = true;
            }
            else{
              this.product = { ...foundProduct}
              this.confirm = {
                quantity: foundProduct.isWeighted === true ? this.barcodeComponents.weight : 1
              };
              
              this.confirmAddToCart();
            }
          }
          else{
            this.messageService.add({
              severity:'error', 
              summary: 'Error', 
              detail: res.message, 
              life: 3000
            });

             //confirmation dialog
            this.confirmationService.confirm({
              message: "Poroduct not found",
              accept: () => {
              }
            });

            //reset barcode components
            this.barcodeComponents = {
              flag: '',
              plu: '',
              weight: 0
            }
          }
        })
      }
      else{
        //foundProduct = this.products.find(product => product.barCode === barcode)!;
        this.productService.getByCode(barcode)
        .subscribe(res => {
          if(res.isSuccess){
            foundProduct = res.data;

            if(foundProduct.status === Status.OutOfStock){
              this.outOfStockModal = true;
            }
            else{
              this.product = { ...foundProduct}
              this.confirm = {
                quantity: foundProduct.isWeighted === true ? this.barcodeComponents.weight : 1
              };
              
              this.confirmAddToCart();
            }
          }
          else{
            this.messageService.add({
              severity:'error', 
              summary: 'Error', 
              detail: res.message, 
              life: 3000
            });

             //confirmation dialog
            this.confirmationService.confirm({
              message: "Poroduct not found",
              accept: () => {
              }
            });

            //reset barcode components
            this.barcodeComponents = {
              flag: '',
              plu: '',
              weight: 0
            }
          }
        })
      }
    }
    

    addToCart(product: ProductInventoryDto){
      this.searchText = '';
      if(product.status === Status.OutOfStock){
        this.outOfStockModal = true;
      }
      else{
        this.productPriceService.get(product.id!)
        .subscribe(res => {
          product.productPrice = res.data;

          this.product = { ...product}
          this.confirm = {
            quantity: undefined
          };
        })
        this.quantityModal = true;
        this.submitted = false;
      }
    }

    onReturn(){
      this.returnApproveModal = true;
    }

    onConfirmReturn(){
      // initialize all variables
      this.hideDialog();
      this.price = 0;
      this.vat = 0;
      this.priceIncludingVat = 0;
      this.isReturn = true;
      this.rowIndex = 0;

      this.salesOrderService.get(this.receiptNumber)
      .subscribe(res => {
        if(res.isSuccess){
          this.receiptNumber = 0;
          this.newOder = res.data;

          console.log(this.newOder)

          this.orderPrice = this.newOder.price!;
          this.orderPriceIncludingVat = this.newOder.priceIncludingVat!;

          this.rate = this.rates.find(x => x.currency === this.newOder.currency)!;
          let eRate = this.rate === undefined ? 1 : this.rate.exchangeRate?.baseToRate;
          this.currency = this.newOder.currency!;

          this.newOder.salesOrderItems!.forEach(item => {
            item.rowIndex = this.rowIndex += 1;
            item.vatPercentage = item.isTaxable === true ? 0.15 : 0,
            item.vat = item.price! * item.vatPercentage!,

            item.currencyPrice = item.price!*eRate!;
            item.currencyUnitPrice = item.unitPrice!*eRate!;
          });

          this.price = this.newOder.price!*eRate!;
          this.vat = this.newOder.vat!*eRate!;
          this.priceIncludingVat = this.price + this.vat;
        }
        else{
          this.messageService.add({
            severity:'error', 
            summary: 'Error', 
            detail: res.message, 
            life: 3000
          });
          this.receiptNumber = 0;
          this.isReturn = false;
          //confirmation dialog
          this.confirmationService.confirm({
            message: res.message,
            accept: () => {
            }
          });
        }
      },
      (error) => {
        
        this.receiptNumber = 0;
        this.messageService.add({
          severity:'error', 
          summary: 'Error', 
          detail: error.message, 
          life: 3000
        });
      });

    }

    deleteSelectedProducts() {
      this.deleteProductsDialog = true;
    }
  
    confirmDeleteSelected() {
      this.isRemoved = true;
      this.rate = this.rates.find(x => x.currency === this.currency)!;
      let eRate = this.rate === undefined ? 1 : this.rate.exchangeRate?.baseToRate;

      this.deleteProductsDialog = false;
      this.selectedOrderItems.forEach(rowIndex => {
        var product = this.newOder.salesOrderItems!.find(x => x.rowIndex === rowIndex);
        var index = this.newOder.salesOrderItems!.findIndex(x => x === product);
        // this.newOder.salesOrderItems?.splice(index, 1);

        let voidProd: SalesOrderItemDto = {
          productId: product!.productId,
          productName: product!.productName,
          barCode: product!.barCode,
          quantity: -product!.quantity!,
          unitPrice: -product!.unitPrice!,
          currencyUnitPrice: -product!.currencyUnitPrice!,
          price: -product!.price!,
          currencyPrice: -product!.currencyPrice!,
          vat: -product!.vat!,
          vatPercentage: -product!.vatPercentage!,
          unit: product!.unit,
          rowIndex: this.rowIndex += 1
        };

        //add void product to sales order items after index
        this.newOder.salesOrderItems?.splice(index + 1, 0, voidProd);

        this.newOder.price! -= Number(product!.price!.toFixed(2));
        this.newOder.vat! -= Number(product!.vat!.toFixed(2));

        // this.price! -= (product.price! * eRate!);
        // this.vat! -= product.vat!;

        this.price = this.newOder.price!*eRate!;
        this.vat = this.newOder.vat!*eRate!;

        this.invoiceValue = this.newOder.price!;
        console.log(this.invoiceValue);
      });

      this.newOder.priceIncludingVat! = this.newOder.price! + this.newOder.vat!;
      this.priceIncludingVat! = (Number((this.price! + this.vat!).toFixed(2)));

      this.messageService.add({ 
        severity: 'success', 
        summary: 'Successful', 
        detail: 'Products Deleted', 
        life: 3000 
      });
    }

    onQuantityChange(){
      this.currentValue = this.confirm.quantity!.toString();
    }

    onSortChange(event: any) {
        const value = event.value;

        if (value.indexOf('!') === 0) {
            this.sortOrder = -1;
            this.sortField = value.substring(1, value.length);
        } else {
            this.sortOrder = 1;
            this.sortField = value;
        }
    }

    changeRate(event: any){
      this.totalPrice = 0;
      this.newOder.salesOrderItems!.forEach(item => {
        item.currencyPrice = item.price!; 
        item.currencyUnitPrice = item.unitPrice!; 
        this.totalPrice += item.price!;
      });

      this.currency = event.value;
      console.log(this.currency)

      if(this.currency === Currency.USD){
        this.priceIncludingVat! = Number(this.newOder.priceIncludingVat!.toFixed(2));
        this.price = this.newOder.price!;
        this.vat = this.newOder.vat!;
      }
      else{
        
        this.rate = this.rates.find(x => x.currency === this.currency)!;

        if(this.rate){
          this.priceIncludingVat = Number((this.newOder.priceIncludingVat! * this.rate.exchangeRate?.baseToRate!).toFixed(2));
          this.price = this.newOder.price! * this.rate.exchangeRate?.baseToRate!;
          this.vat = this.newOder.vat! * this.rate.exchangeRate?.baseToRate!;

          this.newOder.salesOrderItems!.forEach(item => {
            item.currencyPrice = item.currencyPrice! * this.rate.exchangeRate?.baseToRate!; 
            item.currencyUnitPrice = item.unitPrice! * this.rate.exchangeRate?.baseToRate!;
          });
        }
        else{
          this.rateModal = true;
        }
        console.log(this.rate)
      }
    }

    resetRate(){
      this.totalPrice = 0;
      this.priceIncludingVat! = Number(this.newOder.priceIncludingVat!.toFixed(2));
      this.price = this.newOder.price!;
      this.vat = this.newOder.vat!;
      this.newOder.salesOrderItems!.forEach(item => {
        item.currencyPrice = item.price!; 
        item.currencyUnitPrice = item.unitPrice!; 
      });
      
      this.newOder.currency = Currency.USD;
      this.rateModal = false
    }

    create(){
      if(this.value == 0){
        this.trolleyModal = true;
      }
      else{
        this.submitted = false;
        this.createModal = true;
      }
      
    }

    createQuot(){

      if(this.value == 0){
        this.trolleyModal = true;
      }
      else{
    
        this.submitted = false;
        this.isQuotation = true;
        this.newOder.saleType = SaleType.Cash;
        this.newOder.status = SalesOrderStatus.Quotation;
        this.createModal = true;
      }
      
    }

    getOrder(){
      this.salesOrderService.get(this.orderNumber)
      .subscribe(res => {
        this.newOder = res.data;
        console.log(this.newOder)
        this.printInvoice();
      })
    }

    printInvoice(){
      let imgSrc = 'image-url';
      var staus = this.newOder.status === SalesOrderStatus.Complete ? "TAX INVOICE" : "QUOTATION";
      let pdf = new jspdf('p', 'mm', 'a4');

      //shearwate logo
      pdf.setFontSize(8);
      pdf.addImage('assets/demo/images/logos/FARM-LOGO.png\n', 'PNG', 5, 5, 25, 15);
      
      //shearwater address
      pdf.text('Andile Fresh Farm Produce | Post Office Box 000'
        +'\n0 Magwaza Complex | Mkhosana Main\nVictoria Falls | Zimbabwe\n+263 000 0000 00 | example@email.com'
        +'\nVat No 0000000 | BP No 0000000\nwww.andilefreshfarm.com', 5, 25);

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
          +'\n\nAcc Name:      Shearwater Adventures (Pvt) Ltd\nBank Name:    First Capital Bank Limited\nBranch:           Victoria Falls'
          +'\nBranch Code:  2157\nSwift Code:     BARCZWHX\nAccount No:    2157-3780841', 5, 265, { align: 'left', maxWidth: 205 });
      }
  
      addFooter(pdf);
      pdf.save(this.newOder.customerName + '-' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
    }

    pay(){
      if(this.newOder.saleType === SaleType.Credit){
        this.newOder.status = SalesOrderStatus.NotPaid;
        this.newOder.balance = this.newOder.priceIncludingVat;
        this.newOder.isPaid = false;
        this.save();
      }
      else if(this.isQuotation === true){
        this.newOder.status = SalesOrderStatus.Quotation;
        this.newOder.balance = this.totalPrice;
        this.newOder.isPaid = false;
        this.save();
      }
      else{
        this.payModal = true;
      }
    }
  
    getChange(event: any){
      var paid = event.value;
      this.changeValue = this.paidValue! - this.priceIncludingVat;
      this.currentValue2 = this.paidValue!.toString();
    }

    edit(order: SalesOrderDto){
      this.selectedOrder = { ...order};
      this.submitted = false;
      this.editModal = true;
    }
  
    delete(order: SalesOrderDto){
      this.selectedOrder = { ...order};
      this.submitted = false;
      this.editModal = true;
    }
  
    deleteSelected(){
    }
    
    saveOrder(){
      if(this.paidValue! < this.priceIncludingVat){
        if(this.newOder.saleType === SaleType.Credit){
          this.save();
        }
        else{
          this.confirmationService.confirm({
            message: "Order has not been fully paid",
            accept: () => {
            },
            reject: () => {
            }
          });
        }
      }
      else{
        this.newOder.isPaid = true;

        this.rate = this.rates.find(x => x.currency === this.currency)!;

        //this.payment.salesOrderId = this.newOder.id;
        this.payment.creationTime = new Date();
        this.payment.creatorId = this.user.id;
        this.payment.lastModifierUserId = this.user.id;
        this.payment.lastModificationTime = new Date();
        this.payment.methodOfPay = this.newOder.saleType;
        this.payment.totalPrice = Number(this.newOder.priceIncludingVat?.toFixed(2));
        this.payment.paidAmount = Number(this.paidValue!.toFixed(2));
        this.payment.uSDPaidAmount = this.newOder.currency === Currency.USD ? Number(this.paidValue?.toFixed(2)) : Number((this.paidValue!/this.rate.exchangeRate?.baseToRate!).toFixed(2));
        this.payment.paidAmountAfterChange = this.changeValue > 0 ? Number((this.paidValue! - this.changeValue).toFixed(2)) : Number(this.paidValue!.toFixed(2));
        this.payment.uSDPaidAmountAfterChange = this.changeValue > 0 ? Number((this.payment.uSDPaidAmount! - (this.newOder.currency === Currency.USD ? this.changeValue : this.changeValue/this.rate.exchangeRate?.baseToRate!)).toFixed(2)) : Number(this.payment.uSDPaidAmount.toFixed(2));
        this.payment.changeAmount = Number(this.changeValue.toFixed(2));
        this.payment.amount = Number(this.newOder.priceIncludingVat?.toFixed(2));
        this.payment.currency = this.newOder.currency;
        this.payment.orderDate = this.newOder.creationTime;
        this.payment.exchangeRate = this.newOder.currency === Currency.USD ? 1 : this.rate.exchangeRate?.baseToRate!

        if(!Array.isArray(this.newOder.payments)){
          this.newOder.payments = []
        }
        this.newOder.payments.push(this.payment);
        this.save();
      }
    }

    inDirectSale(){
      this.newOder.customerId = this.newOder.customer!.id;
      this.newOder.paidAmount = 0;
      if(this.value == 0){
        this.trolleyModal = true;
      }
      else{
        this.pay();
      }
    }

    directSale(customer: number){
      this.newOder.customer!.id = 0;
      this.newOder.customerId = customer;
      if(this.value == 0){
        this.trolleyModal = true;
      }
      else{
        this.pay();
      }
    }
  
    save(){
      this.submitted = true;

  
      console.log(this.newOder)

      if(this.isQuotation == false){
        console.log("not quote")
        console.log(this.totalPrice)
        if(this.newOder.saleType === SaleType.Credit){
          this.newOder.status = SalesOrderStatus.NotPaid;
          this.newOder.balance = this.totalPrice;
          this.newOder.isPaid = false;
        }
        else if(this.paidValue! < this.totalPrice){
          this.newOder.status = SalesOrderStatus.Incomplete;
          this.newOder.balance = this.totalPrice - this.paidValue!;
          this.newOder.isPaid = false;
        }
        else if(this.paidValue! >= this.priceIncludingVat){
          this.newOder.status = SalesOrderStatus.Complete;
          this.newOder.isPaid = true;
          this.newOder.balance = 0;
          console.log("complete")
        }
      }
      let rate = this.newOder.currency === Currency.USD ? 1 : this.rates.find(x => x.currency === this.newOder.currency)?.exchangeRate?.baseToRate!;
      
      this.newOder.creatorId = this.user.id
      this.newOder.creationTime = new Date();
      this.newOder.lastModificationTime = new Date();
      this.newOder.lastModifierUserId = this.user.id;
      this.newOder.creationTime = new Date(),
      this.newOder.paidAmount = this.newOder.saleType === SaleType.Credit ? 0 :  this.newOder.payments?.reduce((a, b) => a + b.paidAmountAfterChange!, 0);
      this.newOder.balance =  this.newOder.priceIncludingVat! - (this.newOder.paidAmount! / rate);

      console.log(this.newOder)
  
      this.salesOrderService.create(this.newOder)
      .subscribe((res) => {
        console.log(res);
        if(res.isSuccess){
          console.log(res)

          if(this.isQuotation === true){

            this.orderNumber = res.data.id;
            this.getOrder()
            this.messageService.add({
              severity:'success', 
              summary: 'Success', 
              detail: 'Quotation created successfully', 
              life: 3000
            });
            
            this.hideDialog()
          }
          else if(this.newOder.saleType === SaleType.Credit){

            this.orderNumber = res.data.id;
            this.newOder.id = res.data.id;
            this.printInvoice();

            this.messageService.add({
              severity:'success', 
              summary: 'Success', 
              detail: 'Order created successfully', 
              life: 3000
            });
            
            this.hideDialog()
          }
          else{
            // this.payment.salesOrderId = res.data.id;
            // this.savePayment(res.data.id);
            this.orderNumber = res.data.id;
            this.newOder.id = res.data.id;
            this.printReceipt()
            this.hideDialog()
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
      this.changeValue = 0;
      this.paidValue = undefined;
      this.currentValue2 = '';

    }

    getTodaySales(){
      var dateString: string = new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toISOString();

      this.salesOrderService.getAllItemsByDate(dateString, this.user.id!)
      .subscribe(res => {
        this.reconItems = res.data;
        this.fruits = this.reconItems.filter(x => x.subCategory === 'Fruits');
        this.vegetables = this.reconItems.filter(x => x.subCategory === 'Vegetables');
        this.beverages = this.reconItems.filter(x => x.subCategory === 'Beverage');
        this.spices = this.reconItems.filter(x => x.subCategory === 'Spices');
        this.sweets = this.reconItems.filter(x => x.subCategory === 'Sweets');
        this.cereals = this.reconItems.filter(x => x.subCategory === 'Cereals');
        this.unallocated = this.reconItems.filter(x => x.subCategory === 'Unallocated');

        this.onCashUp();
      });
    }

    onCashUp(){
      let l = this.newOder.salesOrderItems!.length * 12;
      var staus =  "CASH UP";
      let pdf = new jspdf('p', 'mm', [100, l+150]);
  
      //shearwate logo
      pdf.setFontSize(8);
      pdf.addImage('assets/demo/images/logos/FARM-LOGO.png\n', 'PNG', 40, 5, 25, 15);
      
      //shearwater address
      pdf.text('Andile Fresh Farm Produce | Post Office Box 000'
        +'\n0 Magwaza Complex | Mkhosana Main\nVictoria Falls | Zimbabwe\n+263 77 957 7216 | example@email.com'
        +'\nVat No 0000000 | BP No 0000000\nwww.andilefreshfarm.com', 50, 25, { align: 'center' });
  
      pdf.setFontSize(12);
  
      pdf.text('TELLER:            ' 
        +  '\nTERMINAL:               '
        +  '\nTIME:         ' , 5, 50);

      pdf.text( this.user.fullName
          +  '\n' + new Date().getTime()
          +  '\n' + new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }), 95, 50, { align: 'right' });
      
  
      pdf.line(5, 65, 95, 65, 'F');
  
      //Sweets table
      pdf.setFontSize(12);
  
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Sweets', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.sweets!.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.sweets.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.sweets.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
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
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });

      //Vegetables table
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Vegetables', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.vegetables!.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.vegetables.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.vegetables.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
        ],
        startY: 70 + 10 + (this.sweets.length * 12),
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 8,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });


      //Fruits table
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Fruits', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.fruits.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.fruits.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.fruits.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
        ],
        startY: 70 + 10 + (this.sweets.length * 12) + (this.vegetables.length * 12),
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 8,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });

      // Beverage table
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Beverage', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.beverages!.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.beverages.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.beverages.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
        ],
        startY: 70 + 10 + (this.sweets.length * 12) + (this.vegetables.length * 12) + (this.fruits.length * 12),
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 8,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });

      // Spices table
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Spices', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.spices!.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.spices.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.spices.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
        ],
        startY: 70 + 10 + (this.sweets.length * 12) + (this.vegetables.length * 12) + (this.fruits.length * 12) + (this.beverages.length * 12),
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 8,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });

      // Cereals table
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Cereals', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.cereals!.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.cereals.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.cereals.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
        ],
        startY: 70 + 10 + (this.sweets.length * 12) + (this.vegetables.length * 12) + (this.fruits.length * 12) + (this.beverages.length * 12) + (this.spices.length * 12),
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 8,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });

      // Unallocated table
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Unallocated', 
            '', 
            '', 
            ''
          ],
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.unallocated!.map(x => 
          [
            x.productName!, 
            x.unitPrice!.toFixed(2), 
            x.quantity!,
            (x.price!).toFixed(2)
          ],  
        ),
        foot: [
          [ 
            'Totals', 
            '', 
            this.unallocated.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.quantity!;}, 0).toFixed(2), 
            '$' + this.unallocated.reduce((accumulator: number, currentItem: SalesOrderItemDto) => { return accumulator + currentItem.price!;}, 0).toFixed(2)
          ]
        ],
        startY: 70 + 10 + (this.sweets.length * 12) + (this.vegetables.length * 12) + (this.fruits.length * 12) + (this.beverages.length * 12) + (this.spices.length * 12) + (this.cereals.length * 12),
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 8,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left'},
          1: { halign: 'right', cellWidth: 15 },
          2: { halign: 'right', cellWidth: 20},
          3: { halign: 'right', cellWidth: 15 },
        },
      });
  

      pdf.autoPrint();
      window.open(pdf.output('bloburl'));

      pdf.save(this.newOder.customerName + '-' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
    }

    printReceipt(){
      var customerName = (this.newOder.customer === null || this.newOder.customer === undefined) ? 'Walk In'  : this.newOder.customer?.name;
      let rate = this.newOder.currency === Currency.USD ? 1 : this.rates.find(x => x.currency === this.newOder.currency)?.exchangeRate?.baseToRate!;
      var paid = 0;
      var change = 0;
      var creditReturn = 0;
      var autoTableEndPosY = 0;

      if(this.isReturn){
        this.newOder.price = this.orderPrice;
        this.newOder.priceIncludingVat = this.orderPriceIncludingVat;
        this.newOder.vat = this.newOder.priceIncludingVat - this.newOder.price;
      }

      this.newOder.payments?.forEach(pay => {
        if(pay.paidAmount! < 0){
          creditReturn += pay.paidAmount!
        }
        else{
          paid += pay.paidAmount!
          change = paid - (pay.amount! * rate)
        }
      });

      let imgSrc = 'image-url';
      let baseWidth = 100;
      let l = this.newOder.salesOrderItems!.length * 12;
      var staus = this.newOder.status === SalesOrderStatus.Complete ? "TAX INVOICE" : "QUOTATION";
      let pdf = new jspdf('p', 'mm', [baseWidth, l+220]);
  
      //shearwate logo
      pdf.setFontSize(10);
      
      //shearwater address
      pdf.text('T&T Solar | Post Office Box 642'
        +'\nChinotimba\nVictoria Falls | Zimbabwe\n+263 783 134 362 | ', baseWidth/2, 25, { align: 'center' });
  
      pdf.setFontSize(12);
  
      pdf.text('Invoice Date:            ' 
          +  '\nInvoice No:               '
          +  '\nBuyer Name:         ' 
          +  '\nReceipt Number:  ' , 3, 50);
  
      pdf.text( new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) 
          +  '\n' + this.newOder.id 
          +  '\n' + 'Walk In'
          +  '\n' + this.newOder.id, baseWidth - 5, 50, { align: 'right' });
      
  
  
      //table
      pdf.setFontSize(12);
  
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [
          [ 
            'Item', 
            'Unit $', 
            'Qty', 
            'Total'
          ]
        ],
        body: this.newOder.salesOrderItems!.map(x => 
          [
            x.productName!, 
            x.currencyUnitPrice!.toFixed(2), 
            x.quantity!,
            (x.currencyPrice!).toFixed(2)
          ],  
        ),
        foot: [
          [  '', '', 'Discount', '0.00'],
          [ '', '', 'Sub Total', (this.newOder.price! * rate!)!.toFixed(2)],
          [  '', '', 'VAT', (this.newOder.vat!* rate).toFixed(2)],
          [ '', '', 'Total', (this.newOder.priceIncludingVat! * rate!).toFixed(2)],
          [ '', '', 'Paid', (paid).toFixed(2)],
          [ '', '', 'Change', (change).toFixed(2)],
          [ '', '', 'Return', (creditReturn).toFixed(2)],
        ],
        startY: 72,
        margin: 3,
        theme: 'striped',
        styles: {
          fontSize: 12,
          cellPadding: 2,
          overflow: 'linebreak',
        },
        columnStyles: {
          0: { halign: 'left', cellWidth: 30},
          1: { halign: 'right' },
          2: { halign: 'right', cellWidth: 25},
          3: { halign: 'right'},
        },
        didDrawPage: function (data) {
          autoTableEndPosY = data.cursor!.y; // Store the Y position where table ends
        }
      });

      let footerY = autoTableEndPosY; // Position footer based on table end
  
      pdf.line(3,  footerY + 5, baseWidth - 3,  footerY + 5, 'F');

      pdf.text('TELLER:            ' 
        +  '\nTERMINAL:               '
        +  '\nTIME:         ' , 3, footerY + 10);

      pdf.text( this.user.fullName
          +  '\n' + new Date().getTime()
          +  '\n' + new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }), baseWidth - 3, footerY + 10, { align: 'right' });
      pdf.text('Thank you for shopping with us!',baseWidth/2,  footerY + 30 , {align: 'center'})
      // pdf.autoPrint();
      // window.open(pdf.output('bloburl'));

      var receipt = pdf.output("blob");
      var newReceipt : PrintReceiptDto = {
        printerName: "SLK-TL320",
        receipt: receipt
      }
      this.salesOrderService.printReceipt(newReceipt).subscribe((res) => {
        console.log(res);
      })

      // pdf.save(this.newOder.customerName + '-' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
    }

    savePayment(orderId: number){
      this.rate = this.rates.find(x => x.currency === this.currency)!;

      //this.payment.salesOrderId = this.newOder.id;
      this.payment.creationTime = new Date();
      this.payment.creatorId = this.user.id;
      this.payment.lastModifierUserId = this.user.id;
      this.payment.lastModificationTime = new Date();
      this.payment.methodOfPay = this.newOder.saleType;
      this.payment.totalPrice = this.newOder.priceIncludingVat;
      this.payment.paidAmount = this.paidValue!;
      this.payment.uSDPaidAmount = this.newOder.currency === Currency.USD ? this.paidValue! : this.paidValue!/this.rate.exchangeRate?.baseToRate!;
      this.payment.paidAmountAfterChange = this.changeValue > 0 ? this.paidValue! - this.changeValue : this.paidValue!;
      this.payment.uSDPaidAmountAfterChange = this.changeValue > 0 ? this.payment.uSDPaidAmount - (this.newOder.currency === Currency.USD ? this.changeValue : this.changeValue/this.rate.exchangeRate?.baseToRate!) : this.payment.uSDPaidAmount;
      this.payment.changeAmount = this.changeValue;
      this.payment.amount = this.newOder.priceIncludingVat;
      this.payment.currency = this.newOder.currency;
      this.payment.orderDate = this.newOder.creationTime;
      this.payment.exchangeRate = this.newOder.currency === Currency.USD ? 1 : this.rate.exchangeRate?.baseToRate!

      console.log(this.payment);

      this.paymentService.create(this.payment)
      .subscribe(res => {
        console.log(res.data);

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
      })

      this.changeValue = 0;
      this.paidValue = undefined;
    }

    closeKeyPads(){
      this.createModal = false;
      this.editModal = false;
      this.deleteModal = false;
      this.trolleyModal = false;
      this.payModal = false;
      this.rateModal = false;
      this.quantityModal = false;
      this.currentValue = '';
      this.currentValue2 = '';
      this.paidValue = undefined;
      this.changeValue = 0;
      this.confirm.quantity = undefined;
    }
  
    hideDialog(){
      this.rowIndex = 0;
      this.currency = Currency.USD;
      this.supervisorCode = '';
      this.isRemoved = false;
      this.isReturn = false;
      this.returnModal = false;
      this.createModal = false;
      this.editModal = false;
      this.deleteModal = false;
      this.trolleyModal = false;
      this.payModal = false;
      this.rateModal = false;
      this.quantityModal = false;
      this.isQuotation = false;
      this.currentValue = '';
      this.currentValue2 = '';
      this.paidValue = undefined;
      this.selectedOrderItems = [];
      this.orderItems = [];
      this.value = 0;
      this.changeValue = 0;
      this.confirm.quantity = undefined;
      this.newOder = {
        id: 0,
        saleType: SaleType.Cash,
        price: 0,
        vat: 0,
        priceIncludingVat: 0,
        balance: 0,
        paidAmount: 0,
        currency: Currency.USD,
        isPaid: false,
        customerId: 0,
        customer: {
          id: 0,
        } as CompanyDto,
      } as SalesOrderDto;

      //reset barcode components
      this.barcodeComponents = {
        flag: '',
        plu: '',
        weight: 0
      }

      this.priceIncludingVat = 0;
      this.price = 0;
      this.vat = 0;
    }
  
    confirmDelete(){
      console.log(this.newOder);
      this.productService.delete(this.newOder.id!)
      .subscribe((res) => {
        console.log(res);
        this.hideDialog();
      });
    }
  
    update(){
      this.submitted = true;
  
      console.log(this.newOder)
      let pay = 0;

      this.newOder.salesOrderItems?.filter(x => x.quantity! < 0).forEach(item => {
        pay += (item.price! + item.vat!);
      });

      //this.payment.salesOrderId = this.newOder.id;
      this.payment.creationTime = new Date();
      this.payment.creatorId = this.user.id;
      this.payment.lastModifierUserId = this.user.id;
      this.payment.lastModificationTime = new Date();
      this.payment.methodOfPay = this.newOder.saleType;
      this.payment.totalPrice = Number(this.newOder.priceIncludingVat?.toFixed(2));
      this.payment.paidAmount = Number(pay!.toFixed(2));
      this.payment.uSDPaidAmount = this.newOder.currency === Currency.USD ? Number(pay?.toFixed(2)) : Number((pay!/this.rate.exchangeRate?.baseToRate!).toFixed(2));
      this.payment.paidAmountAfterChange = this.changeValue > 0 ? Number((pay! - this.changeValue).toFixed(2)) : Number(pay!.toFixed(2));
      this.payment.uSDPaidAmountAfterChange = this.changeValue > 0 ? Number((this.payment.uSDPaidAmount! - (this.newOder.currency === Currency.USD ? this.changeValue : this.changeValue/this.rate.exchangeRate?.baseToRate!)).toFixed(2)) : Number(this.payment.uSDPaidAmount.toFixed(2));
      this.payment.changeAmount = Number(this.changeValue.toFixed(2));
      this.payment.amount = Number(this.newOder.priceIncludingVat?.toFixed(2));
      this.payment.currency = this.newOder.currency;
      this.payment.orderDate = this.newOder.creationTime;
      this.payment.exchangeRate = this.newOder.currency === Currency.USD ? 1 : this.rate.exchangeRate?.baseToRate!

      if(!Array.isArray(this.newOder.payments)){
        this.newOder.payments = []
      }
      this.newOder.payments.push(this.payment);
  
      this.newOder.lastModificationTime = new Date();
      this.newOder.lastModifierUserId = this.user.id;

      this.salesOrderService.update(this.newOder)
      .subscribe((res) => {
        console.log(res.data);

        if(res.isSuccess){
          this.messageService.add({
            severity:'success', 
            summary: 'Success', 
            detail: res.message, 
            life: 3000
          });

          this.printReceipt();
        }
        else{
          this.messageService.add({
            severity:'error', 
            summary: 'Success', 
            detail: res.message, 
            life: 3000
          });
        }
        this.hideDialog();
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
  
    
    onFilter(dv: DataView, event: Event) {
        dv.filter((event.target as HTMLInputElement).value);
    }

    onGlobalFilter(table: Table, event: Event) {
      table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }
}
