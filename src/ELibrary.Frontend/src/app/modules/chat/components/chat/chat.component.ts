/* eslint-disable @typescript-eslint/no-explicit-any */
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { ChangeDetectionStrategy, Component, Inject, NgZone, OnInit, SecurityContext, ViewChild } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { NgDompurifySanitizer } from '@tinkoff/ng-dompurify';
import { Observable, tap } from 'rxjs';
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
      state('out', style({ opacity: 0, transform: 'translateY(100%)', visibility: 'hidden' })),
      transition('out => in', [
        animate('300ms linear')
      ]),
      transition('in => out', [
        animate('300ms linear')
      ])
    ])
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatComponent implements OnInit {
  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  readonly itemHeight = 50;
  readonly scollSize = 370;
  newMessage = '';

  messages$ !: Observable<ChatMessage[]>;
  isChatVisible$ !: Observable<boolean>;
  isCardVisible !: boolean;
  isResponseLoading$ !: Observable<boolean>;

  get advisorProfilePicuture() { return environment.botProfilePicture; }

  constructor(
    private readonly ngZone: NgZone,
    private readonly chatService: ChatService,
    @Inject(CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER) private readonly changeChatVisibilityHandler: CommandHandler<ChangeChatVisibilityCommand>,
    @Inject(SEND_ADVISOR_MESSAGE_COMMAND_HANDLER) private readonly sendAdvisorRequestHandler: CommandHandler<SendAdvisorMessageCommand>,
    private readonly dompurifySanitizer: NgDompurifySanitizer,
    private readonly router: Router
  ) { }

  ngOnInit() {
    this.isChatVisible$ = this.chatService.getChatVisibilityState();

    this.messages$ = this.chatService.getChatMessages().pipe(
      tap(() => {
        this.scrollToBottom();
      })
    );
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
    this.isCardVisible = true;
    const command: ChangeChatVisibilityCommand =
    {
      state: true
    }
    this.changeChatVisibilityHandler.dispatch(command);
  }

  private scrollToBottom() {
    this.ngZone.runOutsideAngular(() => {
      setTimeout(() => {
        if (this.viewport) {
          this.viewport.scrollToIndex(this.viewport.getDataLength() + 1, 'smooth');
        }
      }, 0);
    });
  }

  formatChatMessage(message: ChatMessage): SafeHtml {
    const bookLinkRegex = /BookId:\s*#?(\d+)\s*'([^']+)'/g;

    if (this.isChatMessageLink(message)) {
      const sanitizedHTML = this.sanitizeHTML(
        message.text.replace(
          bookLinkRegex,
          (match, id, title) =>
            `<span><a href="${id}" target="_blank" class="book-link">${title}</a></span>`
        )
      );
      return sanitizedHTML;
    }
    else if (!message.isSent && !bookLinkRegex.test(message.text)) {
      return message.text;
    }
    return message.text;
  }

  isChatMessageLink(message: ChatMessage): boolean {
    return !message.isSent && /BookId:\s*#?\d+/.test(message.text);
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
  trackByIndex(index: number): number {
    return index;
  }
  private sanitizeHTML(html: string): SafeHtml {
    return this.dompurifySanitizer.sanitize(SecurityContext.HTML, html);
  }
  onAnimationDone(event: any) {
    this.isCardVisible = event.toState === 'in';
  }
  processLinks(e: any) {
    const element: HTMLElement = e.target;
    if (element.nodeName === 'A') {
      e.preventDefault();
      const link = element.getAttribute('href');
      this.router.navigate([link]);
    }
  }
}