import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoicingComponent } from './invoicing/invoicing.component';
import { PaymentsComponent } from './payments/payments.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { IncomeStatementComponent } from './income-statement/income-statement.component';
import { TrialBalanceComponent } from './trial-balance/trial-balance.component';

const routes: Routes = [
  {
    path: 'invoicing',
    component: InvoicingComponent
  },
  {
    path: 'proof-of-payments',
    component: PaymentsComponent
  },
  {
    path: 'expenses',
    component: ExpensesComponent
  },
  {
    path: 'income-statement',
    component: IncomeStatementComponent
  },
  {
    path: 'trial-balance',
    component: TrialBalanceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountingRoutingModule { }
