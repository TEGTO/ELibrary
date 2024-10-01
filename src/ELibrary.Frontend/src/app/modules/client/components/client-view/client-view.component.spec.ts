import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { ClientViewComponent } from './client-view.component';

describe('ClientViewComponent', () => {
    let component: ClientViewComponent;
    let fixture: ComponentFixture<ClientViewComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [ClientViewComponent],
            providers: [provideRouter([])],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(ClientViewComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should render wrapper with correct classes', () => {
        const wrapper = fixture.debugElement.query(By.css('.wrapper'));
        expect(wrapper).toBeTruthy();
        expect(wrapper.nativeElement.classList).toContain('bg-zinc-50/60');
        expect(wrapper.nativeElement.classList).toContain('max-w-screen-lg');
        expect(wrapper.nativeElement.classList).toContain('gap-3');
        expect(wrapper.nativeElement.classList).toContain('mx-auto');
        expect(wrapper.nativeElement.classList).toContain('p-4');
    });
});
