import { Currency } from "../enums/currency";

export interface CreditReportDto {
    usdAmount?: number;
    currency: Currency;
    inHand?: number;
    varience?: number;
    amount?: number;
}
