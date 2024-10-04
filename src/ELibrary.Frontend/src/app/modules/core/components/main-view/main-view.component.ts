import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AuthenticationDialogManager, AuthenticationService } from '../../../authentication';
import { Policy, PolicyType, redirectPathes, UserAuth } from '../../../shared';
import { ClientService } from '../../../shop';

@Component({
  selector: 'app-main-view',
  templateUrl: './main-view.component.html',
  styleUrls: ['./main-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainViewComponent implements OnInit {
  userAuth$!: Observable<UserAuth>;

  get managerPath() { return redirectPathes.manager_books; }
  get orderHistoryPath() { return redirectPathes.client_order_history; }

  constructor(
    private readonly authService: AuthenticationService,
    private readonly clientService: ClientService,
    private readonly authDialogManager: AuthenticationDialogManager
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
    this.authDialogManager.openLoginMenu();
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