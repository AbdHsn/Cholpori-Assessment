import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Tasks, TaskStatus } from 'src/models/task-model';
import { TasksService } from 'src/services/tasks.service';
import { ToastService } from 'src/services/toast.service';
import { DeleteDialogComponent } from '../../common-pages/delete-dialog/delete-dialog.component';
import { TaskAddEditComponent } from '../task-add-edit/task-add-edit.component';
import * as signalR from '@microsoft/signalr';
import { SignalRResponse } from 'src/models/signal-r-response';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss'],
})
export class TaskListComponent implements OnInit {
  taskMdlLst: Tasks[] = [];

  searchByTitle: string = '';

  orderColumn = {
    column: 'id',
    order_by: 'DESC',
  };

  page = 1;
  rowSize = 10;
  totalRecord = 0;
  rowSizeOption = [10, 20, 50, 100, 200];

  constructor(
    private _taskSrv: TasksService,
    private modalService: NgbModal,
    private _toastSrv: ToastService
  ) {}

  ngOnInit() {
    this.getTaskGrid();

    this.initializeSignalR();
  }

  getTaskGrid() {
    let postData = {
      columns: [],
      orders: [this.orderColumn],
      start: (this.page - 1) * this.rowSize,
      length: this.rowSize.toString(),
      search: {},
      searches: [{ search_by: 'title', value: this.searchByTitle }],
    };

    this._taskSrv.getGrid(postData).subscribe((res) => {
      this.taskMdlLst = res.data as Tasks[];
      this.totalRecord = res.totalRecords as number;
    });
  }

  onRowSizeOptionChange(selectedSizeOption: any): void {
    this.rowSize = selectedSizeOption.target.value;
    this.page = 1;
    this.getTaskGrid();
  }

  onPaginationChange(pageNumber: any) {
    console.log('pagination changed: ', pageNumber);
    this.page = pageNumber;
    this.getTaskGrid();
  }

  onOrderByClick(columnName: string) {
    this.orderColumn.order_by =
      this.orderColumn.order_by == 'DESC' ? 'ASC' : 'DESC';

    this.orderColumn.column = columnName;

    this.getTaskGrid();
  }

  onCreateTaskClick() {
    const modalRef = this.modalService.open(TaskAddEditComponent);
    modalRef.componentInstance.title = 'Create Task';
    modalRef.componentInstance.taskMdl = {};
    modalRef.result.then(
      (result) => {
        //this.getTaskGrid();
      },
      (reason) => {
        console.log('Dismissed');
      }
    );
  }

  onEditClick(item: Tasks) {
    const modalRef = this.modalService.open(TaskAddEditComponent);
    modalRef.componentInstance.title = 'Update Task';

    modalRef.componentInstance.taskMdl = Object.assign({}, item);
    modalRef.result.then(
      (result) => {
        // this.getTaskGrid();
      },
      (reason) => {
        console.log('Dismissed');
      }
    );
  }

  onDeleteClick(item: Tasks) {
    const modalRef = this.modalService.open(DeleteDialogComponent, {
      centered: true,
    });
    modalRef.componentInstance.title = `Task "${item.title}"`;
    modalRef.result.then((result) => {
      if (result as boolean) {
        this._taskSrv.Delete(item.id).subscribe(
          (res) => {
            if (res as boolean) {
              this._toastSrv.show(`Task ${item.title} successfully deleted.`, {
                classname: 'bg-success text-light',
                delay: 10000,
              });
              //this.getTaskGrid();
            }
          },
          (error: HttpErrorResponse) => {
            console.log('eeeee', error.error);
            this._toastSrv.show(error.error, {
              classname: 'bg-danger text-light',
              delay: 10000,
            });
          }
        );
      }
    });
  }

  onSearchByTitle(event: any) {
    if (event.key === 'Enter') {
      this.getTaskGrid();
    }
  }

  initializeSignalR() {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(this._taskSrv._baseUrl + 'broadcast-message')
      .build();

    connection
      .start()
      .then(function () {
        console.log('SignalR Connected!');
      })
      .catch(function (err) {
        return console.error(err.toString());
      });

    connection.on('BroadcastMessage', (result) => {
      let getTopic = JSON.parse(result as string) as SignalRResponse;
      let latestTask = getTopic.data as Tasks;

      console.log('got topics:', getTopic, latestTask);

      switch (getTopic.topic) {
        case 'Task-Created':
          this.taskMdlLst.push(latestTask);
          break;
        case 'Task-Updated': {
          let getExistedTask = this.taskMdlLst.find(
            (f) => f.id == latestTask.id
          );
          if (getExistedTask) {
            getExistedTask.title = latestTask.title;
            getExistedTask.details = latestTask.details;
            getExistedTask.progress_ratio = latestTask.progress_ratio;
            getExistedTask.status = latestTask.status;
          }
          break;
        }
        case 'Task-Deleted': {
          let getExistedTask = this.taskMdlLst.find(
            (f) => f.id == latestTask.id
          );
          if (getExistedTask) {
            this.taskMdlLst = this.taskMdlLst.filter(
              (f) => f.id != latestTask.id
            );
          }
          break;
        }
        default:
          break;
      }
    });
  }
}
