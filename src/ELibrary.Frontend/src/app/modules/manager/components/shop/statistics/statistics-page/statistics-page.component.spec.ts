import { CUSTOM_ELEMENTS_SCHEMA, DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { CurrencyPipeApplier, getDefaultGetShopStatistics, getDefaultShopStatistics, GetShopStatistics, LocaleService, ShopStatistics } from '../../../../../shared';
import { StatisticsService } from '../../../../../shop';
import { StatisticsPageComponent } from './statistics-page.component';

describe('StatisticsPageComponent', () => {
  let component: StatisticsPageComponent;
  let fixture: ComponentFixture<StatisticsPageComponent>;
  let mockStatisticsService: jasmine.SpyObj<StatisticsService>;
  let mockCurrencyPipeApplier: jasmine.SpyObj<CurrencyPipeApplier>;

  const mockStatistics: ShopStatistics = {
    ...getDefaultShopStatistics(),
    inCartCopies: 10,
    inOrderCopies: 15,
    soldCopies: 100,
    averagePrice: 50,
    earnedMoney: 5000,
  };

  beforeEach(async () => {
    mockStatisticsService = jasmine.createSpyObj('StatisticsService', ['getShopStatistics']);
    mockCurrencyPipeApplier = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);
    const mockLocaleService = jasmine.createSpyObj<LocaleService>('LocaleService', ['getLocale']);

    mockLocaleService.getLocale.and.returnValue("uk-UA");

    await TestBed.configureTestingModule({
      declarations: [StatisticsPageComponent],
      providers: [
        { provide: StatisticsService, useValue: mockStatisticsService },
        { provide: CurrencyPipeApplier, useValue: mockCurrencyPipeApplier },
        { provide: LocaleService, useValue: mockLocaleService },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticsPageComponent);
    component = fixture.componentInstance;

    mockStatisticsService.getShopStatistics.and.returnValue(of(mockStatistics));
    mockCurrencyPipeApplier.applyCurrencyPipe.and.callFake(value => `$${value}`);

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch shop statistics on initialization', () => {
    expect(mockStatisticsService.getShopStatistics).toHaveBeenCalledWith(getDefaultGetShopStatistics());
    fixture.detectChanges();

    const statsDebugElement: DebugElement = fixture.debugElement;
    const inCartCopiesElement = statsDebugElement.query(By.css('.statistics__detail-value span')).nativeElement;

    expect(inCartCopiesElement.textContent).toContain('10');
  });

  it('should apply currency formatting to values', () => {
    const formattedValue = component.applyCurrencyPipe(1000);
    expect(mockCurrencyPipeApplier.applyCurrencyPipe).toHaveBeenCalledWith(1000);
    expect(formattedValue).toBe('$1000');
  });

  it('should call getShopStatistics when filter changes', () => {
    const newFilter: GetShopStatistics = { ...getDefaultGetShopStatistics() };
    component.getShopStatistics(newFilter);

    expect(mockStatisticsService.getShopStatistics).toHaveBeenCalledWith(newFilter);
  });

  it('should display statistics after data is fetched', () => {
    fixture.detectChanges();

    const statsDebugElement: DebugElement = fixture.debugElement;

    const inCartCopiesElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(1) span')).nativeElement;
    const inOrderCopiesElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(2) span')).nativeElement;
    const soldCopiesElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(3) span')).nativeElement;
    const canceledCopiesElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(4) span')).nativeElement;
    const orderAmountElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(5) span')).nativeElement;
    const canceledOrderAmountElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(6) span')).nativeElement;
    const averagePriceElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(7) span')).nativeElement;
    const earnedMoneyElement = statsDebugElement.query(By.css('.statistics__detail-value:nth-child(8) span')).nativeElement;

    expect(inCartCopiesElement.textContent).toContain('10');
    expect(inOrderCopiesElement.textContent).toContain('15');
    expect(soldCopiesElement.textContent).toContain('100');
    expect(canceledCopiesElement.textContent).toContain('0');
    expect(orderAmountElement.textContent).toContain('0');
    expect(canceledOrderAmountElement.textContent).toContain('0');
    expect(averagePriceElement.textContent).toContain('$50');
    expect(earnedMoneyElement.textContent).toContain('$5000');
  });
});
