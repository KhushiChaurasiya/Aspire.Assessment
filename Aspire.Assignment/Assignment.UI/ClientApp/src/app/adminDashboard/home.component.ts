import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { TokenResponse, User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AppDownloaded } from '../_models/appdownloaded';
import { ChartOptions, ChartType, ChartDataset, Chart } from 'chart.js';
import { Logs } from '../_models/logs';


@Component({ templateUrl: 'home.component.html' })
export class HomeComponent  implements AfterViewInit{
    user: TokenResponse | null;
    appDownloaded : AppDownloaded[];
    canvas: any;
    ctx: any;
    totalUser : any;
    totalLogReport : Logs[];

  @ViewChild('pieCanvas') pieCanvas!: { nativeElement: any };

  pieChart: any;

  constructor(private accountService: AccountService, private userService : UserService) {
    this.user = this.accountService.userValue;
}

  ngAfterViewInit(): void {
    debugger;
    this.userService.getDownloadedReport().subscribe(data=>{
                    this.appDownloaded = data;
                    this.pieChartBrowser(data);
                    });

    this.userService.getAllUserCountReport().subscribe(res =>{
      this.totalUser = res;
    });

    this.userService.getAllLogReport().subscribe(res=>{
      this.totalLogReport = res;
      console.log(this.totalLogReport);
    })
    
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
}

// constructor(private accountService: AccountService, private userService : UserService) {
//     this.user = this.accountService.userValue;
// }
//     ngOnInit() {
//         debugger;
//             this.userService.getDownloadedReport().subscribe(data=>{
//             this.appDownloaded = data;
//             });
//     }


// }