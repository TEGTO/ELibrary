import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { CommandHandler, noSpaces, notEmptyString, ValidationMessage } from '../../../shared';
import { AuthenticationDialogManager, passwordValidator, SIGN_IN_COMMAND_HANDLER, SignInCommand } from '../../index';

@Component({
  selector: 'app-auth-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnInit, OnDestroy {
  formGroup!: FormGroup;
  hidePassword = true;
  private destroy$ = new Subject<void>();

  get loginInput() { return this.formGroup.get('login')!; }
  get passwordInput() { return this.formGroup.get('password')!; }

  constructor(
    private readonly authDialogManager: AuthenticationDialogManager,
    @Inject(SIGN_IN_COMMAND_HANDLER) private readonly signInHandler: CommandHandler<SignInCommand>,
    private readonly dialogRef: MatDialogRef<LoginComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        login: new FormControl('', [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, notEmptyString, noSpaces, passwordValidator, Validators.maxLength(256)]),
      });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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
  openRegisterMenuOnKeydown(event: KeyboardEvent) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.authDialogManager.openRegisterMenu();
    }
  }
  openRegisterMenu() {
    this.authDialogManager.openRegisterMenu();
  }
  signInUser() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const command: SignInCommand =
      {
        login: formValues.login,
        password: formValues.password,
        matDialogRef: this.dialogRef
      }
      this.signInHandler.dispatch(command);
    }
  }
}