import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportingRoutingModule } from './reporting-routing.module';
import { StockTakeComponent } from './stock-take/stock-take.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ChartModule } from 'primeng/chart';
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
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
import { TabMenuModule } from 'primeng/tabmenu';
import { TabViewModule } from 'primeng/tabview';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DashboardsRoutingModule } from '../demo/components/dashboard/dashboard-routing.module';
import { CrudRoutingModule } from '../demo/components/pages/crud/crud-routing.module';
import { InventoryRoutingModule } from '../inventory/inventory-routing.module';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { CostOfGoodsSoldReportComponent } from './cost-of-goods-sold-report/cost-of-goods-sold-report.component';
import { SalesReportComponent } from './sales-report/sales-report.component';
import { CardModule } from 'primeng/card';



@NgModule({
  declarations: [
    StockTakeComponent,
    CostOfGoodsSoldReportComponent,
    SalesReportComponent
  ],
  imports: [
    CommonModule,
    ReportingRoutingModule,
    InventoryRoutingModule,
    FormsModule,
    ChartModule,
    MenuModule,
    TableModule,
    StyleClassModule,
    PanelMenuModule,
    ButtonModule,
    CardModule,
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
    DataViewModule,
    BadgeModule,
    AvatarModule,
    TabMenuModule,
    TabViewModule,
    FieldsetModule,
    ConfirmDialogModule,
    CalendarModule,
  
  ],
  providers: [
    ConfirmationService
  ]
})
export class ReportingModule { }
