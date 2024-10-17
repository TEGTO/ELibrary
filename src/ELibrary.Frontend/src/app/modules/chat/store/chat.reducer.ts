/* eslint-disable @typescript-eslint/no-explicit-any */

import { createReducer, on } from "@ngrx/store";
import { changeChatVisibilityState } from "..";

export interface ChatState {
    isVisible: boolean,
    error: any
}
const initialChatState: ChatState = {
    isVisible: false,
    error: null
};

export const chatReducer = createReducer(
    initialChatState,

    on(changeChatVisibilityState, (state, { state: visibilityState }) => ({
        ...state,
        isVisible: visibilityState,
        error: null
    })),
);