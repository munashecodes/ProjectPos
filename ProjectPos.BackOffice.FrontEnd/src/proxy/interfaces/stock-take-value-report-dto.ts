export interface StockTakeValueReportDto {
    productId: number;
    barCode: string;
    productName: string;
    subCategory: string;
    openingStock: number;
    receivedStock: number;
    damagedStock: number;
    returnedStock: number;
    soldStock: number;
    closingStock: number;
    stockOnHand: number;
    stockOnShelf: number;
    variance: number;
}
