import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { TransactionType } from "../enums/transaction-type";
import { ProductDto } from "./product-dto";
import { ProductInventoryDto } from "./product-inventory-dto";
import { StockMovementLogDto } from "./stock-movement-log-dto";

export interface StockMovementDto extends FullAuditedEntityDto<number> {
    productInventoryId?: number;
    batchNumber?: number;
    productName?: string;  
    barCode?: string; 
    quantity?: number;
    transactionType?: TransactionType;
    isssuedTo?: string;
    isAuthorised?: boolean;
    authorisedById?: number;
    categoryId?: number;
    categoryName?: string;
    comment?: string;
    product?: ProductInventoryDto;
    batch?: StockMovementLogDto;
}
