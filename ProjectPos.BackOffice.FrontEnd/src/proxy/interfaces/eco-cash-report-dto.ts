import { Currency } from "../enums/currency";

export interface EcoCashReportDto {
    usdAmount?: number;
    currency: Currency;
    inHand?: number;
    varience?: number;
    amount?: number;
}
