import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-show-department',
  templateUrl: './show-department.component.html',
  styleUrls: ['./show-department.component.css']
})
export class ShowDepartmentComponent implements OnInit {

  constructor(private service: ApiserviceService, private router: Router) { }

  DepartmentList: any = [];
  ModalTitle = "";
  ActivateAddEditDepartComp: boolean = false;
  depart: any;
  id: any;

  

  ngOnInit() {
    this.DepList();
  }

  addClick() {
    this.depart = {
      DepartmentId: "",
      DepartmentName: "",
      DepartmentLocation: "",
      DepartmentDescription: ""

    }
    this.ModalTitle = "Create Department";
    this.ActivateAddEditDepartComp = true;
  }

  editClick=(item: any)=> {
    this.depart = item;
    //alert(item.id);
    this.id = item.id;
  
    this.ModalTitle = "Edit Department";
  
    this.ActivateAddEditDepartComp = true;
  }
  
  deleteClick(item: any) {
    if (confirm('Are you sure??')) {
      this.service.deleteDepartment(item.id).subscribe(data => {
        //alert(data.toString());
        alert("success");
        this.DepList();
      })
    }
  }

  closeClick() {
    this.ActivateAddEditDepartComp = false;
    this.DepList();
  }


  DepList() {
    this.service.getDepartmentList().subscribe((data) => {
        this.DepartmentList = data;
        console.log('Data received:', data);
      },
      (err) => {
        console.error('Error fetching data:', err);
      }
    );
  }
}
