import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Country } from 'src/proxy/enums/country';
import { Currency } from 'src/proxy/enums/currency';
import { OrderPayStatus } from 'src/proxy/enums/order-pay-status';
import { OrderPaymentStatus } from 'src/proxy/enums/order-payment-status';
import { PoFilterOptions } from 'src/proxy/enums/po-filter-options';
import { SaleType } from 'src/proxy/enums/sale-type.enum';
import { Status } from 'src/proxy/enums/status';
import { Unit } from 'src/proxy/enums/unit';
import { AddressDto } from 'src/proxy/interfaces/address-dto';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { GetExchangeRateDto } from 'src/proxy/interfaces/get-exchange-rate-dto';
import { GoodsReceivedVoucherDto } from 'src/proxy/interfaces/goods-received-voucher-dto';
import { GoodsReceivedVoucherLineDto } from 'src/proxy/interfaces/goods-received-voucher-line-dto';
import { PurchaceOrderDto } from 'src/proxy/interfaces/purchace-order-dto';
import { PurchaceOrderLineDto } from 'src/proxy/interfaces/purchace-order-line-dto';
import { PurchaceOrderPaymentDto } from 'src/proxy/interfaces/purchace-order-payment-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { CompanyService } from 'src/proxy/services/company.service';
import { ExchangeRateService } from 'src/proxy/services/exchange-rate.service';
import { GoodsReceivedVoucherService } from 'src/proxy/services/goods-received-voucher.service';
import { PurchaceOrderService } from 'src/proxy/services/purchace-order.service';

@Component({
  selector: 'app-goods-received-voucher',
  templateUrl: './goods-received-voucher.component.html',
  styleUrls: ['./goods-received-voucher.component.scss']
})
export class GoodsReceivedVoucherComponent implements OnInit {

  grvs: GoodsReceivedVoucherDto[] = [];

  filteredGrvs: GoodsReceivedVoucherDto[] = [];

  grvItems: GoodsReceivedVoucherDto[] = [];

  selectedGrvItems: GoodsReceivedVoucherLineDto[] = [];
  
  companies: CompanyDto[] = []

  orders: PurchaceOrderDto[] = [];

  orders1: PurchaceOrderDto[] = [];

  selectedOrder: PurchaceOrderDto = {} as PurchaceOrderDto;

  selectedOrder1: PurchaceOrderDto = {} as PurchaceOrderDto;

  selectedOrderItems: PurchaceOrderLineDto[] = [];

  newGrv: GoodsReceivedVoucherDto = {} as GoodsReceivedVoucherDto;

  selectedGrv: GoodsReceivedVoucherDto = {} as GoodsReceivedVoucherDto;

  selectedGrvItem: GoodsReceivedVoucherLineDto = {} as GoodsReceivedVoucherLineDto;

  currentUser: UserDto = {} as UserDto;

  confirm: any = {
    orderNumber: 0
  };

  supplier: CompanyDto = {} as CompanyDto;

  mainFilters: PoFilterOptions[] = Object.values(PoFilterOptions);

  selectedMainFilter: PoFilterOptions = PoFilterOptions.TODAY;

  statusFilter: OrderPaymentStatus[] = Object.values(OrderPaymentStatus);

  status!: OrderPaymentStatus;

  month = new Date();

  value = 0;

  orderValue = 0;

  orderNumber = 0;

  totalPrice = 0;

  createModal = false;

  addItemsModal = false;

  quantityModal = false;

  editModal = false;

  approveModal = false;

  deleteModal = false;

  trolleyModal = false;

  totalOrderAmount = 0;

  submitted = false;

  payModal = false;

  statuses!: Status[];

  date = "";

  start = "";

  end = "";

  payment: PurchaceOrderPaymentDto = {} as PurchaceOrderPaymentDto;

  units!: Unit[];

  countries!: Country[];

  country!: Country;

  cols: any[] = [];

  paymentTypes: SaleType[] = Object.values(SaleType);

  currencies: Currency [] = Object.values(Currency);

  selectedCurrency: Currency = Currency.USD;

  rates: GetExchangeRateDto[] = [];

  
  rangeDates: Date[] | undefined;

  amountDueCurrency = 0;

