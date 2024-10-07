import { Injectable } from '@angular/core';
import { StartAddingClientCommand } from '../../..';
import { CommandHandler, getClientAddInformationPath, RedirectorService } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class StartAddingClientCommandHandlerService extends CommandHandler<StartAddingClientCommand> {
  constructor(
    private readonly redirector: RedirectorService,
  ) {
    super();
  }

  dispatch(command: StartAddingClientCommand): void {
    this.redirector.redirectTo(getClientAddInformationPath(), { redirectTo: command.redirectAfter });

  }
}
