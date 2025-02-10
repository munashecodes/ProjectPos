import { Unit } from "../enums/unit";

export interface StockTakeReportDto {
    productId: number;
    barCode: string;
    productName: string;
    subCategory: string;
    unit?: Unit; // Optional field
    openingStock: number;
    receivedQuantity: number;
    damagedQuantity: number;
    returnedQuantity: number;
    soldQuantity: number;
    closingStock: number;
    quantityOnHand: number;
    quantityOnShelf: number;
    variance: number;
}
