import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ADMIN_DELETE_USER_COMMAND_HANDLER, AdminDeleteUserCommand, AdminService, START_ADMIN_REGISTER_USER_COMMAND_HANDLER, StartAdminRegisterUserCommand } from '../..';
import { AdminGetUserFilter, AdminUser, CommandHandler, GenericTableComponent, getAdminUserInfoPath, getDefaultAdminGetUserFilter, LocaleService, LocalizedDatePipe, RedirectorService } from '../../../shared';

@Component({
  selector: 'app-admin-user-table',
  templateUrl: './admin-user-table.component.html',
  styleUrl: './admin-user-table.component.scss'
})
export class AdminUserTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<AdminUser[]>;
  totalAmount$!: Observable<number>;

  private filterReq: AdminGetUserFilter = getDefaultAdminGetUserFilter();
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    { header: 'Id', field: 'id', linkPath: (item: any) => getAdminUserInfoPath(item.id) },
    { header: 'User Name', field: 'userName' },
    { header: 'Email', field: 'email' },
    { header: 'Registred At', field: 'registredAt', pipe: new LocalizedDatePipe(this.localeService.getLocale()), pipeArgs: [true] },
  ];

  constructor(
    private readonly localeService: LocaleService,
    private readonly adminService: AdminService,
    private readonly redirector: RedirectorService,
    @Inject(START_ADMIN_REGISTER_USER_COMMAND_HANDLER) private readonly startRegisterHandler: CommandHandler<StartAdminRegisterUserCommand>,
    @Inject(ADMIN_DELETE_USER_COMMAND_HANDLER) private readonly deleteHandler: CommandHandler<AdminDeleteUserCommand>,
  ) { }

  ngOnInit(): void {
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  private updateFilterPagination(pagination: { pageIndex: number, pageSize: number }): void {
    this.filterReq = {
      ...this.filterReq,
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    };
  }

  private fetchTotalAmount(): void {
    this.totalAmount$ = this.adminService.getPaginatedUserAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.adminService.getPaginatedUsers(this.filterReq).pipe(
      map(users => users.slice(0, pagination.pageSize))
    );
  }

  onPageChange(pagination: { pageIndex: number, pageSize: number }): void {
    this.fetchPaginatedItems(pagination);
  }

  filterChange(req: AdminGetUserFilter): void {
    if (this.table) {
      this.table.resetPagination();
    }
    this.filterReq = { ...req };
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  createNew() {
    const command: StartAdminRegisterUserCommand = {};
    this.startRegisterHandler.dispatch(command);
  }
  update(item: AdminUser) {
    this.redirector.redirectTo(getAdminUserInfoPath(item.id));
  }
  delete(item: AdminUser) {
    const command: AdminDeleteUserCommand = {
      userId: item.id
    };
    this.deleteHandler.dispatch(command);
  }
}
