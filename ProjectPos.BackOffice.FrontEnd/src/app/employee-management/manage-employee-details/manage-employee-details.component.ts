import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Department } from 'src/proxy/enums/department';
import { EmploymentType } from 'src/proxy/enums/employee-type';
import { SalaryType } from 'src/proxy/enums/salary-type';
import { EmployeeDetailsDto } from 'src/proxy/interfaces/employee-details-dto';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { EmployeeDetailsService } from 'src/proxy/services/employee-details.service';
import { EmployeeService } from 'src/proxy/services/employee.service';

@Component({
  selector: 'app-manage-employee-details',
  templateUrl: './manage-employee-details.component.html',
  styleUrls: ['./manage-employee-details.component.scss']
})
export class ManageEmployeeDetailsComponent implements OnInit {

  employeeDetails: EmployeeDetailsDto[] = [];
  newEmployeeDetails: EmployeeDetailsDto = {} as EmployeeDetailsDto;
  employees: EmployeeDto[] = [];
  
  departments = Object.values(Department);
  employmentTypes = EmploymentType;
  salaryTypes = Object.values(SalaryType);

  createModal = false;
  editModal = false;
  deleteModal = false;
  submitted = false;
  dialogVisible = false;

  selectedEmployee: EmployeeDto | null = null;

  constructor(
    private employeeDetailsService: EmployeeDetailsService,
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadEmployeeDetails();
  }

  loadEmployees() {
    this.employeeService.getAllList()
      .subscribe(res => {
        if (res.isSuccess) {
          this.employees = res.data;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  loadEmployeeDetails() {
    this.employeeDetailsService.getAllAsync()
      .subscribe(res => {
        if (res.isSuccess) {
          this.employeeDetails = res.data;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  create() {
    this.newEmployeeDetails = {} as EmployeeDetailsDto;
    this.submitted = false;
    this.createModal = true;
    this.dialogVisible = true;
  }

  edit(details: EmployeeDetailsDto) {
    this.newEmployeeDetails = { ...details };
    this.newEmployeeDetails.joiningDate = new Date(details.joiningDate)
    this.editModal = true;
    this.dialogVisible = true;
  }

  delete(details: EmployeeDetailsDto) {
    this.newEmployeeDetails = { ...details };
    this.deleteModal = true;
  }

  save() {
    this.submitted = true;

    if (!this.newEmployeeDetails.employeeId || !this.newEmployeeDetails.department) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.employeeDetailsService.createAsync(this.newEmployeeDetails)
      .subscribe(res => {
        if (res.isSuccess) {
          this.employeeDetails = [...this.employeeDetails, res.data];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: res.message,
            life: 3000
          });
          this.hideDialog();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  update() {
    this.submitted = true;

    if (!this.newEmployeeDetails.employeeId || !this.newEmployeeDetails.department) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.employeeDetailsService.updateAsync(this.newEmployeeDetails)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.employeeDetails.findIndex(x => x.id === this.newEmployeeDetails.id);
          this.employeeDetails[index] = res.data;
          this.employeeDetails = [...this.employeeDetails];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: res.message,
            life: 3000
          });
          this.hideDialog();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  confirmDelete() {
    if (this.newEmployeeDetails.id) {
      this.employeeDetailsService.deleteAsync(this.newEmployeeDetails.id)
        .subscribe(res => {
          if (res.isSuccess) {
            this.employeeDetails = this.employeeDetails.filter(x => x.id !== this.newEmployeeDetails.id);
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Employee details deleted successfully',
              life: 3000
            });
          }
          this.hideDialog();
        });
    }
  }

  hideDialog() {
    this.createModal = false;
    this.editModal = false;
    this.dialogVisible = false;
    this.deleteModal = false;
    this.submitted = false;
    this.newEmployeeDetails = {} as EmployeeDetailsDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
