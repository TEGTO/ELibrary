import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { deleteUser, deleteUserFailure, deleteUserSuccess, getAuthData, getAuthDataFailure, getAuthDataSuccess, logOutUser, logOutUserSuccess, oauthSignInUser, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess, updateUserData, updateUserDataFailure, updateUserDataSuccess } from "../..";
import { AuthenticationApiService, copyAuthTokenToAuthData as copyAuthTokenToUserAuth, copyUserUpdateRequestToAuthData as copyUserUpdateRequestToUserAuth, LocalStorageService, UserApiService, UserAuth } from "../../../shared";

@Injectable()
export class AuthEffects {
    readonly storageUserAuthKey: string = "userAuth";

    constructor(
        private readonly actions$: Actions,
        private readonly authApiService: AuthenticationApiService,
        private readonly userApiService: UserApiService,
        private readonly localStorage: LocalStorageService
    ) { }

    registerUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(registerUser),
            mergeMap((action) =>
                this.authApiService.registerUser(action.req).pipe(
                    map((response) => {
                        this.localStorage.setItem(this.storageUserAuthKey, JSON.stringify(response));
                        return registerSuccess({ userAuth: response });
                    }),
                    catchError(error => of(registerFailure({ error: error.message })))
                )
            )
        )
    );
    singInUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(signInUser),
            mergeMap((action) =>
                this.authApiService.loginUser(action.req).pipe(
                    map((response) => {
                        this.localStorage.setItem(this.storageUserAuthKey, JSON.stringify(response));
                        return signInUserSuccess({ userAuth: response });
                    }),
                    catchError(error => of(signInUserFailure({ error: error.message })))
                )
            )
        )
    );
    oauthSignInUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(oauthSignInUser),
            mergeMap((action) =>
                this.authApiService.loginUserOAuth(action.req).pipe(
                    map((response) => {
                        this.localStorage.setItem(this.storageUserAuthKey, JSON.stringify(response));
                        return signInUserSuccess({ userAuth: response });
                    }),
                    catchError(error => of(signInUserFailure({ error: error.message })))
                )
            )
        )
    );
    getAuthData$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getAuthData),
            mergeMap(() => {
                const json = this.localStorage.getItem(this.storageUserAuthKey);
                if (json !== null) {
                    const userAuth: UserAuth = JSON.parse(json);
                    return of(getAuthDataSuccess({ userAuth: userAuth }));
                }
                else {
                    return of(getAuthDataFailure());
                }
            }),
            catchError(() => of(getAuthDataFailure()))
        )
    );
    logOutUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(logOutUser),
            mergeMap(() => {
                this.localStorage.removeItem(this.storageUserAuthKey);
                return of(logOutUserSuccess());
            })
        )
    );
    refreshToken$ = createEffect(() =>
        this.actions$.pipe(
            ofType(refreshAccessToken),
            mergeMap((action) =>
                this.authApiService.refreshToken(action.authToken).pipe(
                    map((response) => {
                        const json = this.localStorage.getItem(this.storageUserAuthKey);
                        let userAuth: UserAuth = JSON.parse(json!);
                        userAuth = copyAuthTokenToUserAuth(userAuth, response);
                        this.localStorage.setItem(this.storageUserAuthKey, JSON.stringify(userAuth));
                        return refreshAccessTokenSuccess({ authToken: response });
                    }),
                    catchError(error => {
                        this.localStorage.removeItem(this.storageUserAuthKey);
                        return of(refreshAccessTokenFailure({ error: error.message }));
                    })
                )
            )
        )
    );
    updateUserData$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateUserData),
            mergeMap((action) =>
                this.userApiService.updateUser(action.req).pipe(
                    map(() => {
                        const json = this.localStorage.getItem(this.storageUserAuthKey);
                        let userAuth: UserAuth = JSON.parse(json!);
                        userAuth = copyUserUpdateRequestToUserAuth(userAuth, action.req);
                        this.localStorage.setItem(this.storageUserAuthKey, JSON.stringify(userAuth));
                        return updateUserDataSuccess({ req: action.req });
                    }),
                    catchError(error => {
                        return of(updateUserDataFailure({ error: error.message }));
                    })
                )
            )
        )
    );
    deleteUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(deleteUser),
            mergeMap(() =>
                this.userApiService.deleteUser().pipe(
                    map(() => {
                        this.localStorage.removeItem(this.storageUserAuthKey);
                        return deleteUserSuccess();
                    }),
                    catchError(error => {
                        return of(deleteUserFailure({ error: error.message }));
                    })
                )
            )
        )
    );
}