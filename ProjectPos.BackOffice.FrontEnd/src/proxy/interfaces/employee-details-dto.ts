import { Department } from "../enums/department";
import { EmploymentType } from "../enums/employee-type";
import { SalaryType } from "../enums/salary-type";



export interface EmployeeDetailsDto {
    id?: number;
    employeeId: number;
    department: Department;
    position?: string;
    employmentType: EmploymentType;
    bank?: string;
    bankAccountNumber?: string;
    salaryType: SalaryType;
    joiningDate: Date;
    terminationDate?: Date;
    isActive: boolean;
    createdBy?: number;
    createdDate?: Date;
    lastModifiedBy?: number;
    lastModifiedDate?: Date;
} 