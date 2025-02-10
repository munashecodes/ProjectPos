
import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { AccountType } from 'src/proxy/enums/account-type';
import { SaleType } from 'src/proxy/enums/sale-type.enum';
import { AccountCategoryDto } from 'src/proxy/interfaces/account-category-dto';
import { AccountDto } from 'src/proxy/interfaces/account-dto';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { ExpenseDto } from 'src/proxy/interfaces/expense-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AccountCategoryService } from 'src/proxy/services/account-category.service';
import { AccountService } from 'src/proxy/services/account.service';
import { CompanyService } from 'src/proxy/services/company.service';
import { ExpenseService } from 'src/proxy/services/expense.service';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.scss']
})
export class ExpensesComponent implements OnInit {

  expenses: ExpenseDto[] = [];

  filteredExpenses: ExpenseDto[] = [];

  selectedExpense: ExpenseDto = {} as ExpenseDto;

  newExpense: ExpenseDto = {} as ExpenseDto;

  paymentMethods: SaleType[] = Object.values(SaleType);

  companies: CompanyDto[] = [];

  accountCategories: AccountCategoryDto[] = [];

  accountCategory: AccountCategoryDto = {} as AccountCategoryDto;

  filteredAccountCategories: AccountCategoryDto[] = [];

  company: CompanyDto = {} as CompanyDto

  account: AccountDto = {} as AccountDto;

  acoounts: AccountDto[] = [];

  filteredAcoounts: AccountDto[] = [];

  accountTypes: AccountType[] = Object.values(AccountType);

  accountType: AccountType = AccountType.Expense;

  submitted = false;

  createModal = false;

  editModal = false;

  deleteModal = false;
  
  cols: any[] = [];

  user: UserDto = {} as UserDto;

  constructor(
    private expenseService: ExpenseService,
    private companyService: CompanyService,
    private accountService: AccountService,
    private accountCategoryService: AccountCategoryService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}
  

  ngOnInit(): void {

    this.user = JSON.parse(sessionStorage.getItem('loggedUser')!);

    this.companyService.getAllList()
    .subscribe((res) => {
      this.companies = res.data;

    });

    this.accountCategoryService.getAllasync()
    .subscribe((res) => {
      this.accountCategories = res.data;
      this.filteredAccountCategories = res.data.filter((item: any) => item.accountType === AccountType.Expense);
    });

    this.accountService.getAllAsync()
    .subscribe((res) => {
      this.acoounts = res.data;
      this.filteredAcoounts = res.data.filter((item: any) => item.accountType === AccountType.Expense);
    });

    this.getAllExpenses()
  }

  // Get all expenses
  getAllExpenses() {
    this.expenseService.getAllList().subscribe((res) => {
      this.expenses = res.data;
      this.filteredExpenses = res.data;
    });
  }

  //open approve modal
  openApproveModal(expense: ExpenseDto) {
    this.selectedExpense = { ...expense };
    this.selectedExpense.approvedById = this.user.id;
    this.selectedExpense.isApproved = true;
    this.confirmationService.confirm({
      message: 'Are you sure you want to approve this expense?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.expenseService.approve(this.selectedExpense).subscribe(
          (res) => {
            if (res.isSuccess) {
              this.expenses = this.expenses.map((item) =>
                item.id === res.data.id ? res.data : item
              );
              this.filteredExpenses = this.expenses;

              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'ExpenseDto approved successfully',
              });
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: res.message,
              });
            }
          },
          (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error.message,
            });
          }
        );
      },
    });
  }

  // Open the create modal
  openCreateModal() {
    this.newExpense = {} as ExpenseDto;
    this.createModal = true;
    this.submitted = false;
  }

  //filter expenses by account category
  filterByAccountCategory() {
    this.filteredExpenses = this.expenses.filter((expense) => {
      return expense.account?.accountCategoryId === this.accountCategory.id;
    });
  }

  // Save new expense
  saveExpense() {
    this.submitted = true;

    if (!this.newExpense.payee || this.newExpense.amount === undefined) {
      return;
    }

    this.expenseService.create(this.newExpense).subscribe(
      (res) => {
        if (res.isSuccess) {
          this.expenses = [...this.expenses, res.data];
          this.filteredExpenses = this.expenses;

          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'ExpenseDto created successfully',
          });

          this.createModal = false;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
        });
      }
    );
  }

  // Open edit modal
  openEditModal(expense: ExpenseDto) {
    this.newExpense = { ...expense };
    this.editModal = true;
    this.submitted = false;
  }

  // Update expense
  updateExpense() {
    this.submitted = true;

    if (!this.newExpense.payee || this.newExpense.amount === undefined) {
      return;
    }

    this.expenseService.update(this.newExpense).subscribe(
      (res) => {
        if (res.isSuccess) {
          this.expenses = this.expenses.map((item) =>
            item.id === res.data.id ? res.data : item
          );
          this.filteredExpenses = this.expenses;

          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'ExpenseDto updated successfully',
          });

          this.editModal = false;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
        });
      }
    );
  }

  // Open delete confirmation modal
  openDeleteModal(expense: ExpenseDto) {
    this.selectedExpense = { ...expense };
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the expense?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.expenseService.delete(this.selectedExpense.id!, this.user.id!).subscribe(
          (res) => {
            if (res.isSuccess) {
              this.expenses = this.expenses.filter(e => e.id !== this.selectedExpense.id);
              this.filteredExpenses = this.expenses;

              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'ExpenseDto Removed successfully',
              });
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: res.message,
              });
            }
          },
          (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error.message,
            });
          }
        );
      },
      reject: () => {
        this.deleteModal = false;
      }
    });
  }

  // Delete expense
  deleteExpense(expense: ExpenseDto) {
    this.selectedExpense = { ...expense };
    this.expenseService.delete(this.selectedExpense.id!, this.user.id!)
    .subscribe(
      (res) => {
        if (res.isSuccess) {
          this.expenses = this.expenses.filter(
            (item) => item.id !== this.selectedExpense.id
          );
          this.filteredExpenses = this.expenses;

          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'ExpenseDto deleted successfully',
          });

          this.deleteModal = false;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
        });
      }
    );
  }

  // Filter expenses globally
  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  // Close modal dialogs
  hideDialog() {
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.submitted = false;
  }
}
