import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { StoreModule } from '@ngrx/store';
import { CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER, ChangeChatVisibilityCommandHandlerService, ChatComponent, ChatControllerService, chatReducer, ChatService } from '.';

@NgModule({
  declarations: [
    ChatComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    ScrollingModule,
    FormsModule,
    StoreModule.forFeature('chat', chatReducer),
  ],
  providers: [
    { provide: ChatService, useClass: ChatControllerService },
    { provide: CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER, useClass: ChangeChatVisibilityCommandHandlerService },
  ],
  exports: [ChatComponent]
})
export class ChatModule { }
