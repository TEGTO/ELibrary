/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';

import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { publisherActions, selectPublisherAmount, selectPublishers } from '../../..';
import { CreatePublisherRequest, getDefaultPublisher, LibraryFilterRequest, Publisher, PublisherApiService, UpdatePublisherRequest } from '../../../../shared';
import { PublisherControllerService } from './publisher-controller.service';

describe('PublisherControllerService', () => {
    let service: PublisherControllerService;
    let store: jasmine.SpyObj<Store>;
    let apiService: jasmine.SpyObj<PublisherApiService>;

    const mockPublisherData: Publisher[] = [
        { ...getDefaultPublisher(), id: 1 },
        { ...getDefaultPublisher(), id: 2 },
    ];

    const mockTotalAmount = 10;

    beforeEach(() => {
        const storeSpy = jasmine.createSpyObj('Store', ['dispatch', 'select']);
        const apiServiceSpy = jasmine.createSpyObj('PublisherApiService', ['getById']);

        TestBed.configureTestingModule({
            providers: [
                PublisherControllerService,
                { provide: Store, useValue: storeSpy },
                { provide: PublisherApiService, useValue: apiServiceSpy }
            ]
        });

        service = TestBed.inject(PublisherControllerService);
        store = TestBed.inject(Store) as jasmine.SpyObj<Store>;
        apiService = TestBed.inject(PublisherApiService) as jasmine.SpyObj<PublisherApiService>;

        store.select.and.callFake((selector: any) => {
            if (selector === selectPublishers) {
                return of(mockPublisherData);
            } else if (selector === selectPublisherAmount) {
                return of(mockTotalAmount);
            } else {
                return of(null);
            }
        });
    });

    it('should return publisher data by ID', (done) => {
        const id = 1;
        const expectedPublisher: Publisher = { id: id, name: 'Action' };
        apiService.getById.and.returnValue(of(expectedPublisher));

        service.getById(id).subscribe(result => {
            expect(apiService.getById).toHaveBeenCalledWith(id);
            expect(result).toEqual(expectedPublisher);
            done();
        });
    });

    it('should dispatch getPaginated action and return paginated piblishers', (done) => {
        const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };
        store.select.and.returnValue(of(mockPublisherData));

        service.getPaginated(request).subscribe(result => {
            expect(store.dispatch).toHaveBeenCalledWith(publisherActions.getPaginated({ request }));
            expect(result).toEqual(mockPublisherData);
            done();
        });
    });

    it('should dispatch getTotalAmount action and return total amount of publishers', (done) => {
        store.select.and.returnValue(of(mockTotalAmount));
        const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };

        service.getItemTotalAmount(request).subscribe(result => {
            expect(store.dispatch).toHaveBeenCalledWith(publisherActions.getTotalAmount({ request: request }));
            expect(result).toEqual(mockTotalAmount);
            done();
        });
    });

    it('should dispatch create action for a new publisher', () => {
        const newPublisher: CreatePublisherRequest = { name: 'Name' };

        service.create(newPublisher);

        expect(store.dispatch).toHaveBeenCalledWith(publisherActions.create({ request: newPublisher }));
    });

    it('should dispatch update action for an existing publisher', () => {
        const updatedPublisher: UpdatePublisherRequest = { id: 1, name: 'Name' };

        service.update(updatedPublisher);

        expect(store.dispatch).toHaveBeenCalledWith(publisherActions.update({ request: updatedPublisher }));
    });

    it('should dispatch deleteById action for a publisher by ID', () => {
        const publisherId = 1;

        service.deleteById(publisherId);

        expect(store.dispatch).toHaveBeenCalledWith(publisherActions.deleteById({ id: publisherId }));
    });
});
