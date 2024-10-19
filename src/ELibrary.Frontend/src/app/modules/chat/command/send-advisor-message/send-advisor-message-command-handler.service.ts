import { Injectable } from '@angular/core';
import { ChatService, SendAdvisorMessageCommand } from '../..';
import { AdvisorQueryRequest, CommandHandler } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class SendAdvisorMessageCommandHandlerService extends CommandHandler<SendAdvisorMessageCommand> {

  constructor(
    private readonly chatService: ChatService
  ) {
    super();
  }

  dispatch(command: SendAdvisorMessageCommand): void {
    const req: AdvisorQueryRequest =
    {
      query: command.message
    }
    this.chatService.sendAdvisorRequest(req);
  }
}