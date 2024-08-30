import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserInfoRegistrationComponent } from './user-info-registration.component';

describe('UserInfoRegistrationComponent', () => {
  let component: UserInfoRegistrationComponent;
  let fixture: ComponentFixture<UserInfoRegistrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserInfoRegistrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserInfoRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
