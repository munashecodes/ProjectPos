import { EntityDto } from "../entity-dtos/entity-dto";
import { Category } from "../enums/category";
import { Unit } from "../enums/unit";
import { ProductDto } from "./product-dto";
import { ProductInventoryDto } from "./product-inventory-dto";

export interface SalesOrderItemDto extends EntityDto<number> {
    rowIndex?: number;
    orderNumber?: number;
    productName?: string;
    category?: Category;
    subCategory?: string;
    isTaxable?: boolean;
    barCode?: string;
    productId?: number;
    product?: ProductInventoryDto;
    vat?: number;
    vatPercentage?: number;
    quantity?: number;
    unit?: Unit;
    unitPrice?: number;
    price?: number;
    isReturned?: boolean;
    currencyUnitPrice?: number;
    currencyPrice?: number;
}
