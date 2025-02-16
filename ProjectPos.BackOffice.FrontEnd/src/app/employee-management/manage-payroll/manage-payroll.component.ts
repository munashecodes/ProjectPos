import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { PaymentMethod } from 'src/proxy/enums/payment-method';
import { PayRollCycleDto } from 'src/proxy/interfaces/payroll-cycle-dto';
import { PayRollStatus } from 'src/proxy/interfaces/payroll-status';
import { PaySlipDto } from 'src/proxy/interfaces/payslip-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { PaySlipService } from 'src/proxy/services/pay-slip.service';

@Component({
  selector: 'app-manage-payroll',
  templateUrl: './manage-payroll.component.html',
  styleUrls: ['./manage-payroll.component.scss']
})
export class ManagePayrollComponent implements OnInit {
  
  payrollCycles: PayRollCycleDto[] = [];
  selectedCycle: PayRollCycleDto | null = null;
  paySlips: PaySlipDto[] = [];
  
  payrollStatus = PayRollStatus;
  years: number[] = [];
  selectedYear: number = new Date().getFullYear();

  payslipDialog = false;
  editDialog = false;
  selectedPayslip: PaySlipDto | null = null;

  paymentMethods = Object.values(PaymentMethod);

  user: UserDto = {} as UserDto;

  constructor(
    private paySlipService: PaySlipService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    this.initializeYears();
    this.loadPayrollCycles();
  }

  initializeYears() {
    const currentYear = new Date().getFullYear();
    for(let i = 0; i < 5; i++) {
      this.years.push(currentYear - i);
    }
  }

  loadPayrollCycles() {
    this.paySlipService.getAllPayRollCyclesByYearAsync(this.selectedYear)
      .subscribe(res => {
        if (res.isSuccess) {
          this.payrollCycles = res.data;
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

  generatePayroll() {
    // Using 1 as userId for now - should come from auth service
    this.paySlipService.generatePayRollAsync(this.user.id!)
      .subscribe(res => {
        if (res.isSuccess) {
          this.loadPayrollCycles();
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Payroll generated successfully',
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

  approvePayroll(cycle: PayRollCycleDto) {
    // Using 1 as userId for now - should come from auth service
    this.paySlipService.approvePayRollAsync(cycle.month!, cycle.year!, this.user.id!)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.payrollCycles.findIndex(x => 
            x.month === cycle.month && x.year === cycle.year);
          this.payrollCycles[index] = res.data;
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Payroll approved successfully',
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

  viewPayslips(cycle: PayRollCycleDto) {
    this.selectedCycle = cycle;
    this.paySlipService.getPayRollAsync(cycle.month!, cycle.year!)
      .subscribe(res => {
        if (res.isSuccess) {
          this.paySlips = res.data.paySlips || [];
          this.payslipDialog = true;
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

  editPayslip(payslip: PaySlipDto) {
    this.selectedPayslip = { ...payslip };
    this.editDialog = true;
  }

  savePayslip() {
    if (this.selectedPayslip) {
      this.paySlipService.editPaySlipAsync(this.selectedPayslip)
        .subscribe(res => {
          if (res.isSuccess) {
            const index = this.paySlips.findIndex(x => x.id === this.selectedPayslip!.id);
            this.paySlips[index] = res.data;
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Payslip updated successfully',
              life: 3000
            });
            this.editDialog = false;
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

  calculateTotals() {
    return this.paySlips.reduce((totals, payslip) => {
      return {
        basicSalary: (totals.basicSalary || 0) + (payslip.basicSalary || 0),
        allowance: (totals.allowance || 0) + (payslip.allowance || 0),
        grossSalary: (totals.grossSalary || 0) + (payslip.grossSalary || 0),
        totalDeduction: (totals.totalDeduction || 0) + (payslip.totalDeduction || 0),
        totalNetSalary: (totals.totalNetSalary || 0) + (payslip.totalNetSalary || 0)
      };
    }, {} as Partial<PaySlipDto>);
  }

  onYearChange() {
    this.loadPayrollCycles();
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  getMonthName(month: number): string | null {
    const monthNames: string[] = [
        'January',
        'February',
        'March',
        'April',
        'May',
        'June',
        'July',
        'August',
        'September',
        'October',
        'November',
        'December'
    ];

    if (month < 1 || month > 12 || isNaN(month)) {
        return null; // or throw an error if you prefer
    }
    return monthNames[month - 1]; // month - 1 because array is 0-indexed
}

  getStatusSeverity(status: string): string {
    switch (status) {
      case PayRollStatus.Pending:
        return 'warning';
      case PayRollStatus.Processed:
        return 'info';
      case PayRollStatus.Approved:
        return 'success';
      case PayRollStatus.Cancelled:
        return 'danger';
      default:
        return 'info';
    }
  }
} 