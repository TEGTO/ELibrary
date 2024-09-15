import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationDialogManager, AuthenticationService } from '../../../authentication';
import { AuthData, Policy, PolicyType } from '../../../shared';

@Component({
  selector: 'app-main-view',
  templateUrl: './main-view.component.html',
  styleUrls: ['./main-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainViewComponent implements OnInit {
  authData$!: Observable<AuthData>;

  constructor(
    private readonly authService: AuthenticationService,
    private readonly authDialogManager: AuthenticationDialogManager
  ) { }

  ngOnInit(): void {
    this.authData$ = this.authService.getAuthData();
  }

  openLoginMenu() {
    this.authDialogManager.openLoginMenu();
  }
  checkManagerPolicy(roles: string[]) {
    return Policy.checkPolicy(PolicyType.ManagerPolicy, roles);
  }
  checkAdminPolicy(roles: string[]) {
    return Policy.checkPolicy(PolicyType.AdminPolicy, roles);
  }
}