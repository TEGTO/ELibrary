import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { changeChatVisibilityState, ChatMessage, selectChatMessages, selectChatVisibleState, selectIsResposeLoading, sendAdvisorQuery } from '../..';
import { AdvisorQueryRequest } from '../../../shared';
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
  getChatMessages(): Observable<ChatMessage[]> {
    return this.store.select(selectChatMessages);
  }
  sendAdvisorRequest(req: AdvisorQueryRequest): void {
    this.store.dispatch(sendAdvisorQuery({ req: req }));
  }
  getIsReponseLoading(): Observable<boolean> {
    return this.store.select(selectIsResposeLoading);
  }
}
