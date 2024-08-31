import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorChangeDialogComponent } from './author-change-dialog.component';

describe('AuthorChangeDialogComponent', () => {
  let component: AuthorChangeDialogComponent;
  let fixture: ComponentFixture<AuthorChangeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthorChangeDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthorChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
