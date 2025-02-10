import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { CompanyDto } from "./company-dto";
import { GoodsReceivedVoucherLineDto } from "./goods-received-voucher-line-dto";
import { PurchaceOrderDto } from "./purchace-order-dto";

export interface GoodsReceivedVoucherDto extends FullAuditedEntityDto<number> {
    orderNumber?: number;
    invoiceNumber?: string;
    transpoter?: string;
    supplierId?: number;
    supplierName?: string;
    value?: number;
    purchaseOrder?: PurchaceOrderDto;
    supplier?: CompanyDto;
    receivedItems?: GoodsReceivedVoucherLineDto[];
}
