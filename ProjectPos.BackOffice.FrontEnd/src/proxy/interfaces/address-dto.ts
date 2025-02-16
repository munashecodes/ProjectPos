import { EntityDto } from "../entity-dtos/entity-dto";

export interface AddressDto extends EntityDto<number> {
    street: string;
    addressLine1: string;
    city: string;
    state: string;
    country: string;
    createdOn: Date;
}
