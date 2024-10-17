import { ScrollingModule } from '@angular/cdk/scrolling';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BehaviorSubject } from 'rxjs';
import { ChatService } from '../..';
import { ChatComponent } from './chat.component';

describe('ChatComponent', () => {
  let component: ChatComponent;
  let fixture: ComponentFixture<ChatComponent>;
  let chatService: jasmine.SpyObj<ChatService>;
  let chatState: BehaviorSubject<boolean>;;

  beforeEach(async () => {
    chatService = jasmine.createSpyObj<ChatService>(['getChatVisibilityState', 'changeChatVisibilityState']);
    chatState = new BehaviorSubject<boolean>(false);
    chatService.getChatVisibilityState.and.returnValue(chatState.asObservable());
    chatService.changeChatVisibilityState.and.callFake((state: boolean) => {
      chatState.next(state);
    });

    await TestBed.configureTestingModule({
      declarations: [ChatComponent],
      imports: [
        NoopAnimationsModule,
        MatCardModule,
        MatButtonModule,
        FormsModule,
        ScrollingModule
      ],
      providers: [{ provide: ChatService, useValue: chatService }]
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
    expect(component.messages.length).toBe(1);
    expect(component.messages[0].text).toBe("Hello! Ask me about any book 😊.");
    expect(component.messages[0].isSent).toBeFalse();
  });

  it('should add new message when sendMessage is called', () => {
    component.newMessage = 'This is a test message';
    component.sendMessage();
    fixture.detectChanges();

    expect(component.messages.length).toBe(2);
    expect(component.messages[1].text).toBe('This is a test message');
    expect(component.messages[1].isSent).toBeTrue();
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

    expect(component.messages.length).toBe(1);
  });

  it('should call showChat() when open button is clicked', () => {
    spyOn(component, 'showChat').and.callThrough();

    const openButton = fixture.debugElement.query(By.css('.open-wrapper__open-chat'));
    openButton.triggerEventHandler('click', null);
    fixture.detectChanges();

    expect(component.showChat).toHaveBeenCalled();
  });

  it('should call sendMessage() when Enter key is pressed in input', () => {
    spyOn(component, 'sendMessage').and.callThrough();

    const inputField = fixture.debugElement.query(By.css('input'));
    inputField.triggerEventHandler('keydown.enter', {});

    expect(component.sendMessage).toHaveBeenCalled();
  });

  it('should react to changes in isChatVisible$ true', () => {
    chatService.changeChatVisibilityState(false);
    chatService.changeChatVisibilityState(true);
    fixture.detectChanges();

    component.isChatVisible$.subscribe((visible) => {
      expect(visible).toBeTrue();
    });
  });

  it('should react to changes in isChatVisible$ false', () => {
    chatService.changeChatVisibilityState(true);
    chatService.changeChatVisibilityState(false);
    fixture.detectChanges();

    component.isChatVisible$.subscribe((visible) => {
      expect(visible).toBeFalse();
    });
  });
});