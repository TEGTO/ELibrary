import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable, map } from 'rxjs';
import { AuthenticationDialogManager, AuthenticationService } from '../../../authentication';

@Component({
  selector: 'app-main-view',
  templateUrl: './main-view.component.html',
  styleUrls: ['./main-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainViewComponent implements OnInit {
  isAuthenticated$!: Observable<boolean>;

  constructor(
    private readonly authService: AuthenticationService,
    private readonly authDialogManager: AuthenticationDialogManager
  ) { }

  openLoginMenu() {
    this.authDialogManager.openLoginMenu();
  }

  ngOnInit(): void {
    this.isAuthenticated$ = this.authService.getAuthData().pipe(
      map(data => data.isAuthenticated)
    );
  }
}