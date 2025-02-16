import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table/table';
import { EmployeeDeductionDto } from 'src/proxy/interfaces/employee-deduction-dto';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { DeductionService } from 'src/proxy/services/deduction.service';
import { EmployeeService } from 'src/proxy/services/employee.service';

@Component({
  selector: 'app-manage-employee-deduction',
  templateUrl: './manage-employee-deduction.component.html',
  styleUrls: ['./manage-employee-deduction.component.scss']
})
export class ManageEmployeeDeductionComponent implements OnInit {
  
  newDeduction: EmployeeDeductionDto = {} as EmployeeDeductionDto;
  deductions: EmployeeDeductionDto[] = [];
  employees: EmployeeDto[] = [];

  createModal = false;
  editModal = false;
  deleteModal = false;
  submitted = false;
  dialogVisible = false;

  cols: any[] = [];
  selectedEmployee: EmployeeDto | null = null;
  startDate: Date = new Date();
  endDate: Date = new Date();

  constructor(
    private deductionService: DeductionService,
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDeductions();
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

  loadDeductions() {
    const startDate = new Date(new Date(this.startDate).getTime() + (2 * 60 * 60 * 1000))
    const endDate = new Date(new Date(this.endDate).getTime() + (2 * 60 * 60 * 1000))
    this.deductionService.getByDateRangeAsync(startDate, endDate)
      .subscribe(res => {
        if (res.isSuccess) {
          this.deductions = res.data;
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

  loadDeductionsByDateRange() {
    if (this.startDate && this.endDate) {
      if (this.selectedEmployee) {
        this.deductionService.getByDateRangeAndEmployeeIdAsync(this.selectedEmployee.id!, this.startDate, this.endDate)
          .subscribe(res => {
            if (res.isSuccess) {
              this.deductions = res.data;
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: res.message,
                life: 3000
              });
            }
          });
      } else {
        const startDate = new Date(new Date(this.startDate).getTime() + (2 * 60 * 60 * 1000))
        const endDate = new Date(new Date(this.endDate).getTime() + (2 * 60 * 60 * 1000))
        this.deductionService.getByDateRangeAsync(startDate, endDate)
          .subscribe(res => {
            if (res.isSuccess) {
              this.deductions = res.data;
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
    }
  }

  create() {
    this.newDeduction = {} as EmployeeDeductionDto;
    this.createModal = true;
    this.dialogVisible = true;
    this.submitted = false;
  }

  edit(deduction: EmployeeDeductionDto) {
    this.newDeduction = { ...deduction };
    this.newDeduction.deductionDate = new Date(deduction.deductionDate)
    this.editModal = true;
    this.dialogVisible = true;
    this.submitted = false;
  }

  delete(deduction: EmployeeDeductionDto) {
    this.newDeduction = { ...deduction };
    this.deleteModal = true;
  }

  save() {
    this.submitted = true;

    if (!this.newDeduction.employeeId || !this.newDeduction.amount) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.newDeduction.deductionDate = new Date( new Date(new Date().getTime() + (2 * 60 * 60 * 1000)).toUTCString());

    this.deductionService.createAsync(this.newDeduction)
      .subscribe(res => {
        if (res.isSuccess) {
          this.deductions = [...this.deductions, res.data];
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

    if (!this.newDeduction.employeeId || !this.newDeduction.amount) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.deductionService.updateAsync(this.newDeduction)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.deductions.findIndex(x => x.id === this.newDeduction.id);
          this.deductions[index] = res.data;
          this.deductions = [...this.deductions];
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

  approve(deduction: EmployeeDeductionDto) {
    this.deductionService.approveAsync(deduction.id!)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.deductions.findIndex(x => x.id === deduction.id);
          this.deductions[index] = res.data;
          this.deductions = [...this.deductions];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: res.message,
            life: 3000
          });
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
    if (this.newDeduction.id) {
      this.deductionService.deleteAsync(this.newDeduction.id)
        .subscribe(success => {
          if (success) {
            this.deductions = this.deductions.filter(x => x.id !== this.newDeduction.id);
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Deduction deleted successfully',
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
    this.newDeduction = {} as EmployeeDeductionDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
} 
