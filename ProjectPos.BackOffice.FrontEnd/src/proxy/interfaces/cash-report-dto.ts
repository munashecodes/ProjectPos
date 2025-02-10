import { Currency } from "../enums/currency";

export interface CashReportDto {
    usdAmount?: number;
    currency: Currency;
    inHand?: number;
    varience?: number;
    amount?: number;
}
