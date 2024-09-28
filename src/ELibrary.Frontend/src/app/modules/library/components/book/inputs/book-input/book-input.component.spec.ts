import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookInputComponent } from './book-input.component';

describe('BookInputComponent', () => {
  let component: BookInputComponent;
  let fixture: ComponentFixture<BookInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
