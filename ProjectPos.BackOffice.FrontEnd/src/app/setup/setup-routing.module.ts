import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyComponent } from './company/company.component';
import { ContactPersonComponent } from './contact-person/contact-person.component';
import { SubCategoryComponent } from './sub-category/sub-category.component';
import { ProductComponent } from './product/product.component';
import { ProductPriceComponent } from './product-price/product-price.component';

const routes: Routes = [
  {
    path: 'companies',
    component: CompanyComponent
  },
  {
    path: 'contact-persons',
    component: ContactPersonComponent
  },
  {
    path: 'sub-categories',
    component: SubCategoryComponent
  },
  {
    path: 'products',
    component: ProductComponent
  },
  {
    path: 'product-prices',
    component: ProductPriceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SetupRoutingModule { }
