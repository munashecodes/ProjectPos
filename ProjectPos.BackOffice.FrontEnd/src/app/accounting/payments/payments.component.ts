import { ProofOfPaymentDto } from 'src/proxy/interfaces/proof-of-payment-dto';
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Currency } from 'src/proxy/enums/currency';
import { Months } from 'src/proxy/enums/months';
import { OrderOption } from 'src/proxy/enums/order-option';
import { OrderOption2 } from 'src/proxy/enums/order-option2';
import { SalesOrderStatus } from 'src/proxy/enums/sales-order-status';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { CompanyService } from 'src/proxy/services/company.service';
import { PaymentService } from 'src/proxy/services/payment.service';
import { ProofOfPaymentService } from 'src/proxy/services/proof-of-payment.service';
import { Table } from 'primeng/table';
import { ProofOfPaymentReportDto } from 'src/proxy/interfaces/proof-of-payment-report-dto';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent implements OnInit {

  newProofOfPayment: ProofOfPaymentDto = {} as ProofOfPaymentDto;

  proofOfPayments: ProofOfPaymentDto[] = [];

  filteredPayments: ProofOfPaymentDto[] = [];

  currencies: Currency[] = [];

  customers: CompanyDto[] = [];

  customer: CompanyDto = {} as CompanyDto;

  popReports: ProofOfPaymentReportDto[] = []

  paymentsByDate: ProofOfPaymentDto[] = [];

  paymentsByDateRange: ProofOfPaymentDto[] = [];

  paymentsByMonth: ProofOfPaymentDto[] = [];

  paymentsByUser: ProofOfPaymentDto[] = [];

  paymentsByCustomer: ProofOfPaymentDto[] = [];

  user: UserDto = {} as UserDto;

  date = new Date();

  startDate = new Date();

  endDate = new Date();

  teller: UserDto = {} as UserDto;

  users: UserDto[] = [];

  saleOptions: OrderOption[] = [];

  saleOptions2: OrderOption2[] = [];

  option = OrderOption.TODAY;

  option2 = OrderOption2.ALL;

  statuses: SalesOrderStatus[] = [];

  months: Months[] = [];

  cols: any[] = [];

  month = 0;

  createModal = false;

  deleteModal = false; 

  submitted = false;

  editModal = false;

  reportModal = false;

  isDisabled = true;

  constructor(
    private userService: AuthService,
    private customerService: CompanyService,
    private popService: ProofOfPaymentService,
    private paymentService: PaymentService,
    private messageService: MessageService,
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

    this.popService.getAllByDate(dateString)
    .subscribe(res => {
      this.paymentsByDate = res.data;
      console.log(this.paymentsByDate)
      this.proofOfPayments = this.paymentsByDate;
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
    this.currencies = Object.values(Currency);
  }

  report(){
    this.reportModal = true;
  }

  get(){
    if(this.option === OrderOption.TODAY){
      this.getToday();
    }
  }

  getToday(){
    
    this.filteredPayments = [];
    var todayDate = new Date();
    var dateString: string = todayDate.toISOString();
    console.log(dateString)

    this.popService.getAllByDate(dateString)
    .subscribe(res => {
      this.filteredPayments = res.data;
      console.log(this.filteredPayments)
      this.proofOfPayments = this.filteredPayments;
    });
  }

  getByDate(){
    this.filteredPayments = [];
    console.log(this.date)
    this.date.setDate(this.date.getDate() + 1);
    console.log(this.date)
    var dateString: string = this.date.toISOString();
    console.log(dateString)

    this.popService.getAllByDate(dateString)
    .subscribe(res => {
      this.filteredPayments = res.data;
      console.log(this.filteredPayments)
      this.proofOfPayments = this.filteredPayments;
    });
  }

  getByDateRange(){
    this.filteredPayments = [];
    var startDateString: string = this.startDate.toISOString();
    var endDateString: string = this.endDate.toISOString();

    console.log(endDateString)

    this.popService.getAllByDateRange(startDateString, endDateString)
    .subscribe(res => {
      this.filteredPayments = res.data;
      console.log(this.filteredPayments)
      this.proofOfPayments = this.filteredPayments;
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
    

    this.popService.getAllByMonth(this.month)
    .subscribe(res => {
      this.filteredPayments = res.data;
      this.proofOfPayments = res.data;
      console.log(this.proofOfPayments)
    });

    console.log(this.proofOfPayments);
  }

  getDateReportByCustomer(){
    this.popReports = [];
    console.log(this.date)
    this.date.setDate(this.date.getDate() + 1);
    console.log(this.date)
    var dateString: string = this.date.toISOString();
    console.log(dateString)

    this.popService.getDateReportByCustomer(dateString)
    .subscribe(res => {
      this.popReports = res.data;
      console.log(this.popReports)
    });
  }

  getRangeReportByCustomer(){
    this.popReports = [];
    var startDateString: string = this.startDate.toISOString();
    var endDateString: string = this.endDate.toISOString();

    console.log(endDateString)

    this.popService.getRangeReportByCustomer(startDateString, endDateString)
    .subscribe(res => {
      this.popReports = res.data;
      console.log(this.popReports)
    });
  }

  getMonthReportByCustomer(event: any){
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
    

    this.popService.getMonthReportByCustomer(this.month)
    .subscribe(res => {
      this.popReports = res.data;
    });

    console.log(this.proofOfPayments);

    this.reportModal = true
  }


  filterAll(){
    if(this.option2 === OrderOption2.ALL){
      this.proofOfPayments = this.filteredPayments;
    }
  }

  filterByUser(){
    this.proofOfPayments = this.filteredPayments.filter(x => x.creatorId === this.teller.id);
  }

  filterByCustomer(){
    this.proofOfPayments = this.filteredPayments.filter(x => x.customerId === this.customer.id);
  }

  view(payment: ProofOfPaymentDto){
    this.newProofOfPayment = payment;

    console.log(this.newProofOfPayment)
    this.editModal = true;
  }

  create(){
    this.submitted = false;
    this.createModal = true;
  }

  save(){
    this.submitted = true;

    console.log(this.newProofOfPayment)
    
    this.newProofOfPayment.customerId = this.newProofOfPayment.customer!.id;
    this.newProofOfPayment.creatorId = this.user.id
    this.newProofOfPayment.creationTime = new Date();
    this.newProofOfPayment.lastModificationTime = new Date();
    this.newProofOfPayment.lastModifierUserId = this.user.id;
    this.newProofOfPayment.creationTime = new Date();
    this.newProofOfPayment.usableAmount = this.newProofOfPayment.paidAmount

    console.log(this.newProofOfPayment)

    this.popService.create(this.newProofOfPayment)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        console.log(res)

        res.data.customerName = this.newProofOfPayment.customer?.name

        if(!Array.isArray(this.proofOfPayments)){
          this.proofOfPayments = []
          this.proofOfPayments = [...this.proofOfPayments, res.data];
        }
        else{
          this.proofOfPayments = [...this.proofOfPayments, res.data];
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

    this.hideDialog()
  }

  update(){
    this.submitted = true;

    console.log(this.newProofOfPayment)
    
    this.newProofOfPayment.customerId = this.newProofOfPayment.customer!.id;
    this.newProofOfPayment.lastModificationTime = new Date();
    this.newProofOfPayment.lastModifierUserId = this.user.id;
    this.newProofOfPayment.usableAmount = this.newProofOfPayment.paidAmount

    console.log(this.newProofOfPayment)

    this.popService.update(this.newProofOfPayment)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){
        console.log(res)

        res.data.customerName = this.newProofOfPayment.customer?.name

        if(!Array.isArray(this.proofOfPayments)){
          this.proofOfPayments = []
          this.proofOfPayments = [...this.proofOfPayments, res.data];
        }
        else{
          this.proofOfPayments = [...this.proofOfPayments, res.data];
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

    this.hideDialog()
  }

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.reportModal = false;
    this.newProofOfPayment = {} as ProofOfPaymentDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
