import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AuthEffects, AuthInterceptor, AuthenticatedComponent, AuthenticationControllerService, AuthenticationDialogManager, AuthenticationDialogManagerService, AuthenticationService, LOG_OUT_COMMAND_HANDLER, LogOutCommandHandlerService, LoginComponent, RegisterComponent, SIGN_IN_COMMAND_HANDLER, SIGN_UP_COMMAND_HANDLER, SignInCommandHandlerService, SignUpCommandHandlerService, UPDATE_USER_COMMAND_HANDLER, UnauthenticatedComponent, UpdateUserCommandHandlerService, authReducer } from '.';

@NgModule({
  declarations: [LoginComponent, RegisterComponent, AuthenticatedComponent, UnauthenticatedComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatInputModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    HttpClientModule,
    StoreModule.forFeature('authentication', authReducer),
    EffectsModule.forFeature([AuthEffects]),
  ],
  providers: [
    { provide: AuthenticationDialogManager, useClass: AuthenticationDialogManagerService },
    { provide: AuthenticationService, useClass: AuthenticationControllerService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: SIGN_UP_COMMAND_HANDLER, useClass: SignUpCommandHandlerService },
    { provide: SIGN_IN_COMMAND_HANDLER, useClass: SignInCommandHandlerService },
    { provide: LOG_OUT_COMMAND_HANDLER, useClass: LogOutCommandHandlerService },
    { provide: UPDATE_USER_COMMAND_HANDLER, useClass: UpdateUserCommandHandlerService },
  ],
  exports: [LoginComponent, UnauthenticatedComponent],
})
export class AuthenticationModule { }