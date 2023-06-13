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

  constructor(private userService: UserService, private alertService: AlertService, ) {}

  ngOnInit() {
    console.log("App list page")
      this.GetAllApps();
  }
  delete(id: number) { 
      var ans = confirm("Do you want to delete app with Id: " + id);  
      if (ans) {  
          this.userService.delete(id).subscribe((data) => {  
              this.GetAllApps();
          }, error => console.error(error))  
      }  
  }  

  GetAllApps()
  {
      this.userService.getAll()
         // .pipe(first())
          .subscribe((apps)=>{
            this.apps = apps
          },
          (error) => {     
           this.alertService.error(error);
          }
          );
  }

}
