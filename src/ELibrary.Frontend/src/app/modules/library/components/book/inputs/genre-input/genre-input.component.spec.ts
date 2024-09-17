import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenreInputComponent } from './genre-input.component';

describe('GenreInputComponent', () => {
  let component: GenreInputComponent;
  let fixture: ComponentFixture<GenreInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GenreInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenreInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
