import { Component, OnInit } from '@angular/core';
import jspdf from 'jspdf';
import autoTable from 'jspdf-autotable';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { Country } from 'src/proxy/enums/country';
import { OrderReceivedStatus } from 'src/proxy/enums/order-received-status';
import { PoFilterOptions } from 'src/proxy/enums/po-filter-options';
import { Status } from 'src/proxy/enums/status';
import { Unit } from 'src/proxy/enums/unit';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { PurchaceOrderDto } from 'src/proxy/interfaces/purchace-order-dto';
import { PurchaceOrderLineDto } from 'src/proxy/interfaces/purchace-order-line-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { CompanyService } from 'src/proxy/services/company.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { PurchaceOrderService } from 'src/proxy/services/purchace-order.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';

@Component({
  selector: 'app-purchase-order',
  templateUrl: './purchase-order.component.html',
  styleUrls: ['./purchase-order.component.scss']
})
export class PurchaseOrderComponent implements OnInit {

  orders: PurchaceOrderDto[] = [];

  filteredOrders: PurchaceOrderDto[] = [];

  companies: CompanyDto[] = [];

  stockItems: ProductInventoryDto[] = [];

  filteredStockItems: ProductInventoryDto[] = [];

  newOrder: PurchaceOrderDto = {} as PurchaceOrderDto;

  selectedOrder: PurchaceOrderDto = {} as PurchaceOrderDto;

  selectedItems: any[] = [];

  purchaceOrderItems: PurchaceOrderLineDto[] = [];

  purchaceOrderItem: PurchaceOrderLineDto = {} as PurchaceOrderLineDto;

  selectedOrderItem: PurchaceOrderLineDto = {} as PurchaceOrderLineDto;

  selectedStockItem: ProductInventoryDto = {} as ProductInventoryDto;

  currentUser: UserDto = { } as UserDto

  subCategories: SubCategoryDto[] = [];

  filteredCategories: SubCategoryDto[] = [];

  company: CompanyDto = {} as CompanyDto;

  date = "";

  start = "";

  end = "";

  supplier: CompanyDto = {} as CompanyDto;

  month = new Date();

  mainFilters : PoFilterOptions[] = Object.values(PoFilterOptions);

  selectedMainFilter: PoFilterOptions = PoFilterOptions.TODAY;

  statusFilter: OrderReceivedStatus[] = Object.values(OrderReceivedStatus);

  status!: OrderReceivedStatus;

  rangeDates: Date[] | undefined;

  quantity = 0;

  value = 0;

  orderValue = 0;

  totalPrice = 0;

  createOrderModal = false;

  addItemsModal = false;

  quantityModal = false;

  warningModal = false;

  editModal = false;

  approveModal = false;

  deleteModal = false;

  trolleyModal = false;

  submitted = false;

  statuses!: Status[];

  units!: Unit[];

  countries!: Country[];

  categories: Category[] = [];

  country!: Country;

  cols: any[] = [];


  constructor(
    private purchacingService: PurchaceOrderService,
    private messageService: MessageService,
    private companyService: CompanyService,
    private productService: ProductInventoryService,
    private categoryService: SubCategoryService
  ){}

  ngOnInit(): void {
    this.currentUser = JSON.parse(sessionStorage.getItem('loggedUser') || '{}')
    console.log(this.currentUser)
    
    this.purchacingService.getAllToday()
    .subscribe(res => {
      // this.orders = res.data.filter((x: any) => x.isApproved === false);
      this.orders = res.data
      this.filteredOrders = this.orders;
      console.log(this.orders);
    });

    this.companyService.getAllList()
    .subscribe((res) => {
      this.companies = res.data;
      console.log(this.companies);
    });

    this.productService.getAllList()
    .subscribe((res) => {
      this.stockItems = res.data;
      console.log(this.stockItems);
    });

    this.categoryService.getAllList()
    .subscribe(res => {
      console.log(res);
      this.subCategories = res.data;
    });

    this.statuses = Object.values(Status);
    this.units = Object.values(Unit);
    this.countries = Object.values(Country);
    this.categories = Object.values(Category);
  }

  onMainFilter(){
    if(this.selectedMainFilter === PoFilterOptions.TODAY){
      this.onGetToday();
    }
  }

  // get all purchace orders by today
  onGetToday(){
    this.purchacingService.getAllToday()
    .subscribe((res) => {
      this.orders = res.data;
      this.filteredOrders = res.data;
      console.log(res);
    });
  }

  // get all purchace orders by this date
  onGetByDate(){
    this.date = new Date(new Date(this.date).getTime() + (2 * 60 * 60 * 1000)).toISOString();
    this.purchacingService.getAllByDate(this.date)
    .subscribe((res) => {
      this.orders = res.data;
      this.filteredOrders = res.data;
      console.log(res);
    });
  }

  // get all purchace orders by this month
  onGetByMonth(){
    let mon = this.month.getMonth() + 1
    this.purchacingService.getAllByMonth(mon)
    .subscribe((res) => {
      this.orders = res.data;
      this.filteredOrders = res.data;
      console.log(res);
    });
  }

  // get all purchace orders by this date range
  onGetByDateRange(){
    if(this.rangeDates![1]){
      this.start = new Date(new Date(this.rangeDates![0]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      this.end = new Date(new Date(this.rangeDates![1]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      
      this.purchacingService.getAllByDateRange(this.start, this.end)
      .subscribe((res) => {
        this.orders = res.data;
        this.filteredOrders = res.data;
        console.log(res);
      });
    }
  }

  // get all purchace orders by this supplier
  onGetBySupplier(){
    this.purchacingService.getAllBySupplier(this.supplier.id!)
    .subscribe((res) => {
      this.orders = res.data;
      this.filteredOrders = res.data;
      console.log(res);
    });
  }

  // filter all purchace orders by this status
  onFilterByStatus(){
    if(this.status === OrderReceivedStatus.All){
      this.filteredOrders = this.orders;
    }
    else{
      this.filteredOrders = this.orders.filter(x => x.status === this.status);
    }
  }


  printOrder(order: PurchaceOrderDto){
    let imgSrc = 'image-url';
    let pdf = new jspdf('p', 'mm', 'a4');

    //shearwate logo
    pdf.setFontSize(8);
    pdf.addImage('assets/demo/images/logos/FARM-LOGO.png\n', 'PNG', 5, 5, 25, 15);
    
    //shearwater address
    pdf.text('Andile Fresh Farm Produce | Post Office Box 000'
      +'\n0 Magwaza Complex | Mkhosana Main\nVictoria Falls | Zimbabwe\n+263 000 0000 00 | example@email.com'
      +'\nVat No 0000000 | BP No 0000000\nwww.andilefreshfarm.com', 5, 25);

    pdf.setFontSize(12);
    pdf.text( "PURCHACE ORDER", 100, 45, { align: 'center' });

    pdf.setFontSize(8);
    pdf.text('Order Date:            ' + new Date(order.creationTime!).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) 
        +  '\n Order No:               ' + order.id 
        +  '\n Suplier Name:         ' + order.supplierName 
        +  '\n Order Number:  ' + order.id, 5, 50);

    // pdf.text(order.supplierName 
    //     +  '\n' + order.sup!.address!.street + ' ' + order.customer!.address!.addressLine1 
    //     +  '\n' + order.customer!.address!.city + ' ' + order.customer!.address!.country 
    //     +  '\n' + order.customer!.vatNumber, 205, 50 , { align: 'right'});

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
          'Total'
        ]
      ],
      body: order.purchaceOrderItems!.map(x => 
        [
          // new Date(x.dateOfActivity).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric'}), 
          x.id!,
          x.name!, 
          x.unitPrice!.toFixed(2), 
          x.quantity!,
          x.unit!,
          (x.price!).toFixed(2)
        ],  
      ),
      foot: [
        [ '', '', '', '', 'Sub Total', order.invoiceValue!.toFixed(2)],
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

    // const addFooter = (pdfInstance: jspdf) => {
    //   pdfInstance.setFont('Helvetica', 'normal');
    //   pdfInstance.setFontSize(8);
    //   pdfInstance.line(5, 260, 205, 260, 'F');
    //   pdfInstance.text('Notes: All bank charges including intermediary bank charges are the responsibility of the client.' 
    //     +' The correct and full amount as invoiced must be received into our account. ' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })
    //     +'\n\nAcc Name:      Shearwater Adventures (Pvt) Ltd\nBank Name:    First Capital Bank Limited\nBranch:           Victoria Falls'
    //     +'\nBranch Code:  2157\nSwift Code:     BARCZWHX\nAccount No:    2157-3780841', 5, 265, { align: 'left', maxWidth: 205 });
    // }

    // addFooter(pdf);
    pdf.save("PURCHACE ORDER " + order.id + '-' + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
  }

  filterCategories(event: any){
    var department = event.value;
    this.filteredCategories = this.subCategories.filter(x => x.category === department);
  }

  filterItems(event: any){
    var category =  event.value;
    this.filteredStockItems = this.stockItems.filter(x => x.category === category)
  }

  process(){
    this.newOrder.invoiceValue = 0;
    this.newOrder.purchaceOrderItems?.forEach(item => {
      item.price = item.quantity! * item.unitPrice!;
      this.newOrder.invoiceValue! += item.price;
    })
  }

  confirmQuantity(){
    console.log(this.selectedStockItem)
    this.quantityModal = true

  }

  addItem(event: any){
    this.quantityModal = false;
    console.log(this.quantity)

    if(this.quantity > 0){

      this.selectedOrderItem = {
        id: 0,
        productInventoryId: this.selectedStockItem.id,
        quantity: this.quantity,
        unit: this.selectedStockItem.unit,
        name: this.selectedStockItem.name,
        unitPrice: this.selectedStockItem.cost,
        price: this.quantity*this.selectedStockItem.productPrice?.cost!,
      };

      this.newOrder.invoiceValue! += this.selectedOrderItem.price!;
      console.log(this.selectedOrderItem)
      console.log(this.newOrder.invoiceValue)

      if(!Array.isArray(this.newOrder.purchaceOrderItems)){
        this.newOrder.purchaceOrderItems = [];
        this.newOrder.purchaceOrderItems = [...this.newOrder.purchaceOrderItems!, this.selectedOrderItem];
      }
      else{
        this.newOrder.purchaceOrderItems = [...this.newOrder.purchaceOrderItems!, this.selectedOrderItem];
      }
      // this.newOrder.purchaceOrderItems = [...this.newOrder.purchaceOrderItems!, this.selectedOrderItem]
      console.log(this.newOrder.purchaceOrderItems)
    }
    else{
      this.warningModal = true;
      this.quantity = 0;
      this.totalPrice = 0;
      this.selectedStockItem = {} as any;
      console.log("add quantity");
    }
  }


  removeItem(item: any){ 
    console.log(item)
    var index = this.newOrder.purchaceOrderItems!.findIndex(x => x.productInventoryId === item.productInventoryId);
    this.purchaceOrderItems.splice(index, 1);
  }

  changeQuantity(event: any){
    console.log("quantity changed")
  }

  create(){
    this.submitted = false;

    this.createOrderModal = true;
  }

  delete(order: any){

    this.newOrder = {...order}
    this.deleteModal = true;
    this.submitted = false;
  }

  confirmDelete(){
    this.newOrder.isDeleted = true;
    this.purchacingService.update(this.newOrder)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
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

    var index = this.orders.findIndex(x => x.id === this.newOrder.id);
    this.orders.splice(index, 1)
    this.deleteModal = false;
  }

  edit(order: any){
    this.newOrder = {...order}
    this.newOrder.eta = new Date(order.eta!);
    this.newOrder.creationTime = new Date(order.creationTime!);
    console.log(this.newOrder)
    this.editModal = true;
    this.submitted = false;
  }

  approve(order: any){
    order.approvedById = this.currentUser.id;
    this.newOrder = {...order}

    console.log(this.newOrder)
    this.approveModal = true;
  }

  save(){
    console.log(this.company)
    
    this.newOrder.supplierId = this.company.id;
    this.newOrder.creationTime = new Date();
    this.newOrder.creatorId = this.currentUser.id;
    this.newOrder.lastModificationTime = new Date();
    this.newOrder.lastModifierUserId = this.currentUser.id;
    this.newOrder.company = undefined;
    this.newOrder.isOpen = true;
    this.newOrder.eta = new Date(new Date(this.newOrder.eta).getTime() + (2 * 60 * 60 * 1000));

    console.log(this.newOrder)
    this.purchacingService.create(this.newOrder)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        
        if(!Array.isArray(this.orders)){
          this.orders = []
          this.orders = [...this.orders, res!.data];
        }
        else{
          this.orders = [...this.orders, res!.data];
        }
        this.filteredOrders = this.orders;
        
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

  update(){
    this.newOrder.isApproved = this.newOrder.approvedById! > 0 ? true : false
    this.newOrder.supplierId = this.newOrder.company?.id;
    console.log(this.newOrder)
    this.purchacingService.update(this.newOrder)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if (!Array.isArray(this.orders)) {
          this.orders = [];
        }
        const updatedItem = res.data; // The updated item
        this.orders = this.orders.map(item => item.id === updatedItem.id ? updatedItem : item);
        this.filteredOrders = this.orders;

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
    this.createOrderModal = false;
    this.editModal = false;
    this.submitted = false;
    this.deleteModal = false
    this.approveModal = false;

    this.quantity = 0;
    this.totalPrice = 0;
    this.selectedStockItem = {} as any;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
