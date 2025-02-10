import { EntityDto } from "../entity-dtos/entity-dto";
import { Unit } from "../enums/unit";
import { GoodsReceivedVoucherDto } from "./goods-received-voucher-dto";
import { ProductDto } from "./product-dto";

export interface GoodsReceivedVoucherLineDto extends EntityDto<number> {
    voucherNumber?: number;
    productId?: number;
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
    product?: ProductDto;
    goodsReceivedVoucher?: GoodsReceivedVoucherDto;
}
