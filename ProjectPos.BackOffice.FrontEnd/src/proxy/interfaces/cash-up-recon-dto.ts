import { CashReportDto } from "./cash-report-dto";
import { CreditCardReportDto } from "./credit-card-report-dto";
import { CreditReportDto } from "./credit-report-dto";
import { EcoCashReportDto } from "./eco-cash-report-dto";

export interface CashUpReconDto {
    receiptNumber: number;
    paxName?: string;
    name?: string;
    surname?: string;
    changeAmount?: number;
    paymentTotal?: number;
    cashUpTotal?: number;
    invoiceValue?: number;
    cashTotal?: number;
    cashReport?: CashReportDto[] | null;
    creditReport?: CreditReportDto[] | null;
    ecoCashReport?: EcoCashReportDto[] | null;
    creditCardReport?: CreditCardReportDto[] | null;
}
