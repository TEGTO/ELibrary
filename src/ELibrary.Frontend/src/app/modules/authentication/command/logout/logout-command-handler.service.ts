import { Injectable } from '@angular/core';
import { AuthenticationService } from '../..';
import { CommandHandler, RedirectorService } from '../../../shared';
import { LogOutCommand } from './logout-command';

@Injectable({
  providedIn: 'root'
})
export class LogOutCommandHandlerService extends CommandHandler<LogOutCommand> {

  constructor(
    private readonly authService: AuthenticationService,
    private readonly redirector: RedirectorService
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: LogOutCommand): void {
    this.authService.logOutUser();
    this.redirector.redirectToHome();
  }
}