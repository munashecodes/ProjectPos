import { Currency } from "../enums/currency";

export interface CreditCardReportDto {
    usdAmount?: number;
    currency: Currency;
    inHand?: number;
    varience?: number;
    amount?: number;
}
