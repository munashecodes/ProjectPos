import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { TransactionType } from "../enums/transaction-type";
import { PurchaceInvoiceLineDto } from "./purchace-invoice-line-dto";

export interface PurchaceInvoiceDto extends FullAuditedEntityDto<number> {
    invoiceNumber: number;
    supplier?: string;
    totalValue: number;
    transactionType?: TransactionType;
    date: Date;
    invoiceLines?: PurchaceInvoiceLineDto[];
}
