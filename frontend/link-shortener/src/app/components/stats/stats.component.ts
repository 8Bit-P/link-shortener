import {
  Component,
  computed,
  effect,
  OnInit,
  resource,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ChartModule } from 'primeng/chart';
import { ShortenNumberPipe } from '../../pipes/shorten-number.pipe';
import { Stats } from '../../../interfaces/StatsInterface';
import { URLService } from '../../../services/url.service';
import { firstValueFrom } from 'rxjs';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'stats',
  providers: [MessageService],
  imports: [
    SelectButtonModule,
    FormsModule,
    ChartModule,
    ShortenNumberPipe,
    ToastModule,
  ],
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.css',
})
export class StatsComponent implements OnInit {
  protected totalClicks: number = 0;
  protected clicksOptions: any;
  protected clicksData: any;

  protected placeholderUrl = signal<string>('');

  protected statOptions: Record<string, string>[] = [
    { label: 'Clicks', value: 'clicks' },
    { label: 'Referrers', value: 'referrers' },
    { label: 'Locations', value: 'locations' },
  ];
  protected statToShow: string = 'clicks';

  protected shortenedUrlStatsResource = resource<Stats | null, string | null>({
    request: () => this.shortenedUrlShortCode(),
    loader: async ({ request }) => {
      if (!request) return null;

      try {
        const response: Stats = await firstValueFrom(
          this.urlService.getUrlStats(request)
        );

        this.updateTotalClicks(response);

        return response;
      } catch (err: any) {
        throw new Error(err?.message || 'Unknown error');
      }
    },
  });
  protected urlToCheck = signal<string | null>(null);

  protected shortenedUrl = signal<string | null>(null);

  private shortenedUrlShortCode = computed(() => {
    const shortenedUrl = this.shortenedUrl();
    if (!shortenedUrl) return null;

    try {
      const normalizedUrl = shortenedUrl.startsWith('http')
        ? shortenedUrl
        : `http://${shortenedUrl}`;
      const url = new URL(normalizedUrl);

      return url.pathname.replace(/^\/+/, '');
    } catch {
      return null;
    }
  });

  constructor(
    private messageService: MessageService,
    private urlService: URLService
  ) {
    effect(() => {
      const error = (this.shortenedUrlStatsResource as any).error?.();
      if (error) {
        this.messageService.add({
          severity: 'error',
          summary: 'Error retrieving stats',
          detail: error.message || 'An unexpected error occurred.',
        });
      }
    });
  }

  public ngOnInit(): void {
    this.placeholderUrl.set(window.location.href + 'as34qq32');
    this.initializeClicksChart();
  }

  public onCheck(): void {
    this.shortenedUrl.set(this.urlToCheck());
  }

  private updateTotalClicks(stats: Stats): void {
    this.totalClicks = stats.clicks.reduce(
      (sum, entry) => sum + entry.clicks,
      0
    );

    // Create labels and data arrays for the chart
    const labels = stats.clicks.map((click) => click.dayOfWeek);
    const data = stats.clicks.map((click) => click.clicks);

    this.clicksData = {
      labels,
      datasets: [
        {
          label: 'Clicks',
          data,
          backgroundColor: ['rgb(36, 36, 36)'],
          borderRadius: 5,
        },
      ],
    };
  }

  private initializeClicksChart(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColorSecondary = documentStyle.getPropertyValue(
      '--p-text-muted-color'
    );

    this.clicksOptions = {
      plugins: {
        legend: {
          display: false, // Hide the legend (Sales label)
        },
      },
      scales: {
        x: {
          display: true, // Keep X-axis labels (days of the week)
          ticks: {
            color: textColorSecondary, // Color of the X-axis labels (days)
          },
          grid: {
            display: false, // Remove X-axis grid lines
          },
          border: {
            display: false, // Remove X-axis line
          },
        },
        y: {
          display: true, // Keep Y-axis labels (numbers)
          beginAtZero: true,
          ticks: {
            color: textColorSecondary,
            stepSize: 1, 
            callback: function (value: number) {
              return Number.isInteger(value) ? value : '';
            },
          },
          grid: {
            display: false, // Remove Y-axis grid lines
          },
          border: {
            display: false, // Remove Y-axis line
          },
        },
      },
    };
  }
}
