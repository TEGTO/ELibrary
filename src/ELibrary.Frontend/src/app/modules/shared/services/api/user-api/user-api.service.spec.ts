import { HttpResponse, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AdminUser, AdminUserUpdateDataRequest, AuthenticationMethod, URLDefiner, UserUpdateRequest } from '../../..';
import { UserApiService } from './user-api.service';

describe('UserApiService', () => {
  let service: UserApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj('URLDefiner', ['combineWithUserApiUrl']);
    mockUrlDefiner.combineWithUserApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        UserApiService,
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
    const request: UserUpdateRequest = { email: 'user@example.com', oldPassword: 'oldPass', password: 'newPass' };
    const expectedUrl = `/api/user/update`;

    service.updateUser(request).subscribe((res) => {
      expect(res.status).toBe(200);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(request);
    req.flush(new HttpResponse<void>({ status: 200 }));
  });

  it('should delete user', () => {
    const expectedUrl = `/api/user/delete`;

    service.deleteUser().subscribe((res) => {
      expect(res.status).toBe(200);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(new HttpResponse<void>({ status: 200 }));
  });

  it('should get admin user by info', () => {
    const info = 'someInfo';
    const expectedUrl = `/api/user/admin/users/${info}`;
    const response: AdminUser = {
      id: '1',
      userName: 'adminUser',
      email: 'admin@example.com',
      registredAt: new Date(),
      updatedAt: new Date(),
      roles: ['admin'],
      authenticationMethods: [AuthenticationMethod.BaseAuthentication, AuthenticationMethod.GoogleOAuth]
    };

    service.adminGetUser(info).subscribe((res) => {
      expect(res).toEqual(jasmine.objectContaining({ id: '1' }));
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(response);
  });

  it('should get paginated admin users', () => {
    const request = { pageNumber: 1, pageSize: 10, containsInfo: 'search' };
    const expectedUrl = `/api/user/admin/users`;
    const response: AdminUser[] = [
      {
        id: '1',
        userName: 'adminUser1',
        email: 'admin1@example.com',
        registredAt: new Date(),
        updatedAt: new Date(),
        roles: ['admin'],
        authenticationMethods: [AuthenticationMethod.BaseAuthentication, AuthenticationMethod.GoogleOAuth]
      },
      {
        id: '2',
        userName: 'adminUser2',
        email: 'admin2@example.com',
        registredAt: new Date(),
        updatedAt: new Date(),
        roles: ['user'],
        authenticationMethods: [AuthenticationMethod.BaseAuthentication, AuthenticationMethod.GoogleOAuth]
      }
    ];

    service.adminGetPaginatedUsers(request).subscribe((res) => {
      expect(res.length).toEqual(2);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(response);
  });

  it('should get paginated user amount', () => {
    const request = { pageNumber: 1, pageSize: 10, containsInfo: 'search' };
    const expectedUrl = `/api/user/admin/users/amount`;
    const response = 100;

    service.adminGetPaginatedUserAmount(request).subscribe((res) => {
      expect(res).toBe(response);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(response);
  });

  it('should update admin user', () => {
    const request: AdminUserUpdateDataRequest = {
      currentLogin: 'adminUser',
      email: 'admin@example.com',
      password: 'newPass',
      confirmPassword: 'newPass',
      roles: ['admin']
    };
    const expectedUrl = `/api/user/admin/update`;
    const response: AdminUser = {
      id: '1',
      userName: 'adminUser',
      email: 'admin@example.com',
      registredAt: new Date(0),
      updatedAt: new Date(0),
      roles: ['admin'],
      authenticationMethods: [AuthenticationMethod.BaseAuthentication, AuthenticationMethod.GoogleOAuth]
    };

    service.adminUpdateUser(request).subscribe((res) => {
      expect(res).toEqual(jasmine.objectContaining({ id: '1' }));
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(request);
    req.flush(response);
  });

  it('should delete admin user by id', () => {
    const id = '1';
    const expectedUrl = `/api/user/admin/delete/${id}`;

    service.adminDeleteUser(id).subscribe((res) => {
      expect(res.status).toBe(200);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(new HttpResponse<void>({ status: 200 }));
  });
});