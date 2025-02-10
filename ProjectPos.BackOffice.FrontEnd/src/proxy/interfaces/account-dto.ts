import { AuditedEntityDto } from "../entity-dtos/audited-entity-dto";
import { AccountType } from "../enums/account-type";
import { AccountCategoryDto } from "./account-category-dto";
import { ExpenseDto } from "./expense-dto";

export interface AccountDto extends AuditedEntityDto<number> {
    code?: number; // Optional number property
    name?: string; // Optional string property
    description?: string; // Optional string property
    balance?: number; // Optional number property to handle decimals
    accountType?: AccountType; // Optional AccountType enum
    accountCategoryId?: number; // Optional number property

    accountCategory?: AccountCategoryDto; // Optional related entity
    expenses?: ExpenseDto[]; // Optional collection of related entities
}
