import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { TransactionType } from "../enums/transaction-type";
import { ProductDto } from "./product-dto";

export interface StockMovementDto extends FullAuditedEntityDto<number> {
    productId?: number;
    batchNumber?: number;
    quantity?: number;
    transactionType?: TransactionType;
    isssuedTo?: string;
    isAuthorised?: boolean;
    authorisedById: number;
    categoryId?: number;
    categoryName?: string;
    comment?: string;
    product?: ProductDto;
}
