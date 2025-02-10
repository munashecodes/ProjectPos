import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GoodsReceivedVoucherComponent } from './goods-received-voucher/goods-received-voucher.component';
import { PurchaseOrderComponent } from './purchase-order/purchase-order.component';

const routes: Routes = [
  {
    path: 'orders',
    component: PurchaseOrderComponent
  },
  {
    path: 'grvs',
    component: GoodsReceivedVoucherComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProcurementRoutingModule { }
