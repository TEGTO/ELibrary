import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../..';
import { UserData } from '../../../shared';

@Component({
  selector: 'app-authenticated',
  templateUrl: './authenticated.component.html',
  styleUrl: './authenticated.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthenticatedComponent implements OnInit {
  userData$!: Observable<UserData>;

  constructor(
    private readonly authService: AuthenticationService,
  ) { }

  ngOnInit(): void {
    this.userData$ = this.authService.getUserData();
  }

  logOutUser() {
    this.authService.logOutUser();
  }
}