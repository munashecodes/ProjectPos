import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { PaymentMethod } from "../enums/payment-method";
import { SaleType } from "../enums/sale-type.enum";
import { AccountDto } from "./account-dto";
import { CompanyDto } from "./company-dto";

export interface ExpenseDto extends FullAuditedEntityDto<number> {
    description?: string; // Optional note for the expense
    receiptAttachmentPath?: string; // Optional path to an uploaded receipt or invoice
    accountId: number; // Required Account ID
    paymentMethod?: SaleType; // Optional PaymentMethod enum
    payee: string; // Required: Person or company receiving the payment
    companyId?: number; // Optional: Company the expense is for
    amount: number; // Required: Expense amount
    isApproved?: boolean; // Optional: Whether the expense is approved
    approvedById?: number; // Optional: User ID of approver
    companyName?: string; // Optional: Company name
    accountName?: string; // Optional: Account name

    account?: AccountDto; // Optional: Related Account entity
    company?: CompanyDto; // Optional: Related Company entity
}
