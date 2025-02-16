import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEmployeeDeductionComponent } from './manage-employee-deduction.component';

describe('ManageEmployeeDeductionComponent', () => {
  let component: ManageEmployeeDeductionComponent;
  let fixture: ComponentFixture<ManageEmployeeDeductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageEmployeeDeductionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageEmployeeDeductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
