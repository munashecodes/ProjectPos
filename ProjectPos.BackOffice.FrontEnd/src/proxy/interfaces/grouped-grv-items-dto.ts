import { Category } from "../enums/category";
import { Unit } from "../enums/unit";
import { ProductInventoryDto } from "./product-inventory-dto";

export interface GroupedGrvItemsDto {
    orderNumber?: number;
    productName?: string;
    category?: Category;
    subCategory?: string;
    subCategoryId?: number;
    sellingPrice?: number;
    barCode?: string;
    productId?: number;
    product?: ProductInventoryDto;
    quantity?: number;
    vouvherNumber?: number;
    unit?: Unit;
    unitCost?: number;
    totalCost?: number;
    currencyUnitPrice?: number;
    currencyPrice?: number;
    openingQuantity?: number;
    closingQuantity?: number;
    openingStock?: number;
    closingStock?: number;
    profitMade?: number;
    revenueMade?: number;
}