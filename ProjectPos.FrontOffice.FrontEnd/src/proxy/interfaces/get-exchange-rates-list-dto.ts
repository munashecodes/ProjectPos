import { Currency } from "../enums/currency";
import { ExchangeRateDto } from "./exchange-rate-dto";

export interface GetExchangeRatesListDto {
    currency?: Currency;
    exchangeRates?: ExchangeRateDto[];
}
