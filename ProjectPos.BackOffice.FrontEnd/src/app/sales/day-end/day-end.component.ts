import { ProductInventorySnapshotDto } from './../../../proxy/interfaces/product-inventory-snapshot-dto';
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { CashUpOptions } from 'src/proxy/enums/cash-up-options';
import { SnapShotEnum } from 'src/proxy/enums/snap-shot-enum';
import { CashUpDto } from 'src/proxy/interfaces/cash-up-dto';
import { GetReconListDto } from 'src/proxy/interfaces/get-recon-list-dto';
import { InventorySnapShotSummaryDto } from 'src/proxy/interfaces/inventory-snap-shot-summary-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { CashUpService } from 'src/proxy/services/cash-up.service';
import { InventorySnapshotService } from 'src/proxy/services/inventory-snapshot.service';
import { ProductInventorySnapShotService } from 'src/proxy/services/product-inventory-snap-shot.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';

@Component({
  selector: 'app-day-end',
  templateUrl: './day-end.component.html',
  styleUrls: ['./day-end.component.scss']
})
export class DayEndComponent implements OnInit {

  cashUp: CashUpDto = {} as CashUpDto;

  cashUpList: GetReconListDto[] = [];

  inventory?: ProductInventoryDto[] = [];

  user: UserDto = {} as UserDto;

  teller: UserDto = {} as UserDto;

  snapShot: ProductInventorySnapshotDto = {} as ProductInventorySnapshotDto;

  users: UserDto[] = [];

  createModal = false;

  editModal = false;

  dissabled = true;

  confirmModal = false;

  submitted = false;

  date = new Date();

  todayDate = new Date();

  options: CashUpOptions[] = [];

  option = CashUpOptions.Option;

  cols: any[] = [];

  message = '';

  constructor(
    private cashUpService: CashUpService,
    private userService: AuthService,
    private messageService: MessageService,
    private snapShotService: ProductInventorySnapShotService,
    private inventoryService: ProductInventoryService
  ){}

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    this.todayDate = new Date();
    var dateString: string = this.todayDate.toISOString();
    console.log(dateString)

    this.cashUpService.getAllRecons(dateString)
    .subscribe(res => {
      this.message = res.message;
      console.log(this.message)
      this.cashUpList = res.data;
      console.log(this.cashUpList)
    });

    this.inventoryService.getAllList()
    .subscribe(res => {
      this.inventory = res.data;
      console.log(this.inventory)
    })

    // this.snapShotService.getByDate(new Date().toISOString())
    // .subscribe(res => {
    //   this.snapShot = res.data;
    //   console.log(this.snapShot)
    //   var snap = JSON.parse(this.snapShot.inventory || '[]');
    //   console.log(snap)
    // });

    this.userService.getAllUsers()
    .subscribe(res => {
      this.users = res.data;
      console.log(this.users)
    });

    this.options = Object.values(CashUpOptions)
  }

  confirmCloseDay(){
    this.confirmModal = true;
  }

  closeDay(){

    let snapShotSummary = {
      userId: this.user.id,
      snapShotType: SnapShotEnum.CloseDay
    } as InventorySnapShotSummaryDto;

    this.snapShotService.create(snapShotSummary)
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
    var dateString: string = this.date.toISOString();
    this.cashUpService.getRecon(this.teller.id!, dateString)
    .subscribe(res => {
      this.cashUpList = res.data
    });
  }

  getAll(){
    var dateString: string = this.date.toISOString();

    this.cashUpService.getAllRecons(dateString)
    .subscribe(res => {
      this.cashUpList = res.data;
      console.log(this.cashUpList)
    });
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
