import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { of } from 'rxjs';
import { createClient, deleteUser, getClient, getPaginatedUserAmount, getPaginatedUsers, getUser, registerUser, updateClient, updateUser } from '../..';
import { AdminGetUserFilter, AdminUser, AdminUserRegistrationRequest, AdminUserUpdateDataRequest, Client, CreateClientRequest, getDefaultClient, UpdateClientRequest } from '../../../shared';
import { AdminControllerService } from './admin-controller.service';

describe('AdminControllerService', () => {
  let service: AdminControllerService;
  let store: MockStore;

  const initialState = {};

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AdminControllerService,
        provideMockStore({ initialState })
      ]
    });

    service = TestBed.inject(AdminControllerService);
    store = TestBed.inject(Store) as MockStore;

    spyOn(store, 'dispatch');
  });

  it('should dispatch getUser action and select user by ID', () => {
    const userId = '123';
    const mockUser: AdminUser = { id: '123', userName: 'JohnDoe', email: 'johndoe@example.com', registredAt: new Date(), updatedAt: new Date(), roles: ['Admin'], authenticationMethods: [] };
    spyOn(store, 'select').and.returnValue(of(mockUser));

    let result: AdminUser | undefined;
    service.getUserById(userId).subscribe(user => result = user);

    expect(store.dispatch).toHaveBeenCalledWith(getUser({ info: userId }));
    expect(result).toEqual(mockUser);
  });

  it('should dispatch registerUser action with registration request', () => {
    const req: AdminUserRegistrationRequest = {
      email: 'johndoe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      roles: ['Admin']
    };

    service.registerUser(req);

    expect(store.dispatch).toHaveBeenCalledWith(registerUser({ req }));
  });

  it('should dispatch getPaginatedUsers action and select users', () => {
    const filter: AdminGetUserFilter = { pageNumber: 1, pageSize: 10, containsInfo: 'Doe' };
    const mockUsers: AdminUser[] = [
      { id: '1', userName: 'JohnDoe', email: 'johndoe@example.com', registredAt: new Date(), updatedAt: new Date(), roles: ['Admin'], authenticationMethods: [] }
    ];
    spyOn(store, 'select').and.returnValue(of(mockUsers));

    let result: AdminUser[] | undefined;
    service.getPaginatedUsers(filter).subscribe(users => result = users);

    expect(store.dispatch).toHaveBeenCalledWith(getPaginatedUsers({ req: filter }));
    expect(result).toEqual(mockUsers);
  });

  it('should dispatch getPaginatedUserAmount action and select total user amount', () => {
    const filter: AdminGetUserFilter = { pageNumber: 1, pageSize: 10, containsInfo: 'Doe' };
    const totalAmount = 100;
    spyOn(store, 'select').and.returnValue(of(totalAmount));

    let result: number | undefined;
    service.getPaginatedUserAmount(filter).subscribe(amount => result = amount);

    expect(store.dispatch).toHaveBeenCalledWith(getPaginatedUserAmount({ req: filter }));
    expect(result).toEqual(totalAmount);
  });

  it('should dispatch updateUser action with update request', () => {
    const req: AdminUserUpdateDataRequest = {
      currentLogin: 'johndoe',
      email: 'johndoe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      roles: ['Admin']
    };

    service.updateUser(req);

    expect(store.dispatch).toHaveBeenCalledWith(updateUser({ req }));
  });

  it('should dispatch deleteUser action with user ID', () => {
    const userId = '123';

    service.deleteUser(userId);

    expect(store.dispatch).toHaveBeenCalledWith(deleteUser({ id: userId }));
  });

  it('should dispatch getClient action and select client by user ID', () => {
    const userId = '123';
    const mockClient: Client = {
      ...getDefaultClient()
    };
    spyOn(store, 'select').and.returnValue(of(mockClient));

    let result: Client | undefined;
    service.getClientByUserId(userId).subscribe(client => result = client);

    expect(store.dispatch).toHaveBeenCalledWith(getClient({ userId }));
    expect(result).toEqual(mockClient);
  });

  it('should dispatch createClient action with user ID and request', () => {
    const userId = '123';
    const req: CreateClientRequest = {
      name: 'John',
      middleName: 'Doe',
      lastName: 'Smith',
      dateOfBirth: new Date(),
      address: '123 St',
      phone: '1234567890',
      email: 'john@example.com'
    };

    service.createClientForUser(userId, req);

    expect(store.dispatch).toHaveBeenCalledWith(createClient({ userId, req }));
  });

  it('should dispatch updateClient action with user ID and request', () => {
    const userId = '123';
    const req: UpdateClientRequest = {
      name: 'John',
      middleName: 'Doe',
      lastName: 'Smith',
      dateOfBirth: new Date(),
      address: '123 St',
      phone: '1234567890',
      email: 'john@example.com'
    };

    service.updateClientForUser(userId, req);

    expect(store.dispatch).toHaveBeenCalledWith(updateClient({ userId, req }));
  });
});