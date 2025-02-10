import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
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
    company?: CompanyDto;
    purchaceOrderItems?: PurchaceOrderLineDto[];
    goodsReceivedVouchers?: GoodsReceivedVoucherDto[];
}
