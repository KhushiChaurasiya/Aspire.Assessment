import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { TokenResponse, User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AppDownloaded } from '../_models/appdownloaded';
import { ChartOptions, ChartType, ChartDataset, Chart } from 'chart.js';
import { Logs } from '../_models/logs';
import { AlertService } from '../_services/alert.service';


@Component({ templateUrl: 'home.component.html' })
export class HomeComponent  implements AfterViewInit{
    user: TokenResponse | null;
    appDownloaded : AppDownloaded[];
    canvas: any;
    ctx: any;
    totalUser : any;
    totalLogReport : Logs[];
    model:any;
    FromDate : string;
    ToDate : string;
    bindDateFilter : any[] = [];

  @ViewChild('pieCanvas') pieCanvas!: { nativeElement: any };

  pieChart: any;

  constructor(private accountService: AccountService, private userService : UserService, private alertService : AlertService) {
    this.user = this.accountService.userValue;
}

  ngAfterViewInit(): void {
    this.getAppDownloadedChartReport();
    this.getLogAndUserCountReport();
    
  }

  SelectedFromDate(event :any)
  {
    this.FromDate = event.target.value;
    this.bindDateFilter.push({ key: "FromDate", value: this.FromDate });
    this.getAppDownloadedChartReport()
  }
  SelectedToDate(event :any)
  {
    this.ToDate = event.target.value;
    // this.bindDateFilter.push({"ToDate":this.ToDate});
    this.bindDateFilter.push({ key: "ToDate", value: this.ToDate });
    this.getAppDownloadedChartReport();
  }
  
  getAppName(data:[]){
    const appNameArray = new Array();
    for (let index = 0; index < data.length; index++) {
        const element = data[index]['appName'];
        appNameArray.push(element);
    }
    return appNameArray;
  }
  getAppDownloadedCount(data:[]){
    const appDownloadedCount = new Array();
    for (let index = 0; index < data.length; index++) {
        const element = data[index]['numberOfDownloads'];
        appDownloadedCount.push(element);
    }
    return appDownloadedCount;
  }

  pieChartBrowser(appD : any): void {
    this.canvas = this.pieCanvas.nativeElement;
    this.ctx = this.canvas.getContext('2d');

    this.pieChart = new Chart(this.ctx, {
      type: 'bar',
      data: {
        labels: this.getAppName(appD), 
        datasets: [
          {
            backgroundColor: [
              '#2ecc71',
              '#3498db',
              '#95a5a6',
              '#9b59b6',
              '#f1c40f',
              '#e74c3c',
            ],
            data: this.getAppDownloadedCount(appD),label: 'App Downloaded Report'
          },
        ],
      },
    });
  }

  getAppDownloadedChartReport()
  {
    this.userService.getDownloadedReport(this.bindDateFilter)
    .subscribe({
      next:(data) => {
        this.pieChartBrowser(data);
      },
      error: (error: any) => {
        this.alertService.error(error);
    }
    });
  }
  getLogAndUserCountReport()
  {
    
this.userService.getAllUserCountReport().subscribe(res =>{
  this.totalUser = res;
  });
  
  this.userService.getAllLogReport().subscribe(res=>{
  this.totalLogReport = res;
  console.log(this.totalLogReport);
  });
  }
}