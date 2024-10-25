import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of, throwError } from 'rxjs';
import { RedirectorService } from '../..';
import { RouteReaderService } from './route-reader.service';

describe('RouteReaderService', () => {
  let service: RouteReaderService;
  let redirectServiceMock: jasmine.SpyObj<RedirectorService>;

  beforeEach(() => {
    redirectServiceMock = jasmine.createSpyObj('RedirectorService', ['redirectToHome']);

    TestBed.configureTestingModule({
      providers: [
        RouteReaderService,
        { provide: RedirectorService, useValue: redirectServiceMock },
        { provide: ActivatedRoute, useValue: { paramMap: of(convertToParamMap({})) } }
      ]
    });

    service = TestBed.inject(RouteReaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('readId should return data when id is valid', () => {
    const mockData = { id: '123', name: 'Test Item' };
    const route = { paramMap: of(convertToParamMap({ id: '123' })) } as ActivatedRoute;
    const fetchFn = jasmine.createSpy().and.returnValue(of(mockData));

    service.readId(route, fetchFn).subscribe(data => {
      expect(data).toEqual(mockData);
    });

    expect(fetchFn).toHaveBeenCalledWith('123');
    expect(redirectServiceMock.redirectToHome).not.toHaveBeenCalled();
  });

  it('readId should redirect to home when id is missing', () => {
    const route = { paramMap: of(convertToParamMap({})) } as ActivatedRoute;
    const fetchFn = jasmine.createSpy();

    service.readId(route, fetchFn).subscribe({
      next: () => fail('Expected an empty observable'),
      complete: () => expect(redirectServiceMock.redirectToHome).toHaveBeenCalled()
    });

    expect(fetchFn).not.toHaveBeenCalled();
  });

  it('readId should redirect to home on fetch error', () => {
    const route = { paramMap: of(convertToParamMap({ id: '123' })) } as ActivatedRoute;
    const fetchFn = jasmine.createSpy().and.returnValue(throwError(() => new Error('Fetch error')));

    service.readId(route, fetchFn).subscribe({
      next: () => fail('Expected an empty observable'),
      complete: () => expect(redirectServiceMock.redirectToHome).toHaveBeenCalled()
    });

    expect(fetchFn).toHaveBeenCalledWith('123');
  });

  it('readIdInt should return data when id is a valid integer', () => {
    const mockData = { id: 123, name: 'Test Item' };
    const route = { paramMap: of(convertToParamMap({ id: '123' })) } as ActivatedRoute;
    const fetchFn = jasmine.createSpy().and.returnValue(of(mockData));

    service.readIdInt(route, fetchFn).subscribe(data => {
      expect(data).toEqual(mockData);
    });

    expect(fetchFn).toHaveBeenCalledWith(123);
    expect(redirectServiceMock.redirectToHome).not.toHaveBeenCalled();
  });

  it('readIdInt should redirect to home when id is not a valid integer', () => {
    const route = { paramMap: of(convertToParamMap({ id: 'abc' })) } as ActivatedRoute;
    const fetchFn = jasmine.createSpy();

    service.readIdInt(route, fetchFn).subscribe({
      next: () => fail('Expected an empty observable'),
      complete: () => expect(redirectServiceMock.redirectToHome).toHaveBeenCalled()
    });

    expect(fetchFn).not.toHaveBeenCalled();
  });

  it('readIdInt should redirect to home on fetch error', () => {
    const route = { paramMap: of(convertToParamMap({ id: '123' })) } as ActivatedRoute;
    const fetchFn = jasmine.createSpy().and.returnValue(throwError(() => new Error('Fetch error')));

    service.readIdInt(route, fetchFn).subscribe({
      next: () => fail('Expected an empty observable'),
      complete: () => expect(redirectServiceMock.redirectToHome).toHaveBeenCalled()
    });

    expect(fetchFn).toHaveBeenCalledWith(123);
  });
});