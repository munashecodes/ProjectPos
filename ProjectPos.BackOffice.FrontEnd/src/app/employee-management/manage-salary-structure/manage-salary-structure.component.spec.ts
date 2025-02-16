import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageSalaryStructureComponent } from './manage-salary-structure.component';

describe('ManageSalaryStructureComponent', () => {
  let component: ManageSalaryStructureComponent;
  let fixture: ComponentFixture<ManageSalaryStructureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageSalaryStructureComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageSalaryStructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
