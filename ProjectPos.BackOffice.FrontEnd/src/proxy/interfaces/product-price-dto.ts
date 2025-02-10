import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { ProductInventoryDto } from "./product-inventory-dto";

export interface ProductPriceDto extends FullAuditedEntityDto<number> {
    productInventoryId?: number;
    name?: string;
    barCode?: number;
    cost?: number;
    markUp?: number;
    price?: number;
    product?: ProductInventoryDto;
}
