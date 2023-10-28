import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonHttpService } from './common-http.service';

@Injectable({
  providedIn: 'root'
})
export class BaseService {
  public loader = false;

  constructor(private commonHttp: CommonHttpService) { }

  // GET request
  getById(endpoint: string, id: number): Observable<any> {
    return this.commonHttp.get(endpoint, id);
  }

  // GET request
  getAll(endpoint: string, id: number): Observable<any> {
    return this.commonHttp.get(endpoint, id);
  }

  // GET request
  getWithPagination(endpoint: string): Observable<any> {
    return this.commonHttp.getWithPagination(endpoint);
  }

  // POST request
  post(endpoint: string, body: any): Observable<any> {
    return this.commonHttp.post(endpoint, body);
  }

  // PUT request
  put(endpoint: string, body: any): Observable<any> {
    return this.commonHttp.put(endpoint, body);
  }

  // DELETE request
  delete(endpoint: string): Observable<any> {
    return this.commonHttp.delete(endpoint);
  }

  // Image POST request
  postImage(endpoint: string, body: any): Observable<any> {
    return this.commonHttp.postImage(endpoint, body);
  }

  // Status PUT request
  putStatus(endpoint: string): Observable<any> {
    return this.commonHttp.putStatus(endpoint);
  }
}
