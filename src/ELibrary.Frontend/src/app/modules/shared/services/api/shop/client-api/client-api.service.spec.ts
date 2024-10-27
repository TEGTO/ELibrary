import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ClientResponse, CreateClientRequest, GetClientResponse, UpdateClientRequest, URLDefiner } from '../../../..';
import { ClientApiService } from './client-api.service';

describe('ClientApiService', () => {
  let service: ClientApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithShopApiUrl']);
    mockUrlDefiner.combineWithShopApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        ClientApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(ClientApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('get should fetch client data', () => {
    const expectedUrl = `/api/client`;
    const mockResponse: GetClientResponse = {
      client: {
        id: '1',
        userId: 'user1',
        name: 'John',
        middleName: 'M',
        lastName: 'Doe',
        dateOfBirth: new Date('1990-01-01'),
        address: '123 Main St',
        phone: '1234567890',
        email: 'john@example.com'
      }
    };

    service.get().subscribe(response => {
      expect(response.client).toEqual(mockResponse.client);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('create should post client data', () => {
    const expectedUrl = `/api/client`;
    const requestPayload: CreateClientRequest = {
      name: 'Jane',
      middleName: 'A',
      lastName: 'Smith',
      dateOfBirth: new Date('1992-02-02'),
      address: '456 Elm St',
      phone: '9876543210',
      email: 'jane@example.com'
    };
    const mockResponse: ClientResponse = {
      ...requestPayload,
      id: '2',
      userId: 'user2'
    };

    service.create(requestPayload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('update should put client data', () => {
    const expectedUrl = `/api/client`;
    const requestPayload: UpdateClientRequest = {
      name: 'Updated Name',
      middleName: 'B',
      lastName: 'Updated Last',
      dateOfBirth: new Date('1991-03-03'),
      address: '789 Maple Ave',
      phone: '5555555555',
      email: 'updated@example.com'
    };
    const mockResponse: ClientResponse = {
      ...requestPayload,
      id: '1',
      userId: 'user1'
    };

    service.update(requestPayload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('adminGet should fetch admin client data', () => {
    const clientId = '1';
    const expectedUrl = `/api/client/admin/${clientId}`;
    const mockResponse: GetClientResponse = {
      client: {
        id: '1',
        userId: 'user1',
        name: 'Admin John',
        middleName: 'A',
        lastName: 'Doe',
        dateOfBirth: new Date('1985-06-01'),
        address: '789 Walnut St',
        phone: '1112223333',
        email: 'admin.john@example.com'
      }
    };

    service.adminGet(clientId).subscribe(response => {
      expect(response.client).toEqual(mockResponse.client);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('adminCreate should post admin client data', () => {
    const clientId = '1';
    const expectedUrl = `/api/client/admin/${clientId}`;
    const requestPayload: CreateClientRequest = {
      name: 'Admin Jane',
      middleName: 'B',
      lastName: 'Smith',
      dateOfBirth: new Date('1988-07-22'),
      address: '111 Oak St',
      phone: '4443332222',
      email: 'admin.jane@example.com'
    };
    const mockResponse: ClientResponse = {
      ...requestPayload,
      id: '2',
      userId: 'admin2'
    };

    service.adminCreate(clientId, requestPayload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('adminUpdate should put admin client data', () => {
    const clientId = '1';
    const expectedUrl = `/api/client/admin/${clientId}`;
    const requestPayload: UpdateClientRequest = {
      name: 'Updated Admin Name',
      middleName: 'C',
      lastName: 'Updated Admin Last',
      dateOfBirth: new Date('1980-04-15'),
      address: '333 Cedar St',
      phone: '6667778888',
      email: 'updated.admin@example.com'
    };
    const mockResponse: ClientResponse = {
      ...requestPayload,
      id: '1',
      userId: 'admin1'
    };

    service.adminUpdate(clientId, requestPayload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });
});