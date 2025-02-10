import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageRatesComponent } from './manage-rates/manage-rates.component';
import { CashierReportsComponent } from './cashier-reports/cashier-reports.component';
import { ReconReportsComponent } from './recon-reports/recon-reports.component';

const routes: Routes = [
  {
    path: 'manage-rates',
    component: ManageRatesComponent
  },
  {
    path: 'cashier-reports',
    component: CashierReportsComponent
  },
  {
    path: 'recon-reports',
    component: ReconReportsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CashierRoutingModule { }
