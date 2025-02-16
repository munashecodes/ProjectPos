import { Currency } from "../enums/currency";

export interface SalaryStructureDto {
    id?: number;
    employeeId?: number;
    employeeName?: string;
    employeeSurname?: string;
    currency?: Currency;
    basicSalary?: number;
    housingAllowance?: number;
    transportAllowance?: number;
    otherAllowance?: number;
    medicalBenefit?: number;
    pensionBenefit?: number;
    otherBenefit?: number;
    taxDeduction?: number;
    pensionDeduction?: number;
    aidsLevyDeduction?: number;
    otherDeduction?: number;
    overtimeRate?: number;
    overtimeHours?: number;
    overtimeTotal?: number;
    hourlyRate?: number;
    hoursWorked?: number;
    taxableIncome?: number;
    netSalary?: number;
    notes?: string;
    createdBy?: number;
    createdDate?: Date;
    lastModifiedBy?: number;
    lastModifiedDate?: Date;
} 