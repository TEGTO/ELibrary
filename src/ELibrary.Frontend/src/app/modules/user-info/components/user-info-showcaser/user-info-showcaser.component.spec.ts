import { CUSTOM_ELEMENTS_SCHEMA, DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { UserInfoService } from '../..';
import { UserInfoShowcaserComponent } from './user-info-showcaser.component';

describe('UserInfoShowcaserComponent', () => {
  let component: UserInfoShowcaserComponent;
  let fixture: ComponentFixture<UserInfoShowcaserComponent>;
  let userInfoService: jasmine.SpyObj<UserInfoService>;

  beforeEach(async () => {
    const userInfoServiceSpy = jasmine.createSpyObj('UserInfoService', ['getUserInfo']);

    await TestBed.configureTestingModule({
      declarations: [UserInfoShowcaserComponent],
      providers: [
        { provide: UserInfoService, useValue: userInfoServiceSpy }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(UserInfoShowcaserComponent);
    component = fixture.componentInstance;
    userInfoService = TestBed.inject(UserInfoService) as jasmine.SpyObj<UserInfoService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch and display user info correctly', () => {
    const mockUserInfo = {
      name: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1990-01-01'),
      address: '123 Main St'
    };

    userInfoService.getUserInfo.and.returnValue(of({
      userName: 'johndoe',
      userInfo: mockUserInfo
    }));

    fixture.detectChanges();

    expect(userInfoService.getUserInfo).toHaveBeenCalled();

    const nameInput: DebugElement = fixture.debugElement.queryAll(By.css('input[matInput][type="text"]'))[0];
    const lastNameInput: DebugElement = fixture.debugElement.queryAll(By.css('input[matInput][type="text"]'))[1];
    const addressInput: DebugElement = fixture.debugElement.queryAll(By.css('input[matInput][type="text"]'))[2];

    expect(nameInput.nativeElement.value).toBe('John');
    expect(lastNameInput.nativeElement.value).toBe('Doe');
    expect(addressInput.nativeElement.value).toBe('123 Main St');
  });
});