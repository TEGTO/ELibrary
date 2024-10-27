import { TestBed } from '@angular/core/testing';
import { MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs';
import { CreatePublisherCommand, LibraryDialogManager, PublisherService } from '../../..';
import { Publisher, getDefaultPublisher, mapPublisherToCreatePublisherRequest } from '../../../../shared';
import { CreatePublisherCommandHandlerService } from './create-publisher-command-handler.service';

describe('CreatePublisherCommandHandlerService', () => {
  let service: CreatePublisherCommandHandlerService;
  let mockDialogManager: jasmine.SpyObj<LibraryDialogManager>;
  let mockPublisherService: jasmine.SpyObj<PublisherService>;

  beforeEach(() => {
    const dialogManagerSpy = jasmine.createSpyObj('LibraryDialogManager', ['openPublisherDetailsMenu']);
    const publisherServiceSpy = jasmine.createSpyObj('PublisherService', ['create']);

    TestBed.configureTestingModule({
      providers: [
        CreatePublisherCommandHandlerService,
        { provide: LibraryDialogManager, useValue: dialogManagerSpy },
        { provide: PublisherService, useValue: publisherServiceSpy }
      ]
    });

    service = TestBed.inject(CreatePublisherCommandHandlerService);
    mockDialogManager = TestBed.inject(LibraryDialogManager) as jasmine.SpyObj<LibraryDialogManager>;
    mockPublisherService = TestBed.inject(PublisherService) as jasmine.SpyObj<PublisherService>;
  });

  it('should open the publisher details menu when dispatch is called', () => {
    const command: CreatePublisherCommand = {};
    const mockPublisher: Publisher = getDefaultPublisher();
    const mockDialogRef = createMockMatDialogRef(mockPublisher);

    mockDialogManager.openPublisherDetailsMenu.and.returnValue(mockDialogRef);

    service.dispatch(command);

    expect(mockDialogManager.openPublisherDetailsMenu).toHaveBeenCalledWith(mockPublisher);
  });

  it('should call publisherService.create with the correct request when a publisher is returned', () => {
    const command: CreatePublisherCommand = {};
    const mockPublisher: Publisher = getDefaultPublisher();
    const expectedRequest = mapPublisherToCreatePublisherRequest(mockPublisher);
    const mockDialogRef = createMockMatDialogRef(mockPublisher);

    mockDialogManager.openPublisherDetailsMenu.and.returnValue(mockDialogRef);

    service.dispatch(command);

    expect(mockPublisherService.create).toHaveBeenCalledWith(expectedRequest);
  });

  it('should not call publisherService.create when no publisher is returned', () => {
    const command: CreatePublisherCommand = {};
    const mockDialogRef = createMockMatDialogRef(null);

    mockDialogManager.openPublisherDetailsMenu.and.returnValue(mockDialogRef);

    service.dispatch(command);

    expect(mockPublisherService.create).not.toHaveBeenCalled();
  });

  function createMockMatDialogRef<T>(returnValue: T): MatDialogRef<T> {
    return {
      afterClosed: () => of(returnValue),
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      close: () => { },
    } as MatDialogRef<T>;
  }
});