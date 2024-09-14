import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { AuthenticationCommand, AuthenticationCommandType, AuthenticationService, AuthenticationValidationMessage, passwordValidator } from '../..';
import { UserUpdateRequest } from '../../../shared';

@Component({
  selector: 'app-authenticated',
  templateUrl: './authenticated.component.html',
  styleUrl: './authenticated.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthenticatedComponent implements OnInit, OnDestroy {
  hideNewPassword: boolean = true;
  formGroup: FormGroup = null!;
  private destroy$ = new Subject<void>();

  get emailInput() { return this.formGroup.get('email')!; }
  get oldPasswordInput() { return this.formGroup.get('oldPassword')!; }
  get passwordInput() { return this.formGroup.get('password')!; }

  get validateEmailInput() { return this.validateInput.getEmailValidationMessage(this.emailInput); }
  get validateOldPassword() { return this.validateInput.getPasswordValidationMessage(this.oldPasswordInput); }
  get validatePassword() { return this.validateInput.getPasswordValidationMessage(this.passwordInput); }

  constructor(
    private readonly authService: AuthenticationService,
    private readonly authCommand: AuthenticationCommand,
    private readonly dialogRef: MatDialogRef<AuthenticatedComponent>,
    private readonly validateInput: AuthenticationValidationMessage
  ) { }

  ngOnInit(): void {
    this.authService.getUserData()
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        this.formGroup = new FormGroup(
          {
            email: new FormControl(data.email, [Validators.email, Validators.required, Validators.maxLength(256)]),
            oldPassword: new FormControl('', [Validators.required, Validators.maxLength(256)]),
            password: new FormControl('', [Validators.minLength(8), passwordValidator, Validators.maxLength(256)])
          });
      })
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  updateUser() {
    if (this.formGroup.valid) {
      const req: UserUpdateRequest = {
        email: this.formGroup.value.email,
        oldPassword: this.formGroup.value.oldPassword,
        password: this.formGroup.value.password,
      };
      this.authCommand.dispatchCommand(AuthenticationCommandType.Update, this, req, this.dialogRef);
    }
  }
  logOutUser() {
    this.authCommand.dispatchCommand(AuthenticationCommandType.LogOut, this);
  }
}