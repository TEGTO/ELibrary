import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { AbstractControl, FormControl, ReactiveFormsModule } from '@angular/forms';
import { ValidationMessage } from '../../../shared';
import { AdminUserFilterComponent } from './admin-user-filter.component';

describe('AdminUserFilterComponent', () => {
  let component: AdminUserFilterComponent;
  let fixture: ComponentFixture<AdminUserFilterComponent>;
  let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;

  beforeEach(async () => {
    mockValidationMessage = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

    await TestBed.configureTestingModule({
      declarations: [AdminUserFilterComponent],
      imports: [ReactiveFormsModule],
      providers: [{ provide: ValidationMessage, useValue: mockValidationMessage }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form group with containsInfo control', () => {
    component.ngOnInit();
    expect(component.formGroup.contains('containsInfo')).toBe(true);
    expect(component.containsInfoInput).toBeDefined();
  });

  it('should emit filterChange when form value changes', fakeAsync(() => {
    spyOn(component.filterChange, 'emit');

    component.containsInfoInput.setValue('test info');
    fixture.detectChanges();

    tick(250);

    expect(component.filterChange.emit).toHaveBeenCalledWith(jasmine.objectContaining({ containsInfo: 'test info' }));
  }));

  it('should call validateInputField and use getValidationMessage', () => {
    const mockInputControl = new FormControl('');
    const mockError = { hasError: true, message: 'Error message' };
    mockValidationMessage.getValidationMessage.and.returnValue(mockError);

    const result = component.validateInputField(mockInputControl as AbstractControl);

    expect(mockValidationMessage.getValidationMessage).toHaveBeenCalledWith(mockInputControl);
    expect(result).toEqual(mockError);
  });

  it('should debounce the value change for 100ms', (done) => {
    spyOn(component.filterChange, 'emit');
    component.ngOnInit();

    component.containsInfoInput.setValue('new info');
    setTimeout(() => {
      expect(component.filterChange.emit).toHaveBeenCalled();
      done();
    }, 150);
  });

  it('should unsubscribe on destroy', () => {
    spyOn(component['destroy$'], 'next');
    spyOn(component['destroy$'], 'complete');

    component.ngOnDestroy();

    expect(component['destroy$'].next).toHaveBeenCalled();
    expect(component['destroy$'].complete).toHaveBeenCalled();
  });
});