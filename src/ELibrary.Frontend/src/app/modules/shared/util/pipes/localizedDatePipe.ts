import { Inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'localizedDate'
})
export class LocalizedDatePipe implements PipeTransform {

    constructor(@Inject(LOCALE_ID) private locale: string) { }

    transform(value: Date | string | number | null | undefined, includeTime = false): string | null {
        if (value === null || value === undefined) {
            return null;
        }

        const date = new Date(value);
        if (isNaN(date.getTime())) {
            return null;
        }

        const options: Intl.DateTimeFormatOptions = includeTime
            ? { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' }
            : { day: '2-digit', month: '2-digit', year: 'numeric' };

        return date.toLocaleDateString(this.locale, options);
    }
}
