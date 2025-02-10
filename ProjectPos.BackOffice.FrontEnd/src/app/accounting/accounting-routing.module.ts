import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoicingComponent } from './invoicing/invoicing.component';
import { PaymentsComponent } from './payments/payments.component';
import { ExpensesComponent } from './expenses/expenses.component';

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
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountingRoutingModule { }
