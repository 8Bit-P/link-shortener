import {
  Component,
  OnInit,
  computed,
  effect,
  resource,
  signal,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ShortenUrlComponent } from './components/shorten-url/shorten-url.component';
import { StatsComponent } from './components/stats/stats.component';
import { URLService } from '../services/url.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ShortenUrlComponent, StatsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  // Signal holding the current detected shortcode or null if none
  private shortcode = signal<string | null>(null);

  private originalUrlResource = resource<string | null, string | null>({
    request: () => this.shortcode(),
    loader: async ({ request }) => {
      if (!request) return null;
      try {
        return await firstValueFrom(this.urlService.getOriginalUrl(request));
      } catch (e) {
        throw e;
      }
    },
  });

  constructor(private urlService: URLService) {
    effect(() => {
      const originalUrl = this.originalUrlResource.value();
      if (originalUrl) {
        window.location.href = originalUrl; // redirect to original URL
      }
    });

    // Optional: effect to log errors
    effect(() => {
      const error = (this.originalUrlResource as any).error?.();
      if (error) {
        console.error('Error fetching original URL:', error);
      }
    });
  }

  ngOnInit(): void {
    // Extract shortcode from current location pathname
    const path = window.location.pathname.replace(/^\/+/, ''); // removes leading slash(es)
    if (path) {
      this.shortcode.set(path);
    }
  }
}
