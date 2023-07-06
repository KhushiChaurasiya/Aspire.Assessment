import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { User } from '../_models/user';
import { AlertService } from '../_services/alert.service';

@Component({
  selector: 'app-userdetails',
  templateUrl: './userdetails.component.html'
})
export class UserdetailsComponent implements OnInit {

  user?: User[];

  constructor( private userService: UserService, private alertService : AlertService ) { }

  ngOnInit() {
    this.userService.getAllUser()
    // .pipe(first())
     .subscribe({next:(user) => {
      this.user =user
      console.log(this.user)
    },error: (error: any) => {
      this.alertService.error(error);
    }
    });
  }

}
