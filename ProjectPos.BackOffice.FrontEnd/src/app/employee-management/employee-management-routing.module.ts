import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageEmployeesComponent } from './manage-employees/manage-employees.component';

const routes: Routes = [
  {
    path : 'manage-employees',
    component : ManageEmployeesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeManagementRoutingModule { }
