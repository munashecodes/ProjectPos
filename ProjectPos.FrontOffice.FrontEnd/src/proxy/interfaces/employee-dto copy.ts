import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { AddressDto } from "./address-dto";
import { UserDto } from "./user-dto";

export interface EmployeeDto extends FullAuditedEntityDto<number> {
    name: string;
    surname: string;
    cell: string;
    email: string;
    natId: string;
    dob: Date;
    address: AddressDto;
    user?: UserDto;
}
