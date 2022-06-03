import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from './common.service';

@Injectable()
export class TasksService {
  public _baseUrl: string;

  constructor(private http: HttpClient, private commonSrv: CommonService) {
    this._baseUrl = this.commonSrv.apiUrl;
  }

  create(formData: any) {
    return this.http
      .post<any>(this._baseUrl + 'task-api/create', formData)
      .pipe();
  }

  Update(formData: any) {
    return this.http
      .put<any>(this._baseUrl + 'task-api/update', formData)
      .pipe();
  }

  Delete(id: any) {
    return this.http
      .delete<any>(this._baseUrl + 'task-api/delete/' + id)
      .pipe();
  }

  getGrid(dataTable: any) {
    console.log(this._baseUrl);
    return this.http.post<any>(this._baseUrl + 'task-api/get-grid', dataTable);
  }

  get() {
    console.log('URL', this._baseUrl + 'task-api/get');
    return this.http.get(this._baseUrl + 'task-api/get');
  }

  getById(id: number): Promise<any> {
    return this.http.get(this._baseUrl + 'task-api/get/' + id).toPromise();
  }
}
