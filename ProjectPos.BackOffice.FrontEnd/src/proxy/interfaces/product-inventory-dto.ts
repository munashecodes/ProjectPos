import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Category } from "../enums/category";
import { Grade } from "../enums/grade";
import { Status } from "../enums/status";
import { Unit } from "../enums/unit";
import { ProductDto } from "./product-dto";
import { ProductPriceDto } from "./product-price-dto";

export interface ProductInventoryDto extends FullAuditedEntityDto<number> {
    productId?: number;
    barCode?: string;
    description?: string
    subCategoryName?: string;
    pLUCode?: string;
    flag?: number;
    isWeighted?: boolean;
    name?: string;
    grade?: Grade;
    idealQuantity?: number;
    quantityOnHand?: number;
    quantityOnShelf?: number;
    stockCount?: number;
    quantity?: number;
    cost?: number;
    markUp?: number;
    unit?: Unit;
    status?: Status;
    category?: Category;
    product?: ProductDto;
    isTaxable?:boolean
    subCategoryId?: number;
    
    productPrice?: ProductPriceDto;
}
