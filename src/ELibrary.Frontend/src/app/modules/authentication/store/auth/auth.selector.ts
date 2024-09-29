import { MemoizedSelector, createFeatureSelector, createSelector } from "@ngrx/store";
import { AuthState } from "../..";
import { UserAuth } from "../../../shared";

export const selectAuthState = createFeatureSelector<AuthState>('authentication');
export const selectUserAuth: MemoizedSelector<object, UserAuth> = createSelector(
    selectAuthState,
    (state: AuthState) => (state.userAuth)
);
export const selectIsRegistrationSuccess = createSelector(
    selectAuthState,
    (state: AuthState) => state.isRegistrationSuccess
);
export const selectIsRefreshSuccessful = createSelector(
    selectAuthState,
    (state: AuthState) => state.isRefreshSuccessful
);
export const selectIsUpdateSuccess = createSelector(
    selectAuthState,
    (state: AuthState) => state.isUpdateSuccess
);
export const selectAuthErrors = createSelector(
    selectAuthState,
    (state: AuthState) => state.error
);