import { TestBed } from '@angular/core/testing';
import { ManagerOrderDetailsCommand } from '../../..';
import { getDefaultOrder, getManagerOrderDetailsPath, RedirectorService } from '../../../../shared';
import { ManagerOrderDetailsCommandHandlerService } from './manager-order-details-command-handler.service';

describe('ManagerOrderDetailsCommandHandlerService', () => {
    let service: ManagerOrderDetailsCommandHandlerService;
    let redirectorSpy: jasmine.SpyObj<RedirectorService>;

    beforeEach(() => {
        const spy = jasmine.createSpyObj('RedirectorService', ['redirectTo']);

        TestBed.configureTestingModule({
            providers: [
                ManagerOrderDetailsCommandHandlerService,
                { provide: RedirectorService, useValue: spy }
            ]
        });

        service = TestBed.inject(ManagerOrderDetailsCommandHandlerService);
        redirectorSpy = TestBed.inject(RedirectorService) as jasmine.SpyObj<RedirectorService>;
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('dispatch should call redirector.redirectTo with correct path', () => {
        const mockOrderId = 0;
        const command: ManagerOrderDetailsCommand = { order: getDefaultOrder() };
        const expectedPath = getManagerOrderDetailsPath(mockOrderId);

        service.dispatch(command);

        expect(redirectorSpy.redirectTo).toHaveBeenCalledWith(expectedPath);
    });
});