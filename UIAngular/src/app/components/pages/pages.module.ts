import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesComponent } from './pages.component';
import { TaskListComponent } from './task/task-list/task-list.component';
import { TaskAddEditComponent } from './task/task-add-edit/task-add-edit.component';
import { PagesRoutingModule } from './pages-routing.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [CommonModule, NgbModule, PagesRoutingModule],
  declarations: [PagesComponent, TaskListComponent, TaskAddEditComponent],
})
export class PagesModule {}
