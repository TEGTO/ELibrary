import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookChangeDialogComponent } from './book-change-dialog.component';

describe('BookChangeDialogComponent', () => {
  let component: BookChangeDialogComponent;
  let fixture: ComponentFixture<BookChangeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookChangeDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
