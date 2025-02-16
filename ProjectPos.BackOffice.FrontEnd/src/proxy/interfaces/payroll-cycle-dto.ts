import { PayRollStatus } from "./payroll-status";
import { PaySlipDto } from "./payslip-dto";

export interface PayRollCycleDto {
    id?: number;
    month?: number;
    year?: number;
    startDate?: Date;
    endDate?: Date;
    isClosed?: boolean;
    payRollStatus?: PayRollStatus;
    paySlips?: PaySlipDto[];
} 