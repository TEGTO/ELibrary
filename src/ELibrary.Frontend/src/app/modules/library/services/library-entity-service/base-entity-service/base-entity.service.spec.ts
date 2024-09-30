// /* eslint-disable @typescript-eslint/no-explicit-any */
// import { TestBed } from '@angular/core/testing';
// import { Store } from '@ngrx/store';
// import { of } from 'rxjs';
// import { BaseControllerService } from './base-entity.service';

// class TestControllerService extends BaseControllerService<
//   any,
//   any,
//   any,
//   any
// > { }
// describe('BaseControllerService', () => {
//   let service: BaseControllerService<any, any, any, any>;
//   let apiService: jasmine.SpyObj<any>;
//   let store: jasmine.SpyObj<Store<any>>;
//   let actions: any;
//   let selectors: any;

//   beforeEach(() => {
//     apiService = jasmine.createSpyObj('apiService', ['getById']);
//     store = jasmine.createSpyObj('Store', ['dispatch', 'select']);

//     actions = {
//       getPaginated: jasmine.createSpy('getPaginated'),
//       getTotalAmount: jasmine.createSpy('getTotalAmount'),
//       create: jasmine.createSpy('create'),
//       update: jasmine.createSpy('update'),
//       deleteById: jasmine.createSpy('deleteById')
//     };

//     selectors = {
//       selectItems: jasmine.createSpy('selectItems'),
//       selectTotalAmount: jasmine.createSpy('selectTotalAmount')
//     };

//     TestBed.configureTestingModule({
//       providers: [
//         { provide: apiService, useValue: apiService },
//         { provide: Store, useValue: store },
//         { provide: actions, useValue: actions },
//         { provide: selectors, useValue: selectors }
//       ]
//     });

//     service = new TestControllerService(
//       apiService,
//       store,
//       actions,
//       selectors
//     );
//   });

//   it('should get item by ID', () => {
//     const mockResponse = { id: 1, name: 'Item 1' };
//     apiService.getById.and.returnValue(of(mockResponse));

//     const result$ = service.getById(1);

//     expect(apiService.getById).toHaveBeenCalledWith(1);
//     result$.subscribe((response) => {
//       expect(response).toEqual(mockResponse);
//     });
//   });

//   it('should dispatch action for paginated items and select them from store', () => {
//     const mockRequest = { page: 1, pageSize: 10 };
//     const mockItems = [{ id: 1, name: 'Item 1' }, { id: 2, name: 'Item 2' }];
//     store.select.and.returnValue(of(mockItems));

//     const result$ = service.getPaginated(mockRequest);

//     expect(store.dispatch).toHaveBeenCalledWith(actions.getPaginated({ request: mockRequest }));
//     result$.subscribe((items) => {
//       expect(items).toEqual(mockItems);
//     });
//   });

//   it('should dispatch action for total amount and select it from store', () => {
//     const mockRequest = { filter: 'filter' };
//     const mockTotalAmount = 100;
//     store.select.and.returnValue(of(mockTotalAmount));

//     const result$ = service.getItemTotalAmount(mockRequest);

//     expect(store.dispatch).toHaveBeenCalledWith(actions.getTotalAmount({ request: mockRequest }));
//     result$.subscribe((totalAmount) => {
//       expect(totalAmount).toEqual(mockTotalAmount);
//     });
//   });

//   it('should dispatch create action', () => {
//     const mockRequest = { name: 'New Item' };
//     service.create(mockRequest);

//     expect(store.dispatch).toHaveBeenCalledWith(actions.create({ request: mockRequest }));
//   });

//   it('should dispatch update action', () => {
//     const mockRequest = { id: 1, name: 'Updated Item' };
//     service.update(mockRequest);

//     expect(store.dispatch).toHaveBeenCalledWith(actions.update({ request: mockRequest }));
//   });

//   it('should dispatch deleteById action', () => {
//     const id = 1;
//     service.deleteById(id);

//     expect(store.dispatch).toHaveBeenCalledWith(actions.deleteById({ id }));
//   });
// });
