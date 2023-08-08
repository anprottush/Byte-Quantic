import { Component, Input, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {

  constructor(private service: ApiserviceService) { }
  @Input() emp: any;
  EmployeeId = "";
  EmployeeName = "";
  DateOfJoining = "";
  EmployeePosition = "";
  EmployeeSalary = "";
  DepartmentId = "";


  ngOnInit() {
    this.EmployeeId = this.emp.EmployeeId;
    this.EmployeeName = this.emp.EmployeeName;
    this.DateOfJoining = this.emp.DateOfJoining;
    this.EmployeePosition = this.emp.EmployeePosition;
    this.EmployeeSalary = this.emp.EmployeeSalary;
    this.DepartmentId = this.emp.DepartmentId;
    // this.loadEmployeeList();
  }

  createEmployee() {
    var emp = {
      Name: this.EmployeeName,
      JoiningDate: this.DateOfJoining,
      Position: this.EmployeePosition,
      Salary: this.EmployeeSalary,
      DepartmentId: this.DepartmentId
    };

    this.service.addEmployee(emp).subscribe(res => {
      console.log('Employee created successfully!');
      alert(res.toString())
    
    },(error) => {
      console.error('Error creating employee:', error);
      alert('An error occurred while creating the employee. Please try again.');
    }
    );
  }

  updateEmployee() {
    var emp = {
      Id: this.EmployeeId,
      Name: this.EmployeeName,
      JoiningDate: this.DateOfJoining,
      Position: this.EmployeePosition,
      Salary: this.EmployeeSalary,
      DepartmentId: this.DepartmentId
    };

    this.service.updateEmployee(emp).subscribe(res => {
      console.log('Employee update successfully!');
      alert(res.toString())
    
    },(error) => {
      console.error('Error updating employee:', error);
      alert('An error occurred while updating the employee. Please try again.');
    }
    );
  }
}