import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookStatisticsFilterComponent } from './book-statistics-filter.component';

describe('BookStatisticsFilterComponent', () => {
  let component: BookStatisticsFilterComponent;
  let fixture: ComponentFixture<BookStatisticsFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookStatisticsFilterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookStatisticsFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
