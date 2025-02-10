import { AuditedEntityDto } from "../entity-dtos/audited-entity-dto";
import { Category } from "../enums/category";

export interface StockTakeLogDto extends AuditedEntityDto<number> {
    department: Category
}
