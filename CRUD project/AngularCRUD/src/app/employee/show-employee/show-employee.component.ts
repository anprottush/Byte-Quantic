import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';

@Component({
  selector: 'app-show-employee',
  templateUrl: './show-employee.component.html',
  styleUrls: ['./show-employee.component.css']
})
export class ShowEmployeeComponent implements OnInit {

  constructor(private service: ApiserviceService) { }

  EmployeeList: any = [];
  ModalTitle = "";
  ActivateAddEditEmpComp: boolean = false;
  emp: any;

  ngOnInit(): void {
    this.EmpList();
  }

  addClick() {
    this.emp = {
      EmployeeId: "0",
      EmployeeName: "",
      DateOfJoining: "",
      EmployeePosition : "",
      EmployeeSalary : "",
      DepartmentId: "",
      
    }
    this.ModalTitle = "Add Employee";
    this.ActivateAddEditEmpComp = true;
  }

  editClick(item: any) {
    this.emp = item;
    this.ModalTitle = "Edit Employee";
    this.ActivateAddEditEmpComp = true;
  }

  deleteClick(item: any) {
    if (confirm('Are you sure??')) {
      this.service.deleteEmployee(item.id).subscribe(data => {
        alert(data.toString());
        this.EmpList();
      })
    }
  }

  closeClick() {
    this.ActivateAddEditEmpComp = false;
    this.EmpList();
  }

  EmpList() {
    this.service.getEmployeeList().subscribe(data => {
      this.EmployeeList = data;
      console.log('Data received:', data);
    },
    (err) => {
      console.error('Error fetching data:', err);
    }
    );
  }
}