import { EntityDto } from "../entity-dtos/entity-dto";
import { Unit } from "../enums/unit";
import { GoodsReceivedVoucherDto } from "./goods-received-voucher-dto";
import { ProductDto } from "./product-dto";
import { ProductInventoryDto } from "./product-inventory-dto";

export interface GoodsReceivedVoucherLineDto extends EntityDto<number> {
    voucherNumber?: number;
    productInventoryId?: number;
    productName?: string;
    barCode?: string;
    orderedQuantity?: number;
    receivedQuantity?: number;
    issuedQuantity?: number;
    qtyOnHand?: number;
    isIssued?: boolean;
    unit?: Unit;
    orderPrice?: number;
    unitPrice?: number;
    price?: number;
    receivedDate?: Date;
    expiryDate?: Date;
    product?: ProductInventoryDto;
    goodsReceivedVoucher?: GoodsReceivedVoucherDto;
}
