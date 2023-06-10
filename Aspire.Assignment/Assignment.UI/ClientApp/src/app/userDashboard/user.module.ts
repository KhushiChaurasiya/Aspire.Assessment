import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserappdetailsComponent } from './userappdetails.component';
import { UserlayoutComponent } from './userlayout.component';
import { UserRoutingModule } from './user-routing.module';
import { SearchFilterPipe } from '../_pipes/search-filter.pipe';



@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    UserRoutingModule,
],
declarations: [
  UserappdetailsComponent,
  UserlayoutComponent,
  SearchFilterPipe,
],
})
export class UserModule { }
