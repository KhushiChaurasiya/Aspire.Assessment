import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-devlayout',
  templateUrl: './devlayout.component.html'
})
export class DevlayoutComponent implements OnInit {

  constructor(private accountService : AccountService) { }

  ngOnInit(): void {
  }

  logout() {
    this.accountService.logout();
}

}
