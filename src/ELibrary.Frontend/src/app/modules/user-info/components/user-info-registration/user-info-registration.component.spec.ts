import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserInfoRegistrationComponent } from './user-info-registration.component';

describe('UserInfoRegistrationComponent', () => {
  let component: UserInfoRegistrationComponent;
  let fixture: ComponentFixture<UserInfoRegistrationComponent>;
  let parentFormGroup: FormGroup;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserInfoRegistrationComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        BrowserAnimationsModule,
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserInfoRegistrationComponent);
    component = fixture.componentInstance;

    parentFormGroup = new FormGroup({});
    component.parentFormGroup = parentFormGroup;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should add controls to the parent form group on initialization', () => {
    expect(component.firstNameInput).toBeTruthy();
    expect(component.lastNameInput).toBeTruthy();
    expect(component.dateOfBirthInput).toBeTruthy();
    expect(component.addressInput).toBeTruthy();
  });

  it('should validate required fields', () => {
    component.firstNameInput.setValue('');
    component.dateOfBirthInput.setValue(null);
    fixture.detectChanges();

    expect(component.firstNameInput.hasError('required')).toBeTrue();
    expect(component.dateOfBirthInput.hasError('required')).toBeTrue();
  });

  it('should validate max length for first name, last name, and address', () => {
    const longString = 'a'.repeat(257);

    component.firstNameInput.setValue(longString);
    component.lastNameInput.setValue(longString);
    component.addressInput.setValue(longString);
    fixture.detectChanges();

    expect(component.firstNameInput.hasError('maxlength')).toBeTrue();
    expect(component.lastNameInput.hasError('maxlength')).toBeTrue();
    expect(component.addressInput.hasError('maxlength')).toBeTrue();
  });

  it('should validate date of birth using minDateValidator', () => {
    const futureDate = new Date();
    futureDate.setDate(futureDate.getDate() + 1);

    component.dateOfBirthInput.setValue(futureDate);
    fixture.detectChanges();

    expect(component.dateOfBirthInput.hasError('minDate')).toBeFalse();
  });

  it('should have validation errors', () => {
    component.firstNameInput.setValue('');
    component.dateOfBirthInput.setValue(null);
    fixture.detectChanges();

    component.dateOfBirthInput.setValue(new Date(Date.now() + 86400000));
    fixture.detectChanges();

    expect(component.firstNameInput.valid).toBeFalse();
    expect(component.firstNameInput.hasError('required')).toBeTruthy();
  });
});