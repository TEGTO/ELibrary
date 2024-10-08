import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ADMIN_CREATE_CLIENT_COMMAND_HANDLER, ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, AdminCreateClientCommand, AdminService, AdminUpdateClientCommand } from '../..';
import { Client, CommandHandler, dateInPastValidator, getDefaultGetOrdersFilter, GetOrdersFilter, noSpaces, notEmptyString, RouteReader, ValidationMessage } from '../../../shared';

@Component({
  selector: 'app-admin-client-details',
  templateUrl: './admin-client-details.component.html',
  styleUrl: './admin-client-details.component.scss'
})
export class AdminClientDetailsComponent implements OnInit {
  private formGroup!: FormGroup;
  private userId!: string;

  client$!: Observable<Client | undefined>;

  get nameInput() { return this.formGroup.get('name')! as FormControl; }
  get middleNameInput() { return this.formGroup.get('middleName')! as FormControl; }
  get lastNameInput() { return this.formGroup.get('lastName')! as FormControl; }
  get dateOfBirthInput() { return this.formGroup.get('dateOfBirth')! as FormControl; }
  get addressInput() { return this.formGroup.get('address')! as FormControl; }
  get phoneInput() { return this.formGroup.get('phone')! as FormControl; }
  get emailInput() { return this.formGroup.get('email')! as FormControl; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly adminService: AdminService,
    private readonly routeReader: RouteReader,
    private readonly route: ActivatedRoute,
    @Inject(ADMIN_CREATE_CLIENT_COMMAND_HANDLER) private readonly createClientHandler: CommandHandler<AdminCreateClientCommand>,
    @Inject(ADMIN_UPDATE_CLIENT_COMMAND_HANDLER) private readonly updateClientHandler: CommandHandler<AdminUpdateClientCommand>
  ) { }

  ngOnInit(): void {
    this.client$ = this.routeReader.readId(
      this.route,
      (id: string) => {
        this.userId = id;
        return this.adminService.getClientByUserId(id);
      },
    );
  }

  getFormGroup(client: Client): FormGroup {
    if (!this.formGroup) {
      this.formGroup = new FormGroup(
        {
          name: new FormControl(client.name, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
          middleName: new FormControl(client.middleName, [Validators.maxLength(256)]),
          lastName: new FormControl(client.lastName, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
          dateOfBirth: new FormControl(client.dateOfBirth, [Validators.required, dateInPastValidator()]),
          address: new FormControl(client.address, [Validators.maxLength(512)]),
          phone: new FormControl(client.phone, [Validators.maxLength(256)]),
          email: new FormControl(client.email, [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
        });
    }
    return this.formGroup;
  }
  updateClient(client: Client) {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const command: AdminUpdateClientCommand = {
        userId: client.userId,
        name: formValues.name,
        middleName: formValues.middleName,
        lastName: formValues.lastName,
        dateOfBirth: formValues.dateOfBirth,
        address: formValues.address,
        phone: formValues.phone,
        email: formValues.email,
      }
      this.updateClientHandler.dispatch(command);
    }
  }
  addClient() {
    console.log(this.userId);
    const command: AdminCreateClientCommand =
    {
      userId: this.userId
    };
    this.createClientHandler.dispatch(command);
  }
  getClientOrderFilter(client: Client): GetOrdersFilter {
    return {
      ...getDefaultGetOrdersFilter(),
      clientId: client.id
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}