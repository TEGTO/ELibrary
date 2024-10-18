/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { changeChatVisibilityState, sendAdvisorQuery, sendAdvisorQueryFailure, sendAdvisorQuerySuccess } from "..";

export interface ChatMessage {
    text: string;
    isSent: boolean;
}

export interface ChatState {
    messages: ChatMessage[],
    isVisible: boolean,
    error: any
}
const initialChatState: ChatState = {
    messages: [
        { text: "Hello! Ask me about any book 😊.", isSent: false }
    ],
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

    on(sendAdvisorQuery, (state, { req }) => ({
        ...state,
        messages: [...state.messages, { isSent: true, text: req.message }],
        error: null
    })),
    on(sendAdvisorQuerySuccess, (state, { response }) => ({
        ...state,
        messages: [...state.messages, { isSent: false, text: response.message }],
        error: null
    })),
    on(sendAdvisorQueryFailure, (state, { error }) => ({
        ...state,
        error: error
    })),
);