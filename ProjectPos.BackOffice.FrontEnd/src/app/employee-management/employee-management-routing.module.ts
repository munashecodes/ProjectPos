import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageEmployeesComponent } from './manage-employees/manage-employees.component';
import { ManageEmployeeDetailsComponent } from './manage-employee-details/manage-employee-details.component';
import { ManageSalaryStructureComponent } from './manage-salary-structure/manage-salary-structure.component';
import { ManageEmployeeDeductionComponent } from './manage-employee-deduction/manage-employee-deduction.component';
import { ManagePayrollComponent } from './manage-payroll/manage-payroll.component';
import { ManageAttendanceComponent } from './manage-attendance/manage-attendance.component';
import { ManageOvertimeComponent } from './manage-overtime/manage-overtime.component';

const routes: Routes = [
  {
    path : 'manage-employees',
    component : ManageEmployeesComponent
  },
  {
    path: 'manage-employee-details',
    component: ManageEmployeeDetailsComponent
  },
  {
    path: 'manage-salary-structure',
    component: ManageSalaryStructureComponent
  },
  {
    path: 'manage-deductions',
    component: ManageEmployeeDeductionComponent
  },
  {
    path: 'manage-payroll',
    component: ManagePayrollComponent
  },
  {
    path: 'manage-attendance',
    component: ManageAttendanceComponent
  },
  {
    path: 'manage-overtime',
    component: ManageOvertimeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeManagementRoutingModule { }
