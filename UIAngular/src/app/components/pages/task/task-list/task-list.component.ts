import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Tasks } from 'src/models/task-model';
import { TasksService } from 'src/services/tasks.service';
import { ToastService } from 'src/services/toast.service';
import { DeleteDialogComponent } from '../../common-pages/delete-dialog/delete-dialog.component';
import { TaskAddEditComponent } from '../task-add-edit/task-add-edit.component';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss'],
})
export class TaskListComponent implements OnInit {
  taskMdlLst: Tasks[] = [];

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
  }

  getTaskGrid() {
    let postData = {
      columns: [],
      orders: [this.orderColumn],
      start: (this.page - 1) * this.rowSize,
      length: this.rowSize.toString(),
      search: {},
      searches: [
        // { search_by: 'id', value: id.current },
        // { search_by: 'pdrNumber', value: pdrNumber.current },
        // { search_by: 'workOrder', value: workOrder.current },
        // { search_by: 'location', value: location.current },
        // { search_by: 'qcInspector', value: qcInspector.current },
        // { search_by: 'annex', value: annex.current },
        // { search_by: 'specItem', value: specItem.current },
        // { search_by: 'title', value: title.current },
        // { search_by: 'dateCompleted', value: dateCompleted.current },
        // { search_by: 'unsatFindings', value: unsatFindings.current },
        // { search_by: 'fmName', value: fmName.current },
        // { search_by: 'fmTitle', value: fmTitle.current },
        // { search_by: 'dateIssued', value: dateIssued.current },
        // { search_by: 'dateDue', value: dateDue.current },
        // { search_by: 'status', value: status.current },
      ],
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
        this.getTaskGrid();
      },
      (reason) => {
        console.log('Dismissed');
      }
    );
  }

  onEditClick(item: Tasks) {
    const modalRef = this.modalService.open(TaskAddEditComponent);
    modalRef.componentInstance.title = 'Update Task';
    modalRef.componentInstance.taskMdl = item;
    modalRef.result.then(
      (result) => {
        this.getTaskGrid();
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
              this.getTaskGrid();
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
}
