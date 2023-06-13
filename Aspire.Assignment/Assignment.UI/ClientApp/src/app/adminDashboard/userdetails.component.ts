import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-userdetails',
  templateUrl: './userdetails.component.html'
})
export class UserdetailsComponent implements OnInit {

  user?: User[];

  constructor( private userService: UserService ) { }

  ngOnInit() {
    this.userService.getAllUser()
    // .pipe(first())
     .subscribe((user) => {
      this.user =user
      console.log(this.user)
    });
  }

}
