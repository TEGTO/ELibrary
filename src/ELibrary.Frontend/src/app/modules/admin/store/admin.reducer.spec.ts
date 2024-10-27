/* eslint-disable @typescript-eslint/no-explicit-any */
import { adminReducer, AdminState, createClientFailure, createClientSuccess, deleteUserFailure, deleteUserSuccess, getClientFailure, getClientSuccess, getPaginatedUserAmountFailure, getPaginatedUserAmountSuccess, getPaginatedUsersFailure, getPaginatedUsersSuccess, getUserFailure, getUserSuccess, registerUserFailure, registerUserSuccess, updateClientFailure, updateClientSuccess, updateUserFailure, updateUserSuccess } from "..";
import { AdminUser, AuthenticationMethod, Client } from "../../shared";

const mockUser: AdminUser = {
    id: '1',
    userName: 'johndoe',
    email: 'john.doe@example.com',
    registredAt: new Date(),
    updatedAt: new Date(),
    roles: ['admin'],
    authenticationMethods: [AuthenticationMethod.BaseAuthentication]
};

const mockClient: Client = {
    id: 'client1',
    userId: '1',
    name: 'John',
    middleName: 'M',
    lastName: 'Doe',
    dateOfBirth: new Date('1990-01-01'),
    address: '123 Main St',
    phone: '1234567890',
    email: 'john.doe@example.com'
};
const initialAdminState: AdminState = {
    users: [],
    userTotalAmount: 0,
    clients: [],
    error: null
};

describe('adminReducer', () => {
    it('should return the default state', () => {
        const action = { type: 'Unknown' } as any;
        const state = adminReducer(undefined, action);
        expect(state).toEqual(initialAdminState);
    });

    describe('getUserSuccess', () => {
        it('should set users and clear error', () => {
            const action = getUserSuccess({ user: mockUser });
            const result = adminReducer(initialAdminState, action);
            expect(result.users).toEqual([mockUser]);
            expect(result.error).toBeNull();
        });
    });

    describe('getUserFailure', () => {
        it('should set error', () => {
            const error = 'Failed to get user';
            const action = getUserFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });

    describe('registerUserSuccess', () => {
        it('should add a user, increase userTotalAmount, and clear error', () => {
            const action = registerUserSuccess({ user: mockUser });
            const result = adminReducer(initialAdminState, action);
            expect(result.users).toEqual([mockUser]);
            expect(result.userTotalAmount).toEqual(1);
            expect(result.error).toBeNull();
        });
    });

    describe('registerUserFailure', () => {
        it('should set error', () => {
            const error = 'Registration failed';
            const action = registerUserFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });

    describe('getPaginatedUsersSuccess', () => {
        it('should set users and clear error', () => {
            const users = [mockUser];
            const action = getPaginatedUsersSuccess({ users });
            const result = adminReducer(initialAdminState, action);
            expect(result.users).toEqual(users);
            expect(result.error).toBeNull();
        });
    });

    describe('getPaginatedUsersFailure', () => {
        it('should reset state and set error', () => {
            const error = 'Failed to get paginated users';
            const action = getPaginatedUsersFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result).toEqual({
                ...initialAdminState,
                error
            });
        });
    });

    describe('getPaginatedUserAmountSuccess', () => {
        it('should set userTotalAmount and clear error', () => {
            const amount = 5;
            const action = getPaginatedUserAmountSuccess({ amount });
            const result = adminReducer(initialAdminState, action);
            expect(result.userTotalAmount).toEqual(amount);
            expect(result.error).toBeNull();
        });
    });

    describe('getPaginatedUserAmountFailure', () => {
        it('should reset state and set error', () => {
            const error = 'Failed to get user amount';
            const action = getPaginatedUserAmountFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result).toEqual({
                ...initialAdminState,
                error
            });
        });
    });

    describe('updateUserSuccess', () => {
        it('should update existing user and clear error', () => {
            const updatedUser = { ...mockUser, userName: 'johnupdated' };
            const currentState = { ...initialAdminState, users: [mockUser] };
            const action = updateUserSuccess({ user: updatedUser });
            const result = adminReducer(currentState, action);
            expect(result.users).toEqual([updatedUser]);
            expect(result.error).toBeNull();
        });
    });

    describe('updateUserFailure', () => {
        it('should set error', () => {
            const error = 'Failed to update user';
            const action = updateUserFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });

    describe('deleteUserSuccess', () => {
        it('should remove user and decrease userTotalAmount', () => {
            const currentState = { ...initialAdminState, users: [mockUser], userTotalAmount: 1 };
            const action = deleteUserSuccess({ id: mockUser.id });
            const result = adminReducer(currentState, action);
            expect(result.users).toEqual([]);
            expect(result.userTotalAmount).toEqual(0);
        });
    });

    describe('deleteUserFailure', () => {
        it('should set error', () => {
            const error = 'Failed to delete user';
            const action = deleteUserFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });

    describe('getClientSuccess', () => {
        it('should add client and clear error', () => {
            const action = getClientSuccess({ client: mockClient });
            const result = adminReducer(initialAdminState, action);
            expect(result.clients).toEqual([mockClient]);
            expect(result.error).toBeNull();
        });
    });

    describe('getClientFailure', () => {
        it('should set error', () => {
            const error = 'Failed to get client';
            const action = getClientFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });

    describe('createClientSuccess', () => {
        it('should add client and clear error', () => {
            const action = createClientSuccess({ client: mockClient });
            const result = adminReducer(initialAdminState, action);
            expect(result.clients).toEqual([mockClient]);
            expect(result.error).toBeNull();
        });
    });

    describe('createClientFailure', () => {
        it('should set error', () => {
            const error = 'Failed to create client';
            const action = createClientFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });

    describe('updateClientSuccess', () => {
        it('should update existing client and clear error', () => {
            const updatedClient = { ...mockClient, name: 'Jane Doe' };
            const currentState = { ...initialAdminState, clients: [mockClient] };
            const action = updateClientSuccess({ client: updatedClient });
            const result = adminReducer(currentState, action);
            expect(result.clients).toEqual([updatedClient]);
            expect(result.error).toBeNull();
        });
    });

    describe('updateClientFailure', () => {
        it('should set error', () => {
            const error = 'Failed to update client';
            const action = updateClientFailure({ error });
            const result = adminReducer(initialAdminState, action);
            expect(result.error).toEqual(error);
        });
    });
});