import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenreTableComponent } from './genre-table.component';

describe('GenreTableComponent', () => {
  let component: GenreTableComponent;
  let fixture: ComponentFixture<GenreTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GenreTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenreTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
