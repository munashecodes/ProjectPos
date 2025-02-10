import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoodsReturnedNoteComponent } from './goods-returned-note.component';

describe('GoodsReturnedNoteComponent', () => {
  let component: GoodsReturnedNoteComponent;
  let fixture: ComponentFixture<GoodsReturnedNoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GoodsReturnedNoteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GoodsReturnedNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
