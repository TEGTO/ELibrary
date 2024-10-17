import { createAction, props } from "@ngrx/store";

export const changeChatVisibilityState = createAction(
    '[Chart] Change Chat Visibility State',
    props<{ state: boolean }>()
);
