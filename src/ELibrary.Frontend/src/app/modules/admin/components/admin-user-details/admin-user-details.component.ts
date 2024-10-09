import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ADMIN_DELETE_USER_COMMAND_HANDLER, ADMIN_UPDATE_USER_COMMAND_HANDLER, AdminDeleteUserCommand, AdminService, AdminUpdateUserCommand } from '../..';
import { changePasswordValidator, confirmPasswordValidator, confirmPasswordValidatorGroup } from '../../../authentication';
import { AdminUser, CommandHandler, getRoleArray, noSpaces, notEmptyString, RouteReader, ValidationMessage } from '../../../shared';

@Component({
  selector: 'app-admin-user-details',
  templateUrl: './admin-user-details.component.html',
  styleUrl: './admin-user-details.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminUserDetailsComponent implements OnInit {
  hidePassword = true;
  isFormSent = false;
  private formGroup!: FormGroup;

  user$!: Observable<AdminUser>;

  get emailInput() { return this.formGroup.get('email')! as FormControl; }
  get passwordInput() { return this.formGroup.get('password')! as FormControl; }
  get passwordConfirmInput() { return this.formGroup.get('passwordConfirm')! as FormControl; }
  get rolesInput() { return this.formGroup.get('roles')! as FormControl; }
  get roles() { return getRoleArray(); }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly adminService: AdminService,
    private readonly routeReader: RouteReader,
    private readonly route: ActivatedRoute,
    @Inject(ADMIN_UPDATE_USER_COMMAND_HANDLER) private readonly updateHandler: CommandHandler<AdminUpdateUserCommand>,
    @Inject(ADMIN_DELETE_USER_COMMAND_HANDLER) private readonly deleteHandler: CommandHandler<AdminDeleteUserCommand>,
  ) { }

  ngOnInit(): void {
    this.user$ = this.routeReader.readId(
      this.route,
      (id: string) => this.adminService.getUserById(id),
    );
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validate(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  getFormGroup(user: AdminUser): FormGroup {
    if (!this.formGroup) {
      this.formGroup = new FormGroup(
        {
          email: new FormControl(user.email, [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
          password: new FormControl('', [noSpaces, changePasswordValidator, Validators.maxLength(256)]),
          passwordConfirm: new FormControl('', [noSpaces, confirmPasswordValidator, Validators.maxLength(256)]),
          roles: new FormControl(user.roles, [Validators.required]),
        },
        { validators: confirmPasswordValidatorGroup }
      );
    }

    return this.formGroup;
  }
  hidePasswordOnKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.hidePassword = !this.hidePassword;
    }
  }
  updateUser(user: AdminUser) {
    this.isFormSent = true;
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const command: AdminUpdateUserCommand =
      {
        currentLogin: user.email,
        email: formValues.email,
        password: formValues.password,
        confirmPassword: formValues.passwordConfirm,
        roles: formValues.roles,
      }
      this.updateHandler.dispatch(command);
    }
  }
  deleteUser(user: AdminUser) {
    const command: AdminDeleteUserCommand = {
      userId: user.id
    };
    this.deleteHandler.dispatch(command);
  }
}
