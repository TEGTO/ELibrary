import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { AuthenticationService, START_LOGIN_COMMAND_HANDLER, StartLoginCommand } from '../../../authentication';
import { CommandHandler, getAdminUerTable, getManagerBooksPath, Roles, UserAuth } from '../../../shared';
import { ClientService } from '../../../shop';
import { MainViewComponent } from './main-view.component';

describe('MainViewComponent', () => {
    let component: MainViewComponent;
    let fixture: ComponentFixture<MainViewComponent>;
    let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
    let clientServiceSpy: jasmine.SpyObj<ClientService>;
    let startLoginCommandHandlerSpy: jasmine.SpyObj<CommandHandler<StartLoginCommand>>;

    const routes: Routes = [];

    const mockUserAuth: UserAuth = {
        isAuthenticated: true,
        authToken: {
            accessToken: 'mockAccessToken',
            refreshToken: 'mockRefreshToken',
            refreshTokenExpiryDate: new Date(),
        },
        email: 'test@example.com',
        roles: [Roles.CLIENT, Roles.MANAGER, Roles.ADMINISTRATOR]
    };
    let behaviourUserAuth: BehaviorSubject<UserAuth>;

    beforeEach(async () => {
        const authServiceSpyObj = jasmine.createSpyObj('AuthenticationService', ['getUserAuth']);
        const clientServiceSpyObj = jasmine.createSpyObj('ClientService', ['getClient']);
        const startLoginCommandHandlerSpyObj = jasmine.createSpyObj<CommandHandler<StartLoginCommand>>(['dispatch']);
        behaviourUserAuth = new BehaviorSubject<UserAuth>(mockUserAuth)

        await TestBed.configureTestingModule({
            declarations: [MainViewComponent],
            providers: [
                { provide: AuthenticationService, useValue: authServiceSpyObj },
                { provide: ClientService, useValue: clientServiceSpyObj },
                { provide: START_LOGIN_COMMAND_HANDLER, useValue: startLoginCommandHandlerSpyObj },
            ],
            imports: [
                RouterModule.forRoot(routes),
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        authServiceSpy = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
        clientServiceSpy = TestBed.inject(ClientService) as jasmine.SpyObj<ClientService>;
        startLoginCommandHandlerSpy = TestBed.inject(START_LOGIN_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<StartLoginCommand>>;

        fixture = TestBed.createComponent(MainViewComponent);
        component = fixture.componentInstance;
    });

    beforeEach(() => {
        authServiceSpy.getUserAuth.and.returnValue(behaviourUserAuth.asObservable());
        clientServiceSpy.getClient.and.returnValue(of(null));
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize userAuth$ observable on ngOnInit', () => {
        expect(component.userAuth$).toBeDefined();
        component.userAuth$.subscribe((userAuth) => {
            expect(userAuth).toEqual(mockUserAuth);
        });
    });

    it('should call clientService.getClient() on ngOnInit if user is authenticated', () => {
        expect(clientServiceSpy.getClient).toHaveBeenCalled();
    });

    it('should not call clientService.getClient() if user is not authenticated', () => {
        clientServiceSpy.getClient.calls.reset();
        behaviourUserAuth.next({ ...mockUserAuth, roles: [Roles.CLIENT], isAuthenticated: false })
        fixture.detectChanges();
        expect(clientServiceSpy.getClient).not.toHaveBeenCalled();
    });

    it('should open login menu when openLoginMenu is called', () => {
        component.openLoginMenu();
        expect(startLoginCommandHandlerSpy.dispatch).toHaveBeenCalled();
    });

    it('should return true for ManagerPolicy if the user has the MANAGER role', () => {
        const result = component.checkManagerPolicy(mockUserAuth.roles);
        expect(result).toBeTrue();
    });

    it('should return false for ManagerPolicy if the user does not have the MANAGER role', () => {
        const result = component.checkManagerPolicy([Roles.CLIENT]);
        expect(result).toBeFalse();
    });

    it('should return true for AdminPolicy if the user has the ADMINISTRATOR role', () => {
        const result = component.checkAdminPolicy([Roles.ADMINISTRATOR]);
        expect(result).toBeTrue();
    });

    it('should return false for AdminPolicy if the user does not have the ADMINISTRATOR role', () => {
        const result = component.checkAdminPolicy([Roles.MANAGER]);
        expect(result).toBeFalse();
    });

    it('should render elements correctly when user is authenticated', fakeAsync(() => {
        const compiled = fixture.nativeElement as HTMLElement;
        const cartButton = compiled.querySelector('app-shopping-cart-button');
        expect(cartButton).toBeTruthy();

        const receiptButton = fixture.debugElement.query(By.css('a[mat-icon-button]')).nativeElement;
        expect(receiptButton.textContent).toContain('receipt_long');
    }));

    it('should not render admin button if user is not an admin', () => {
        behaviourUserAuth.next({ ...mockUserAuth, roles: [Roles.CLIENT] })
        fixture.detectChanges();
        const adminButton = fixture.debugElement.query(By.css(`a[href="/${getAdminUerTable()}"]`));
        expect(adminButton).toBeFalsy();
    });

    it('should render admin button if user is an admin', () => {
        component.userAuth$ = of({ ...mockUserAuth, roles: [Roles.ADMINISTRATOR] });
        fixture.detectChanges();
        const adminButton = fixture.debugElement.query(By.css(`a[href="/${getAdminUerTable()}"]`)).nativeElement;
        expect(adminButton).toBeTruthy();
    });

    it('should display the shopping cart button if user has the CLIENT role', () => {
        const compiled = fixture.nativeElement as HTMLElement;
        const shoppingCartButton = compiled.querySelector('app-shopping-cart-button');
        expect(shoppingCartButton).toBeTruthy();
    });

    it('should display the manager button if user has the MANAGER role', () => {
        const managerButton = fixture.debugElement.query(By.css(`a[href="/${getManagerBooksPath()}"]`)).nativeElement;
        expect(managerButton).toBeTruthy();
    });

    it('should not render manager button if user is not an manager', fakeAsync(() => {
        behaviourUserAuth.next({ ...mockUserAuth, roles: [Roles.CLIENT] })
        fixture.detectChanges();
        const managerButton = fixture.debugElement.query(By.css(`a[href="/${getManagerBooksPath()}"]`));
        expect(managerButton).toBeFalsy();
    }));
});