import {
  Component,
  computed,
  effect,
  OnInit,
  resource,
  signal,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { URLService } from '../../../services/url.service';
import { UrlRequest } from '../../../interfaces/UrlRequest';
import { ShortenedUrl } from '../../../interfaces/ShortenedUrl';
import { firstValueFrom } from 'rxjs';
@Component({
  selector: 'shorten-url',
  templateUrl: './shorten-url.component.html',
  styleUrls: ['./shorten-url.component.css'],
  providers: [MessageService],
  imports: [ReactiveFormsModule, ToastModule],
})
export class ShortenUrlComponent implements OnInit {
  protected form: FormGroup;
  protected currentUrl: string = '';
  protected shortenedUrlResource = resource<
    ShortenedUrl | null,
    UrlRequest | null
  >({
    request: () => this.urlRequest(),
    loader: async ({ request }) => {
      if (!request) return Promise.resolve(null);
      return await firstValueFrom(this.urlService.createShortUrl(request));
    },
  });
  protected shortenedUrl = computed(() => {
    const resourceShortCode = this.shortenedUrlResource.value()?.shortCode;
    return this.currentUrl + '/' + resourceShortCode;
  });

  private urlRequest = signal<UrlRequest | null>(null);

  constructor(
    private fb: FormBuilder,
    private messageService: MessageService,
    private urlService: URLService
  ) {
    this.form = this.fb.group({
      url: ['', [Validators.required]],
    });

    effect(() => {
      if (
        this.shortenedUrlResource.hasValue() &&
        this.shortenedUrlResource.value() !== null
      ) {
        const result = this.shortenedUrlResource.value();
        this.messageService.add({
          severity: 'success',
          summary: 'Short URL created',
          detail: this.shortenedUrl(),
        });
      }

      if (this.shortenedUrlResource.error()) {
        this.messageService.add({
          severity: 'error',
          summary: 'Shortening failed',
          detail: 'Unknown error',
        });
      }
    });
  }

  public ngOnInit(): void {
    //Set base url as currentUrl
    this.currentUrl = window.location.host;
  }

  //Send request to server to create short URL
  public onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return; 
    }

    const data: UrlRequest = {
      originalURL: this.form.value.url,
    };
    this.urlRequest.set(data);
  }

  public copyToClipboard(): void {
    if (!this.shortenedUrlResource.value()) return;

    navigator.clipboard
      .writeText(this.shortenedUrl())
      .then(() => {
        this.messageService.add({
          severity: 'contrast',
          summary: 'Copied to clipboard',
          detail: this.shortenedUrl(),
        });
      })
      .catch((err) => {
        console.error('Failed to copy URL: ', err);
      });
  }
}
