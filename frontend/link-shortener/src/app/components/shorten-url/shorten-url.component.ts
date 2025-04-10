import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';

@Component({
  selector: 'app-shorten-url',
  templateUrl: './shorten-url.component.html',
  styleUrls: ['./shorten-url.component.css'] ,
  imports: [ReactiveFormsModule],
})
export class ShortenUrlComponent {

  protected form: FormGroup;

  protected shortURL: string = 'https://example.com/short-url';
  protected loading: boolean = false;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      url: [''],
    })
  } 

  public onSubmit(): void {
    console.log(this.form.value);
  }

  public copyToClipboard(): void {
    navigator.clipboard.writeText(this.shortURL).then(() => {
      console.log('URL copied to clipboard!');
    }).catch((err) => {
      console.error('Failed to copy URL: ', err);
    });
  }
}
