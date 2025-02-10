import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Currency } from "../enums/currency";
import { SalesOrderDto } from "./sales-order-dto";

export interface ProofOfPaymentDto extends FullAuditedEntityDto<number> {
    UserId?: number;
    UserName?: string;
    SalesOrders?: SalesOrderDto[];
    AmountTotal?: number;
    PaidTotal?: number;
    BalanceTotal?: number;
    Reference?: string;
    CustomerId?: number;
    Bank?: string;
    CustomerName?: string;
    CreatorName?: string;
    BranchCode?: string;
    Currency?: Currency;
    PaidAmaount?: number;
    UsableAmaount?: number;
    BankingDate?: Date;
}
