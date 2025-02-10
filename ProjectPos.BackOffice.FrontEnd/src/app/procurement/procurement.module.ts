import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProcurementRoutingModule } from './procurement-routing.module';
import { PurchaseOrderComponent } from './purchase-order/purchase-order.component';
import { GoodsReceivedVoucherComponent } from './goods-received-voucher/goods-received-voucher.component';
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
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { OrderPaymentStatusPipe } from 'src/proxy/pipes/order-payment-status.pipe';
import { GoodsReturnedNoteComponent } from './goods-returned-note/goods-returned-note.component';


@NgModule({
  declarations: [
    PurchaseOrderComponent,
    GoodsReceivedVoucherComponent,
    GoodsReturnedNoteComponent
  ],
  imports: [
    CommonModule,
    ProcurementRoutingModule,
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
    TabMenuModule,
    TabViewModule,
    ConfirmDialogModule,
    OrderPaymentStatusPipe
  ],
  providers: [
    ConfirmationService
  ]
})
export class ProcurementModule { }
