import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEmployeeDetailsComponent } from './manage-employee-details.component';

describe('ManageEmployeeDetailsComponent', () => {
  let component: ManageEmployeeDetailsComponent;
  let fixture: ComponentFixture<ManageEmployeeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageEmployeeDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageEmployeeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
