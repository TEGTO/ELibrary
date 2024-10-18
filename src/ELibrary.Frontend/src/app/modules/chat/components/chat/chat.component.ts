import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, Inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER, ChangeChatVisibilityCommand, ChatMessage, ChatService } from '../..';
import { environment } from '../../../../../environment/environment';
import { CommandHandler } from '../../../shared';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  animations: [
    trigger('slideInOut', [
      state('in', style({ opacity: 1, transform: 'translateY(0)' })),
      state('out', style({ opacity: 0, transform: 'translateY(100%)' })),
      transition('out => in', [
        style({ opacity: 0, transform: 'translateY(100%)' }),
        animate('300ms linear')
      ]),
      transition('in => out', [
        animate('300ms linear', style({ opacity: 0, transform: 'translateY(100%)' }))
      ])
    ])
  ]
})
export class ChatComponent implements OnInit {
  readonly itemHeight = 25;
  readonly scollSize = 350;
  newMessage = '';

  messages$ !: Observable<ChatMessage[]>;
  isChatVisible$ !: Observable<boolean>;

  get advisorProfilePicuture() { return environment.botProfilePicture; }

  constructor(
    private readonly chatService: ChatService,
    @Inject(CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER) private readonly changeChatVisibilityHandler: CommandHandler<ChangeChatVisibilityCommand>
  ) { }

  ngOnInit() {
    this.isChatVisible$ = this.chatService.getChatVisibilityState();
    this.messages$ = this.chatService.getChatMessages();
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
      this.newMessage = '';
    }
  }
}