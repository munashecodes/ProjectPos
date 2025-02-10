import { SalesOrderDto } from "./sales-order-dto";

export interface GetSalesOrderByUserDto {
    UserId?: number;
    UserName?: string;
    SalesOrders?: SalesOrderDto[];
    AmountTotal?: number;
    PaidTotal?: number;
    BalanceTotal?: number;
}
