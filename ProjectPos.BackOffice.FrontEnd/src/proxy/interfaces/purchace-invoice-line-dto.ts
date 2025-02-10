import { EntityDto } from "../entity-dtos/entity-dto";
import { ProductDto } from "./product-dto";
import { PurchaceInvoiceDto } from "./purchace-invoice-dto";

export interface PurchaceInvoiceLineDto extends EntityDto<number>{
    productId: number;
    barCode?: string;
    invoiceNumber: number;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
    invoice?: PurchaceInvoiceDto;
    product?: ProductDto;
}
