import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { GetCurrentUserResponse, URLDefiner } from '../../..';
import { UserInfoApiService } from './user-info-api.service';

describe('UserInfoApiService', () => {
  let service: UserInfoApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithUserApiUrl']);
    mockUrlDefiner.combineWithUserApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        UserInfoApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });

    service = TestBed.inject(UserInfoApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get the current user', () => {
    const expectedReq = `/api/userinfo/user`;
    const response: GetCurrentUserResponse = {
      userName: 'johndoe',
      userInfo: {
        name: 'John',
        lastName: 'Doe',
        dateOfBirth: new Date('1990-01-01'),
        address: '123 Main St'
      }
    };

    service.getCurrentUser().subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/userinfo/user');
    req.flush(response);
  });

  it('should handle error on getCurrentUser', () => {
    const expectedReq = `/api/userinfo/user`;

    service.getCurrentUser().subscribe(
      () => fail('Expected an error, not a success'),
      (error) => {
        expect(error).toBeTruthy();
      }
    );

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 404, statusText: 'Not Found' });
  });
})