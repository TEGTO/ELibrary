import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookStockDetailsComponent } from './book-stock-details.component';

describe('BookStockDetailsComponent', () => {
  let component: BookStockDetailsComponent;
  let fixture: ComponentFixture<BookStockDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookStockDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookStockDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
