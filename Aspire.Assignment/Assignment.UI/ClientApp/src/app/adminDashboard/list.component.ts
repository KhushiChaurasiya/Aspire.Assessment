import { Component, OnInit } from '@angular/core';
import { App } from '../_models/app';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html'
})
export class ListComponent implements OnInit {
  apps?: App[];

  constructor(private userService: UserService) {}

  ngOnInit() {
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
    this.userService.getAll().subscribe((data) => {  
        console.log(data);
        this.apps = data;
    }, error => console.error(error))  
    // this.userService.getAll().subscribe(app => this.apps = app);
    // console.log(this.apps);
  }

}
