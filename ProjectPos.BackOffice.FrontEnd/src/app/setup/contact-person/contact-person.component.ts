import { JobTitle } from './../../../proxy/enums/job-title';
import { ContactPersonDto } from './../../../proxy/interfaces/contact-person-dto';
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Country } from 'src/proxy/enums/country';
import { Title } from 'src/proxy/enums/title';
import { Unit } from 'src/proxy/enums/unit';
import { AddressDto } from 'src/proxy/interfaces/address-dto';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { CompanyService } from 'src/proxy/services/company.service';
import { ContactPersonService } from 'src/proxy/services/contact-person.service';

@Component({
  selector: 'app-contact-person',
  templateUrl: './contact-person.component.html',
  styleUrls: ['./contact-person.component.scss']
})
export class ContactPersonComponent implements OnInit {

  newContactPerson: ContactPersonDto = {
    address: {} as AddressDto
  } as ContactPersonDto;

  companies: CompanyDto[] = [];

  contactPersons: ContactPersonDto[] = [];

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  units: Unit[] = [];

  countries: Country[] = [];

  country!: Country;

  user: UserDto = {} as UserDto;

  titles: Title[] = [];

  street: string = '';

  addressLine: string = '';

  city: string = '';

  state: string = '';

  JobTitles: JobTitle[] = [];

  cols: any[] = [];

  constructor(
    private companyService: CompanyService,
    private contactPersonService: ContactPersonService,
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

    // get all contactPersons from rhe database
    this.contactPersonService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.contactPersons = res.data;
    });

    // serialize enums
    this.countries = Object.values(Country);
    this.units = Object.values(Unit);
    this.titles = Object.values(Title);
    this.JobTitles = Object.values(JobTitle);
  }

  // open create modal
  create(){
    this.newContactPerson = {} as ContactPersonDto;
    this.createModal = true;
    this.submitted = false;
  }

  // open edit modal
  edit(contactPerson: ContactPersonDto){
    // initialize contactPerson to fill the edit form
    this.newContactPerson = { ...contactPerson};
    this.newContactPerson.company = this.companies.filter(x => x.id === contactPerson.companyId)[0];
    this.editModal = true;
    this.submitted = false;
  }

  // open delete modal
  delete(contactPerson: ContactPersonDto){
    // initialize contactPerson
    this.newContactPerson = {} as ContactPersonDto;
    this.newContactPerson = { ...contactPerson};
    this.deleteModal = true;
    this.submitted = false;
  }

  // open delete modal to delete multiple selected items
  deleteSelected(){
  }

  // scalled save button event on create modal
  save(){
    this.submitted = true;
    this.newContactPerson.address = {} as AddressDto;

    this.newContactPerson.companyId = this.newContactPerson.company!.id;
    this.newContactPerson.creationTime = new Date();
    this.newContactPerson.creatorId = this.user.id;
    this.newContactPerson.lastModificationTime = new Date();
    this.newContactPerson.lastModifierUserId = this.user.id;
    this.newContactPerson.isDeleted = false;
    this.newContactPerson.address.country = this.country;
    this.newContactPerson.address.street = this.street;
    this.newContactPerson.address.addressLine1 = this.addressLine;
    this.newContactPerson.address.city = this.city;
    this.newContactPerson.address.state = this.state;
    this.newContactPerson.company = undefined;

    // send
    this.contactPersonService.create(this.newContactPerson)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.companies)){
          this.contactPersons = []
        }
        else{
          this.contactPersons = [...this.contactPersons, res.data];
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
    
    this.hideDialog();
  }

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.newContactPerson = {} as ContactPersonDto;
  }

  confirmDelete(){
    
    this.newContactPerson.deletionTime = new Date();
    this.newContactPerson.deleterId = this.user.id;
    this.newContactPerson.isDeleted = true;

    this.contactPersonService.delete(this.newContactPerson.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.contactPersons)){
          this.contactPersons = []
        }
        else{
          this.contactPersons = this.contactPersons.filter(x => x.id !== this.newContactPerson.id);
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
    this.newContactPerson = {} as ContactPersonDto;
  }

  update(){
    this.submitted = true;

    this.newContactPerson.lastModificationTime = new Date();
    this.newContactPerson.lastModifierUserId = this.user.id;
    this.newContactPerson.isDeleted = false;

    this.contactPersonService.update(this.newContactPerson)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.companies)){
          this.companies = []
        }
        else{
          var index = this.companies.findIndex(x => x.id === this.newContactPerson.id);
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
    this.newContactPerson = {} as ContactPersonDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
