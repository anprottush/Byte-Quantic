import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiserviceService {
  readonly apiUrl = 'https://localhost:7041/api/';

  constructor(private http: HttpClient) { }

  // Department
  getDepartmentList(){
    return this.http.get(this.apiUrl + 'department/all');
  }

createDepartment(dept: any){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'}) };
    return this.http.post(this.apiUrl + 'department/create', dept,httpOptions);
  }
  
  updateDepartment(dept: any) {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'}) };
    return this.http.put(this.apiUrl + 'department/update/', dept, httpOptions);
  }

  deleteDepartment(deptId: number) {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'}) };
    return this.http.delete(this.apiUrl + 'department/delete/' + deptId, httpOptions);
  }

  // Employee
  getEmployeeList() {
    return this.http.get(this.apiUrl + 'employee/all');
  }

  addEmployee(emp: any){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'}) };
    return this.http.post(this.apiUrl + 'employee/create', emp, httpOptions);
  }

  updateEmployee(emp: any) {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'}) };
    return this.http.put(this.apiUrl + 'employee/update/', emp, httpOptions);
  }

  deleteEmployee(empId: number) {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'}) };
    return this.http.delete(this.apiUrl + 'employee/delete/' + empId, httpOptions);
  }

}