import { Injectable } from '@angular/core';
import { ChangeChatVisibilityCommand, ChatService } from '../..';
import { CommandHandler } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ChangeChatVisibilityCommandHandlerService extends CommandHandler<ChangeChatVisibilityCommand> {

  constructor(
    private readonly chatService: ChatService
  ) {
    super();
  }

  dispatch(command: ChangeChatVisibilityCommand): void {
    this.chatService.changeChatVisibilityState(command.state);
  }
}