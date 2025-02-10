import { EntityDto } from "../entity-dtos/entity-dto";
import { Unit } from "../enums/unit";
import { ProductDto } from "./product-dto";
import { ProductInventoryDto } from "./product-inventory-dto";
import { PurchaceOrderDto } from "./purchace-order-dto";

export interface PurchaceOrderLineDto extends EntityDto<number> {
    orderNumber?: number;
    productInventoryId?: number;
    name?: string;
    quantity?: number;
    unit?: Unit;
    unitPrice?: number;
    price?: number;
    purchaceOrder?: PurchaceOrderDto;
    product?: ProductInventoryDto;
}
