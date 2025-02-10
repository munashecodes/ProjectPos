import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Role } from 'src/proxy/enums/role';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { EmployeeService } from 'src/proxy/services/employee.service';
import { UserService } from 'src/proxy/services/user.service';

@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html',
  styleUrls: ['./manage-users.component.scss']
})
export class ManageUsersComponent implements OnInit  {

  users: UserDto[] = [];

  roles: Role[] = [];

  newUser: UserDto = {} as UserDto;

  createModal = false;

  editModal = false;
  
  viewModal = false;

  isActive =''

  isNotActive =''

  submitted = false;

  editing = false

  deleteSelectedModal = false;

  deleteModal = false;

  cols: any[] = [];

  newUsers: UserDto[] = [];

  employees: EmployeeDto[] = [];

  dialogClass: string = '';

  constructor(
    private userService: UserService,
    private messageService: MessageService,
    private employeeService: EmployeeService,
    private router: Router){}

  ngOnInit(): void {

    this.setDialogClass(); // Set initial dialog class
    window.addEventListener('resize', () => {
      this.setDialogClass(); // Update dialog class on window resize
    });

    this.userService.getAllList()
    .subscribe(res => {
      console.log(res);
      this.users = res.data;
    });

    // get all employees from rhe database
    this.employeeService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.employees = res.data;
    });

    this.roles = Object.values(Role);
  }

  setDialogClass() {
    if (window.innerWidth <= 414) {
      this.dialogClass = 'full-width-dialog';
      console.log('Called Resize Dialog in Mobile View');
      console.log('CSS Applied: '+ this.dialogClass);

    } else {
      this.dialogClass = 'default-dialog-width';
      console.log('Called Resize Dialog in Wide Screen View');
      console.log('CSS Applied: '+ this.dialogClass);
    }
  }

  onRowEditInit(user: UserDto) {
    this.editing = true
  }

  onRowEditSave(user: UserDto) {
    this.editing = true
  }

  onRowEditCancel(user: UserDto) {
    this.editing = true
  }

  create(){
    this.newUser = {} as UserDto;
    this.createModal = true;
    this.submitted = false
  }

  deleteSelected(){

  }

  delete(user: any){
    this.newUser = {} as UserDto;
    this.newUser = { ...user};
    this.deleteModal = true;
    this.submitted = false;
  }
  

  edit(user: any){
    this.newUser = {} as UserDto;
    this.newUser = {...user}
    this.editModal = true
    this.submitted = false
  }

  save(){
    console.log(this.newUser)
    this.newUser.employeeId = this.newUser.employee?.id;
    this.newUser.fullName = this.newUser.employee?.name + " " + this.newUser.employee?.surname
    if(this.isActive === "true"){
      this.newUser.isActive = true;
    }
    else{
      this.newUser.isActive = false;
    }
    this.newUser.employee = undefined

    this.userService.create(this.newUser)
        .subscribe((res) => {
          console.log(res);
          if(res.isSuccess){

            if(!Array.isArray(this.users)){
              this.users = []
            }
            else{
              this.users = [...this.users, res.data];
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
              summary: 'Error', 
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
        
    this.createModal = false;
    this.hideDialog();

  }

  update(){
    console.log(this.newUser);
    
    if(this.isActive === "true"){
      this.newUser.isActive = true;
    }
    else if(this.isActive === "false"){
      this.newUser.isActive = false;
    }

    this.userService.update(this.newUser)
        .subscribe((res) => {
          console.log(res);
          if(res.isSuccess){

            if(!Array.isArray(this.users)){
              this.users = []
            }
            else{
              var index = this.employees.findIndex(x => x.id === this.newUser.id);
              this.users.splice(index, 1)

              this.users = [...this.users, res.data];
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
              summary: 'Error', 
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
        
    this.editModal = false
  }

  confirmDelete(){
    this.userService.delete(this.newUser.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.users)){
          this.users = []
        }
        else{
          var index = this.employees.findIndex(x => x.id === this.newUser.id);
          this.users.splice(index, 1)
        }

        this.messageService.add({
          severity:'success', 
          summary: 'Success', 
          detail: res.message ,
           life: 3000});
      }
      else{
        this.messageService.add({
          severity:'error', 
          summary: 'Error',
           detail: res.message ,
            life: 3000});
      }
    },
    (error) => {
      this.messageService.add({
        severity:'error', 
        summary: 'Error',
         detail: error.message, 
         life: 3000});
    });

    var index = this.users.findIndex(x => x.id === this.newUser.id);
    this.users.splice(index, 1)
    this.deleteModal = false;
  }

  confirmDeleteSelected() {
    this.deleteSelectedModal = true;
  
    this.newUsers.forEach((newUser, index) => {
      this.userService.delete(newUser.id!).subscribe(
        (res) => {
          console.log(res);
          if (res.isSuccess) {
            // Handling success response
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: res.message,
              life: 3000
            });
  
            // Removing the deleted user from the local array
            if(!Array.isArray(this.users)){
              this.users = []
            }
            else{
              var index = this.employees.findIndex(x => x.id === this.newUser.id);
              this.users.splice(index, 1)
            }

          } else {
            // Handling error response
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: res.message,
              life: 3000
            });
          }
        },
        (error) => {
          // Handling HTTP error
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message,
            life: 3000
          });
        },
      )
    });
  }
  
  hideDialog(){
    this.createModal = false;
    this.editModal = false
    this.deleteModal = false;
    this.viewModal = false;
    this.newUser = {} as UserDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
