import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AdvisorQueryRequest } from "../../../shared";
import { ChatMessage } from "../../store/chat.reducer";

@Injectable({
    providedIn: 'root'
})
export abstract class ChatService {
    abstract getChatVisibilityState(): Observable<boolean>;
    abstract changeChatVisibilityState(state: boolean): void;
    abstract getChatMessages(): Observable<ChatMessage[]>;
    abstract sendAdvisorRequest(req: AdvisorQueryRequest): void;
    abstract getIsReponseLoading(): Observable<boolean>;
}