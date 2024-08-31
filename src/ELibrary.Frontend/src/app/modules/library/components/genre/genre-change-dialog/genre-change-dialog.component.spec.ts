import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenreChangeDialogComponent } from './genre-change-dialog.component';

describe('GenreChangeDialogComponent', () => {
  let component: GenreChangeDialogComponent;
  let fixture: ComponentFixture<GenreChangeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GenreChangeDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenreChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
