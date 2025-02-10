import { CashUpService } from './../../proxy/services/cash-up.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { LayoutService } from "./service/app.layout.service";
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { Router } from '@angular/router';
import { Currency } from 'src/proxy/enums/currency';
import { CashUpDto } from 'src/proxy/interfaces/cash-up-dto';
import { ExchangeRateService } from 'src/proxy/services/exchange-rate.service';
import { GetExchangeRateDto } from 'src/proxy/interfaces/get-exchange-rate-dto';
import jspdf, { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';
import { interval } from 'rxjs';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent implements OnInit {

    items!: MenuItem[];

    cashUpForm: CashUpDto = {} as CashUpDto;

    cashUps: CashUpDto[] = [];

    user: UserDto = {} as UserDto;

    menuItems: MenuItem[] = [];

    rates: GetExchangeRateDto[] = [];

    cashUpModal = false;

    currencies: Currency[] = [];

    totalAmount = 0;

    reconItems: SalesOrderItemDto[] = [];

    fruits: SalesOrderItemDto[] = [];

    beverages: SalesOrderItemDto[] = [];

    spices: SalesOrderItemDto[] = [];

    sweets: SalesOrderItemDto[] = [];

    vegetables: SalesOrderItemDto[] = [];

    cereals: SalesOrderItemDto[] = [];

    unallocated :  SalesOrderItemDto[] = [];

    currentDate: Date = new Date();
    
    label: any;

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(public layoutService: LayoutService,
        private authService: AuthService,
        private salesOrderService: SalesOrderService,
        private router: Router,
        private messageService: MessageService,
        private rateService: ExchangeRateService,
        private cashUpService: CashUpService
        ) { }

    ngOnInit(): void {
        this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

        this.rateService.getAllList()
        .subscribe(res => {
            this.rates = res.data;
            console.log(res.data)
        });

        // Get the current date
        this.currentDate = new Date();

        // Update the time every second
        interval(1000).subscribe(() => {
            this.currentDate = new Date();
        });


        console.log(this.user);
            this.label = (this.user.fullName) || '';
            this.items = [
                { label: 'Angular.io', icon: 'pi pi-external-link', url: 'http://angular.io' },
                { label: 'Theming', icon: 'pi pi-bookmark', routerLink: ['/theming'] }
            ];
    
            this.menuItems = [
                {
                    label: 'Cash Up', 
                    icon: 'pi pi-fw pi-check',
                    command: (onclick)=> {this.getTodaySales()}
                },
                {
                    label: 'Profile', 
                    icon: 'pi pi-fw pi-user',
                    routerLink: ['/profiling/profile'] 
                },
                {
                    label: 'Settings', 
                    icon: 'pi pi-fw pi-cog'
                },
                {
                    separator: true
                },
                {
                    label: 'Log Out', 
                    icon: 'pi pi-fw pi-sign-out',
                    command: (onclick)=> {this.logout()}
                },
            ];

            this.currencies = Object.values(Currency)
    }

    addCashUp(cashUp: CashUpDto){
        cashUp.creationTime = new Date();
        cashUp.creatorId = this.user.id;
        cashUp.lastModificationTime = new Date();
        cashUp.lastModifierUserId = this.user.id;
        cashUp.isDeleted = false;
        cashUp.rate = cashUp.currency == Currency.USD ? 1 : this.rates.find(x => x.currency === cashUp.currency)?.exchangeRate!.baseToRate;
        cashUp.usdAmount = cashUp.amount!/cashUp.rate!;

        this.totalAmount += cashUp.usdAmount;
        
        console.log(cashUp)
        this.cashUps = [...this.cashUps, cashUp]
        console.log(this.cashUps)

        cashUp = {} as CashUpDto;
        this.cashUpForm = {} as CashUpDto;
    }

    removeItem(cashUp: CashUpDto){
        console.log(cashUp)
        var index = this.cashUps!.findIndex(x => x.currency === cashUp.currency);
        this.cashUps.splice(index, 1);
    }

    cashUp(){
        this.cashUpModal = true;
        // this.onCashUp();
    }

    hideDialog(){
        this.cashUpModal = false;
    }

    save(){
        this.cashUpService.create(this.cashUps)
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
        this.cashUpModal = false;
    }

    getTodaySales() {
        const today = new Date(new Date().getTime() + (2 * 60 * 60 * 1000));
        const fourDaysAgo = new Date(today.setDate(today.getDate())).toISOString();

        this.salesOrderService.getAllItemsByDate(today.toISOString(), this.user.id!).subscribe(res => {
            this.reconItems = res.data;
            this.categorizeItems();
            this.onCashUp();
            this.cashUpModal = true;
        });
    }
    
    categorizeItems() {
        const categories = ['Fruits', 'Vegetables', 'Beverage', 'Spices', 'Sweets', 'Cereals', 'Unallocated'];
        this.fruits = this.reconItems.filter(x => x.subCategory === categories[0]);
        this.vegetables = this.reconItems.filter(x => x.subCategory === categories[1]);
        this.beverages = this.reconItems.filter(x => x.subCategory === categories[2]);
        this.spices = this.reconItems.filter(x => x.subCategory === categories[3]);
        this.sweets = this.reconItems.filter(x => x.subCategory === categories[4]);
        this.cereals = this.reconItems.filter(x => x.subCategory === categories[5]);
        this.unallocated = this.reconItems.filter(x => x.subCategory === categories[6]);
    }
    
    onCashUp() {
        let offset2 = 10.5;
        let totalLength = (this.reconItems.length);
        let paperL = totalLength * offset2;
        let pdf = new jspdf('p', 'mm', [100, paperL + 50]);
    
        // Logo and address
        pdf.setFontSize(8);
        pdf.addImage('assets/demo/images/logos/FARM-LOGO.png\n', 'PNG', 40, 5, 25, 15);
        pdf.text('Andile Fresh Farm Produce | Post Office Box 000'
            + '\n0 Magwaza Complex | Mkhosana Main\nVictoria Falls | Zimbabwe\n+263 77 957 7216 | example@email.com'
            + '\nVat No 0000000 | BP No 0000000\nwww.andilefreshfarm.com', 50, 25, { align: 'center' });
    
        pdf.setFontSize(12);
        pdf.text('TELLER:            '
            + '\nTERMINAL:               '
            + '\nTIME:         ', 5, 50);
    
        pdf.text(this.user.fullName
            + '\n' + new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })
            + '\n' + new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }), 95, 50, { align: 'right' });
    
        pdf.line(5, 65, 95, 65, 'F');
    
        // Consolidating all items into one array
        let groupedData = [
            { category: 'Sweets', items: this.sweets },
            { category: 'Vegetables', items: this.vegetables },
            { category: 'Fruits', items: this.fruits },
            { category: 'Beverage', items: this.beverages },
            { category: 'Spices', items: this.spices },
            { category: 'Cereals', items: this.cereals },
            { category: 'Unallocated', items: this.unallocated }
        ];
    
        let tableData: any[] = [];
    
        groupedData.forEach(group => {
            if (group.items.length > 0) {
                // Category header
                tableData.push([{ content: group.category, colSpan: 4, styles: { halign: 'center', fontStyle: 'bold' } }]);
    
                // Items in the category
                group.items.forEach(item => {
                    tableData.push([
                        item.productName!,
                        item.unitPrice!.toFixed(2),
                        item.quantity!,
                        item.price!.toFixed(2)
                    ]);
                });
    
                // Category totals
                tableData.push([
                    'Totals',
                    '',
                    group.items.reduce((accumulator: number, currentItem: SalesOrderItemDto) => accumulator + currentItem.quantity!, 0).toFixed(2),
                    '$' + group.items.reduce((accumulator: number, currentItem: SalesOrderItemDto) => accumulator + currentItem.price!, 0).toFixed(2)
                ]);
            }
        });
    
        // Render table
        autoTable(pdf, {
            headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
            head: [['Item', 'Unit $', 'Qty', 'Total']],
            body: tableData,
            startY: 70,
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
        window.open(pdf.output('bloburl'));
    }
    
  

    logout(){
        this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');
        this.authService.logout(this.user)
        .subscribe((res) => {

            if(res.isSuccess){

                localStorage.removeItem('token');
                localStorage.removeItem('userId');
                localStorage.removeItem('userName');
                sessionStorage.removeItem('loggedUser');
                this.router.navigate(['/auth/login']);

                this.messageService.add({
                    severity:'success', 
                    summary: 'Success', 
                    detail: res.message , 
                    life: 3000
                });
            }
            else{
                this.router.navigate(['/auth/login']);
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
    }
}
