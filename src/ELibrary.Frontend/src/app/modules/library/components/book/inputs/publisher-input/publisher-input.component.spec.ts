// import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
// import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
// import { of } from 'rxjs';
// import { PublisherService } from '../../../..';
// import { Publisher, ValidationMessage } from '../../../../../shared';
// import { PublisherInputComponent } from './publisher-input.component';

// describe('PublisherInputComponent', () => {
//   let component: PublisherInputComponent;
//   let fixture: ComponentFixture<PublisherInputComponent>;
//   let publisherService: jasmine.SpyObj<PublisherService>;
//   let validationMessage: jasmine.SpyObj<ValidationMessage>;

//   const mockPublishers: Publisher[] = [
//     { id: 1, name: 'Publisher 1' },
//     { id: 2, name: 'Publisher 2' }
//   ];

//   beforeEach(async () => {
//     const publisherServiceSpy = jasmine.createSpyObj('PublisherService', ['getPaginated']);
//     const validationMessageSpy = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

//     await TestBed.configureTestingModule({
//       declarations: [PublisherInputComponent],
//       imports: [ReactiveFormsModule],
//       providers: [
//         { provide: PublisherService, useValue: publisherServiceSpy },
//         { provide: ValidationMessage, useValue: validationMessageSpy }
//       ]
//     }).compileComponents();

//     fixture = TestBed.createComponent(PublisherInputComponent);
//     component = fixture.componentInstance;
//     publisherService = TestBed.inject(PublisherService) as jasmine.SpyObj<PublisherService>;
//     validationMessage = TestBed.inject(ValidationMessage) as jasmine.SpyObj<ValidationMessage>;

//     publisherService.getPaginated.and.returnValue(of(mockPublishers));

//     component.formGroup = new FormGroup({
//       publisher: new FormControl(null)
//     });
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });

//   it('should initialize form control if not present', () => {
//     const formGroup = new FormGroup({});
//     component.formGroup = formGroup;
//     component.ngOnInit();

//     expect(formGroup.contains('publisher')).toBeTrue();
//   });

//   it('should fetch items on init', fakeAsync(() => {
//     component.ngOnInit();
//     tick();

//     expect(publisherService.getPaginated).toHaveBeenCalledWith({
//       pageNumber: 1,
//       pageSize: component.pageAmount,
//       containsName: ''
//     });
//     expect(component.items.length).toBe(2);
//     expect(component.items).toEqual(mockPublishers);
//   }));

//   it('should update items when value changes', fakeAsync(() => {
//     component.ngOnInit();
//     component.input.setValue('Publisher 1');
//     tick(100);

//     expect(publisherService.getPaginated).toHaveBeenCalledWith({
//       pageNumber: 1,
//       pageSize: component.pageAmount,
//       containsName: 'Publisher 1'
//     });
//   }));

//   it('should display the publisher name using displayWith', () => {
//     const publisher: Publisher = { id: 1, name: 'Test Publisher' };
//     const displayValue = component.displayWith(publisher);
//     expect(displayValue).toBe('Test Publisher');
//   });

//   it('should return empty string if publisher is undefined in displayWith', () => {
//     const displayValue = component.displayWith(undefined);
//     expect(displayValue).toBe('');
//   });

//   it('should return the correct id in trackByPublisher', () => {
//     const publisher: Publisher = { id: 1, name: 'Publisher 1' };
//     const trackId = component.trackByPublisher(0, publisher);
//     expect(trackId).toBe(1);
//   });

//   it('should validate the input field using validateInputField', () => {
//     const control = new FormControl();
//     validationMessage.getValidationMessage.and.returnValue({
//       hasError: true,
//       message: 'Error message'
//     });

//     const validation = component.validateInputField(control);

//     expect(validationMessage.getValidationMessage).toHaveBeenCalledWith(control);
//     expect(validation.hasError).toBeTrue();
//     expect(validation.message).toBe('Error message');
//   });

//   it('should calculate selection size based on items', () => {
//     component.items = mockPublishers;
//     const selectionSize = component.selectionSize;

//     expect(selectionSize).toBe(component.amountItemsInView * component.itemHeight);
//   });

//   it('should set up scroll listeners', () => {
//     const mockScroller = jasmine.createSpyObj('CdkVirtualScrollViewport', ['elementScrolled', 'measureScrollOffset']);
//     mockScroller.elementScrolled.and.returnValue(of(0));
//     component.scroller = mockScroller;

//     component.ngAfterViewInit();

//     expect(mockScroller.elementScrolled).toHaveBeenCalled();
//   });
// });