import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthToken, URLDefiner, UserAuthenticationRequest, UserAuthenticationResponse, UserRegistrationRequest } from '../../..';
import { AuthenticationApiService } from './authentication-api.service';

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
    const expectedReq = `/api/auth/login`;
    const request: UserAuthenticationRequest = {
      login: 'login',
      password: 'password'
    };
    const response: UserAuthenticationResponse = {
      authToken: {
        accessToken: 'accessToken',
        refreshToken: 'refreshToken',
        refreshTokenExpiryDate: new Date()
      },
      userName: 'userName',
    };

    service.loginUser(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/auth/login');
    req.flush(response);
  });

  it('should register user', () => {
    const expectedReq = `/api/auth/register`;
    const request: UserRegistrationRequest = {
      userName: 'userName',
      password: 'password',
      confirmPassword: 'confirmPassword',
      userInfo: {
        name: "",
        lastName: "",
        dateOfBirth: new Date(),
        address: ""
      }
    };

    service.registerUser(request).subscribe();

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/auth/register');
  });

  it('should refresh token', () => {
    const expectedReq = `/api/auth/refresh`;
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
    expect(mockUrlDefiner.combineWithUserApiUrl).toHaveBeenCalledWith('/auth/refresh');
    req.flush(response);
  });

  it('should handle error on login', () => {
    const expectedReq = `/api/auth/login`;
    const request: UserAuthenticationRequest = {
      login: 'login',
      password: 'password'
    };

    service.loginUser(request).subscribe(
      () => fail('Expected an error, not a success'),
      (error) => {
        expect(error).toBeTruthy();
      }
    );

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 400, statusText: 'Bad Request' });
  });
});