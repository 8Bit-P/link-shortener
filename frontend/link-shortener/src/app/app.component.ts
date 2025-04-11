import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ShortenUrlComponent } from './components/shorten-url/shorten-url.component';
import { StatsComponent } from "./components/stats/stats.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ShortenUrlComponent, StatsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {}
