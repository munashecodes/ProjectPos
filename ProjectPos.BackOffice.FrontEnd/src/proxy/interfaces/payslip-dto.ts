import { PaymentMethod } from "../enums/payment-method";

export interface PaySlipDto {
    id?: number;
    employeeId: number;
    employeeName?: string;
    employeeSurname?: string;
    month: number;
    year: number;
    paymentMethod: PaymentMethod;
    basicSalary: number;
    allowance?: number;
    tillShortageDeduction?: number;
    grossSalary?: number;
    netSalary?: number;
    tax?: number;
    totalEarning?: number;
    totalDeduction?: number;
    totalNetSalary?: number;
    isPaid: boolean;
    isApproved: boolean;
    approvedBy?: number;
    isPostedToJournal: boolean;
    createdBy?: number;
    createdDate?: Date;
    lastModifiedBy?: number;
    lastModifiedDate?: Date;
} 