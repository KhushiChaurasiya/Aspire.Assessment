import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout.component';
import { CommonModule } from '@angular/common';
import { UserdetailsComponent } from './userdetails.component';
import { RoleComponent } from './role.component';
import { ListComponent } from './list.component';
import { HomeComponent } from './home.component';

const routes: Routes = [{
  path: 'home', component: HomeComponent,
 
}, { path: 'userList', component: UserdetailsComponent},
{path:'userRole', component:RoleComponent },
{
  path:'appList', component:ListComponent
}]

@NgModule({
  imports: [CommonModule,RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
