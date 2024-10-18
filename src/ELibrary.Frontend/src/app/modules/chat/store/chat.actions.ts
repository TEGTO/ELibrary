/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { AdvisorQueryRequest, AdvisorResponse } from "../../shared";

export const changeChatVisibilityState = createAction(
    '[Chat] Change Chat Visibility State',
    props<{ state: boolean }>()
);

export const sendAdvisorQuery = createAction(
    '[Chat] Send Advisor Query',
    props<{ req: AdvisorQueryRequest }>()
);
export const sendAdvisorQuerySuccess = createAction(
    '[Chat] Send Advisor Query Success',
    props<{ response: AdvisorResponse }>()
);
export const sendAdvisorQueryFailure = createAction(
    '[Chat] Send Advisor Query Failure',
    props<{ error: any }>()
);
