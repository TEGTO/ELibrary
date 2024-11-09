import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StatisticsChartComponent } from './statistics-chart.component';

describe('StatisticsChartComponent', () => {
  let component: StatisticsChartComponent;
  let fixture: ComponentFixture<StatisticsChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StatisticsChartComponent],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(StatisticsChartComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('initChartOptions', () => {
    it('should initialize chart options for daily data', () => {
      const mockData = new Map<Date, number>([
        [new Date('2024-09-28'), 11],
        [new Date('2024-09-29'), 5]
      ]);

      const result = component.initChartOptions(mockData);
      expect(result).toBeTrue();
      expect(component.multiMonth).toBeFalse();
      expect(component.chartOptions.series![0].data).toEqual([
        { x: new Date('2024-09-28').getTime(), y: 11 },
        { x: new Date('2024-09-29').getTime(), y: 5 }
      ]);
    });

    it('should initialize chart options for multi-month data', () => {
      const mockData = new Map<Date, number>([
        [new Date('2024-09-28'), 11],
        [new Date('2024-10-28'), 5]
      ]);

      const result = component.initChartOptions(mockData);
      expect(result).toBeTrue();
      expect(component.multiMonth).toBeTrue();
      expect(component.chartOptions.series![0].data).toEqual([
        { x: jasmine.any(Number), y: 11 },
        { x: jasmine.any(Number), y: 5 }
      ]);
    });
  });

  describe('getDateRange', () => {
    it('should return correct date range for the data', () => {
      const mockData = new Map<Date, number>([
        [new Date('2024-09-28'), 11],
        [new Date('2024-10-02'), 5]
      ]);

      const dateRange = component['getDateRange'](mockData);
      expect(dateRange.start).toEqual(new Date('2024-09-28'));
      expect(dateRange.end).toEqual(new Date('2024-10-02'));
    });
  });

  describe('isMultiMonth', () => {
    it('should return true for multi-month data', () => {
      const result = component['isMultiMonth'](new Date('2024-09-01'), new Date('2024-10-01'));
      expect(result).toBeTrue();
    });

    it('should return false for single-month data', () => {
      const result = component['isMultiMonth'](new Date('2024-09-01'), new Date('2024-09-30'));
      expect(result).toBeFalse();
    });
  });

  describe('formatDataByDay', () => {
    it('should format data by day', () => {
      const mockData = new Map<Date, number>([
        [new Date('2024-09-28'), 11],
        [new Date('2024-09-29'), 5]
      ]);

      const formattedData = component['formatDataByDay'](mockData);
      expect(formattedData).toEqual([
        { x: new Date('2024-09-28').getTime(), y: 11 },
        { x: new Date('2024-09-29').getTime(), y: 5 }
      ]);
    });
  });

  describe('aggregateDataByMonth', () => {
    it('should aggregate data by month', () => {
      const mockData = new Map<Date, number>([
        [new Date('2024-09-28'), 11],
        [new Date('2024-09-29'), 5],
        [new Date('2024-10-01'), 8]
      ]);

      const aggregatedData = component['aggregateDataByMonth'](mockData);
      expect(aggregatedData).toEqual([
        { x: jasmine.any(Number), y: 16 },  // September total
        { x: jasmine.any(Number), y: 8 }    // October total
      ]);
    });
  });

  describe('labelFormatter', () => {
    it('should format labels correctly for daily data', () => {
      component.multiMonth = false;
      const result = component['labelFormatter']('', new Date('2024-09-28').getTime());
      expect(result).toBe('28 Sep');
    });

    it('should format labels correctly for multi-month data', () => {
      component.multiMonth = true;
      const result = component['labelFormatter']('', new Date('2024-09-28').getTime());
      expect(result).toBe('Sep 2024');
    });
  });

  describe('tooltipformatter', () => {
    it('should format tooltips correctly for daily data', () => {
      component.multiMonth = false;
      const result = component['tooltipformatter'](new Date('2024-09-28').getTime());
      expect(result).toBe('28 Sep');
    });

    it('should format tooltips correctly for multi-month data', () => {
      component.multiMonth = true;
      const result = component['tooltipformatter'](new Date('2024-09-28').getTime());
      expect(result).toBe('Sep 2024');
    });
  });
});
