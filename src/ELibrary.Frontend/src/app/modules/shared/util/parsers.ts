
export function parseDate(dateString: any): Date | null {
    return dateString ? new Date(dateString) : null;
}

export function parseBoolean(value: any): boolean {
    return value ?? false;
}

export function parseNumber(value: any): number {
    return value ?? 0;
}