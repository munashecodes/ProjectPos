import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageOvertimeComponent } from './manage-overtime.component';

describe('ManageOvertimeComponent', () => {
  let component: ManageOvertimeComponent;
  let fixture: ComponentFixture<ManageOvertimeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageOvertimeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageOvertimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
