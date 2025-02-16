import { Component, OnInit } from '@angular/core';
import { FilterItems } from 'src/proxy/enums/filter-items';
import { Months } from 'src/proxy/enums/months';
import { DateRangeDto } from 'src/proxy/interfaces/date-range-dto';
import { TrialBalanceAccountsDto } from 'src/proxy/interfaces/trial-balance-dto';
import { TrialBalanceService } from 'src/proxy/services/trial-balance.service';

@Component({
  selector: 'app-trial-balance',
  templateUrl: './trial-balance.component.html',
  styleUrls: ['./trial-balance.component.scss']
})
export class TrialBalanceComponent implements OnInit {

  trialBalanceModal = false

  trialbalance: TrialBalanceAccountsDto[]=[]

  selectedFilter: any;

  debitBalance: number = 0

  creditTotal: number = 0

  filterItems = FilterItems;

  day: number = 0

  monthFilter = Months

  selectedYear: number = new Date().getFullYear()

  date: any

  dateQuery: any

  dateRange: DateRangeDto = {} as DateRangeDto;

  
  month = new Date();
  
  rangeDates: Date[] | undefined;


  ngOnInit(): void {
      
  }

  constructor(private trialBalanceService: TrialBalanceService) { }


  filterBookingsMethod(){

  }

  filterByMonth(event: any){
    let mon = this.month.getMonth() + 1;
  }

  filterByDate(){

  }

  async filterByDateRange(){
    this.trialBalanceService.getAll(this.dateRange).subscribe((res) => {
      if(res.isSuccess){
        this.trialbalance = res.data;
        this.creditTotal = this.trialbalance.reduce((acc, item) => acc + (item.creditBalance ?? 0), 0);
        this.debitBalance = this.trialbalance.reduce((acc, item) => acc + (item.debitBalance ?? 0), 0);
        
        this.trialBalanceModal = true;
        console.log(this.trialbalance)
      }
    })
   
    
  }

}
