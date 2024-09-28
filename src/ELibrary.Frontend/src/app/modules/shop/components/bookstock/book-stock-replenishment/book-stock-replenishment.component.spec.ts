import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookStockReplenishmentComponent } from './book-stock-replenishment.component';

describe('BookStockReplenishmentComponent', () => {
  let component: BookStockReplenishmentComponent;
  let fixture: ComponentFixture<BookStockReplenishmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookStockReplenishmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookStockReplenishmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
