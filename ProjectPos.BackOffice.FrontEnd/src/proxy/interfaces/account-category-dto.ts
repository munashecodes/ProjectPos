import { AuditedEntityDto } from "../entity-dtos/audited-entity-dto";
import { AccountType } from "../enums/account-type";

export interface AccountCategoryDto extends AuditedEntityDto<number> {
    name?: string; // Optional string property
    description?: string; // Optional string property
    accountType?: AccountType;
}
