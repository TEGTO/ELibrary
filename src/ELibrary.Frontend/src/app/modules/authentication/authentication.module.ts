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
import { AuthInterceptor, AuthenticatedComponent, AuthenticationControllerService, AuthenticationDialogManager, AuthenticationDialogManagerService, AuthenticationService, LoginComponent, RegisterComponent, RegistrationEffects, SignInEffects, authReducer, registrationReducer, userDataReducer } from '.';
import { UserInfoModule } from "../user-info/user-info.module";
import { UnauthenticatedComponent } from './components/unauthenticated/unauthenticated.component';

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
    UserInfoModule,
    HttpClientModule,
    StoreModule.forFeature('authentication', authReducer),
    StoreModule.forFeature('registration', registrationReducer),
    StoreModule.forFeature('userdata', userDataReducer),
    EffectsModule.forFeature([RegistrationEffects, SignInEffects]),
  ],
  providers: [
    { provide: AuthenticationDialogManager, useClass: AuthenticationDialogManagerService },
    { provide: AuthenticationService, useClass: AuthenticationControllerService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  exports: [LoginComponent, UnauthenticatedComponent],
})
export class AuthenticationModule { }
