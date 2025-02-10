import { CashUpDto } from "./cash-up-dto";

export interface GetCashUpListDto {
    cashUpDate?: Date;
    userId?: number;
    cashUpName?: string;
    cashUps?: CashUpDto[];
}
