import { Component, OnInit } from '@angular/core';
import { Task } from 'src/models/task-model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss'],
})
export class TaskListComponent implements OnInit {
  taskMdlLst: Task[] = [
    {
      id: 1,
      title: 'Task 1',
      details: 'Task 1 Description',
      progress_ratio: 75,
    },
  ];
  constructor() {}

  ngOnInit() {
    this.getTaskList();
  }

  getTaskList() {}
}
