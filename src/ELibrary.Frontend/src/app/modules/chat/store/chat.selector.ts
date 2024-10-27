import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ChatState } from "..";

export const selectChatState = createFeatureSelector<ChatState>('chat');
export const selectChatVisibleState = createSelector(
    selectChatState,
    (state: ChatState) => state.isVisible
);
export const selectChatMessages = createSelector(
    selectChatState,
    (state: ChatState) => state.messages
);
export const selectIsResposeLoading = createSelector(
    selectChatState,
    (state: ChatState) => state.isResponseLoading
);