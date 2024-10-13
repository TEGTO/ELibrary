import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AdminState } from "..";

export const selectAdminState = createFeatureSelector<AdminState>('admin');
export const selectUsers = createSelector(
    selectAdminState,
    (state: AdminState) => state.users
);
export const selectUserTotalAmount = createSelector(
    selectAdminState,
    (state: AdminState) => state.userTotalAmount
);
export const selectUserById = (userId: string) => createSelector(
    selectAdminState,
    (state: AdminState) => state.users.find(user => user.id === userId)!
);
export const selectClientByUserId = (userId: string) => createSelector(
    selectAdminState,
    (state: AdminState) => state.clients.find(client => client.userId === userId)
);
export const selectAdminStateError = createSelector(
    selectAdminState,
    (state: AdminState) => state.error
);