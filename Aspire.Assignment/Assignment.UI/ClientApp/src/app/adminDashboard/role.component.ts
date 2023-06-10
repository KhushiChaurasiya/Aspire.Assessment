import { Component, OnInit } from '@angular/core';
import { Role } from '../_models/role';
import { RoleService } from '../_services/role.service';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html'
})
export class RoleComponent implements OnInit {
  role?: Role[];

  constructor( private roleService: RoleService ) { }

  ngOnInit() {
    this.roleService.getAll()
    // .pipe(first())
     .subscribe((role) => {
      this.role =role});
  }
}
