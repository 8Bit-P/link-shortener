import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Stats } from '../interfaces/StatsInterface';
import { UpdateUrlRequest, UrlRequest } from '../interfaces/UrlRequest';
import { ShortenedUrl } from '../interfaces/ShortenedUrl';

@Injectable({
  providedIn: 'root',
})
export class URLService {
  private apiUrl = 'http://localhost:8080/api/url';

  constructor(private http: HttpClient) {}

  // POST /api/url
  public createShortUrl(request: UrlRequest): Observable<ShortenedUrl> {
    return this.http.post<ShortenedUrl>(this.apiUrl, request);
  }

  // GET /api/url/{shortCode}
  public getOriginalUrl(shortCode: string): Observable<string> {
    return this.http.get(this.apiUrl + `/${shortCode}`, {
      responseType: 'text',
    });
  }

  // GET /api/url/{shortCode}/stats
  public getUrlStats(shortCode: string): Observable<Stats> {
    return this.http.get<Stats>(this.apiUrl + `/${shortCode}/stats`);
  }

  // PUT /api/url
  public updateShortUrl(request: UpdateUrlRequest): Observable<ShortenedUrl> {
    return this.http.put<ShortenedUrl>(this.apiUrl, request);
  }

  // DELETE /api/url/{shortCode}
  public deleteShortUrl(shortCode: string): Observable<void> {
    return this.http.delete<void>(this.apiUrl + `/${shortCode}`);
  }
}
