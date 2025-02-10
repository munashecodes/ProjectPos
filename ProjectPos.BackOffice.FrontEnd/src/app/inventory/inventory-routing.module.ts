import { ProductInventoryComponent } from './product-inventory/product-inventory.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockMovementComponent } from './stock-movement/stock-movement.component';

const routes: Routes = [
  {
    path: 'product-inventory',
    component: ProductInventoryComponent
  },
  {
    path: 'stock-movement',
    component: StockMovementComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule { }
