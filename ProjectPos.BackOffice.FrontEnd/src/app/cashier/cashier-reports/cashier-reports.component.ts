import { Component, OnInit } from '@angular/core';
import html2canvas from 'html2canvas';
import jspdf from 'jspdf';
import autoTable from 'jspdf-autotable';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { CashUpOptions } from 'src/proxy/enums/cash-up-options';
import { InvFilter } from 'src/proxy/enums/inv-filter';
import { ReconFilter } from 'src/proxy/enums/recon-filter';
import { SnapShotEnum } from 'src/proxy/enums/snap-shot-enum';
import { CashUpDto } from 'src/proxy/interfaces/cash-up-dto';
import { CashUpReconDto } from 'src/proxy/interfaces/cash-up-recon-dto';
import { GetCashUpListDto } from 'src/proxy/interfaces/get-cash-up-list-dto';
import { InventorySnapShotSummaryDto } from 'src/proxy/interfaces/inventory-snap-shot-summary-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { ProductInventorySnapshotDto } from 'src/proxy/interfaces/product-inventory-snapshot-dto';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { CashUpService } from 'src/proxy/services/cash-up.service';
import { ProductInventorySnapShotService } from 'src/proxy/services/product-inventory-snap-shot.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';

@Component({
  selector: 'app-cashier-reports',
  templateUrl: './cashier-reports.component.html',
  styleUrls: ['./cashier-reports.component.scss']
})
export class CashierReportsComponent implements OnInit {

  cashUprecons: CashUpReconDto[] = [];

  users: UserDto[] = [];

  cashUpModal: boolean = false;

  filterOptions: ReconFilter[] = Object.values(ReconFilter);

  filter: ReconFilter = ReconFilter.ALL;

  currencyCountCash: number = 0;

  currencyCountEcoCash: number = 0;

  currencyCountCreditCard: number = 0;

  currencyCountCredit: number = 0;

  isCreditCard: boolean = false;

  isCash: boolean = false;

  isEcoCash: boolean = false;

  isCredit: boolean = false;

  cashCurrencies: any[] = [];

  ecoCashCurrencies: any[] = [];

  creditCardCurrencies: any[] = [];

  creditCurrencies: any[] = [];

  display='';

  products: SalesOrderItemDto[] = [];

  cols: any[] = [];

  userCashUpModal: boolean = false;

  user: UserDto = {} as UserDto;

  date!: Date;

  teller: UserDto = {} as UserDto;

  snapShot: ProductInventorySnapshotDto = {} as ProductInventorySnapshotDto;

  createModal = false;

  editModal = false;

  dissabled = true;

  confirmModal = false;

  submitted = false;

  todayDate = new Date();

  options: CashUpOptions[] = [];

  option = CashUpOptions.Option;
  
  inventory?: ProductInventoryDto[] = [];

  message = '';

  totalCount = 0;

  totalSales = 0;

  fruits: SalesOrderItemDto[] = [];

    beverages: SalesOrderItemDto[] = [];

    spices: SalesOrderItemDto[] = [];

    sweets: SalesOrderItemDto[] = [];

    vegetables: SalesOrderItemDto[] = [];

    cereals: SalesOrderItemDto[] = [];

    unallocated :  SalesOrderItemDto[] = [];

    categories: { name: string, items: SalesOrderItemDto[] }[] = [];

  constructor(
    private cashUpService: CashUpService,
    private authService: AuthService,
    private salesOrderService: SalesOrderService,
    private snapShotService: ProductInventorySnapShotService,
    private inventoryService: ProductInventoryService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    this.todayDate = new Date();
    var dateString: string = this.todayDate.toISOString();
    console.log(dateString)

    this.cashUpService.getAllRecons(dateString)
    .subscribe(res => {
      this.message = res.message;
      console.log(this.message)
    });

    
    this.inventoryService.getAllList()
    .subscribe(res => {
      this.inventory = res.data;
      console.log(this.inventory)
    })

    this.authService.getAllUsers()
    .subscribe(res => {
      this.users = res.data;
    })

    this.salesOrderService.getTodaySales(0)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })
    })
  }

  categorizeItems() {
    // const categories = ['Fruits', 'Vegetables', 'Beverage', 'Spices', 'Sweets', 'Cereals', 'Unallocated'];
    // this.fruits = this.products.filter(x => x.subCategory === categories[0]);
    // this.vegetables = this.products.filter(x => x.subCategory === categories[1]);
    // this.beverages = this.products.filter(x => x.subCategory === categories[2]);
    // this.spices = this.products.filter(x => x.subCategory === categories[3]);
    // this.sweets = this.products.filter(x => x.subCategory === categories[4]);
    // this.cereals = this.products.filter(x => x.subCategory === categories[5]);
    // this.unallocated = this.products.filter(x => x.subCategory === categories[6]);

    const categoryMap = new Map<string, any[]>();

    // Group products by subCategory dynamically
    this.products.forEach(product => {
        if (!categoryMap.has(product.subCategory!)) {
            categoryMap.set(product.subCategory!, []);
        }
        categoryMap.get(product.subCategory!)!.push(product);
    });

    // Assign categorized items to properties dynamically
    this.categories = Array.from(categoryMap.keys()).map(category => ({
        name: category,
        items: categoryMap.get(category) || [],
    }));

    this.onPrintSales();
  }

  onPrintSales() {
    const date = new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toUTCString();
      let offset2 = 10.5;
      let totalLength = (this.products.length);
      let paperL = totalLength * offset2;
      let pdf = new jspdf('p', 'mm', 'a4');

      // Logo and address
      pdf.setFontSize(10);
      pdf.addImage('assets/demo/images/logos/FARM-LOGO.png\n', 'PNG', 5, 5, 25, 15);
      pdf.text(`Andile Fresh Farm Produce | Total Sales: ${date}`, 5, 25);

      pdf.line(5, 30, 205, 30, 'F');

      // Consolidating all items into one array
      // 

      let tableData: any[] = [];
      
      this.categories.forEach(category => {
        if (category.items.length > 0) {
            // Category header
            tableData.push([{ content: category.name, colSpan: 4, styles: { halign: 'center', fontStyle: 'bold' } }]);

            // Items in the category
            category.items.forEach(item => {
                tableData.push([
                    item.productName!,
                    item.unitPrice!.toFixed(2),
                    item.quantity!,
                    item.price!.toFixed(2),
                ]);
            });

            // Category totals
            tableData.push([
                'Totals',
                '',
                category.items.reduce((acc, item) => acc + item.quantity!, 0).toFixed(2),
                '$' + category.items.reduce((acc, item) => acc + item.price!, 0).toFixed(2),
            ]);
        }
    });

      // groupedData.forEach(group => {
      //     if (group.items.length > 0) {
      //         // Category header
      //         tableData.push([{ content: group.category, colSpan: 4, styles: { halign: 'center', fontStyle: 'bold' } }]);

      //         // Items in the category
      //         group.items.forEach(item => {
      //             tableData.push([
      //                 item.productName!,
      //                 item.unitPrice!.toFixed(2),
      //                 item.quantity!,
      //                 item.price!.toFixed(2)
      //             ]);
      //         });

      //         // Category totals
      //         tableData.push([
      //             'Totals',
      //             '',
      //             group.items.reduce((accumulator: number, currentItem: SalesOrderItemDto) => accumulator + currentItem.quantity!, 0).toFixed(2),
      //             '$' + group.items.reduce((accumulator: number, currentItem: SalesOrderItemDto) => accumulator + currentItem.price!, 0).toFixed(2)
      //         ]);
      //     }
      // });

      // Render table
      autoTable(pdf, {
          headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
          head: [['Item', 'Unit $', 'Qty', 'Total']],
          body: tableData,
          startY: 35,
          margin: 5,
          theme: 'striped',
          styles: {
              fontSize: 8,
              cellPadding: 2,
              overflow: 'linebreak',
          },
          columnStyles: {
              0: { halign: 'left' },
              1: { halign: 'right', cellWidth: 15 },
              2: { halign: 'right', cellWidth: 20 },
              3: { halign: 'right', cellWidth: 15 },
          },
      });

      pdf.autoPrint();
      pdf.save(`ToTal Sales: ${date}.pdf`); 
  }

  public captureReportModal() {  
    const date = new Date(new Date(this.date).getTime() + (2 * 60 * 60 * 1000)).toUTCString();
    let pdf = new jspdf('l', 'mm', 'a4');
    const data1 = document.getElementById('pdf-section-1');  
  
    const addFooter = (pdfInstance: jspdf) => {
        pdfInstance.setFont('Helvetica', 'normal');
        pdfInstance.setFontSize(10);
        pdfInstance.text('Cash Up: ' + date , 60, 350);
    }
    
    html2canvas(data1!).then(canvas => {  
      let imgWidth = 250;   
      let pageHeight = 300;    
      let imgHeight = canvas.height * imgWidth / canvas.width;  
      let heightLeft = imgHeight;  
  
      const contentDataURL = canvas.toDataURL('image/png')  
        
      let position = 0;  
      pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight)  
  
      addFooter(pdf);
      pdf.save(`cashup: ${date}.pdf`);                                                                                                                                                                                                                                       
    });


  }
  
  getRecons() {
    this.isCash = false;
    this.isCreditCard = false;
    this.isEcoCash = false;
    this.cashCurrencies = [];
    this.creditCardCurrencies = [];
    this.ecoCashCurrencies = [];
    this.currencyCountCash = 0;
    this.currencyCountCreditCard = 0;
    this.currencyCountEcoCash = 0;

    const date = new Date(new Date(this.date).getTime() + (2 * 60 * 60 * 1000)).toUTCString();
    this.cashUpService.getAllCashUpRecons(date)
    .subscribe(res => {
      this.cashUprecons = res.data;
      this.cashUprecons.forEach(cashUp => {
        if(this.isCash === false){
          if(cashUp.cashReport!.length !== 0){
            this.isCash = true;
            let count = cashUp.cashReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.cashCurrencies.includes(cashUp.cashReport![i].currency);
              if(isExist === false){
                this.cashCurrencies.push(cashUp.cashReport![i].currency);
              }
            }
            if(this.currencyCountCash < count){
              this.currencyCountCash = count;
            }
          }
        }
        else{
          if(cashUp.cashReport!.length !== 0){
            this.isCash = true;
            let count = cashUp.cashReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.cashCurrencies.includes(cashUp.cashReport![i].currency);
              if(isExist === false){
                this.cashCurrencies.push(cashUp.cashReport![i].currency);
              }
            }
            if(this.currencyCountCash < count){
              this.currencyCountCash = count;
            }
          }
        }

        if(this.isCreditCard === false){
          if(cashUp.creditCardReport!.length !== 0){
            this.isCreditCard = true;
            let count = cashUp.creditCardReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.creditCardCurrencies.includes(cashUp.creditCardReport![i].currency);
              if(isExist === false){
                this.creditCardCurrencies.push(cashUp.creditCardReport![i].currency);
              }
            }
            if(this.currencyCountCreditCard < count){
              this.currencyCountCreditCard = count;
            }
          }
        }
        else{
          if(cashUp.creditCardReport!.length !== 0){
            this.isCreditCard = true;
            let count = cashUp.creditCardReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.creditCardCurrencies.includes(cashUp.creditCardReport![i].currency);
              if(isExist === false){
                this.creditCardCurrencies.push(cashUp.creditCardReport![i].currency);
              }
            }
            if(this.currencyCountCreditCard < count){
              this.currencyCountCreditCard = count;
            }
          }
        }

        if(this.isEcoCash === false){
          if(!Array.isArray(cashUp.ecoCashReport)){
            cashUp.ecoCashReport = []
          }
          if(cashUp.ecoCashReport!.length !== 0){
            this.isEcoCash = true;
            let count = cashUp.ecoCashReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.ecoCashCurrencies.includes(cashUp.ecoCashReport![i].currency);
              if(isExist === false){
                this.ecoCashCurrencies.push(cashUp.ecoCashReport![i].currency);
              }
            }
            if(this.currencyCountEcoCash < count){
              this.currencyCountEcoCash = count;
            }
          }
        }
        else{
          if(cashUp.ecoCashReport!.length !== 0){
            this.isEcoCash = true;
            let count = cashUp.ecoCashReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.ecoCashCurrencies.includes(cashUp.ecoCashReport![i].currency);
              if(isExist === false){
                this.ecoCashCurrencies.push(cashUp.ecoCashReport![i].currency);
              }
            }
            if(this.currencyCountEcoCash < count){
              this.currencyCountEcoCash = count;
            }
          }
        }

        if(this.isCredit === false){
          if(!Array.isArray(cashUp.creditReport)){
            cashUp.creditReport = []
          }
          if(cashUp.creditReport!.length !== 0){
            this.isCredit = true;
            let count = cashUp.creditReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.creditCurrencies.includes(cashUp.creditReport![i].currency);
              if(isExist === false){
                this.creditCurrencies.push(cashUp.creditReport![i].currency);
              }
            }
            if(this.currencyCountCredit < count){
              this.currencyCountCredit = count;
            }
          }
        }
        else{
          if(cashUp.creditReport!.length !== 0){
            this.isCredit = true;
            let count = cashUp.creditReport!.length;
            for(let i = 0; i < count; i++){
              let isExist = this.creditCurrencies.includes(cashUp.creditReport![i].currency);
              if(isExist === false){
                this.creditCurrencies.push(cashUp.creditReport![i].currency);
              }
            }
            if(this.currencyCountCredit < count){
              this.currencyCountCredit = count;
            }
          }
        }

        
        
      })

      this.currencyCountCash = this.cashCurrencies.length;
      this.currencyCountCreditCard = this.creditCardCurrencies.length;
      this.currencyCountEcoCash = this.ecoCashCurrencies.length;
      this.currencyCountCredit = this.creditCurrencies.length;

      this.cashUprecons.forEach(cashUp => {

        this.cashCurrencies.forEach(currency => {
          const exist = cashUp.cashReport!.some(c => c.currency === currency);
          if(exist === false){
            let index = this.cashCurrencies!.indexOf(currency);
            cashUp.cashReport!.splice(index, 0, {
              currency: currency,
              amount: 0
            });
          }
        });

        this.creditCardCurrencies.forEach(currency => {
          const exist = cashUp.creditCardReport!.some(c => c.currency === currency);
          if(exist === false){
            let index = this.creditCardCurrencies!.indexOf(currency);
            cashUp.creditCardReport!.splice(index, 0, {
              currency: currency,
              amount: 0
            });
          }
        });

        this.ecoCashCurrencies.forEach(currency => {
          const exist = cashUp.ecoCashReport!.some(c => c.currency === currency);
          if(exist === false){
            let index = this.ecoCashCurrencies!.indexOf(currency);
            cashUp.ecoCashReport!.splice(index, 0, {
              currency: currency,
              amount: 0
            });
          }
        });

        this.creditCurrencies.forEach(currency => {
          const exist = cashUp.creditReport!.some(c => c.currency === currency);
          if(exist === false){
            let index = this.creditCurrencies!.indexOf(currency);
            cashUp.creditReport!.splice(index, 0, {
              currency: currency,
              amount: 0
            });
          }
        });
      })
      console.log(this.currencyCountCash)
      console.log(this.cashCurrencies)
      console.log(this.creditCardCurrencies)
      console.log(this.ecoCashCurrencies)

      
      
    })
    this.cashUpModal = true;
  }

  confirmCloseDay(){
    
    this.confirmModal = true;
  }

  closeDay(){

    let snapshotSummary = {
      userId: this.user.id,
      snapShotType: SnapShotEnum.CloseDay
    } as InventorySnapShotSummaryDto;

    this.snapShotService.create(snapshotSummary)
    .subscribe(res => {
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

    this.confirmModal = false;
  }

  getReconByUser() {
    const date = new Date(this.date.getTime() + (2 * 60 * 60 * 1000)).toISOString();
    this.cashUpService.getCashUpReconById(this.user.id, date)
    .subscribe(res => {
      if(!Array.isArray(this.cashUprecons)){
        this.cashUprecons = []
      }
      this.cashUprecons = [...this.cashUprecons, res.data];
      console.log(this.cashUprecons)
    })
  }

  load(){
    if(this.filter === ReconFilter.ALL){
      this.getRecons();
    }
    else{
      this.getReconByUser();
      this.userCashUpModal = true;
    }
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
