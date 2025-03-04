import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Currency } from "../enums/currency";
import { PaymentMethod } from "../enums/payment-method";
import { SaleType } from "../enums/sale-type.enum";

export interface PaymentDto extends FullAuditedEntityDto<number> {
    salesOrderId?: number;
    totalPrice?: number;
    amount?: number;
    paidAmount?: number;
    paidAmountAfterChange?: number;
    uSDPaidAmount?: number;
    uSDPaidAmountAfterChange?: number;
    changeAmount?: number;
    currency?: Currency;
    exchangeRate?: number;
    orderDate?: Date;
    vat?: number;
    ecocashSuccessCode?: string
    methodOfPay?: SaleType;
}
