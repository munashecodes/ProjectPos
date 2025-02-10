import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeptorsAndCreditorsComponent } from './deptors-and-creditors.component';

describe('DeptorsAndCreditorsComponent', () => {
  let component: DeptorsAndCreditorsComponent;
  let fixture: ComponentFixture<DeptorsAndCreditorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeptorsAndCreditorsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeptorsAndCreditorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
