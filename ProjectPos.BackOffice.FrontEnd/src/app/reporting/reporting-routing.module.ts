import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockTakeComponent } from './stock-take/stock-take.component';

const routes: Routes = [
  {
    path: 'stock-take',
    component: StockTakeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportingRoutingModule { }
