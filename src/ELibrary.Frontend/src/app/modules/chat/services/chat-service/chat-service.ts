import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ChatMessage } from "../../store/chat.reducer";

@Injectable({
    providedIn: 'root'
})
export abstract class ChatService {
    abstract getChatVisibilityState(): Observable<boolean>;
    abstract changeChatVisibilityState(state: boolean): void;
    abstract getChatMessages(): Observable<ChatMessage[]>;
}