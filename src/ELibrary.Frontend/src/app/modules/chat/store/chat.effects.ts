import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, debounceTime, map, mergeMap, of } from "rxjs";
import { sendAdvisorQuery, sendAdvisorQueryFailure, sendAdvisorQuerySuccess } from "..";
import { environment } from "../../../../environment/environment";
import { AdvisorApiService } from "../../shared";

@Injectable()
export class ChatEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: AdvisorApiService,
    ) { }

    sendAdvisorQuery$ = createEffect(() =>
        this.actions$.pipe(
            ofType(sendAdvisorQuery),
            debounceTime(environment.botChatDebouncingTimeInMilliseconds),
            mergeMap((action) =>
                this.apiService.sendQuery(action.req).pipe(
                    map(response => sendAdvisorQuerySuccess({ response: response })),
                    catchError(error => of(sendAdvisorQueryFailure({ error: error.message })))
                )
            )
        )
    );

}