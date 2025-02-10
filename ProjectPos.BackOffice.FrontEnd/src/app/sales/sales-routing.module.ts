import { ManageSalesComponent } from './manage-sales/manage-sales.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DayEndComponent } from './day-end/day-end.component';

const routes: Routes = [
  {
    path: 'manage-sales',
    component: ManageSalesComponent
  },
  {
    path: 'end-day',
    component: DayEndComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule { }
