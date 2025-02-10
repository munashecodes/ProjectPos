import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { TransactionType } from "../enums/transaction-type";
import { StockMovementDto } from "./stock-movement-dto";

export interface StockMovementLogDto extends FullAuditedEntityDto<number> {
    transactionType?: TransactionType;
    isssuedTo?: string;
    isAuthorised?: boolean;
    authorisedById?: number;
    authorisedBy?: string;
    createdBy?: string;
    stockMovements?: StockMovementDto[];
}
