import { FullAuditedEntityDto } from "../entity-dtos/full-audited-entity-dto";
import { Category } from "../enums/category";
import { Grade } from "../enums/grade";
import { GoodsReceivedVoucherLineDto } from "./goods-received-voucher-line-dto";
import { ProductInventoryDto } from "./product-inventory-dto";
import { ProductInventorySnapshotDto } from "./product-inventory-snapshot-dto";
import { PurchaceInvoiceLineDto } from "./purchace-invoice-line-dto";
import { PurchaceOrderLineDto } from "./purchace-order-line-dto";
import { StockMovementDto } from "./stock-movement-dto";
import { SubCategoryDto } from "./sub-category-dto";

export interface ProductDto extends FullAuditedEntityDto<number> {
    barCode?: string;
    name?: string;
    description?: string;
    category?: Category;
    subCategoryId?: number;
    subCatergoryName?: string;
    cost?: number;
    grade?: Grade;
    isTaxable?: boolean;
    image?: string;
    imageDisplay?: string;
    subCategory?: SubCategoryDto;
    orderedItems?: PurchaceOrderLineDto[];
    receivedItems?: GoodsReceivedVoucherLineDto[];
    stockMovements?: StockMovementDto[];
    productInventories?: ProductInventoryDto[];
    productInventorySnapshots?: ProductInventorySnapshotDto[];
    invoiceLines?: PurchaceInvoiceLineDto[];
}
