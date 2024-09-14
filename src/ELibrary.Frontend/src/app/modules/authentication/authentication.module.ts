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
import { AuthEffects, AuthInterceptor, AuthenticatedComponent, AuthenticationCommand, AuthenticationCommandService, AuthenticationControllerService, AuthenticationDialogManager, AuthenticationDialogManagerService, AuthenticationService, AuthenticationValidationMessage, AuthenticationValidationMessageService, LoginComponent, RegisterComponent, UnauthenticatedComponent, authReducer, registrationReducer, userDataReducer } from '.';

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
    StoreModule.forFeature('registration', registrationReducer),
    StoreModule.forFeature('userdata', userDataReducer),
    EffectsModule.forFeature([AuthEffects]),
  ],
  providers: [
    { provide: AuthenticationCommand, useClass: AuthenticationCommandService },
    { provide: AuthenticationValidationMessage, useClass: AuthenticationValidationMessageService },
    { provide: AuthenticationDialogManager, useClass: AuthenticationDialogManagerService },
    { provide: AuthenticationService, useClass: AuthenticationControllerService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  exports: [LoginComponent, UnauthenticatedComponent],
})
export class AuthenticationModule { }
