import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockTakeComponent } from './stock-take/stock-take.component';
import { CostOfGoodsSoldReportComponent } from './cost-of-goods-sold-report/cost-of-goods-sold-report.component';
import { SalesReportComponent } from './sales-report/sales-report.component';

const routes: Routes = [
  {
    path: 'stock-take',
    component: StockTakeComponent
  },
  {
    path: 'cogs-report',
    component: CostOfGoodsSoldReportComponent
  },
  {
    path: 'sales-report',
    component: SalesReportComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportingRoutingModule { }
