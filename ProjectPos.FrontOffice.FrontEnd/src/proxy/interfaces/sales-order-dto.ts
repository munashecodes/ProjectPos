import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Currency } from "../enums/currency";
import { SaleType } from "../enums/sale-type.enum";
import { SalesOrderStatus } from "../enums/sales-order-status";
import { CompanyDto } from "./company-dto";
import { PaymentDto } from "./payment-dto";
import { SalesOrderItemDto } from "./sales-order-item-dto";

export interface SalesOrderDto extends FullAuditedEntityDto<number>{
    saleType?: SaleType;
    price?: number;
    vat?: number;
    priceIncludingVat?: number;
    balance?: number;
    paidAmount?: number;
    currency?: Currency;
    isPaid?: boolean;
    customerId?: number;
    userName?: string;
    status?: SalesOrderStatus;
    customer?: CompanyDto;
    customerName?: string;
    salesOrderItems?: SalesOrderItemDto[];
    payments?: PaymentDto[];
}
