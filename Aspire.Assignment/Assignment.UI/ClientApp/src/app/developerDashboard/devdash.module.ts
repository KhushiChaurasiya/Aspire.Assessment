import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { DevdashRoutingModule } from './devdash-routing.module';
import { ListComponent } from './list.component';
import { AddEditComponent } from './add-edit.component';
import { DevlayoutComponent } from './devlayout.component';



@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DevdashRoutingModule
],
declarations: [
    DevlayoutComponent,
    ListComponent,
     AddEditComponent
  ],
})
export class DevdashModule { }
