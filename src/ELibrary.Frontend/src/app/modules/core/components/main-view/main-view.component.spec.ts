import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/compiler';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { AuthenticationDialogManager, AuthenticationService } from '../../../authentication';
import { UserAuth } from '../../../shared';
import { ClientService } from '../../../shop';
import { MainViewComponent } from './main-view.component';

describe('MainViewComponent', () => {
    let component: MainViewComponent;
    let fixture: ComponentFixture<MainViewComponent>;
    let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
    let clientServiceSpy: jasmine.SpyObj<ClientService>;
    let authDialogManagerSpy: jasmine.SpyObj<AuthenticationDialogManager>;

    const mockUserAuth: UserAuth = {
        isAuthenticated: true,
        authToken: {
            accessToken: 'mockAccessToken',
            refreshToken: 'mockRefreshToken',
            refreshTokenExpiryDate: new Date(),
        },
        email: 'test@example.com',
        roles: ['CLIENT', 'MANAGER']
    };

    beforeEach(async () => {
        const authServiceSpyObj = jasmine.createSpyObj('AuthenticationService', ['getUserAuth']);
        const clientServiceSpyObj = jasmine.createSpyObj('ClientService', ['getClient']);
        const authDialogManagerSpyObj = jasmine.createSpyObj('AuthenticationDialogManager', ['openLoginMenu']);

        await TestBed.configureTestingModule({
            declarations: [MainViewComponent],
            providers: [
                { provide: AuthenticationService, useValue: authServiceSpyObj },
                { provide: ClientService, useValue: clientServiceSpyObj },
                { provide: AuthenticationDialogManager, useValue: authDialogManagerSpyObj }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA] // To avoid errors with custom elements like app-shopping-cart-button
        }).compileComponents();

        authServiceSpy = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
        clientServiceSpy = TestBed.inject(ClientService) as jasmine.SpyObj<ClientService>;
        authDialogManagerSpy = TestBed.inject(AuthenticationDialogManager) as jasmine.SpyObj<AuthenticationDialogManager>;

        fixture = TestBed.createComponent(MainViewComponent);
        component = fixture.componentInstance;
    });

    beforeEach(() => {
        authServiceSpy.getUserAuth.and.returnValue(of(mockUserAuth));
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

    it('should call clientService.getClient() on ngOnInit', () => {
        expect(clientServiceSpy.getClient).toHaveBeenCalled();
    });

    it('should open the login menu when openLoginMenu is called', () => {
        component.openLoginMenu();
        expect(authDialogManagerSpy.openLoginMenu).toHaveBeenCalled();
    });

    it('should return true for ManagerPolicy if the user has the MANAGER role', () => {
        const result = component.checkManagerPolicy(mockUserAuth.roles);
        expect(result).toBeTrue();
    });

    it('should return false for ManagerPolicy if the user does not have the MANAGER role', () => {
        const result = component.checkManagerPolicy(['CLIENT']);
        expect(result).toBeFalse();
    });

    it('should return true for AdminPolicy if the user has the ADMINISTRATOR role', () => {
        const result = component.checkAdminPolicy(['ADMINISTRATOR']);
        expect(result).toBeTrue();
    });

    it('should return false for AdminPolicy if the user does not have the ADMINISTRATOR role', () => {
        const result = component.checkAdminPolicy(['MANAGER']);
        expect(result).toBeFalse();
    });

    it('should render elements correctly when user is authenticated', () => {
        const compiled = fixture.nativeElement as HTMLElement;
        const cartButton = compiled.querySelector('app-shopping-cart-button');
        expect(cartButton).toBeTruthy();

        const receiptButton = fixture.debugElement.query(By.css('span[mat-icon-button]')).nativeElement;
        expect(receiptButton.textContent).toContain('receipt_long');
    });

    it('should not render admin button if user is not an admin', () => {
        const compiled = fixture.nativeElement as HTMLElement;
        const adminButton = compiled.querySelector('span[mat-icon-button="admin_panel_settings"]');
        expect(adminButton).toBeFalsy();
    });

    it('should render admin button if user is an admin', () => {
        component.userAuth$ = of({ ...mockUserAuth, roles: ['ADMINISTRATOR'] });
        fixture.detectChanges();

        const adminButton = fixture.debugElement.query(By.css('span[mat-icon-button]')).nativeElement;
        expect(adminButton.textContent).toContain('admin_panel_settings');
    });
});
