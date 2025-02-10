import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { CashUpOptions } from 'src/proxy/enums/cash-up-options';
import { CashUpDto } from 'src/proxy/interfaces/cash-up-dto';
import { GetCashUpListDto } from 'src/proxy/interfaces/get-cash-up-list-dto';
import { GetReconListDto } from 'src/proxy/interfaces/get-recon-list-dto';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { CashUpService } from 'src/proxy/services/cash-up.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';

@Component({
  selector: 'app-recon-reports',
  templateUrl: './recon-reports.component.html',
  styleUrls: ['./recon-reports.component.scss']
})
export class ReconReportsComponent implements OnInit {

  cashUp: CashUpDto = {} as CashUpDto;

  cashUpList: GetReconListDto[] = [];

  user: UserDto = {} as UserDto;

  teller: UserDto = {} as UserDto;
  
  products: SalesOrderItemDto[] = [];

  users: UserDto[] = [];

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  date = new Date();

  totalCount = 0;

  totalSales = 0;

  options: CashUpOptions[] = [];

  option = CashUpOptions.Option;

  cols: any[] = [];

  constructor(
    private cashUpService: CashUpService,
    private userService: AuthService,
    private salesOrderService: SalesOrderService,
    private messageService: MessageService
  ){}

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    var todayDate = new Date();
    var dateString: string = todayDate.toISOString();
    console.log(dateString)

    this.cashUpService.getAllRecons(dateString)
    .subscribe(res => {
      this.cashUpList = res.data;
      console.log(this.cashUpList)
    });

    this.userService.getAllUsers()
    .subscribe(res => {
      this.users = res.data;
      console.log(this.users)
    });

    this.options = Object.values(CashUpOptions)
  }

  getRecon(){
    console.log(this.date)
    if(this.option === CashUpOptions.All){
      this.getAll();
    }
  }

  setOption(){
    this.option = CashUpOptions.Option
  }

  getSingle(){
    console.log(this.teller)
    var dateString: string = new Date(this.date.getTime() + (2 * 60 * 60 * 1000)).toISOString();

    this.salesOrderService.getAllItemsByDate(dateString, this.teller.id!)
    .subscribe(res => {
      this.products = res.data;

      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })
      console.log(this.products)
    });
  }

  getAll(){
    var dateString: string = new Date(this.date.getTime() + (2 * 60 * 60 * 1000)).toISOString();

    this.salesOrderService.getAllItemsByDate(dateString, 0)
    .subscribe(res => {
      this.products = res.data;
      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })
      console.log(this.products)
    });
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
