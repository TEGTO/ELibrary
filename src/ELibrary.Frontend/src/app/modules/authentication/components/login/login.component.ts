import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { UserAuthenticationRequest } from '../../../shared';
import { AuthenticationCommand, AuthenticationCommandType, AuthenticationDialogManager, AuthenticationValidationMessage } from '../../index';

@Component({
  selector: 'auth-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnInit, OnDestroy {
  formGroup!: FormGroup;
  hidePassword: boolean = true;
  private destroy$ = new Subject<void>();

  get loginInput() { return this.formGroup.get('login')!; }
  get passwordInput() { return this.formGroup.get('password')!; }

  get validateLoginInput() { return this.validateInput.getEmailValidationMessage(this.loginInput); }
  get validatePassword() { return this.validateInput.getPasswordValidationMessage(this.passwordInput); }

  constructor(
    private readonly authDialogManager: AuthenticationDialogManager,
    private readonly authCommand: AuthenticationCommand,
    private readonly dialogRef: MatDialogRef<LoginComponent>,
    private readonly validateInput: AuthenticationValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        login: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(256)]),
      });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  openRegisterMenu() {
    const dialogRef = this.authDialogManager.openRegisterMenu();
  }
  signInUser() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: UserAuthenticationRequest = {
        login: formValues.login,
        password: formValues.password,
      };
      this.authCommand.dispatchCommand(AuthenticationCommandType.SignIn, this, req, this.dialogRef);
    }
  }
}