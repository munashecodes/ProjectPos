import { OrderPaymentStatusPipe } from './order-payment-status.pipe';

describe('OrderPaymentStatusPipe', () => {
  it('create an instance', () => {
    const pipe = new OrderPaymentStatusPipe();
    expect(pipe).toBeTruthy();
  });
});
