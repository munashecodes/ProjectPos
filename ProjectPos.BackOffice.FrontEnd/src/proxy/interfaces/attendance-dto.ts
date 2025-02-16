export interface AttendanceDto {
    id?: number;
    employeeName?: string;
    employeeId?: number;
    date: Date;
    isPresent?: boolean;
    notes?: string;
} 