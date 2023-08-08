import { Component, OnInit, Input } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-add-edit-department',
  templateUrl: './add-edit-department.component.html',
  styleUrls: ['./add-edit-department.component.css']
})
export class AddEditDepartmentComponent implements OnInit {

  constructor(private service: ApiserviceService, private route: ActivatedRoute) { }

  @Input() depart: any;
  DepartmentId = "";
  DepartmentName = "";
  DepartmentLocation = "";
  DepartmentDescription = "";


  ngOnInit() {
       this.DepartmentId = this.depart.DepartmentId;
    this.DepartmentName = this.depart.DepartmentName;
    this.DepartmentLocation = this.depart.DepartmentLocation;
    this.DepartmentDescription = this.depart.DepartmentDescription;
    
    // this.route.params.subscribe(params => {
    //   //this.DepartmentId = +params['id'];
    //   alert(this.DepartmentId);
    // });
    
    
    

  }

  createDepartment() {
    var dept = {
      Name: this.DepartmentName,
      Location: this.DepartmentLocation,
      Description: this.DepartmentDescription
    };
    
    this.service.createDepartment(dept).subscribe((res) => {
      
      console.log('Department created successfully!');
      alert(res.toString())
    
    },(error) => {
      console.error('Error creating department:', error);
      alert('An error occurred while creating the department. Please try again.');
    }
    
    );
  }

  updateDepartment() {
    var dept = {
      Id: this.DepartmentId,
      Name: this.DepartmentName,
      Location: this.DepartmentLocation,
      Description: this.DepartmentDescription
    };
    this.service.updateDepartment(dept).subscribe((res) => {
      //alert()
      console.log('Department update successfully!');
      alert(res.toString())
    
    },(error) => {
      console.error('Error updating department:', error);
      alert('An error occurred while updating the department. Please try again.');
    }
    
    );
  }
}