import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Currency } from "../enums/currency";

export interface ExchangeRateDto extends FullAuditedEntityDto<number> {
    currency?: Currency;
    baseCurrency?: Currency;
    baseToRate: number;
    dateEffected: Date;
}
