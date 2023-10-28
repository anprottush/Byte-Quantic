import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from './../../shared/models/ApiResponse';
@Injectable({
  providedIn: 'root',
})
export class CommonHttpService {
  private apiUrl = environment.apiUrl;
  private pageNo: number = 1;
  private pageSize: number = 100;

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Access-Control-Allow-Headers': 'Content-Type'
    }),
    params: new HttpParams()
    .set('pageNumber', this.pageNo)
    .set('pageSize', this.pageSize)
  };

  constructor(private http: HttpClient) { }

  // GET request
  get(endpoint: string, id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}${endpoint}/${id}`, this.httpOptions);
  }

  // GET request
  getWithPagination(endpoint: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}${endpoint}`, this.httpOptions);
  }

  //POST request
  post(endpoint: string, body: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}${endpoint}`, body, this.httpOptions);
  }

  //PUT request
  put(endpoint: string, body: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}${endpoint}`, body, this.httpOptions);
  }

  // DELETE request
  delete(endpoint: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}${endpoint}`, this.httpOptions);
  }

  // Image POST request
  postImage(endpoint: string, body: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'enctype': 'multipart/form-data' }) };
    return this.http.post<any>(`${this.apiUrl}${endpoint}`, body, httpOptions);
  }

  // Status PUT request
  putStatus(endpoint: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}${endpoint}`, this.httpOptions);
  }
}
