import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { AddressDto } from "./address-dto";

export interface CompanyDto extends FullAuditedEntityDto<number> {
    cell: any;
    name?: string;
    addressId?: number;
    vatNumber?: number;
    regNumber?: number;
    accountNumber?: number;
    phone?: string;
    email?: string;
    isSupplier?: boolean;
    hasCreditFacility?: boolean;
    address?: AddressDto
}
