import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { GetCurrentUserResponse, UserInfoApiService } from '../../../shared';
import { UserInfoController } from './user-info-controller.service';

describe('UserInfoController', () => {
  let service: UserInfoController;
  let apiService: jasmine.SpyObj<UserInfoApiService>;

  beforeEach(() => {
    const apiServiceSpy = jasmine.createSpyObj('UserInfoApiService', ['getCurrentUser']);

    TestBed.configureTestingModule({
      providers: [
        UserInfoController,
        { provide: UserInfoApiService, useValue: apiServiceSpy }
      ]
    });

    service = TestBed.inject(UserInfoController);
    apiService = TestBed.inject(UserInfoApiService) as jasmine.SpyObj<UserInfoApiService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return user info when getUserInfo is called', () => {
    const expectedUserInfo: GetCurrentUserResponse = {
      userName: 'JohnDoe',
      userInfo: {
        name: 'John',
        lastName: 'Doe',
        dateOfBirth: new Date('1990-01-01'),
        address: '123 Main St'
      }
    };

    apiService.getCurrentUser.and.returnValue(of(expectedUserInfo));

    service.getUserInfo().subscribe((userInfo) => {
      expect(userInfo).toEqual(expectedUserInfo);
    });

    expect(apiService.getCurrentUser).toHaveBeenCalled();
  });
});