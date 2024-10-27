import { TestBed } from "@angular/core/testing";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { isObservable, of } from "rxjs";
import { getDefaultUserAuth, Policy, PolicyType, RedirectorService, RoleGuard, Roles, UserAuth } from "../..";
import { AuthenticationService } from "../../../authentication";

describe('RoleGuard', () => {
    let guard: RoleGuard;
    let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
    let redirectorSpy: jasmine.SpyObj<RedirectorService>;

    const mockActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
    const mockRouterStateSnapshot = {} as RouterStateSnapshot;

    beforeEach(() => {
        authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getUserAuth']);
        redirectorSpy = jasmine.createSpyObj('RedirectorService', ['redirectTo']);

        TestBed.configureTestingModule({
            providers: [
                RoleGuard,
                { provide: AuthenticationService, useValue: authServiceSpy },
                { provide: RedirectorService, useValue: redirectorSpy },
            ],
        });

        guard = TestBed.inject(RoleGuard);
    });

    function executeGuard(route: ActivatedRouteSnapshot, state: RouterStateSnapshot, callback: (result: boolean) => void) {
        const result = guard.canActivate(route, state);
        if (isObservable(result)) {
            result.subscribe(callback);
        } else if (result instanceof Promise) {
            result.then(callback);
        } else {
            callback(result);
        }
    }

    it('should allow access if user has the expected roles', () => {
        const expectedPolicies: PolicyType[] = [PolicyType.AdminPolicy];
        mockActivatedRouteSnapshot.data = { policy: expectedPolicies };
        const userAuth: UserAuth = { ...getDefaultUserAuth(), roles: [Roles.ADMINISTRATOR] };

        authServiceSpy.getUserAuth.and.returnValue(of(userAuth));
        spyOn(Policy, 'checkPolicy').and.returnValue(true);

        executeGuard(mockActivatedRouteSnapshot, mockRouterStateSnapshot, (result) => {
            expect(result).toBe(true);
            expect(redirectorSpy.redirectTo).not.toHaveBeenCalled();
        });

    });

    it('should deny access and redirect if user lacks the expected roles', () => {
        const expectedPolicies: PolicyType[] = [PolicyType.ManagerPolicy];
        mockActivatedRouteSnapshot.data = { policy: expectedPolicies };
        const userAuth: UserAuth = { ...getDefaultUserAuth(), roles: [Roles.CLIENT] };

        authServiceSpy.getUserAuth.and.returnValue(of(userAuth));
        spyOn(Policy, 'checkPolicy').and.returnValue(false);

        executeGuard(mockActivatedRouteSnapshot, mockRouterStateSnapshot, (result) => {
            expect(result).toBe(false);
            expect(redirectorSpy.redirectTo).toHaveBeenCalledWith('');
        });
    });

    it('should call checkPolicy for each expected policy', () => {
        const expectedPolicies: PolicyType[] = [PolicyType.AdminPolicy, PolicyType.ManagerPolicy];
        mockActivatedRouteSnapshot.data = { policy: expectedPolicies };
        const userAuth: UserAuth = { ...getDefaultUserAuth(), roles: [Roles.ADMINISTRATOR] };

        authServiceSpy.getUserAuth.and.returnValue(of(userAuth));
        const checkPolicySpy = spyOn(Policy, 'checkPolicy').and.returnValue(true);

        executeGuard(mockActivatedRouteSnapshot, mockRouterStateSnapshot, () => {
            expect(checkPolicySpy).toHaveBeenCalledTimes(expectedPolicies.length);
        });
    });
});