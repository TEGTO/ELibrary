import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AuthEffects, AuthInterceptor, AuthenticatedComponent, AuthenticationControllerService, AuthenticationDialogManager, AuthenticationDialogManagerService, AuthenticationService, LOG_OUT_COMMAND_HANDLER, LogOutCommandHandlerService, LoginComponent, OAUTH_LOGIN_COMMAND_HANDLER, OAuthCallbackComponent, OAuthLoginCommandHandlerService, RegisterComponent, SIGN_IN_COMMAND_HANDLER, SIGN_UP_COMMAND_HANDLER, START_LOGIN_COMMAND_HANDLER, START_OAUTH_LOGIN_COMMAND_HANDLER, START_REGISTRATION_COMMAND_HANDLER, SignInCommandHandlerService, SignUpCommandHandlerService, StartLoginCommandHandlerService, StartOAuthLoginCommandHandlerService, StartRegistrationCommandHandlerService, UPDATE_USER_COMMAND_HANDLER, UpdateUserCommandHandlerService, authReducer } from '.';

@NgModule({
  declarations: [LoginComponent, RegisterComponent, AuthenticatedComponent, OAuthCallbackComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatInputModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    StoreModule.forFeature('authentication', authReducer),
    EffectsModule.forFeature([AuthEffects]),
  ],
  providers: [
    provideHttpClient(
      withInterceptorsFromDi(),
    ),
    { provide: AuthenticationDialogManager, useClass: AuthenticationDialogManagerService },
    { provide: AuthenticationService, useClass: AuthenticationControllerService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },

    { provide: START_REGISTRATION_COMMAND_HANDLER, useClass: StartRegistrationCommandHandlerService },
    { provide: START_LOGIN_COMMAND_HANDLER, useClass: StartLoginCommandHandlerService },
    { provide: SIGN_UP_COMMAND_HANDLER, useClass: SignUpCommandHandlerService },
    { provide: SIGN_IN_COMMAND_HANDLER, useClass: SignInCommandHandlerService },
    { provide: LOG_OUT_COMMAND_HANDLER, useClass: LogOutCommandHandlerService },
    { provide: UPDATE_USER_COMMAND_HANDLER, useClass: UpdateUserCommandHandlerService },
    { provide: START_OAUTH_LOGIN_COMMAND_HANDLER, useClass: StartOAuthLoginCommandHandlerService },
    { provide: OAUTH_LOGIN_COMMAND_HANDLER, useClass: OAuthLoginCommandHandlerService },
  ],
  exports: [LoginComponent, OAuthCallbackComponent],
})
export class AuthenticationModule { }
