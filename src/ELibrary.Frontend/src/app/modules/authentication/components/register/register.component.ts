import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserRegistrationRequest } from '../../../shared';
import { AuthenticationCommand, AuthenticationCommandType, AuthenticationValidationMessage, confirmPasswordValidator, passwordValidator } from '../../index';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnInit {
  formGroup!: FormGroup;
  hidePassword: boolean = true;

  get emailInput() { return this.formGroup.get('email')!; }
  get passwordInput() { return this.formGroup.get('password')!; }
  get passwordConfirmInput() { return this.formGroup.get('passwordConfirm')!; }

  get validateEmailInput() { return this.validateInput.getEmailValidationMessage(this.emailInput); }
  get validatePassword() { return this.validateInput.getPasswordValidationMessage(this.passwordInput); }
  get validateConfirmPassword() { return this.validateInput.getConfirmPasswordValidationMessage(this.passwordConfirmInput); }

  constructor(
    private readonly authCommand: AuthenticationCommand,
    private readonly dialogRef: MatDialogRef<RegisterComponent>,
    private readonly validateInput: AuthenticationValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, passwordValidator, Validators.maxLength(256)]),
        passwordConfirm: new FormControl('', [Validators.required, confirmPasswordValidator, Validators.maxLength(256)])
      });
  }

  registerUser() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: UserRegistrationRequest = {
        email: formValues.email,
        password: formValues.password,
        confirmPassword: formValues.passwordConfirm
      };
      this.authCommand.dispatchCommand(AuthenticationCommandType.SignUp, this, req, this.dialogRef);
    }
  }
}