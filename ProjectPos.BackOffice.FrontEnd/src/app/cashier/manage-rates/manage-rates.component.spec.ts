import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageRatesComponent } from './manage-rates.component';

describe('ManageRatesComponent', () => {
  let component: ManageRatesComponent;
  let fixture: ComponentFixture<ManageRatesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageRatesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageRatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
