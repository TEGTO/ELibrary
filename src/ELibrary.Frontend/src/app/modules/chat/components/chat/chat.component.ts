import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatService } from '../..';

interface ChatMessage {
  text: string;
  isSent: boolean;
}

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
  messages: ChatMessage[] = [
    { text: "Hello! Ask me about any book 😊.", isSent: false },
  ];

  isChatVisible$ !: Observable<boolean>;

  constructor(private readonly chatService: ChatService) { }

  ngOnInit() {
    this.isChatVisible$ = this.chatService.getChatVisibilityState();
  }

  hideChat() {
    this.chatService.changeChatVisibilityState(false);
  }

  showChat() {
    this.chatService.changeChatVisibilityState(true);
  }

  sendMessage() {
    if (this.newMessage.trim()) {
      this.messages = [...this.messages, { text: this.newMessage, isSent: true }]
      this.newMessage = '';
    }
  }
}