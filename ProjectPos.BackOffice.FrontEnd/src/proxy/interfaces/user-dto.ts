import { EntityDto } from "../entity-dtos/entity-dto";
import { Role } from "../enums/role";
import { SystemName } from "../enums/system-name";
import { AccessLogDto } from "./access-log-dto";
import { EmployeeDto } from "./employee-dto";

export interface UserDto extends EntityDto<number> {
    userName?: string;
    fullName?: string;
    password?: string;
    employeeId?: number;
    system?: SystemName;
    role?: Role;
    jwtToken?: string;
    isActive?: boolean;
    accessLogs?: AccessLogDto[]
    employee?: EmployeeDto
}

export interface UserSignInDto extends EntityDto<number> {
    userName?: string;
    fullName?: string;
    password?: string;
    role?: Role;
    supervisorCode?: string
}


