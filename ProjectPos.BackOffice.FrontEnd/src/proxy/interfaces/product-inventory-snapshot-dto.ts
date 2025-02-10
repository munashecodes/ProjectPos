import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Grade } from "../enums/grade";
import { SnapShotEnum } from "../enums/snap-shot-enum";
import { Status } from "../enums/status";
import { Unit } from "../enums/unit";
import { ProductDto } from "./product-dto";

export interface ProductInventorySnapshotDto extends FullAuditedEntityDto<number> {
    inventory?: string;
    snapShotType?: SnapShotEnum;
}
