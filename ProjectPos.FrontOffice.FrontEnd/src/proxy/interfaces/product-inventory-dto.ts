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
    flag?: number;
    pLUCode?: string;
    isWeighted?: boolean;
    name?: string;
    grade?: Grade;
    rating?: number;
    idealQuantity?: number;
    quantityOnHand?: number;
    quantityOnShelf?: number;
    unit?: Unit;
    status?: Status;
    category?: Category;
    product?: ProductDto;
    subCategoryId?: number;
    productPrice?: ProductPriceDto;
}
