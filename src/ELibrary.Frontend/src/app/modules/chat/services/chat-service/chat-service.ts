import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export abstract class ChatService {
    abstract getChatVisibilityState(): Observable<boolean>;
    abstract changeChatVisibilityState(state: boolean): void;
}