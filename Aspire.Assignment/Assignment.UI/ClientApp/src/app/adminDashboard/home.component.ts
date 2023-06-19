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
    bindDateFilter : any[] = [];
    toDate : Date= new Date() ;
    fromDate : Date = new Date(); 
    filterLogWiseDate : Date = new Date();
    level : string = "Error";

  

  @ViewChild('pieCanvas') pieCanvas!: { nativeElement: any };

  pieChart: any;

  constructor(private accountService: AccountService, private userService : UserService, private alertService : AlertService) {
    this.user = this.accountService.userValue;
}

  ngAfterViewInit(): void {
    this.getAppDownloadedChartReport();
    this.getUserCountReport();
    this.getLogReport();
    
  }

  SelectedFromDate(event :any)
  {
    this.fromDate = event.target.value;
    this.pieChart.destroy();
    this.getAppDownloadedChartReport()
  }
  SelectedToDate(event :any)
  {
    this.toDate = event.target.value;
    // this.bindDateFilter.push({"ToDate":this.ToDate});
    this.pieChart.destroy();
    this.getAppDownloadedChartReport();
  }
  
  SelectedLogDateTime(event: any)
  {
    debugger;
    this.filterLogWiseDate = event.target.value;
    this.getLogReport();
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
    debugger;
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
    this.userService.getDownloadedReport(this.fromDate,this.toDate).subscribe({
      next:(data) => {
        this.pieChartBrowser(data);
      },
      error: (error: any) => {
        this.alertService.error(error);
      }
    });
  }
  getUserCountReport()
  {
    this.userService.getAllUserCountReport().subscribe({
      next:(res) => {
        this.totalUser = res;
      },
      error: (error: any) => {
        this.alertService.error(error);
      }
    });
  }
  getLogReport()
  {
    debugger;
   
    this.userService.getAllLogReport(this.filterLogWiseDate,this.level).subscribe({
      next:(res) => {
        this.totalLogReport = res;
      console.log(this.totalLogReport);
      },
      error: (error: any) => {
        this.alertService.error(error);
      }
    });
  }
  SelectedLevel(event:any)
  {
    debugger;
    this.level = event.target.value;
    this.getLogReport();
  }
}