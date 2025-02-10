import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { OrderPayStatus } from "../enums/order-pay-status";
import { OrderPaymentStatus } from "../enums/order-payment-status";
import { CompanyDto } from "./company-dto";
import { GoodsReceivedVoucherLineDto } from "./goods-received-voucher-line-dto";
import { PurchaceOrderDto } from "./purchace-order-dto";
import { PurchaceOrderPaymentDto } from "./purchace-order-payment-dto";

export interface GoodsReceivedVoucherDto extends FullAuditedEntityDto<number> {
    orderNumber?: number;
    invoiceNumber?: string;
    transpoter?: string;
    supplierId?: number;
    supplierName?: string;
    value?: number;
    isApproved?: boolean;
    paidAmount?: number;
    isPaid?: boolean;
    usdPaidAmount?: number;
    amountDue?: number;
    status?: OrderPaymentStatus;
    purchaseOrder?: PurchaceOrderDto;
    supplier?: CompanyDto;
    receivedItems?: GoodsReceivedVoucherLineDto[];
    purchaceOrderPayments?: PurchaceOrderPaymentDto[];
}
