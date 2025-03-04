import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { IncomeStatementDto } from 'src/proxy/interfaces/income-statement-dto';
import { IncomeStatementService } from 'src/proxy/services/income-statement.service';

@Component({
  selector: 'app-income-statement',
  templateUrl: './income-statement.component.html',
  styleUrls: ['./income-statement.component.scss']
})
export class IncomeStatementComponent implements OnInit {
  
  incomeStatement: IncomeStatementDto | null = null;
  startDate: Date = new Date();
  endDate: Date = new Date();
  loading = false;

  constructor(
    private incomeStatementService: IncomeStatementService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    // Set default date range to current month
    this.startDate = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
    this.endDate = new Date();
    this.generateReport();
  }

  generateReport() {
    this.loading = true;
    const start = new Date(new Date(this.startDate).getTime() + (2 * 60 * 60 * 1000)).toUTCString();
    const end = new Date(new Date(this.endDate).getTime() + (2 * 60 * 60 * 1000)).toUTCString();
    this.incomeStatementService.generateIncomeStatementAsync(start, end)
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.incomeStatement = response.data;
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: response.message,
              life: 3000
            });
          }
          this.loading = false;
        },
        error: (error) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message,
            life: 3000
          });
          this.loading = false;
        }
      });
  }

  onDateChange() {
    this.generateReport();
  }

  // Helper method to format currency
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }
}
