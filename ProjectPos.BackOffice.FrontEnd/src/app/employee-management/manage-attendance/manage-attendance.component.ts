import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { AttendanceDto } from 'src/proxy/interfaces/attendance-dto';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { AttendanceService } from 'src/proxy/services/attendance.service';
import { EmployeeService } from 'src/proxy/services/employee.service';

@Component({
  selector: 'app-manage-attendance',
  templateUrl: './manage-attendance.component.html',
  styleUrls: ['./manage-attendance.component.scss']
})
export class ManageAttendanceComponent implements OnInit {
  
  newAttendance: AttendanceDto = {} as AttendanceDto;
  attendances: AttendanceDto[] = [];
  employees: EmployeeDto[] = [];

  createModal = false;
  editModal = false;
  deleteModal = false;
  submitted = false;

  cols: any[] = [];
  selectedEmployee: EmployeeDto | null = null;
  startDate: Date = new Date();
  endDate: Date = new Date();

  dialogVisible = false;

  constructor(
    private attendanceService: AttendanceService,
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadAttendances();
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

  loadAttendances() {
    this.attendanceService.getAllAsync()
      .subscribe(res => {
        if (res.isSuccess) {
          this.attendances = res.data;
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

  loadAttendancesByDateRange() {
    if (this.startDate && this.endDate) {
      if (this.selectedEmployee) {
        this.attendanceService.getByDateRangeAndEmployeeIdAsync(this.startDate, this.endDate, this.selectedEmployee.id!)
          .subscribe(res => {
            if (res.isSuccess) {
              this.attendances = res.data;
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
      } else {
        this.attendanceService.getByDateRangeAsync(this.startDate, this.endDate)
          .subscribe(res => {
            if (res.isSuccess) {
              this.attendances = res.data;
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
          }, error => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error.message,
              life: 3000
            });
          });
      }
    }
  }

  create() {
    this.newAttendance = {} as AttendanceDto;
    this.newAttendance.date = new Date();
    this.createModal = true;
    this.dialogVisible = true;
    this.submitted = false;
  }

  edit(attendance: AttendanceDto) {
    this.newAttendance = { ...attendance };
    this.newAttendance.date = new Date(attendance.date)
    this.editModal = true;
    this.dialogVisible = true;
    this.submitted = false;
  }

  delete(attendance: AttendanceDto) {
    this.newAttendance = { ...attendance };
    this.deleteModal = true;
  }

  save() {
    this.submitted = true;

    if (!this.newAttendance.employeeId || !this.newAttendance.date) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.attendanceService.createAsync(this.newAttendance)
      .subscribe(
        res => {
          this.attendances = [...this.attendances, res.data];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Attendance record created successfully',
            life: 3000
          });
          this.hideDialog();
        },
        error => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message,
            life: 3000
          });
        }
      );
  }

  update() {
    this.submitted = true;

    if (!this.newAttendance.employeeId || !this.newAttendance.date) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.attendanceService.updateAsync(this.newAttendance)
      .subscribe(
        res => {
          const index = this.attendances.findIndex(x => x.id === this.newAttendance.id);
          this.attendances[index] = res.data;
          this.attendances = [...this.attendances];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Attendance record updated successfully',
            life: 3000
          });
          this.hideDialog();
        },
        error => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message,
            life: 3000
          });
        }
      );
  }

  confirmDelete() {
    if (this.newAttendance.id) {
      this.attendanceService.deleteAsync(this.newAttendance.id)
        .subscribe(
          success => {
            if (success) {
              this.attendances = this.attendances.filter(x => x.id !== this.newAttendance.id);
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Attendance record deleted successfully',
                life: 3000
              });
            }
            this.hideDialog();
          },
          error => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error.message,
              life: 3000
            });
          }
        );
    }
  }

  hideDialog() {
    this.createModal = false;
    this.editModal = false;
    this.dialogVisible = false;
    this.deleteModal = false;
    this.submitted = false;
    this.newAttendance = {} as AttendanceDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
} 
