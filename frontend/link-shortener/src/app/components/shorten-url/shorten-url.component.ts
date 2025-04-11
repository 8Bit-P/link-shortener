import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api'; 
@Component({
  selector: 'shorten-url',
  templateUrl: './shorten-url.component.html',
  styleUrls: ['./shorten-url.component.css'] ,
  providers: [MessageService],
  imports: [ReactiveFormsModule,ToastModule],
})
export class ShortenUrlComponent {

  protected form: FormGroup;

  protected shortURL: string = 'https://example.com/short-url';
  protected loading: boolean = false;

  constructor(private fb: FormBuilder, private messageService: MessageService) {
    this.form = this.fb.group({
      url: [''],
    })
  } 

  public onSubmit(): void {
    console.log(this.form.value);
  }

  public copyToClipboard(): void {
    navigator.clipboard.writeText(this.shortURL).then(() => {
      this.messageService.add({severity:'contrast', summary:'Copied to clipboard', detail:this.shortURL});
    }).catch((err) => {
      console.error('Failed to copy URL: ', err);
    });
  }
}
