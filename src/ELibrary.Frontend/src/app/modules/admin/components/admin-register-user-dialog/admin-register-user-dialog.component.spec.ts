import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRegisterUserDialogComponent } from './admin-register-user-dialog.component';

describe('AdminRegisterUserDialogComponent', () => {
  let component: AdminRegisterUserDialogComponent;
  let fixture: ComponentFixture<AdminRegisterUserDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminRegisterUserDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRegisterUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
