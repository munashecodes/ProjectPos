import { FullAuditedEntityDto } from '../entity-dtos/full-audited-entity-dto';
import { Currency } from '../enums/currency';
import { SaleType } from '../enums/sale-type.enum';
import { GoodsReceivedVoucherDto } from './goods-received-voucher-dto';

export interface PurchaceOrderPaymentDto extends FullAuditedEntityDto<number> {
    goodsReceivedVoucherId?: number;
    orderAmount?: number;
    paidAmount?: number;
    usdPaidAmount?: number;
    paidAmountAfterChange: number;
    usdPaidAmountAfterChange: number;
    changeAmount?: number;
    exchangeRate?: number;
    paidBy?: string;
    currency?: Currency;
    methodOfPay?: SaleType;
    orderDate: Date;
    supplierName?: string;
    goodsReceivedVoucher?: GoodsReceivedVoucherDto;
}
