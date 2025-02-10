import { Currency } from "../enums/currency";

export interface ReconDto {
    currency?: Currency;
    rate?: number;
    amount?: number;
    usdAmount?: number;
    salesAmount?: number;
    variance?: number;
    cashUpDate?: Date;
    userName?: string;
    userId?: number;
}
