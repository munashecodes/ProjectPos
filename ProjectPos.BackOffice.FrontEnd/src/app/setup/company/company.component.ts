import { state } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Country } from 'src/proxy/enums/country';
import { Unit } from 'src/proxy/enums/unit';
import { AddressDto } from 'src/proxy/interfaces/address-dto';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { CompanyService } from 'src/proxy/services/company.service';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit {

  newCompany: CompanyDto = {
  } as CompanyDto;

  companies: CompanyDto[] = [];

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  units: Unit[] = [];

  countries: Country[] = [];

  country: any;

  street: string = '';

  addressLine: string = '';

  city: string = '';

  state: string = '';

  cols: any[] = [];

  user: UserDto = {} as UserDto;

  dialogClass: string = '';

  constructor(
    private companyService: CompanyService,
    private messageService: MessageService
  ) { }

  // life cycle hook
  ngOnInit(): void {

    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    // get all companies from rhe database
    this.companyService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.companies = res.data;
    });

    // serialize enums
    this.countries = Object.values(Country);
    this.units = Object.values(Unit);
  }

  // open create modal
  create(){
    // initialize company
    this.newCompany = {} as CompanyDto;
    this.createModal = true;
    this.submitted = false;
  }

  // open edit modal
  edit(company: CompanyDto){
    // initialize company to fill the edit form
    this.newCompany = {} as CompanyDto;
    this.newCompany = { ...company};
    this.editModal = true;
    this.submitted = false;
  }

  // open delete modal
  delete(company: CompanyDto){
    // initialize company
    this.newCompany = {} as CompanyDto;
    this.newCompany = { ...company};
    this.deleteModal = true;
    this.submitted = false;
  }

  // open delete modal to delete multiple selected items
  deleteSelected(){
  }

  // scalled save button event on create modal
  save(){
    this.submitted = true;
    this.newCompany.address = {} as AddressDto;

    this.newCompany.creationTime = new Date();
    this.newCompany.creatorId = this.user.id;
    this.newCompany.lastModificationTime = new Date();
    this.newCompany.lastModifierUserId = this.user.id;
    this.newCompany.isDeleted = false;
    this.newCompany.address.country = this.country;
    this.newCompany.address.street = this.street;
    this.newCompany.address.addressLine1 = this.addressLine;
    this.newCompany.address.city = this.city;
    this.newCompany.address.state = this.state;

    // send
    this.companyService.create(this.newCompany)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.companies)){
          this.companies = []
        }
        else{
          this.companies = [...this.companies, res.data];
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
    
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.hideDialog();
  }

  hideDialog(){
    this.street = '';
    this.addressLine = '';
    this.city = '';
    this.state = '';
    this.country = '';
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.newCompany = {} as CompanyDto;
  }

  confirmDelete(){

    this.newCompany.deletionTime = new Date();
    this.newCompany.deleterId = this.user.id;
    this.newCompany.isDeleted = true;

    this.companyService.delete(this.newCompany.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.companies)){
          this.companies = []
        }
        else{
          var index = this.companies.findIndex(x => x.id === this.newCompany.id);
        this.companies.splice(index, 1)
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

    this.deleteModal = false;
    this.newCompany = {} as CompanyDto;
  }

  update(){
    this.submitted = true;

    this.newCompany.lastModificationTime = new Date();
    this.newCompany.lastModifierUserId = this.user.id;
    this.newCompany.isDeleted = false;

    this.companyService.update(this.newCompany)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.companies)){
          this.companies = []
        }
        else{
          var index = this.companies.findIndex(x => x.id === this.newCompany.id);
          this.companies.splice(index, 1);

          this.companies = [...this.companies, res.data];
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

    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.newCompany = {} as CompanyDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
