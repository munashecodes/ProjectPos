import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { OrderOption } from 'src/proxy/enums/order-option';
import { StockMovementDto } from 'src/proxy/interfaces/stock-movement-dto';
import { StockMovementLogDto } from 'src/proxy/interfaces/stock-movement-log-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { StockMovementServiceService } from 'src/proxy/services/stock-movement-service.service';

@Component({
  selector: 'app-stock-movement',
  templateUrl: './stock-movement.component.html',
  styleUrls: ['./stock-movement.component.scss']
})
export class StockMovementComponent implements OnInit {

  batch: StockMovementLogDto = {} as StockMovementLogDto;

  batches: StockMovementLogDto[] = [];

  filteredBatches: StockMovementLogDto[] = [];

  mainFilter: OrderOption[] = Object.values(OrderOption);

  selectedMainFilter: OrderOption = OrderOption.TODAY;

  movement: StockMovementDto = {} as StockMovementDto;

  approveModal: boolean = false;

  movementModal: boolean = false;

  cols: any[] = [];

  user: UserDto = {} as UserDto;

  date: any;

  rangeDates: Date[] | undefined;

  start: any;

  end: any;

  month: number = new Date().getMonth();

  constructor(
    private stockMovementService: StockMovementServiceService,
    private messageService: MessageService,
  ) { }

  ngOnInit(): void {
    
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    this.stockMovementService.getAllToday()
    .subscribe((res: any) => {
      this.batches = res.data;
      this.filteredBatches = this.batches
      console.log(res.data);
    });

  }

  onApprove(batch: StockMovementLogDto) {
    this.approveModal = true;
    this.batch = batch;
  }

  onApproveBatch() {
    this.stockMovementService.approve(this.batch.id!, this.user.id!)
    .subscribe((res: any) => {
      this.approveModal = false;
      this.stockMovementService.getAllToday()
      .subscribe((res: any) => {
        this.batches = res;
        if(res.isSuccess){
          if (!Array.isArray(this.batches)) {
            this.batches = [];
          }
          
          const updatedItem = res.data; // The updated item
          this.batches = this.batches.map(item => item.id === updatedItem.id ? updatedItem : item);
          this.filteredBatches = this.batches;
          
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
      });
    },
    (error) => {
      this.messageService.add({
        severity:'error', 
        summary: 'Error', 
        detail: error.message, 
        life: 3000
      });
    });

    this.batch = {} as StockMovementLogDto;
    this.approveModal = false;
  }
  
  onMainFilter(){
    if(this.selectedMainFilter === OrderOption.TODAY){
      this.onGetToday();
    }
  }

  // get all goods received vouchers by today
  onGetToday(){
    this.stockMovementService.getAllToday()
    .subscribe((res) => {
      this.batches = res.data;
      this.filteredBatches = res.data;
      console.log(res);
    });
  }

  // get all goods received vouchers by month
  onGetMonth(){
    this.stockMovementService.getByMonth(this.month + 1)
    .subscribe((res) => {
      this.batches = res.data;
      this.filteredBatches = res.data;
      console.log(res);
    });
  }

  // get all goods received vouchers by date range
  onGetDateRange(){
    if(this.rangeDates![1]){
      
      console.log(this.rangeDates)
        this.start = new Date(new Date(this.rangeDates![0]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
        this.end = new Date(new Date(this.rangeDates![1]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      this.stockMovementService.getAllByDateRange(this.start, this.end)
      .subscribe((res) => {
        this.batches = res.data;
        this.filteredBatches = res.data;
        console.log(res);
      });
    }
  }

  // get all goods received vouchers by date
  onGetDate(){
    this.date = new Date(new Date(this.date).getTime() + 2 * 60 * 60 * 1000).toISOString();
    this.stockMovementService.getAllByDate(this.date)
    .subscribe((res) => {
      this.batches = res.data;
      this.filteredBatches = res.data;
      console.log(res);
    });
  }

  onSaveDamages(){

  }

  hideDialog(){
    this.approveModal = false;
    this.movementModal = false;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
