import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AbstractControl, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { ADMIN_CREATE_CLIENT_COMMAND_HANDLER, ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, AdminCreateClientCommand, AdminService, AdminUpdateClientCommand } from '../..';
import { Client, CommandHandler, getDefaultClient, RouteReader, ValidationMessage } from '../../../shared';
import { AdminClientDetailsComponent } from './admin-client-details.component';

describe('AdminClientDetailsComponent', () => {
  let component: AdminClientDetailsComponent;
  let fixture: ComponentFixture<AdminClientDetailsComponent>;
  let mockAdminService: jasmine.SpyObj<AdminService>;
  let mockCreateClientHandler: jasmine.SpyObj<CommandHandler<AdminCreateClientCommand>>;
  let mockUpdateClientHandler: jasmine.SpyObj<CommandHandler<AdminUpdateClientCommand>>;
  let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;
  let mockRouteReader: jasmine.SpyObj<RouteReader>;

  let client$: BehaviorSubject<Client | undefined>;

  const mockClient: Client = getDefaultClient();

  beforeEach(async () => {
    mockAdminService = jasmine.createSpyObj('AdminService', ['getClientByUserId']);
    mockCreateClientHandler = jasmine.createSpyObj<CommandHandler<AdminCreateClientCommand>>(['dispatch']);
    mockUpdateClientHandler = jasmine.createSpyObj<CommandHandler<AdminUpdateClientCommand>>(['dispatch']);
    mockValidationMessage = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);
    mockRouteReader = jasmine.createSpyObj<RouteReader>(['readId']);

    mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: "" });

    client$ = new BehaviorSubject<Client | undefined>(mockClient);

    mockRouteReader.readId.and.returnValue(client$.asObservable());

    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatInputModule,
        BrowserAnimationsModule
      ],
      declarations: [AdminClientDetailsComponent],
      providers: [
        { provide: AdminService, useValue: mockAdminService },
        { provide: ValidationMessage, useValue: mockValidationMessage },
        { provide: ActivatedRoute, useValue: { paramMap: of({ get: () => '123' }) } },
        { provide: RouteReader, useValue: mockRouteReader },
        { provide: ADMIN_CREATE_CLIENT_COMMAND_HANDLER, useValue: mockCreateClientHandler },
        { provide: ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, useValue: mockUpdateClientHandler },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminClientDetailsComponent);
    component = fixture.componentInstance;
    mockAdminService.getClientByUserId.and.returnValue(of(mockClient));
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should bind client data from client$ observable', () => {
    component.ngOnInit();
    fixture.detectChanges();

    expect(mockRouteReader.readId).toHaveBeenCalled();
    component.client$.subscribe((client) => {
      expect(client).toEqual(mockClient);
    });
  });

  it('should initialize form with client data', () => {
    component.ngOnInit();
    const formGroup = component.getFormGroup(mockClient);
    expect(formGroup.get('name')?.value).toBe(mockClient.name);
    expect(formGroup.get('email')?.value).toBe(mockClient.email);
  });

  it('should dispatch updateClient command when form is valid and updateClient is called', () => {
    component.ngOnInit();
    const formGroup = component.getFormGroup(mockClient);

    formGroup.patchValue({
      name: 'John',
      email: 'john.doe@example.com',
      id: "sfsfs",
      userId: "sfsfs",
      middleName: "sfsfs",
      lastName: "3232",
      dateOfBirth: new Date(0),
      address: "2222@gmail",
      phone: "222",
    });

    component.updateClient(mockClient);

    expect(mockUpdateClientHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch addClient command when addClient is called', () => {
    component.ngOnInit();
    component['userId'] = '123';
    component.addClient();

    expect(mockCreateClientHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      userId: '123',
    }));
  });

  it('should call validateInputField and use getValidationMessage', () => {
    const mockInputControl = new FormControl('');
    const mockError = { hasError: true, message: 'Error message' };
    mockValidationMessage.getValidationMessage.and.returnValue(mockError);

    const result = component.validateInputField(mockInputControl as AbstractControl);

    expect(mockValidationMessage.getValidationMessage).toHaveBeenCalledWith(mockInputControl);
    expect(result).toEqual(mockError);
  });

  it('should render client_undefined template if no client data is available', () => {
    client$.next(undefined);
    fixture.detectChanges();

    const boldText = fixture.debugElement.query(By.css('p[class="text-lg text-gray-600"]')).nativeElement;
    expect(boldText.textContent).toContain('Provide the client for an user.');
  });
});