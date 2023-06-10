import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-userlayout',
  templateUrl: './userlayout.component.html'
})
export class UserlayoutComponent implements OnInit {

  constructor(private accountService :AccountService) { }

  ngOnInit(){
    
  }
  
  logout() {
    this.accountService.logout();
}
}
