import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CommandHandler, noSpaces, notEmptyString, ValidationMessage } from '../../../shared';
import { confirmPasswordValidator, passwordValidator, SIGN_UP_COMMAND_HANDLER, SignUpCommand } from '../../index';

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

  constructor(
    @Inject(SIGN_UP_COMMAND_HANDLER) private readonly signUpHandler: CommandHandler<SignUpCommand>,
    private readonly validateInput: ValidationMessage,
    private readonly dialogRef: MatDialogRef<RegisterComponent>,
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        email: new FormControl('', [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, notEmptyString, noSpaces, passwordValidator, Validators.maxLength(256)]),
        passwordConfirm: new FormControl('', [Validators.required, notEmptyString, noSpaces, confirmPasswordValidator, Validators.maxLength(256)])
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
      const command: SignUpCommand =
      {
        email: formValues.email,
        password: formValues.password,
        confirmPassword: formValues.passwordConfirm,
        matDialogRef: this.dialogRef
      }
      this.signUpHandler.dispatch(command);
    }
  }
}