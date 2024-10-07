import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AuthenticationService, START_LOGIN_COMMAND_HANDLER, StartLoginCommand } from '../../../authentication';
import { CommandHandler, getClientOrderHistoryPath, getManagerBooksPath, Policy, PolicyType, UserAuth } from '../../../shared';
import { ClientService } from '../../../shop';

@Component({
  selector: 'app-main-view',
  templateUrl: './main-view.component.html',
  styleUrls: ['./main-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainViewComponent implements OnInit {
  userAuth$!: Observable<UserAuth>;

  get managerPath() { return getManagerBooksPath(); }
  get orderHistoryPath() { return getClientOrderHistoryPath(); }

  constructor(
    private readonly authService: AuthenticationService,
    private readonly clientService: ClientService,
    @Inject(START_LOGIN_COMMAND_HANDLER) private readonly loginHandler: CommandHandler<StartLoginCommand>
  ) { }

  ngOnInit(): void {
    this.userAuth$ = this.authService.getUserAuth().pipe(
      tap((userAuth) => {
        if (userAuth.isAuthenticated) {
          this.clientService.getClient();
        }
      })
    );
  }

  openLoginMenu() {
    const command: StartLoginCommand = {};
    this.loginHandler.dispatch(command);
  }
  checkClientPolicy(roles: string[]) {
    return Policy.checkPolicy(PolicyType.ClientPolicy, roles);
  }
  checkManagerPolicy(roles: string[]) {
    return Policy.checkPolicy(PolicyType.ManagerPolicy, roles);
  }
  checkAdminPolicy(roles: string[]) {
    return Policy.checkPolicy(PolicyType.AdminPolicy, roles);
  }
}