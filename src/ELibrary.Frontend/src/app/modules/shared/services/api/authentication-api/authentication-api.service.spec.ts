import { HttpClientTestingModule, HttpTestingController } from "@angular/common/http/testing";
import { TestBed } from "@angular/core/testing";
import { AuthToken, URLDefiner, UserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from "../../..";
import { AuthenticationApiService } from "./authentication-api.service";

describe('AuthenticationApiService', () => {
  let service: AuthenticationApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithUserApiUrl']);
    mockUrlDefiner.combineWithUserApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthenticationApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });

    service = TestBed.inject(AuthenticationApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login user', () => {
    const expectedReq = `/api/user/login`;
    const request: UserAuthenticationRequest = {
      login: 'login',
      password: 'password'
    };
    const response: UserAuth = {
      isAuthenticated: true,
      authToken: {
        accessToken: 'accessToken',
        refreshToken: 'refreshToken',
        refreshTokenExpiryDate: new Date()
      },
      email: 'userName',
      roles: ['user']
    };

    service.loginUser(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/user/login');
    req.flush(response);
  });

  it('should register user', () => {
    const expectedReq = `/api/user/register`;
    const request: UserRegistrationRequest = {
      email: 'user@example.com',
      password: 'password',
      confirmPassword: 'password'
    };
    const response: UserAuth = {
      isAuthenticated: true,
      authToken: {
        accessToken: 'accessToken',
        refreshToken: 'refreshToken',
        refreshTokenExpiryDate: new Date()
      },
      email: 'user@example.com',
      roles: ['user']
    };

    service.registerUser(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/user/register');
    req.flush(response);
  });

  it('should refresh token', () => {
    const expectedReq = `/api/user/refresh`;
    const request: AuthToken = {
      accessToken: 'oldAccessToken',
      refreshToken: 'oldRefreshToken',
      refreshTokenExpiryDate: new Date()
    };
    const response: AuthToken = {
      accessToken: 'newAccessToken',
      refreshToken: 'newRefreshToken',
      refreshTokenExpiryDate: new Date()
    };

    service.refreshToken(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/user/refresh');
    req.flush(response);
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

  it('should handle error on register user', () => {
    const expectedReq = `/api/user/register`;
    const request: UserRegistrationRequest = {
      email: 'user@example.com',
      password: 'password',
      confirmPassword: 'password'
    };

    service.registerUser(request).subscribe(
      () => fail('Expected an error, not a success'),
      (error) => {
        expect(error).toBeTruthy();
      }
    );

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 400, statusText: 'Bad Request' });
  });

  it('should handle error on refresh token', () => {
    const expectedReq = `/api/user/refresh`;
    const request: AuthToken = {
      accessToken: 'oldAccessToken',
      refreshToken: 'oldRefreshToken',
      refreshTokenExpiryDate: new Date()
    };

    service.refreshToken(request).subscribe(
      () => fail('Expected an error, not a success'),
      (error) => {
        expect(error).toBeTruthy();
      }
    );

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 400, statusText: 'Bad Request' });
  });
});
