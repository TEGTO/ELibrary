import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { AuthenticationService } from '../..';
import { getDefaultUserAuth } from '../../../shared';
import { AuthenticatedComponent } from './authenticated.component';

describe('AuthenticatedComponent', () => {
  let component: AuthenticatedComponent;
  let fixture: ComponentFixture<AuthenticatedComponent>;
  let authService: jasmine.SpyObj<AuthenticationService>;

  beforeEach(waitForAsync(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getUserData', 'logOutUser']);

    TestBed.configureTestingModule({
      declarations: [AuthenticatedComponent],
      providers: [
        { provide: AuthenticationService, useValue: authServiceSpy },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(AuthenticatedComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;

    authService.getUserAuth.and.returnValue(of(getDefaultUserAuth()));
  }));

  beforeEach(() => {
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load user data on initialization', () => {
    component.ngOnInit();
    fixture.detectChanges();

    expect(authService.getUserAuth).toHaveBeenCalled();
  });

  it('should call logOutUser on logout button click', () => {
    const logoutButton = fixture.debugElement.query(By.css('button#logout-button')).nativeElement;
    logoutButton.click();

    expect(authService.logOutUser).toHaveBeenCalled();
  });
});