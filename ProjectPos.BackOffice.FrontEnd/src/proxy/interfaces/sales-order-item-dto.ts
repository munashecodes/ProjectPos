import { EntityDto } from "../entity-dtos/entity-dto";
import { Category } from "../enums/category";
import { Unit } from "../enums/unit";
import { ProductDto } from "./product-dto";
import { ProductInventoryDto } from "./product-inventory-dto";

export interface SalesOrderItemDto extends EntityDto<number> {
    orderNumber?: number;
    productName?: string;
    category?: Category;
    subCategory?: string;
    subCategoryId?: number;
    barCode?: string;
    productId?: number;
    product?: ProductInventoryDto;
    quantity?: number;
    unit?: Unit;
    unitPrice?: number;
    price?: number;
    currencyUnitPrice?: number;
    currencyPrice?: number;
}
