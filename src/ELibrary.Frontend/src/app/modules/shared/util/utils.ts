
export function combineDateTime(date: Date, time: Date): Date {
    const hours = time.getHours();
    const minutes = time.getMinutes();
    const newDateTime = new Date(date);
    newDateTime.setHours(hours, minutes, 0, 0);
    return newDateTime;
}

export function getDateOrNull(date: Date | null, time: Date | null): Date | null {
    if (date !== null) {
        const t = time ?? new Date(0, 0, 0, 0);
        return combineDateTime(date, t);
    }
    return null;
}