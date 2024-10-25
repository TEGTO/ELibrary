import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER, ChangeChatVisibilityCommand, ChatMessage, ChatService, SEND_ADVISOR_MESSAGE_COMMAND_HANDLER, SendAdvisorMessageCommand } from '../..';
import { environment } from '../../../../../environment/environment';
import { CommandHandler } from '../../../shared';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  animations: [
    trigger('slideInOut', [
      state('in', style({ opacity: 1, transform: 'translateY(0)' })),
      state('out', style({ opacity: 0, transform: 'translateY(100%)', display: "none" })),
      transition('out => in', [
        style({ opacity: 0, transform: 'translateY(100%)' }),
        animate('300ms linear')
      ]),
      transition('in => out', [
        animate('300ms linear', style({ opacity: 0, transform: 'translateY(100%)' }))
      ])
    ])
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatComponent implements OnInit {
  readonly itemHeight = 25;
  readonly scollSize = 370;
  newMessage = '';

  messages$ !: Observable<ChatMessage[]>;
  isChatVisible$ !: Observable<boolean>;
  isResponseLoading$ !: Observable<boolean>;

  get advisorProfilePicuture() { return environment.botProfilePicture; }

  constructor(
    private readonly chatService: ChatService,
    @Inject(CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER) private readonly changeChatVisibilityHandler: CommandHandler<ChangeChatVisibilityCommand>,
    @Inject(SEND_ADVISOR_MESSAGE_COMMAND_HANDLER) private readonly sendAdvisorRequestHandler: CommandHandler<SendAdvisorMessageCommand>
  ) { }

  ngOnInit() {
    this.isChatVisible$ = this.chatService.getChatVisibilityState();
    this.messages$ = this.chatService.getChatMessages();
    this.isResponseLoading$ = this.chatService.getIsReponseLoading();
  }

  hideChat() {
    const command: ChangeChatVisibilityCommand =
    {
      state: false
    }
    this.changeChatVisibilityHandler.dispatch(command);
  }

  showChat() {
    const command: ChangeChatVisibilityCommand =
    {
      state: true
    }
    this.changeChatVisibilityHandler.dispatch(command);
  }

  sendMessage() {
    if (this.newMessage.trim()) {
      const command: SendAdvisorMessageCommand =
      {
        message: this.newMessage
      }
      this.sendAdvisorRequestHandler.dispatch(command)
      this.newMessage = '';
    }
  }
}