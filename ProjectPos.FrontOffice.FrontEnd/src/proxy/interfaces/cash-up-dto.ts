import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Currency } from "../enums/currency";

export interface CashUpDto extends FullAuditedEntityDto<number> {
    currency?: Currency;
    rate?: number;
    amount?: number;
    usdAmount?: number;
    userName?: string;
}

