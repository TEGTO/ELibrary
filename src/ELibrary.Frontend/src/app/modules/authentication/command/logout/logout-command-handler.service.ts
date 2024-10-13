import { Injectable } from '@angular/core';
import { CommandHandler } from '../../../shared';
import { AuthenticationService } from '../../services/authentication-service/authentication-service';
import { LogOutCommand } from './logout-command';

@Injectable({
  providedIn: 'root'
})
export class LogOutCommandHandlerService extends CommandHandler<LogOutCommand> {

  constructor(
    private readonly authService: AuthenticationService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: LogOutCommand): void {
    this.authService.logOutUser();
  }

}