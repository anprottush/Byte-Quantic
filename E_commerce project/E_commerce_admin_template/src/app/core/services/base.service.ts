import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { ApiUrlService } from './api-ulr.service';
import { CommonHttpService } from './common-http.service';
import { ApiResponse } from './../../shared/models/ApiResponse';
@Injectable({
  providedIn: 'root'
})
export class BaseService<T> {
  public loader = false;

  constructor(private commonHttp: CommonHttpService) { }

  // GET request
  get(endpoint: string, id: number): Observable<ApiResponse> {
    return this.commonHttp.get(endpoint, id);
  }

  // GET request
  getWithPagination(endpoint: string): Observable<ApiResponse> {
    return this.commonHttp.getWithPagination(endpoint);
  }

  // POST request
  post(endpoint: string, body: any): Observable<ApiResponse> {
    return this.commonHttp.post(endpoint, body);
  }

  // PUT request
  put(endpoint: string, body: any): Observable<ApiResponse> {
    return this.commonHttp.put(endpoint, body);
  }

  // DELETE request
  delete(endpoint: string, id: number): Observable<ApiResponse> {
    return this.commonHttp.delete(endpoint, id);
  }

  // Image POST request
  postImage(endpoint: string, id: number, body: any): Observable<ApiResponse> {
    return this.commonHttp.postImage(endpoint, id, body);
  }
}
