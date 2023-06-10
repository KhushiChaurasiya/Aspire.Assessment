import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserappdetailsComponent } from './userappdetails.component';
import { UserlayoutComponent } from './userlayout.component';
import { HomeComponent } from './home.component';

const routes: Routes = [{
  path: 'home', component: HomeComponent},
  { path: 'userapp', component: UserappdetailsComponent }
  
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
