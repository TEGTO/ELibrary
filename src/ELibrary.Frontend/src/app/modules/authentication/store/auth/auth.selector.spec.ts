// import { getDefaultAuthToken } from "../../../shared";
// import { AuthState } from "./auth.reducer";
// import { selectAuthErrors, selectAuthState, selectIsRefreshSuccessful, selectIsRegistrationSuccess, selectIsUpdateSuccess, selectUserAuth } from "./auth.selector";

// describe('Authentication Selectors', () => {
//     const initialState: AuthState = {
//         userAuth: {
//             isAuthenticated: false,
//             authToken: getDefaultAuthToken(),
//             email: '',
//             roles: [],
//         },
//         isRegistrationSuccess: false,
//         isRefreshSuccessful: false,
//         isUpdateSuccess: false,
//         error: null,
//     };

//     const errorState: AuthState = {
//         ...initialState,
//         error: 'An error occurred',
//     };

//     it('should select the authentication state', () => {
//         const result = selectAuthState.projector(initialState);
//         expect(result).toEqual(initialState);
//     });

//     it('should select userAuth', () => {
//         const result = selectUserAuth.projector(initialState);
//         expect(result).toEqual(initialState.userAuth);
//     });

//     it('should select isRegistrationSuccess', () => {
//         const result = selectIsRegistrationSuccess.projector(initialState);
//         expect(result).toEqual(initialState.isRegistrationSuccess);
//     });

//     it('should select isRefreshSuccessful', () => {
//         const result = selectIsRefreshSuccessful.projector(initialState);
//         expect(result).toEqual(initialState.isRefreshSuccessful);
//     });

//     it('should select isUpdateSuccess', () => {
//         const result = selectIsUpdateSuccess.projector(initialState);
//         expect(result).toEqual(initialState.isUpdateSuccess);
//     });

//     it('should select auth errors', () => {
//         const result = selectAuthErrors.projector(errorState);
//         expect(result).toEqual(errorState.error);
//     });
// });
