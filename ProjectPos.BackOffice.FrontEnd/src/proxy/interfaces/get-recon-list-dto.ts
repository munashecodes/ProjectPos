import { ReconDto } from "./recon-dto";

export interface GetReconListDto {
    cashUpDate?: Date;
    userId?: number;
    cashUpName?: string;
    cashUps?: ReconDto[];
    cashTotal?: number;
    saleTotal?: number
}
