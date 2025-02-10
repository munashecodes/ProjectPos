import { ProofOfPaymentDto } from "./proof-of-payment-dto";

export interface ProofOfPaymentReportDto {
    userId?: number;
    userName?: string;
    customerId?: number;
    customerName?: string;
    createdDate?: Date;
    bookingDate?: Date;
    proofOfPayments?: ProofOfPaymentDto[];
    paidTotal?: number;
    usableTotal?: number;
    usedTotal?: number;
}
