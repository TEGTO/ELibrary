import { TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthenticationApiService, URLDefiner, UserUpdateRequest } from '../../..';
import { UserApiService } from './user-api.service';

describe('UserApiService', () => {
  let service: UserApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithUserApiUrl']);
    mockUrlDefiner.combineWithUserApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        AuthenticationApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(UserApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should update user', () => {
    const expectedReq = `/api/user/update`;
    const request: UserUpdateRequest = {
      email: 'updated@example.com',
      oldPassword: 'oldPassword',
      password: 'newPassword'
    };

    service.updateUser(request).subscribe(res => {
      expect(res.status).toBe(200);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('PUT');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/user/update');
    req.flush({}, { status: 200, statusText: 'OK' });
  });

  it('should delete user', () => {
    const expectedReq = `/api/user/delete`;

    service.deleteUser().subscribe(res => {
      expect(res.status).toBe(200);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('DELETE');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/user/delete');
    req.flush({}, { status: 200, statusText: 'OK' });
  });

});
