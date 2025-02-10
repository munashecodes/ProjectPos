import { Pipe, PipeTransform } from '@angular/core';
import { OrderPaymentStatus } from '../enums/order-payment-status';

@Pipe({
  name: 'orderPaymentStatus',
  standalone: true
})
export class OrderPaymentStatusPipe implements PipeTransform {

  status: any;

  color: any;

  transform(value: OrderPaymentStatus): string {

    if(value === OrderPaymentStatus.NotPaid){
      this.color = "red";
    }
    else if(value === OrderPaymentStatus.PartiallyPaid){
      this.color = "blue"
    }
    else if(value === OrderPaymentStatus.Paid){
      this.color = "green"
    }

    return this.color;
  }

}
