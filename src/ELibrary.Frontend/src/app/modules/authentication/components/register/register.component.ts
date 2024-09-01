import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { catchError, of, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { SnackbarManager, UserRegistrationRequest } from '../../../shared';
import { AuthenticationService, confirmPasswordValidator } from '../../index';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnInit, OnDestroy {
  formGroup!: FormGroup;
  hidePassword: boolean = true;
  private destroy$ = new Subject<void>();

  get nameInput() { return this.formGroup.get('userName')!; }
  get passwordInput() { return this.formGroup.get('password')!; }
  get passwordConfirmInput() { return this.formGroup.get('passwordConfirm')!; }

  constructor(
    private readonly authService: AuthenticationService,
    private readonly dialogRef: MatDialogRef<RegisterComponent>,
    private readonly snackbarManager: SnackbarManager
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        userName: new FormControl('', [Validators.required, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(256)]),
        passwordConfirm: new FormControl('', [Validators.required, confirmPasswordValidator, Validators.maxLength(256)])
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  registerUser() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const userData: UserRegistrationRequest = {
        userName: formValues.userName,
        password: formValues.password,
        confirmPassword: formValues.passwordConfirm,
        userInfo: {
          name: formValues.firstName,
          lastName: formValues.lastName,
          dateOfBirth: formValues.dateOfBirth,
          address: formValues.address
        }
      };
      this.authService.registerUser(userData).pipe(
        takeUntil(this.destroy$),
        tap(isSuccess => {
          if (isSuccess) {
            this.snackbarManager.openInfoSnackbar('✔️ The registration is successful!', 5);
            this.dialogRef.close();
          }
        }),
        switchMap(isSuccess => isSuccess ? of(null) : this.authService.getRegistrationErrors()),
        tap(errors => {
          if (errors) {
            this.snackbarManager.openErrorSnackbar(errors.split('\n'));
          }
        }),
        catchError(err => {
          this.snackbarManager.openErrorSnackbar(['An error occurred during registration.']);
          return of(null);
        })
      ).subscribe();
    }
  }
}