import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthenticationDialogManager } from '../..';

@Component({
  selector: 'app-auth-unauthenticated',
  templateUrl: './unauthenticated.component.html',
  styleUrl: './unauthenticated.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UnauthenticatedComponent {

  constructor(
    private readonly authDialogManager: AuthenticationDialogManager
  ) { }

  openLoginMenu() {
    this.authDialogManager.openLoginMenu();
  }
}
