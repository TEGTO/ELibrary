/* eslint-disable @typescript-eslint/no-explicit-any */
import { ScrollingModule } from '@angular/cdk/scrolling';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BehaviorSubject } from 'rxjs';
import { CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER, ChangeChatVisibilityCommand, ChatMessage, ChatService, SEND_ADVISOR_MESSAGE_COMMAND_HANDLER, SendAdvisorMessageCommand } from '../..';
import { CommandHandler } from '../../../shared';
import { ChatComponent } from './chat.component';

describe('ChatComponent', () => {
  let component: ChatComponent;
  let fixture: ComponentFixture<ChatComponent>;
  let chatServiceSpy: jasmine.SpyObj<ChatService>;
  let changeChatVisibilityHandlerSpy: jasmine.SpyObj<CommandHandler<ChangeChatVisibilityCommand>>;
  let sendAdvisorMessageCommandHandlerSpy: jasmine.SpyObj<CommandHandler<SendAdvisorMessageCommand>>;
  let chatState: BehaviorSubject<boolean>;
  let messagesSubject: BehaviorSubject<ChatMessage[]>;
  let loadingSubject: BehaviorSubject<boolean>;

  beforeEach(async () => {
    chatServiceSpy = jasmine.createSpyObj<ChatService>(
      ['getChatVisibilityState', 'getChatMessages', 'getIsReponseLoading']
    );
    changeChatVisibilityHandlerSpy = jasmine.createSpyObj<CommandHandler<ChangeChatVisibilityCommand>>(['dispatch']);
    sendAdvisorMessageCommandHandlerSpy = jasmine.createSpyObj<CommandHandler<SendAdvisorMessageCommand>>(['dispatch']);

    chatState = new BehaviorSubject<boolean>(false);
    messagesSubject = new BehaviorSubject<ChatMessage[]>([{ text: 'Hello! Ask me about any book 😊.', isSent: false }]);
    loadingSubject = new BehaviorSubject<boolean>(false);

    chatServiceSpy.getChatVisibilityState.and.returnValue(chatState.asObservable());
    chatServiceSpy.getChatMessages.and.returnValue(messagesSubject.asObservable());
    chatServiceSpy.getIsReponseLoading.and.returnValue(loadingSubject.asObservable());

    await TestBed.configureTestingModule({
      declarations: [ChatComponent],
      imports: [
        NoopAnimationsModule,
        MatCardModule,
        MatButtonModule,
        FormsModule,
        ScrollingModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: ChatService, useValue: chatServiceSpy },
        { provide: CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER, useValue: changeChatVisibilityHandlerSpy },
        { provide: SEND_ADVISOR_MESSAGE_COMMAND_HANDLER, useValue: sendAdvisorMessageCommandHandlerSpy }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default message', () => {
    fixture.detectChanges();
    component.messages$.subscribe((messages) => {
      expect(messages.length).toBe(1);
      expect(messages[0].text).toBe('Hello! Ask me about any book 😊.');
      expect(messages[0].isSent).toBeFalse();
    });
  });

  it('should call sendAdvisorRequestHandler when sendMessage is called', () => {
    component.newMessage = 'This is a test message';
    component.sendMessage();
    fixture.detectChanges();

    expect(sendAdvisorMessageCommandHandlerSpy.dispatch).toHaveBeenCalled();
  });

  it('should clear the input field after sending a message', () => {
    component.newMessage = 'This is another test message';
    component.sendMessage();
    fixture.detectChanges();

    expect(component.newMessage).toBe('');
  });

  it('should not add empty messages when sendMessage is called', () => {
    component.newMessage = '  ';
    component.sendMessage();
    fixture.detectChanges();

    component.messages$.subscribe((messages) => {
      expect(messages.length).toBe(1);
    });
  });

  it('should call showChat() when open button is clicked', () => {
    spyOn(component, 'showChat').and.callThrough();

    const openButton = fixture.debugElement.query(By.css('.open-wrapper__open-chat'));
    openButton.triggerEventHandler('click', null);
    fixture.detectChanges();

    expect(component.showChat).toHaveBeenCalled();
  });

  it('should react to changes in isChatVisible$ true', () => {
    chatState.next(true);
    fixture.detectChanges();

    component.isChatVisible$.subscribe((visible) => {
      expect(visible).toBeTrue();
    });
  });

  it('should react to changes in isChatVisible$ false', () => {
    chatState.next(false);
    fixture.detectChanges();

    component.isChatVisible$.subscribe((visible) => {
      expect(visible).toBeFalse();
    });
  });

  it('should show loading dots when isResponseLoading$ is true', () => {
    loadingSubject.next(true);
    chatState.next(true);
    component.isCardVisible = true;

    fixture.detectChanges();

    const loadingDots = fixture.debugElement.query(By.css('.loading-dots'));
    expect(loadingDots).toBeTruthy();
  });

  it('should hide loading dots when isResponseLoading$ is false', () => {
    loadingSubject.next(false);
    fixture.detectChanges();

    const loadingDots = fixture.debugElement.query(By.css('.loading-dots'));
    expect(loadingDots).toBeFalsy();
  });

  it('should update isCardVisible when onAnimationDone is triggered', fakeAsync(() => {
    component.onAnimationDone({ toState: 'in' });
    fixture.detectChanges();
    expect(component.isCardVisible).toBeTrue();

    component.onAnimationDone({ toState: 'out' });
    fixture.detectChanges();
    expect(component.isCardVisible).toBeFalse();
  }));

  it('should scroll to bottom when new messages are received', fakeAsync(() => {
    const scrollToIndexSpy = spyOn<any>(component, 'scrollToBottom').and.callThrough();

    component.ngOnInit();

    component.messages$.subscribe();

    messagesSubject.next([
      { text: 'Hello!', isSent: true },
      { text: 'Hi there!', isSent: false }
    ]);

    tick(1000);
    fixture.detectChanges();

    expect(scrollToIndexSpy).toHaveBeenCalled();
  }));

  it('should format message correctly if it contains a book link', () => {
    const message = { text: "BookId:#123 'The Hobbit'", isSent: false };
    const formattedMessage = component.formatChatMessage(message) as string;

    expect(formattedMessage.toString()).toContain('<div><a class="book-link" href="123">The Hobbit</a></div>');
  });

  it('should detect if a message is a book link', () => {
    const message = { text: "BookId:#123 'The Hobbit'", isSent: false };
    expect(component.isChatMessageLink(message)).toBeTrue();

    const normalMessage = { text: 'Just a normal message', isSent: false };
    expect(component.isChatMessageLink(normalMessage)).toBeFalse();
  });
});