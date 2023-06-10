import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevlayoutComponent } from './devlayout.component';
import { RouterModule, Routes } from '@angular/router';
import { ListComponent } from './list.component';
import { AddEditComponent } from './add-edit.component';
import { HomeComponent } from './home.component';

const routes: Routes = [
{path: 'home', component: HomeComponent},
{ path: 'appsdetails', component: ListComponent },
{ path: 'add', component: AddEditComponent },
{ path: 'edit/:id', component: AddEditComponent }]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DevdashRoutingModule { }
