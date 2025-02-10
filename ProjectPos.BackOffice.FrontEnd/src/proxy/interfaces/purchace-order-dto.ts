import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { OrderReceivedStatus } from "../enums/order-received-status";
import { CompanyDto } from "./company-dto";
import { GoodsReceivedVoucherDto } from "./goods-received-voucher-dto";
import { PurchaceOrderLineDto } from "./purchace-order-line-dto";

export interface PurchaceOrderDto extends FullAuditedEntityDto<number> {
    invoiceValue?: number;
    isReceived: boolean;
    isOpen: boolean;
    supplierId?: number;
    supplierName?: string;
    isApproved: boolean;
    approvedById?: number;
    eta: Date;
    status?: OrderReceivedStatus;
    company?: CompanyDto;
    purchaceOrderItems?: PurchaceOrderLineDto[];
    goodsReceivedVouchers?: GoodsReceivedVoucherDto[];
}
