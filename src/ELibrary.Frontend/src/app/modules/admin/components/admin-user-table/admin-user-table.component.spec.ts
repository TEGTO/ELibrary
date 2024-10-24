/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { ADMIN_DELETE_USER_COMMAND_HANDLER, AdminDeleteUserCommand, AdminService, START_ADMIN_REGISTER_USER_COMMAND_HANDLER } from '../..';
import { AdminGetUserFilter, AdminUser, CommandHandler, LocaleService, RedirectorService } from '../../../shared';
import { AdminUserTableComponent } from './admin-user-table.component';

describe('AdminUserTableComponent', () => {
  let component: AdminUserTableComponent;
  let fixture: ComponentFixture<AdminUserTableComponent>;
  let adminServiceMock: jasmine.SpyObj<AdminService>;
  let redirectorServiceMock: jasmine.SpyObj<RedirectorService>;
  let startRegisterHandlerMock: jasmine.SpyObj<CommandHandler<any>>;
  let deleteHandlerMock: jasmine.SpyObj<CommandHandler<AdminDeleteUserCommand>>;

  beforeEach(async () => {
    adminServiceMock = jasmine.createSpyObj('AdminService', ['getPaginatedUserAmount', 'getPaginatedUsers']);
    redirectorServiceMock = jasmine.createSpyObj('RedirectorService', ['redirectTo']);
    startRegisterHandlerMock = jasmine.createSpyObj('CommandHandler', ['dispatch']);
    deleteHandlerMock = jasmine.createSpyObj('CommandHandler', ['dispatch']);

    adminServiceMock.getPaginatedUserAmount.and.returnValue(of(100));
    adminServiceMock.getPaginatedUsers.and.returnValue(of([
      { id: '1', userName: 'JohnDoe', email: 'john@example.com', registredAt: new Date(), updatedAt: new Date(), roles: ['admin'], authenticationMethods: [] }
    ]));

    await TestBed.configureTestingModule({
      declarations: [AdminUserTableComponent],
      providers: [
        { provide: AdminService, useValue: adminServiceMock },
        { provide: RedirectorService, useValue: redirectorServiceMock },
        { provide: START_ADMIN_REGISTER_USER_COMMAND_HANDLER, useValue: startRegisterHandlerMock },
        { provide: ADMIN_DELETE_USER_COMMAND_HANDLER, useValue: deleteHandlerMock },
        LocaleService,  // Provide LocaleService or mock it if necessary
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch total user amount on init', () => {
    component.ngOnInit();
    expect(adminServiceMock.getPaginatedUserAmount).toHaveBeenCalled();
  });

  it('should fetch paginated users on init', () => {
    component.ngOnInit();
    expect(adminServiceMock.getPaginatedUsers).toHaveBeenCalled();
  });

  it('should update pagination and fetch paginated items on page change', () => {
    const pagination = { pageIndex: 2, pageSize: 10 };
    component.onPageChange(pagination);
    expect(adminServiceMock.getPaginatedUsers).toHaveBeenCalledWith(jasmine.objectContaining({
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    }));
  });

  it('should reset pagination and fetch new items on filter change', () => {
    const filterChangeSpy = spyOn<any>(component, 'fetchPaginatedItems');
    component.table = jasmine.createSpyObj('GenericTableComponent', ['resetPagination']);

    const filter: AdminGetUserFilter = { containsInfo: 'test', pageNumber: 1, pageSize: 10 };
    component.filterChange(filter);

    expect(component.table.resetPagination).toHaveBeenCalled();
    expect(filterChangeSpy).toHaveBeenCalledWith(component['defaultPagination']);
  });

  it('should dispatch start register user command when creating a new user', () => {
    component.createNew();
    expect(startRegisterHandlerMock.dispatch).toHaveBeenCalled();
  });

  it('should redirect to user page on update', () => {
    const user: AdminUser = { id: '1', userName: 'JohnDoe', email: 'john@example.com', registredAt: new Date(), updatedAt: new Date(), roles: ['admin'], authenticationMethods: [] };
    component.update(user);
    expect(redirectorServiceMock.redirectTo).toHaveBeenCalledWith(jasmine.stringMatching(user.id));
  });

  it('should dispatch delete user command when deleting a user', () => {
    const user: AdminUser = { id: '1', userName: 'JohnDoe', email: 'john@example.com', registredAt: new Date(), updatedAt: new Date(), roles: ['admin'], authenticationMethods: [] };
    component.delete(user);
    expect(deleteHandlerMock.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({ userId: user.id }));
  });
});