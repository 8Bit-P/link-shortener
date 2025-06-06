export interface Stats {
    referrers: Referrer[];
    locations: Location[];
    clicks: Clicks[];
}

export interface Referrer {
    referrer: string;
    clicks: number;
} 

export interface Location {
    country: string;
    clicks: number;
} 

export interface Clicks {
    dayOfWeek: string;
    clicks: number;
} 