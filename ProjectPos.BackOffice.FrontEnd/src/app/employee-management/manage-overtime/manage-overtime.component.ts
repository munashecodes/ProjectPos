import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { OvertimeRecordDto } from 'src/proxy/interfaces/overtime-record-dto';
import { EmployeeService } from 'src/proxy/services/employee.service';
import { OvertimeService } from 'src/proxy/services/overtime.service';

@Component({
  selector: 'app-manage-overtime',
  templateUrl: './manage-overtime.component.html',
  styleUrls: ['./manage-overtime.component.scss']
})
export class ManageOvertimeComponent implements OnInit {
  
  newOvertime: OvertimeRecordDto = {} as OvertimeRecordDto;
  overtimeRecords: OvertimeRecordDto[] = [];
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
    private overtimeService: OvertimeService,
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadOvertimeRecords();
  }

  loadEmployees() {
    this.employeeService.getAllList()
      .subscribe(res => {
        if (res.isSuccess) {
          this.employees = res.data;
          // this.messageService.add({
          //   severity: 'success',
          //   summary: 'Success',
          //   detail: res.message,
          //   life: 3000
          // });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      }, error => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
          life: 3000
        });
      });
  }

  loadOvertimeRecords() {
    this.overtimeService.getAllAsync()
      .subscribe(res => {
        if (res.isSuccess) {
          this.overtimeRecords = res.data;
          // this.messageService.add({
          //   severity: 'success',
          //   summary: 'Success',
          //   detail: res.message,
          //   life: 3000
          // });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      }, error => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
          life: 3000
        });
      });
  }

  loadOvertimeByDateRange() {
    if (this.startDate && this.endDate) {
      if (this.selectedEmployee) {
        this.overtimeService.getByDateRangeAndEmployeeIdAsync(this.startDate, this.endDate, this.selectedEmployee.id!)
          .subscribe(res => {
            if (res.isSuccess) {
              this.overtimeRecords = res.data;
              // this.messageService.add({
              //   severity: 'success',
              //   summary: 'Success',
              //   detail: res.message,
              //   life: 3000
              // });
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
        this.overtimeService.getByDateRangeAsync(this.startDate, this.endDate)
          .subscribe(res => {
            if (res.isSuccess) {
              this.overtimeRecords = res.data;
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
    }
  }

  create() {
    this.newOvertime = {} as OvertimeRecordDto;
    this.createModal = true;
    this.dialogVisible = true;
    this.submitted = false;
  }

  edit(overtime: OvertimeRecordDto) {
    this.newOvertime = { ...overtime };
    this.newOvertime.date = new Date(this.newOvertime.date);
    this.editModal = true;
    this.dialogVisible = true;
    this.submitted = false;
  }

  delete(overtime: OvertimeRecordDto) {
    this.newOvertime = { ...overtime };
    this.deleteModal = true;
  }

  save() {
    this.submitted = true;

    if (!this.newOvertime.employeeId || !this.newOvertime.date || !this.newOvertime.hours || !this.newOvertime.rate) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    

    this.newOvertime.amount = this.newOvertime.hours * this.newOvertime.rate;

    this.newOvertime.date = new Date(this.newOvertime.date.getTime() + (2 * 60 * 60 * 1000)).toISOString();

    this.overtimeService.createAsync(this.newOvertime)
      .subscribe(res => {
        if (res.isSuccess) {
          this.overtimeRecords = [...this.overtimeRecords, res.data];
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

    if (!this.newOvertime.employeeId || !this.newOvertime.date || !this.newOvertime.hours) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.overtimeService.updateAsync(this.newOvertime)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.overtimeRecords.findIndex(x => x.id === this.newOvertime.id);
          this.overtimeRecords[index] = res.data;
          this.overtimeRecords = [...this.overtimeRecords];
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

  approve(overtime: OvertimeRecordDto, userId: number) {
    this.overtimeService.approveAsync(overtime.id!, userId)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.overtimeRecords.findIndex(x => x.id === overtime.id);
          this.overtimeRecords[index] = res.data;
          this.overtimeRecords = [...this.overtimeRecords];
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
    if (this.newOvertime.id) {
      this.overtimeService.deleteAsync(this.newOvertime.id)
        .subscribe(success => {
          if (success) {
            this.overtimeRecords = this.overtimeRecords.filter(x => x.id !== this.newOvertime.id);
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Overtime record deleted successfully',
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
    this.newOvertime = {} as OvertimeRecordDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
} 
