export interface EmployeeDeductionDto {
    id?: number;
    employeeId?: number;
    employeeName?: string;
    deductionDate?: any;
    amount?: number;
    reason?: string;
    isApproved?: boolean;
    notes?: string;
} 