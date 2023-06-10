import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html'
})
export class AdminLayoutComponent implements OnInit {
  constructor(private accountService :AccountService) { }

  ngOnInit() {
  }
  
  logout() {
    this.accountService.logout();
}

}
