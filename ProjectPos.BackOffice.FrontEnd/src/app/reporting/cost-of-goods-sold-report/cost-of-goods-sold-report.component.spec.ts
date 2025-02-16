import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostOfGoodsSoldReportComponent } from './cost-of-goods-sold-report.component';

describe('CostOfGoodsSoldReportComponent', () => {
  let component: CostOfGoodsSoldReportComponent;
  let fixture: ComponentFixture<CostOfGoodsSoldReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CostOfGoodsSoldReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CostOfGoodsSoldReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
