import { Component, OnInit } from '@angular/core';
import { App } from '../_models/app';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html'
})
export class ListComponent implements OnInit {
  apps?: App[];

  constructor(private userService: UserService, private alertService : AlertService) {}

  ngOnInit() {
      this.GetAllApps();
  }
  delete(id: number) {  
      var ans = confirm("Do you want to delete app with Id: " + id);  
      if (ans) {  
          this.userService.delete(id).subscribe((data) => {  
              this.GetAllApps();
          }, error => this.alertService.error(error))  
      }  
  }  

  GetAllApps()
  {
    this.userService.getAll()
    .subscribe({
      next:(data) => {
        this.apps = data;
      },
      error: (error: any) => {
        this.alertService.error(error);
      }
    });
    
  }

}
