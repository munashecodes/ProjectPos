import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Currency } from 'src/proxy/enums/currency';
import { ExchangeRateDto } from 'src/proxy/interfaces/exchange-rate-dto';
import { GetExchangeRateDto } from 'src/proxy/interfaces/get-exchange-rate-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { ExchangeRateService } from 'src/proxy/services/exchange-rate.service';

@Component({
  selector: 'app-manage-rates',
  templateUrl: './manage-rates.component.html',
  styleUrls: ['./manage-rates.component.scss']
})
export class ManageRatesComponent implements OnInit {

  newRate: ExchangeRateDto = {} as ExchangeRateDto;

  rates: GetExchangeRateDto[] = [];

  currencies: Currency[] = [];

  user: UserDto = {} as UserDto;

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  cols: any[] = [];

  dialogClass: string = '';


  constructor(
    private ratesService: ExchangeRateService,
    private messageService: MessageService
  ){ }
  
  ngOnInit(): void {
    
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    var todayDate = new Date();
    var dateString: string = todayDate.toISOString();

    console.log(dateString);
    this.ratesService.getAllList()
    .subscribe(res => {
      this.rates = res.data;
      console.log(res.data)
    });

    
    this.currencies = Object.values(Currency);
  }

  create(){
    this.createModal = true
  }

  edit(rate: GetExchangeRateDto){
    this.newRate = {
      currency: rate.currency,
      baseCurrency: rate.exchangeRate?.baseCurrency,
      baseToRate: rate.exchangeRate?.baseToRate!,
      dateEffected: rate.dateEffected
    }

    this.editModal = true;
  }

  delete(rate: ExchangeRateDto){

  }

  deleteSelected(){

  }

  confirmDelete(){
    
  }

  save(){
    this.newRate.creationTime = new Date();
    this.newRate.baseCurrency = Currency.USD;
    this.newRate.creatorId = this.user.id;
    this.newRate.lastModificationTime = new Date();
    this.newRate.lastModifierUserId = this.user.id;
    this.newRate.creationTime = new Date();

    console.log(this.newRate)
    this.ratesService.create(this.newRate)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        var rate = {} as GetExchangeRateDto;
        rate.exchangeRate = res.data;
        rate.currency = res.data.currency;
        rate.dateEffected = res.data.dateEffected;

        if(!Array.isArray(this.rates)){
          this.rates = []
        }
        else{
          this.rates = [...this.rates, rate];
        }

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

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;

    this.newRate = {} as ExchangeRateDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
