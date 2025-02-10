import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { JobTitle } from "../enums/job-title";
import { Title } from "../enums/title";
import { AddressDto } from "./address-dto";
import { CompanyDto } from "./company-dto";

export interface ContactPersonDto extends FullAuditedEntityDto<number> {
    companyId?: number;
    addressId: number;
    firstName?: string;
    lastName?: string;
    title?: Title;
    jobPosition?: JobTitle;
    phone1?: string;
    phone2?: string;
    email?: string;
    company?: CompanyDto;
    address?: AddressDto;
}
