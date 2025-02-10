import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoodsReceivedVoucherComponent } from './goods-received-voucher.component';

describe('GoodsReceivedVoucherComponent', () => {
  let component: GoodsReceivedVoucherComponent;
  let fixture: ComponentFixture<GoodsReceivedVoucherComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GoodsReceivedVoucherComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GoodsReceivedVoucherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
