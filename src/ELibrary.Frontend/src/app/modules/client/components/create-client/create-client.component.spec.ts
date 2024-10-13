import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { By } from "@angular/platform-browser";
import { ActivatedRoute } from "@angular/router";
import { of } from "rxjs";
import { CommandHandler, getDefaultClient, RedirectorService } from "../../../shared";
import { ADD_CLIENT_COMMAND_HANDLER, AddClientCommand, ClientService } from "../../../shop";
import { CreateClientComponent } from "./create-client.component";

describe('CreateClientComponent', () => {
    let component: CreateClientComponent;
    let fixture: ComponentFixture<CreateClientComponent>;
    let redirectorServiceSpy: jasmine.SpyObj<RedirectorService>;
    let addClientHandlerSpy: jasmine.SpyObj<CommandHandler<AddClientCommand>>;
    let activatedRouteStub: Partial<ActivatedRoute>;

    beforeEach(async () => {
        const clientServiceSpyObj = jasmine.createSpyObj<ClientService>(['getClient']);
        const redirectorServiceSpyObj = jasmine.createSpyObj('RedirectorService', ['redirectTo', 'redirectToHome']);
        const addClientHandlerSpyObj = jasmine.createSpyObj('CommandHandler', ['dispatch']);

        clientServiceSpyObj.getClient.and.returnValue(of(getDefaultClient()));

        activatedRouteStub = {
            queryParams: of({ redirectTo: '/dashboard' })
        };

        await TestBed.configureTestingModule({
            declarations: [CreateClientComponent],
            providers: [
                { provide: ClientService, useValue: clientServiceSpyObj },
                { provide: RedirectorService, useValue: redirectorServiceSpyObj },
                { provide: ADD_CLIENT_COMMAND_HANDLER, useValue: addClientHandlerSpyObj },
                { provide: ActivatedRoute, useValue: activatedRouteStub },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
        }).compileComponents();

        fixture = TestBed.createComponent(CreateClientComponent);
        component = fixture.componentInstance;
        redirectorServiceSpy = TestBed.inject(RedirectorService) as jasmine.SpyObj<RedirectorService>;
        addClientHandlerSpy = TestBed.inject(ADD_CLIENT_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<AddClientCommand>>;
    });

    beforeEach(() => {
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should subscribe to queryParams and set redirectUrl', () => {
        expect(component.redirectUrl).toBe('/dashboard');
    });

    it('should redirect to the provided URL if client is available', () => {

        component.ngOnInit();
        fixture.detectChanges();

        expect(redirectorServiceSpy.redirectTo).toHaveBeenCalledWith('/dashboard');
    });

    it('should redirect to home if no redirect URL is provided', () => {
        activatedRouteStub.queryParams = of({});

        component.ngOnInit();
        fixture.detectChanges();

        expect(redirectorServiceSpy.redirectToHome).toHaveBeenCalled();
    });

    it('should dispatch addClient command when addClient is called', () => {
        const addClientButton = fixture.debugElement.query(By.css('button'));
        addClientButton.nativeElement.click();

        expect(addClientHandlerSpy.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({}));
    });

    it('should unsubscribe on component destroy', () => {
        spyOn(component.destroy$, 'next');
        spyOn(component.destroy$, 'complete');

        component.ngOnDestroy();

        expect(component.destroy$.next).toHaveBeenCalled();
        expect(component.destroy$.complete).toHaveBeenCalled();
    });
});
