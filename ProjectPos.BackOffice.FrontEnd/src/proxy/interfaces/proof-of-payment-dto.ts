import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Currency } from "../enums/currency";
import { CompanyDto } from "./company-dto";
import { SalesOrderDto } from "./sales-order-dto";

export interface ProofOfPaymentDto extends FullAuditedEntityDto<number> {
    reference?: string;
    customerId?: number;
    bank?: string;
    customerName?: string;
    creatorName?: string;
    branchCode?: string;
    currency?: Currency;
    paidAmount?: number;
    usableAmount?: number;
    usedAmount?: number;
    bankingDate?: Date;
    customer?: CompanyDto
}
