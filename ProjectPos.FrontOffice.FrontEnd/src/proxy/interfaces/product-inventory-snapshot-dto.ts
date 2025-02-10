import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Grade } from "../enums/grade";
import { Status } from "../enums/status";
import { Unit } from "../enums/unit";
import { ProductDto } from "./product-dto";

export interface ProductInventorySnapshotDto extends FullAuditedEntityDto<number> {
    productId?: number;
    idealQuantity?: number;
    quantityOnHand?: number;
    quantityOnShelf?: number;
    unit?: Unit;
    grade?: Grade;
    status?: Status;
    products?: ProductDto;
}