  constructor(
    private purchacigService: PurchaceOrderService,
    private messageService: MessageService,
    private companyService: CompanyService,
    private grvService: GoodsReceivedVoucherService,
    private ratesService: ExchangeRateService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    this.grvService.getAllToday()
    .subscribe((res) => {
      this.grvs = res.data;
      this.filteredGrvs = res.data;
      console.log(res);
    });

    this.ratesService.getAllList()
        .subscribe(res => {
          this.rates = res.data;
          console.log('rates',res.data)
        });

    this.purchacigService.getAllList()
    .subscribe((res) => {
      this.orders1 = res.data;

      this.orders = this.orders1.filter(x => x.isApproved === true && x.isOpen === true);
      console.log(this.orders);
    });

    this.companyService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.companies = res.data;
    });

    this.statuses = Object.values(Status);
    this.units = Object.values(Unit);
    this.countries = Object.values(Country);

    this.currentUser = JSON.parse(sessionStorage.getItem('loggedUser') || '{}'); 

  }

  onMainFilter(){
    if(this.selectedMainFilter === PoFilterOptions.TODAY){
      this.onGetToday();
    }
  }

  // get all goods received vouchers by today
  onGetToday(){
    this.grvService.getAllToday()
    .subscribe((res) => {
      this.grvs = res.data;
      this.filteredGrvs = res.data;
      console.log(res);
    });
  }

  // get all goods received vouchers by date
  onGetByDate(){
    this.date = new Date(new Date(this.date).getTime() + (2 * 60 * 60 * 1000)).toISOString();
    this.grvService.getAllByDate(this.date)
    .subscribe((res) => {
      this.grvs = res.data;
      this.filteredGrvs = res.data;
      console.log(res);
    });
  }

  // get all goods received vouchers by date range
  onGetByDateRange(){
    if(this.rangeDates![1]){
      
    console.log(this.rangeDates)
      this.start = new Date(new Date(this.rangeDates![0]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      this.end = new Date(new Date(this.rangeDates![1]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      this.grvService.getAllByDateRange(this.start, this.end)
      .subscribe((res) => {
        this.grvs = res.data;
        this.filteredGrvs = res.data;
        console.log(res);
      });
    }
  }

  // get all goods received vouchers by month
  onGetByMonth(){
    let mon = this.month.getMonth() + 1;
    this.grvService.getAllByMonth(mon)
    .subscribe((res) => {
      this.grvs = res.data;
      this.filteredGrvs = res.data;
      console.log(res);
    });
  }

  // get all goods received vouchers by supplier id
  onGetBySupplier(){
    this.grvService.getAllBySupplier(this.supplier.id!)
    .subscribe((res) => {
      this.grvs = res.data;
      this.filteredGrvs = res.data;
      console.log(res);
    });
  }

  // filter goods received vouchers by status
  onFilterByStatus(){
    if(this.status === OrderPaymentStatus.All){
      this.filteredGrvs = this.grvs;
    }
    else{
      this.filteredGrvs = this.grvs.filter(x => x.status === this.status);
    }
  }

  onSelectCurrency(){
    let rate = this.rates.find(x => x.currency === this.payment.currency)!;
    
    if(this.payment.currency === Currency.USD){
      this.amountDueCurrency = this.newGrv.amountDue!;
    }
    else if(!rate){
      this.confirmationService.confirm({
        message: "Currency You Selected Has no Cross Rate?",
        header: "WARNING!",
        icon: "pi pi-exclamation-triangle",
        accept: () => {
          this.payment.currency = Currency.USD;
          this.amountDueCurrency = this.newGrv.amountDue!;
        }
      });
    }
    else{
      this.amountDueCurrency = this.newGrv.amountDue! * rate.exchangeRate?.baseToRate!;
    }
  }

  onAddPayment(){
    this.amountDueCurrency = this.newGrv.amountDue!;
    this.payModal = true;
  }

  conformAddPayment(){
    
    let rate = this.rates.find(x => x.currency === this.payment.currency)!;
    this.payModal = false;
    this.payment.orderAmount = this.newGrv.value;
    this.payment.creationTime = new Date(new Date().getTime() + (2 * 60 * 60 * 1000));
    this.payment.creatorId = this.currentUser.id;
    this.payment.lastModificationTime = new Date(new Date().getTime() + (2 * 60 * 60 * 1000));
    this.payment.orderDate = new Date(new Date(this.newGrv.creationTime!).getTime() + (2 * 60 * 60 * 1000));
    this.payment.lastModifierUserId = this.currentUser.id;
    this.payment.paidBy = this.currentUser.fullName;
    this.payment.exchangeRate! = this.payment.currency === Currency.USD ? 1 : rate.exchangeRate?.baseToRate!;
    this.payment.usdPaidAmount = this.payment.currency === Currency.USD ? this.payment.paidAmount! : this.payment.paidAmount! / this.payment.exchangeRate!;

    if(!Array.isArray(this.newGrv.purchaceOrderPayments)){
      this.newGrv.purchaceOrderPayments = [];
    }
    this.newGrv.purchaceOrderPayments = [...this.newGrv.purchaceOrderPayments!, this.payment];
    this.newGrv.usdPaidAmount = this.newGrv.purchaceOrderPayments.reduce((sum, payment) => sum + (payment.usdPaidAmount || 0), 0);
    this.newGrv.paidAmount = this.newGrv.purchaceOrderPayments.reduce((sum, payment) => sum + (payment.paidAmount || 0), 0);
    this.newGrv.amountDue = this.newGrv.value! - this.newGrv.usdPaidAmount;
    this.newGrv.isPaid = this.newGrv.amountDue > 0 ? false : true;

    this.payment = {} as PurchaceOrderPaymentDto;
  }

  process(){
    this.newGrv.value = 0;
    this.newGrv.receivedItems?.forEach(item => {
      item.price = item.receivedQuantity! * item.unitPrice!;
      this.newGrv.value! += item.price;
      this.newGrv.amountDue = this.newGrv.value!;
    })
  }

  createGrv(){

    console.log(this.selectedOrder1)
    this.selectedOrderItems = this.selectedOrder1.purchaceOrderItems!;
    console.log(this.selectedOrder1.purchaceOrderItems)

    if(this.selectedOrder1 === null){
      console.log(this.selectedOrder1)
    }
    else{
      console.log(this.selectedOrder1.purchaceOrderItems)

      this.selectedOrder1.purchaceOrderItems!.forEach(orderItem => {
        this.selectedGrvItem = {
          id: 0,
          voucherNumber: 0,
          productInventoryId: orderItem.productInventoryId,
          productName: orderItem.product!.name,
          barCode: orderItem.product!.barCode,
          orderedQuantity: orderItem.quantity,
          receivedQuantity: 0,
          issuedQuantity: 0,
          isIssued: false,
          unit: orderItem.unit,
          price: orderItem.price,
          orderPrice: orderItem.unitPrice,
          unitPrice: orderItem.unitPrice,
          receivedDate: new Date
        };
  
        console.log(this.selectedGrvItem)
        this.selectedGrvItems = [ ...this.selectedGrvItems, this.selectedGrvItem];
      });
  
      this.newGrv = {
        id: 0,
        orderNumber: this.selectedOrder1.id,
        invoiceNumber: '',
        supplierId: this.selectedOrder1.supplierId,
        supplierName: this.selectedOrder1.supplierName,
        transpoter: '',
        value: 0,
        receivedItems: this.selectedGrvItems,
        amountDue: 0,
        paidAmount: 0,
        usdPaidAmount: 0,
        status: OrderPaymentStatus.NotPaid
      };

      console.log(this.newGrv)
  
      this.quantityModal = false;
      this.addItemsModal = false;
      this.createModal = true;
      this.submitted = false;
    }
    
  }


  addItem(){
    

    this.quantityModal = true;
  }


  onApprove(grv: GoodsReceivedVoucherDto){
    this.newGrv = { ...grv};
    this.approveModal = true;
    this.submitted = false;
  }
  
  edit(grv: GoodsReceivedVoucherDto){
    this.newGrv = { ...grv};
    this.editModal = true;
    this.submitted = false;
  }

  delete(grv: GoodsReceivedVoucherDto){
    this.selectedGrv = { ...grv};
    this.deleteModal = true;
    this.submitted = false;
  }

  deleteSelected(){
  }

  save(){
    this.submitted = true;

    this.newGrv.creatorId = this.currentUser.id;
    this.newGrv.creationTime =  new Date(new Date().getTime() + (2 * 60 * 60 * 1000));
    this.newGrv.lastModificationTime = new Date(new Date().getTime() + (2 * 60 * 60 * 1000))
    this.newGrv.lastModifierUserId = this.currentUser.id;
    
    console.log(this.newGrv);

    this.grvService.create(this.newGrv)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        
        if(!Array.isArray(this.grvs)){
          this.grvs = []
        }
        console.log(res.data)
          this.grvs = [...this.grvs, res.data];
          this.filteredGrvs = this.grvs
        
        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message ,
          life: 3000
        });
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Error', 
          detail: res.message , 
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

    this.hideDialog();

  }

  hideDialog(){
    this.value = 0;
    this.totalPrice = 0;
    this.addItemsModal = false;
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.trolleyModal = false;
    this.approveModal = false;
    
    this.selectedOrderItems = [];

    this.selectedOrder1 = {} as PurchaceOrderDto;

    this.selectedGrvItems = [];
  }

  confirmDelete(){
    this.grvService.delete(this.newGrv.id!)
    .subscribe((res) => {
      console.log(res);
      
    });

    this.deleteModal = false;
  }

  approve(){
    this.newGrv.isApproved = true;
    this.newGrv.invoiceNumber = this.newGrv.invoiceNumber?.toString();

    this.grvService.approve(this.newGrv)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        
        if (!Array.isArray(this.grvs)) {
          this.grvs = [];
        }
        
        const updatedItem = res.data; // The updated item
        this.grvs = this.grvs.map(item => item.id === updatedItem.id ? updatedItem : item);
        this.filteredGrvs = this.grvs
        
        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message ,
          life: 3000
        });
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Error', 
          detail: res.message , 
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

    this.hideDialog()
  }

  update(){

    console.log(this.newGrv);

    this.newGrv.invoiceNumber = this.newGrv.invoiceNumber?.toString();

    this.grvService.update(this.newGrv)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        
        if (!Array.isArray(this.grvs)) {
          this.grvs = [];
        }
        
        const updatedItem = res.data; // The updated item
        this.grvs = this.grvs.map(item => item.id === updatedItem.id ? updatedItem : item);
        this.filteredGrvs = this.grvs
        
        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message ,
          life: 3000
        });
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Error', 
          detail: res.message , 
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

    this.hideDialog()
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
