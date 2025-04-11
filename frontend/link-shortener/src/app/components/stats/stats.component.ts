import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ChartModule } from 'primeng/chart';
import { ShortenNumberPipe } from '../../pipes/shorten-number.pipe';
import { Referrer, Location } from '../../../interfaces/StatsInterface';

@Component({
  selector: 'stats',
  imports: [SelectButtonModule, FormsModule, ChartModule, ShortenNumberPipe],
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.css',
})
export class StatsComponent implements OnInit {

  protected totalClicks: number = 1000;

  protected statOptions: Record<string, string>[] = [
    { label: 'Clicks', value: 'clicks' },
    { label: 'Referrers', value: 'referrers' },
    { label: 'Locations', value: 'locations' },
  ];
  protected statToShow: string  = "clicks";

  protected clicksData: any;
  protected clicksOptions: any;

  protected referrersData: Referrer[] = []
  protected locationsData: Location[] = []

  constructor() {}

  public ngOnInit(): void {
    this.initializeClicksChart();
    this.initializeReferrers();
    this.initializeLocations();
  }

  private initializeClicksChart(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
  
    this.clicksData = {
      labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      datasets: [
        {
          label: 'Sales', // This label is still used internally but won't show in the legend
          data: [540, 325, 702, 620, 590, 800, 700],
          backgroundColor: [
            'rgb(36, 36, 36)', // Chart color
          ],
          borderRadius: 5,
        },
      ],
    };
  
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
            color: textColorSecondary, // Color of the Y-axis labels (numbers)
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
  
  private initializeReferrers(): void {
    this.referrersData = [
      { referrer: 'Google', clicks: 150 },
      { referrer: 'Facebook', clicks: 200 },
      { referrer: 'Twitter', clicks: 200 },
      { referrer: 'LinkedIn', clicks: 350 },
    ];
  }

  private initializeLocations(): void {
    this.locationsData = [
      { country: 'Germany', clicks: 150 },
      { country: 'Spain', clicks: 200 }
    ];
  }
}
