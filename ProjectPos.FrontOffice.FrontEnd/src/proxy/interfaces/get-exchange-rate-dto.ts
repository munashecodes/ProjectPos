import { Currency } from "../enums/currency";
import { ExchangeRateDto } from "./exchange-rate-dto";

export interface GetExchangeRateDto {
    currency?: Currency;
    exchangeRate?: ExchangeRateDto;
    dateEffected: Date;
}
