/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { CREATE_PUBLISHER_COMMAND_HANDLER, CreatePublisherCommand, DELETE_PUBLISHER_COMMAND_HANDLER, DeletePublisherCommand, PublisherService, UPDATE_PUBLISHER_COMMAND_HANDLER, UpdatePublisherCommand } from '../../../../../library';
import { CommandHandler, GenericTableComponent, getDefaultPublisher, Publisher } from '../../../../../shared';
import { PublisherTableComponent } from './publisher-table.component';

describe('PublisherTableComponent', () => {
  let component: PublisherTableComponent;
  let fixture: ComponentFixture<PublisherTableComponent>;
  let mockPublisherService: jasmine.SpyObj<PublisherService>;
  let mockCreatePublisherCommandHandler: jasmine.SpyObj<CommandHandler<CreatePublisherCommand>>;
  let mockUpdatePublisherCommandHandler: jasmine.SpyObj<CommandHandler<UpdatePublisherCommand>>;
  let mockDeletePublisherCommandHandler: jasmine.SpyObj<CommandHandler<DeletePublisherCommand>>;

  const mockItems = [
    getDefaultPublisher(),
    getDefaultPublisher()
  ];
  const mockAmount = 100;

  beforeEach(async () => {
    mockPublisherService = jasmine.createSpyObj('PublisherService', [
      'getItemTotalAmount',
      'getPaginated',
      'createPublisher',
      'updatePublisher',
      'deletePublisherById'
    ]);

    mockCreatePublisherCommandHandler = jasmine.createSpyObj<CommandHandler<CreatePublisherCommand>>([
      'dispatch',
    ]);

    mockUpdatePublisherCommandHandler = jasmine.createSpyObj<CommandHandler<UpdatePublisherCommand>>([
      'dispatch',
    ]);

    mockDeletePublisherCommandHandler = jasmine.createSpyObj<CommandHandler<DeletePublisherCommand>>([
      'dispatch',
    ]);

    mockPublisherService.getItemTotalAmount.and.returnValue(of(mockAmount));
    mockPublisherService.getPaginated.and.returnValue(of(mockItems));

    await TestBed.configureTestingModule({
      imports: [GenericTableComponent, BrowserAnimationsModule],
      declarations: [PublisherTableComponent],
      providers: [
        { provide: PublisherService, useValue: mockPublisherService },
        { provide: CREATE_PUBLISHER_COMMAND_HANDLER, useValue: mockCreatePublisherCommandHandler },
        { provide: UPDATE_PUBLISHER_COMMAND_HANDLER, useValue: mockUpdatePublisherCommandHandler },
        { provide: DELETE_PUBLISHER_COMMAND_HANDLER, useValue: mockDeletePublisherCommandHandler }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublisherTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize and fetch paginated items', () => {
    spyOn<any>(component, 'fetchPaginatedItems');
    component.ngOnInit();
    expect(component["fetchPaginatedItems"]).toHaveBeenCalledWith({ pageIndex: 1, pageSize: 10 });
  });

  it('should bind data to generic-table component', () => {
    component.items$ = of(mockItems);
    fixture.detectChanges();

    const genericTable = fixture.debugElement.query(By.directive(GenericTableComponent));
    expect(genericTable).toBeTruthy();

    const componentInstance = genericTable.componentInstance as GenericTableComponent;
    expect(componentInstance.items).toEqual(mockItems);
    expect(componentInstance.totalItemAmount).toBe(mockAmount);
  });

  it('should handle pageChange and update items$', () => {
    const mockItems = [getDefaultPublisher()];
    mockPublisherService.getPaginated.and.returnValue(of(mockItems));

    component.onPageChange({ pageIndex: 1, pageSize: 10 });
    fixture.detectChanges();

    component.items$.subscribe(items => {
      expect(items).toEqual(mockItems.slice(0, 10));
    });
  });

  it('should dispatch create publisher command', () => {

    component.createNew();
    fixture.detectChanges();

    expect(mockCreatePublisherCommandHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch update publisher command', () => {
    const mockPublisher: Publisher = getDefaultPublisher();

    component.update(mockPublisher);
    fixture.detectChanges();

    expect(mockUpdatePublisherCommandHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch delete publisher command', () => {
    const mockPublisher: Publisher = getDefaultPublisher();

    component.delete(mockPublisher);
    fixture.detectChanges();

    expect(mockDeletePublisherCommandHandler.dispatch).toHaveBeenCalled();
  });
});
