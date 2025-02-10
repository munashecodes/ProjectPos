import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReconReportsComponent } from './recon-reports.component';

describe('ReconReportsComponent', () => {
  let component: ReconReportsComponent;
  let fixture: ComponentFixture<ReconReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReconReportsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReconReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
