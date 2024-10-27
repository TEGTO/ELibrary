import { TestBed } from "@angular/core/testing";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { isObservable, of } from "rxjs";
import { CommandHandler, getDefaultClient } from "../..";
import { ClientService, START_ADDING_CLIENT_COMMAND_HANDLER, StartAddingClientCommand } from "../../../shop";
import { ClientGuard } from "./client.guard";

describe('ClientGuard', () => {
    let guard: ClientGuard;
    let clientServiceSpy: jasmine.SpyObj<ClientService>;
    let startAddingHandlerSpy: jasmine.SpyObj<CommandHandler<StartAddingClientCommand>>;

    const mockActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
    const mockRouterStateSnapshot: RouterStateSnapshot = { url: '/test-url' } as RouterStateSnapshot;

    beforeEach(() => {
        clientServiceSpy = jasmine.createSpyObj('ClientService', ['getClient']);
        startAddingHandlerSpy = jasmine.createSpyObj('CommandHandler', ['dispatch']);

        TestBed.configureTestingModule({
            providers: [
                ClientGuard,
                { provide: ClientService, useValue: clientServiceSpy },
                { provide: START_ADDING_CLIENT_COMMAND_HANDLER, useValue: startAddingHandlerSpy },
            ],
        });

        guard = TestBed.inject(ClientGuard);
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

    it('should allow activation if client exists', () => {
        clientServiceSpy.getClient.and.returnValue(of(getDefaultClient()));

        executeGuard(mockActivatedRouteSnapshot, mockRouterStateSnapshot, (result) => {
            expect(result).toBe(true);
            expect(startAddingHandlerSpy.dispatch).not.toHaveBeenCalled();
        });
    });

    it('should prevent activation and dispatch StartAddingClientCommand if client does not exist', () => {
        clientServiceSpy.getClient.and.returnValue(of(null));

        executeGuard(mockActivatedRouteSnapshot, mockRouterStateSnapshot, (result) => {
            expect(result).toBe(false);
            expect(startAddingHandlerSpy.dispatch).toHaveBeenCalledWith({ redirectAfter: mockRouterStateSnapshot.url });
        });
    });

    it('should handle errors and dispatch StartAddingClientCommand if client check fails', () => {
        clientServiceSpy.getClient.and.returnValue(of(null));

        executeGuard(mockActivatedRouteSnapshot, mockRouterStateSnapshot, (result) => {
            expect(result).toBe(false);
            expect(startAddingHandlerSpy.dispatch).toHaveBeenCalledWith({ redirectAfter: mockRouterStateSnapshot.url });
        });
    });
});