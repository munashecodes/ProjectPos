import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ChartModule } from 'primeng/chart';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { FileUploadModule } from 'primeng/fileupload';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MenuModule } from 'primeng/menu';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { PanelMenuModule } from 'primeng/panelmenu';
import { PickListModule } from 'primeng/picklist';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RatingModule } from 'primeng/rating';
import { RippleModule } from 'primeng/ripple';
import { SplitButtonModule } from 'primeng/splitbutton';
import { SplitterModule } from 'primeng/splitter';
import { StyleClassModule } from 'primeng/styleclass';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DashboardsRoutingModule } from '../demo/components/dashboard/dashboard-routing.module';
import { CrudRoutingModule } from '../demo/components/pages/crud/crud-routing.module';
import { EmployeeManagementRoutingModule } from './employee-management-routing.module';
import { ManageEmployeesComponent } from './manage-employees/manage-employees.component';
import { ManageEmployeeDetailsComponent } from './manage-employee-details/manage-employee-details.component';
import { TagModule } from 'primeng/tag';
import { InputSwitchModule } from 'primeng/inputswitch';
import { ManageSalaryStructureComponent } from './manage-salary-structure/manage-salary-structure.component';
import { ManageEmployeeDeductionComponent } from './manage-employee-deduction/manage-employee-deduction.component';
import { ManagePayrollComponent } from './manage-payroll/manage-payroll.component';
import { ManageOvertimeComponent } from './manage-overtime/manage-overtime.component';
import { ManageAttendanceComponent } from './manage-attendance/manage-attendance.component';
import { SelectButtonModule } from 'primeng/selectbutton';


@NgModule({
  declarations: [
    ManageEmployeesComponent,
    ManageEmployeeDetailsComponent,
    ManageEmployeeDeductionComponent,
    ManageSalaryStructureComponent,
    ManagePayrollComponent,
    ManageOvertimeComponent,
    ManageAttendanceComponent,
  ],
  imports: [
    CommonModule,
    EmployeeManagementRoutingModule,
    FormsModule,
    ChartModule,
    MenuModule,
    TableModule,
    StyleClassModule,
    PanelMenuModule,
    ButtonModule,
    DashboardsRoutingModule,
    ToastModule,
    DialogModule,
    CrudRoutingModule,
    FileUploadModule,
    RippleModule,
    ToolbarModule,
    RatingModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    RadioButtonModule,
    InputNumberModule,
    CalendarModule,
    SplitButtonModule,
    PickListModule,
    SplitterModule,
    ReactiveFormsModule,
    MultiSelectModule,
    MessageModule,
    TagModule,
    InputSwitchModule,
    DialogModule,
    SelectButtonModule
  ]
})
export class EmployeeManagementModule { }
