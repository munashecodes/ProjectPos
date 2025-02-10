import { Category } from "../enums/category";
import { SnapShotEnum } from "../enums/snap-shot-enum";

export interface InventorySnapShotSummaryDto {
    userId?: number;
    snapShotType?: SnapShotEnum;
    department?: Category;
}
