import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { changeChatVisibilityState, selectChatVisibleState } from '../..';
import { ChatService } from './chat-service';

@Injectable({
  providedIn: 'root'
})
export class ChatControllerService implements ChatService {

  constructor(private readonly store: Store) { }

  getChatVisibilityState(): Observable<boolean> {
    return this.store.select(selectChatVisibleState);
  }
  changeChatVisibilityState(state: boolean): void {
    this.store.dispatch(changeChatVisibilityState({ state: state }));
  }
}
