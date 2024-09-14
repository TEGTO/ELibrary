import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/compiler';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { AuthenticationDialogManager, AuthenticationService, UnauthenticatedComponent } from '../../../authentication';
import { AuthData } from '../../../shared';
import { MainViewComponent } from './main-view.component';

describe('MainViewComponent', () => {
    let component: MainViewComponent;
    let fixture: ComponentFixture<MainViewComponent>;
    let authService: jasmine.SpyObj<AuthenticationService>;
    let authDialogManager: jasmine.SpyObj<AuthenticationDialogManager>;

    beforeEach(async () => {
        const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getAuthData']);
        const authDialogManagerSpy = jasmine.createSpyObj('AuthenticationDialogManager', ['openLoginMenu']);

        await TestBed.configureTestingModule({
            declarations: [MainViewComponent, UnauthenticatedComponent],
            imports: [
                RouterTestingModule,
            ],
            providers: [
                { provide: AuthenticationService, useValue: authServiceSpy },
                { provide: AuthenticationDialogManager, useValue: authDialogManagerSpy }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA,]
        }).compileComponents();

        authService = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
        authDialogManager = TestBed.inject(AuthenticationDialogManager) as jasmine.SpyObj<AuthenticationDialogManager>;
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(MainViewComponent);
        component = fixture.componentInstance;
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize isAuthenticated$ with authentication status', () => {
        const mockAuthData: AuthData = { isAuthenticated: true, accessToken: 'token', refreshToken: 'refreshToken', refreshTokenExpiryDate: new Date() };
        authService.getAuthData.and.returnValue(of(mockAuthData));

        component.ngOnInit();
        fixture.detectChanges();

        component.authData$.subscribe(isAuthenticated => {
            expect(isAuthenticated).toBeTrue();
        });

        expect(authService.getAuthData).toHaveBeenCalled();
    });

    it('should display the authenticated view when the user is authenticated', () => {
        const mockAuthData: AuthData = { isAuthenticated: true, accessToken: 'token', refreshToken: 'refreshToken', refreshTokenExpiryDate: new Date() };
        authService.getAuthData.and.returnValue(of(mockAuthData));

        component.ngOnInit();
        fixture.detectChanges();

        const headerElement = fixture.debugElement.query(By.css('header.root__header'));
        expect(headerElement).toBeTruthy();

        const unauthenticatedView = fixture.debugElement.query(By.css('auth-unauthenticated'));
        expect(unauthenticatedView).toBeNull();
    });

    it('should display the unauthenticated view when the user is not authenticated', () => {
        const mockAuthData: AuthData = { isAuthenticated: false, accessToken: '', refreshToken: '', refreshTokenExpiryDate: new Date() };
        authService.getAuthData.and.returnValue(of(mockAuthData));

        component.ngOnInit();
        fixture.detectChanges();

        const unauthenticatedView = fixture.debugElement.query(By.css('auth-unauthenticated'));
        expect(unauthenticatedView).toBeTruthy();

        const headerElement = fixture.debugElement.query(By.css('header.root__header'));
        expect(headerElement).toBeNull();
    });

    it('should open the login menu when openLoginMenu is called', () => {
        component.openLoginMenu();
        expect(authDialogManager.openLoginMenu).toHaveBeenCalled();
    });
});