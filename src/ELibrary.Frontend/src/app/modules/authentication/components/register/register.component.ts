import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserRegistrationRequest, ValidationMessage } from '../../../shared';
import { AuthenticationCommand, AuthenticationCommandType, confirmPasswordValidator, passwordValidator } from '../../index';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnInit {
  formGroup!: FormGroup;
  hidePassword = true;

  get emailInput() { return this.formGroup.get('email')!; }
  get passwordInput() { return this.formGroup.get('password')!; }
  get passwordConfirmInput() { return this.formGroup.get('passwordConfirm')!; }

  get validateEmailInput() { return this.validateInput.getEmailValidationMessage(this.emailInput); }
  get validatePassword() { return this.validateInput.getPasswordValidationMessage(this.passwordInput); }
  get validateConfirmPassword() { return this.validateInput.getConfirmPasswordValidationMessage(this.passwordConfirmInput); }

  constructor(
    private readonly authCommand: AuthenticationCommand,
    private readonly dialogRef: MatDialogRef<RegisterComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, passwordValidator, Validators.maxLength(256)]),
        passwordConfirm: new FormControl('', [Validators.required, confirmPasswordValidator, Validators.maxLength(256)])
      });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  hidePasswordOnKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.hidePassword = !this.hidePassword;
    }
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