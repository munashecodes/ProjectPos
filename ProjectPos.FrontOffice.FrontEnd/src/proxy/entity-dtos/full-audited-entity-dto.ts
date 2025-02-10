import { AuditedEntityDto } from "./audited-entity-dto";

export interface FullAuditedEntityDto<T> extends AuditedEntityDto<T> {
    isDeleted?: boolean;
    deleterId?: number;
    deletionTime?: Date;
}
