import { Component, OnInit } from '@angular/core';
import jspdf from 'jspdf';
import autoTable from 'jspdf-autotable';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Country } from 'src/proxy/enums/country';
import { Role } from 'src/proxy/enums/role';
import { AddressDto } from 'src/proxy/interfaces/address-dto';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { EmployeeService } from 'src/proxy/services/employee.service';

@Component({
  selector: 'app-manage-employees',
  templateUrl: './manage-employees.component.html',
  styleUrls: ['./manage-employees.component.scss']
})
export class ManageEmployeesComponent implements OnInit {
 
  newEmployee: EmployeeDto = {
    address: {} as AddressDto
  } as EmployeeDto;

  employees: EmployeeDto[] = [];

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  roles: Role[] = [];

  countries: Country[] = [];

  user: UserDto = {} as UserDto;
  
  country!: Country;

  cols: any[] = [];

  dialogClass: string = '';

  constructor(
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) { }

  // life cycle hook
  ngOnInit(): void {

    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    // get all employees from rhe database
    this.employeeService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.employees = res.data;
    });

    // serialize enums
    this.countries = Object.values(Country);
    this.roles = Object.values(Role);
  }

  // open create modal
  create(){
    this.createModal = true;
    this.submitted = false;
  }

  // open edit modal
  edit(company: EmployeeDto){
    // initialize company to fill the edit form
    this.newEmployee = { ...company};
    this.editModal = true;
    this.submitted = false;
  }

  // open delete modal
  delete(company: EmployeeDto){
    // initialize company
    this.newEmployee = { ...company};
    this.deleteModal = true;
    this.submitted = false;
  }

  // open delete modal to delete multiple selected items
  deleteSelected(){
  }

  // scalled save button event on create modal
  save(){
    this.submitted = true;

    this.newEmployee.creationTime = new Date();
    this.newEmployee.creatorId = this.user.id;
    this.newEmployee.lastModificationTime = new Date();
    this.newEmployee.lastModifierUserId = this.user.id;
    this.newEmployee.isDeleted = false;

    // send
    this.employeeService.create(this.newEmployee)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.employees)){
          this.employees = []
        }
        else{
          this.employees = [...this.employees, res.data];
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
  }

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.newEmployee = {} as EmployeeDto;
  }

  confirmDelete(){
    this.employeeService.delete(this.newEmployee.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.employees)){
          this.employees = []
        }
        else{
          var index = this.employees.findIndex(x => x.id === this.newEmployee.id);
          this.employees.splice(index, 1)
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
  }

  update(){
    this.submitted = true;

    this.newEmployee.lastModificationTime = new Date();
    this.newEmployee.lastModifierUserId = this.user.id;
    this.newEmployee.isDeleted = false;

    this.employeeService.update(this.newEmployee)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.employees)){
          this.employees = []
        }
        else{
          var index = this.employees.findIndex(x => x.id === this.newEmployee.id);
          this.employees.splice(index, 1);

          this.employees = [...this.employees, res.data];
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
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
