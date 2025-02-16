export interface OvertimeRecordDto {
    id?: number;
    employeeId?: number;
    employeeName?: string;
    date?: Date;
    hours?: number;
    amount?: number;
    isApproved?: boolean;
    notes?: string;
} 