// /* eslint-disable @typescript-eslint/no-explicit-any */
// import { deleteUserFailure, deleteUserSuccess, getAuthDataFailure, getAuthDataSuccess, logOutUserSuccess, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess, updateUserData, updateUserDataFailure, updateUserDataSuccess } from "../..";
// import { AuthToken, getDefaultUserAuth, UserAuth, UserAuthenticationRequest } from "../../../shared";
// import { authReducer, AuthState } from "./auth.reducer";

// describe('AuthReducer', () => {
//     const initialAuthState: AuthState = {
//         isRegistrationSuccess: false,
//         isUpdateSuccess: false,
//         isRefreshSuccessful: false,
//         userAuth: getDefaultUserAuth(),
//         error: null
//     };

//     const mockUserAuth: UserAuth = {
//         isAuthenticated: true,
//         authToken: {
//             accessToken: 'mockAccessToken',
//             refreshToken: 'mockRefreshToken',
//             refreshTokenExpiryDate: new Date(),
//         },
//         email: 'test@example.com',
//         roles: ['CLIENT']
//     };

//     const mockAuthToken: AuthToken = {
//         accessToken: 'newAccessToken',
//         refreshToken: 'newRefreshToken',
//         refreshTokenExpiryDate: new Date(),
//     };

//     it('should return the initial state', () => {
//         const action = { type: 'Unknown' } as any;
//         const state = authReducer(initialAuthState, action);
//         expect(state).toBe(initialAuthState);
//     });

//     it('should handle registerUser action', () => {
//         const action = registerUser({ req: { email: 'test@example.com', password: 'password', confirmPassword: 'password' } });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual(initialAuthState);
//     });

//     it('should handle registerSuccess action', () => {
//         const action = registerSuccess({ userAuth: mockUserAuth });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             userAuth: mockUserAuth,
//             isRegistrationSuccess: true,
//             error: null
//         });
//     });

//     it('should handle registerFailure action', () => {
//         const error = 'Registration failed';
//         const action = registerFailure({ error });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             error
//         });
//     });

//     it('should handle signInUser action', () => {
//         const req: UserAuthenticationRequest =
//         {
//             login: 'test@example.com',
//             password: 'password',
//         }
//         const action = signInUser({ req: req });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual(initialAuthState);
//     });

//     it('should handle signInUserSuccess action', () => {
//         const action = signInUserSuccess({ userAuth: mockUserAuth });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             userAuth: mockUserAuth,
//             error: null
//         });
//     });

//     it('should handle signInUserFailure action', () => {
//         const error = 'Sign in failed';
//         const action = signInUserFailure({ error });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             error
//         });
//     });

//     it('should handle refreshAccessToken action', () => {
//         const action = refreshAccessToken({ authToken: mockAuthToken });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             isRefreshSuccessful: false,
//             error: null
//         });
//     });

//     it('should handle refreshAccessTokenSuccess action', () => {
//         const action = refreshAccessTokenSuccess({ authToken: mockAuthToken });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             isRefreshSuccessful: true,
//             userAuth: {
//                 ...initialAuthState.userAuth,
//                 authToken: mockAuthToken
//             },
//             error: null
//         });
//     });

//     it('should handle refreshAccessTokenFailure action', () => {
//         const error = 'Refresh token failed';
//         const action = refreshAccessTokenFailure({ error });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             error
//         });
//     });

//     it('should handle logOutUserSuccess action', () => {
//         const action = logOutUserSuccess();
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual(initialAuthState);
//     });

//     it('should handle getAuthDataSuccess action', () => {
//         const action = getAuthDataSuccess({ userAuth: mockUserAuth });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             userAuth: mockUserAuth,
//             error: null
//         });
//     });

//     it('should handle getAuthDataFailure action', () => {
//         const action = getAuthDataFailure();
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual(initialAuthState);
//     });

//     it('should handle updateUserData action', () => {
//         const action = updateUserData({ req: { email: 'test@example.com', oldPassword: 'oldPassword', password: 'newPassword' } });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             isUpdateSuccess: false,
//             error: null
//         });
//     });

//     it('should handle updateUserDataSuccess action', () => {
//         const action = updateUserDataSuccess({ req: { email: 'test@example.com', oldPassword: 'oldPassword', password: 'newPassword' } });
//         const state = authReducer(initialAuthState, action);
//         expect(state.isUpdateSuccess).toBeTrue();
//     });

//     it('should handle updateUserDataFailure action', () => {
//         const error = 'Update user failed';
//         const action = updateUserDataFailure({ error });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             isUpdateSuccess: false,
//             error
//         });
//     });

//     it('should handle deleteUserSuccess action', () => {
//         const action = deleteUserSuccess();
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual(initialAuthState);
//     });

//     it('should handle deleteUserFailure action', () => {
//         const error = 'Delete user failed';
//         const action = deleteUserFailure({ error });
//         const state = authReducer(initialAuthState, action);
//         expect(state).toEqual({
//             ...initialAuthState,
//             error
//         });
//     });
// });
