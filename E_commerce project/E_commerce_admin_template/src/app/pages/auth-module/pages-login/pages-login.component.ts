import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { Login } from '../../../shared/models/authenticate/login';
import { BaseService } from '../../../core/services/base.service';
import { ApiResponse } from '../../../shared/models/ApiResponse';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-pages-login',
  templateUrl: './pages-login.component.html',
  styleUrls: ['./pages-login.component.css']
})
export class PagesLoginComponent implements OnInit {
  public loginForm: FormGroup;
  private endpoint = 'authenticate/login';

  constructor(private baseService: BaseService<Login>,
    private toastr: ToastrService,) {

    this.loginForm = new FormGroup({
      username: new FormControl('', Validators.required), 
      password: new FormControl('', Validators.required), 
     
    });
   }

  ngOnInit(): void {
    
  }
  onSubmit(loginform:any)
  {
    alert(this.loginForm.value.username);

    var loginData = {
      username: this.loginForm.value.username,
      password: this.loginForm.value.password
      

    };

    console.log('Valid?', this.loginForm.valid); // true or false
    console.log('User Name', loginData.username);
    console.log('Password', loginData.password);
    
    // if(this.loginForm.valid)
    // {
    //   this.baseService.post(this.endpoint,loginData).subscribe((res: ApiResponse) => {
    //     if (res.success) {
    //       //this.loading = true;
    //       this.toastr.success('Success! Login', res.message[0]);
    //       //this.router.navigate(['./product/brand']);
    //       } else {
    //         this.toastr.warning('Warning!', res.message[0]);
    //       }
    //     }, (error: HttpErrorResponse) => {
    //       //this.loading = false;
    //       console.error('Http Error:', error);
    //       this.toastr.error('Error!', 'Server not found!');
    //     });
    // }
    // else
    // {
    //   //this.loading = false;
    //   this.toastr.error('Error!', 'Invalid Data!');
    // }
  }
}
