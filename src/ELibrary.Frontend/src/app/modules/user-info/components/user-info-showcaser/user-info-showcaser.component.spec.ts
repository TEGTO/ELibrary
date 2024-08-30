import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserInfoShowcaserComponent } from './user-info-showcaser.component';

describe('UserInfoShowcaserComponent', () => {
  let component: UserInfoShowcaserComponent;
  let fixture: ComponentFixture<UserInfoShowcaserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserInfoShowcaserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserInfoShowcaserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
