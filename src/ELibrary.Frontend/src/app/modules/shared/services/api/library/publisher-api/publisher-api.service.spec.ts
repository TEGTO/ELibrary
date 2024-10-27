import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { CreatePublisherRequest, LibraryFilterRequest, PublisherResponse, UpdatePublisherRequest, URLDefiner } from '../../../..';
import { PublisherApiService } from './publisher-api.service';

describe('PublisherApiService', () => {
  let service: PublisherApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithLibraryApiUrl']);
    mockUrlDefiner.combineWithLibraryApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        PublisherApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(PublisherApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getById should retrieve a publisher by ID', () => {
    const publisherId = 1;
    const expectedUrl = `/api/publisher/${publisherId}`;
    const mockResponse: PublisherResponse = {
      id: publisherId,
      name: 'Sample Publisher',
    };

    service.getById(publisherId).subscribe(response => {
      expect(response.id).toEqual(publisherId);
      expect(response.name).toEqual('Sample Publisher');
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('getPaginated should fetch paginated publishers', () => {
    const request: LibraryFilterRequest = { containsName: "", pageSize: 10, pageNumber: 1 };
    const expectedUrl = `/api/publisher/pagination`;
    const mockResponse: PublisherResponse[] = [
      { id: 1, name: 'Publisher 1' },
      { id: 2, name: 'Publisher 2' },
    ];

    service.getPaginated(request).subscribe(response => {
      expect(response.length).toBe(2);
      expect(response[0].name).toBe('Publisher 1');
      expect(response[1].name).toBe('Publisher 2');
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('getItemTotalAmount should return the total number of items', () => {
    const request: LibraryFilterRequest = { containsName: "", pageSize: 10, pageNumber: 1 };
    const expectedUrl = `/api/publisher/amount`;
    const mockTotal = 42;

    service.getItemTotalAmount(request).subscribe(total => {
      expect(total).toBe(mockTotal);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockTotal);
  });

  it('create should post a new publisher', () => {
    const request: CreatePublisherRequest = { name: 'New Publisher' };
    const expectedUrl = `/api/publisher`;
    const mockResponse: PublisherResponse = { id: 1, name: 'New Publisher' };

    service.create(request).subscribe(response => {
      expect(response.name).toBe('New Publisher');
      expect(response.id).toBe(1);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('update should put updated publisher data', () => {
    const request: UpdatePublisherRequest = { id: 1, name: 'Updated Publisher' };
    const expectedUrl = `/api/publisher`;
    const mockResponse: PublisherResponse = { id: 1, name: 'Updated Publisher' };

    service.update(request).subscribe(response => {
      expect(response.name).toBe('Updated Publisher');
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('deleteById should delete the publisher by ID', () => {
    const publisherId = 1;
    const expectedUrl = `/api/publisher/${publisherId}`;

    service.deleteById(publisherId).subscribe(response => {
      expect(response.status).toBe(200);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush({}, { status: 200, statusText: 'OK' });
  });
});