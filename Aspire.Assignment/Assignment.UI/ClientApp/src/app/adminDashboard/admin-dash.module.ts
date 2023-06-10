import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminLayoutComponent } from './admin-layout.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { UserdetailsComponent } from './userdetails.component';
import { RoleComponent } from './role.component';
import { ListComponent } from './list.component';
import { HomeComponent } from './home.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AdminRoutingModule
],
declarations: [
  HomeComponent,
  AdminLayoutComponent,
  UserdetailsComponent,
  RoleComponent,
  ListComponent
],
})
export class AdminDashModule { }
