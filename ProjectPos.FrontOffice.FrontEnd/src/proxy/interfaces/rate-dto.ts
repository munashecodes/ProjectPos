import { AuditedEntityDto } from "../entity-dtos/audited-entity-dto";
import { Currency } from "../enums/currency";

export interface RateDto extends AuditedEntityDto<number> {
    currency: Currency;
    dayRate: number;
}
