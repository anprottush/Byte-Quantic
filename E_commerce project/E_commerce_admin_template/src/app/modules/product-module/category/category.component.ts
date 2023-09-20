import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../../../shared/models/product/category';
import { BaseService } from '../../../core/services/base.service';
import { ApiResponse } from '../../../shared/models/ApiResponse';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';


@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  public categoryList: Category[] = [];
  myForm: FormGroup;
  public loading = false;
  private endpoint = 'category';
  displayedColumns: string[] = ['id', 'categoryName', 'motherCategoryId', 'description', 'imageUrl', 'action'];
  dataSource : any;
  ActivateAddEditForm: boolean = true;
  public editData: any;
  

  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;

  categoryName:string='';
  motherCategoryId:number=0;
  description:string='';
  imageUrl:string='';
  // selectedFile: File | null = null;
  // newCategoryId: number | null = null;

  constructor(
    private baseService: BaseService<Category>,
    private toastr: ToastrService,
    private elementRef: ElementRef,
    private router: Router,
    private confirmDialogService: ConfirmDialogService,
    private fb: FormBuilder,
    ) {
      this.myForm = this.fb.group({

        categoryName: ['', Validators.required],
        motherCategoryId: [0],
        imageUrl: ['', Validators.required],
        description: ['', Validators.required],
        isActive: [true]
      });
      
     }

  ngOnInit(): void {
    this.loadCategorydata();
  }



  loadCategorydata()
  {
    //this.loading = true;
    this.baseService.getWithPagination(this.endpoint).subscribe((res: ApiResponse) => {
      //this.loading = false;
      console.log(res);
      if (res.success) {
        this.categoryList = res.payload.data;
        this.dataSource = new MatTableDataSource<Category>(this.categoryList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        
      } else {
        this.categoryList = [];
        this.toastr.warning('Warning!', res.message[0]);
      }
    }, (error: HttpErrorResponse) => {
      this.loading = false;
      this.toastr.error('Error!', 'Server not found!');
    });


  }

  addClick(){
    this.ActivateAddEditForm=true;
  }
    
  editClick(category:any)
  {
    this.ActivateAddEditForm=false;
    this.editData = category;
    this.categoryName=this.editData.categoryName;
    this.motherCategoryId= this.editData.motherCategoryId;
    this.description= this.editData.description;
    this.imageUrl=this.editData.imageUrl;
    
    
  }

  showDialog(category: Category) {
    this.confirmDialogService.confirmThis(
      `Are you sure to delete ${category.categoryName}?`,
      () => {
        
        this.baseService.delete(this.endpoint,category.id).subscribe(
          (res: ApiResponse) => {
            if (res.success) {
              
              this.toastr.success('Success!', 'Category deleted successfully');
             
              const index = this.categoryList.indexOf(category);
              if (index !== -1) {
                this.categoryList.splice(index, 1);
                
                this.dataSource.data = this.categoryList;
              }
            } else {
              this.toastr.warning('Warning!', res.message[0]);
            }
          },
          (error) => {
            this.toastr.error('Error!', 'Server not found!');
          }
        );
      },
      () => {
        // User clicked "No" to cancel deletion
        // You can optionally add code here or leave it empty
      }
    );
  }
  
  

  Filterchange(event: Event) {
    const filvalue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filvalue;
  }

  // onFileSelected(event: Event) {
  //   const inputElement = event.target as HTMLInputElement;
  //   if (inputElement.files && inputElement.files.length > 0) {
  //     this.selectedFile = inputElement.files[0];
  //   }
  // }



  DataSave(form: FormGroup):void {

    // var addCategorydata = {
      
    //   CategoryName: this.myForm.value.categoryName,
    //   MotherCategoryId: this.myForm.value.motherCategoryId,
    //   Description:this.myForm.value.description,
    //   ImageUrl: this.myForm.value.imageUrl

    // };

    // console.log('Valid?', this.myForm.valid); // true or false
    // console.log('Name', addCategorydata.CategoryName);
    // console.log('Mother Category Id', addCategorydata.MotherCategoryId);
    // console.log('Description', addCategorydata.Description);
    // console.log('Image', addCategorydata.ImageUrl);


    if(this.myForm.valid)
    {
      if (this.ActivateAddEditForm)
      {
        let data : any = this.myForm.getRawValue();
        debugger;
        console.log(data);
        console.log(this.endpoint);
        this.baseService.post(this.endpoint,data).subscribe((res: ApiResponse) => {
          if (res.success) {

            console.log('Response from API:', res);
            this.toastr.success('Success! Category Created successfully', res.message[0]);
            this.ActivateAddEditForm=false;
            this.router.navigate(['./product/category']);
            this.dataSource.data = this.categoryList;   
    
          } else {
            //this.brandForm = [];
            this.toastr.warning('Warning!', res.message[0]);
          }
        }, (error: HttpErrorResponse) => {
          this.loading = false;
          this.toastr.error('Error!', 'Server not found!');
          console.error('API Error:', error);
        });
      }
      else
      {
        var editCategorydata = {
          Id: this.editData.id,
          CategoryName: this.myForm.value.categoryName,
          MotherCategoryId: this.myForm.value.motherCategoryId,
          Description:this.myForm.value.description,
          ImageUrl: this.myForm.value.imageUrl
    
        };
        
        this.baseService.put(this.endpoint,editCategorydata).subscribe((res: ApiResponse) => {
          //this.loading = false;
          if (res.success) {
            this.toastr.success('Success! Category Updated successfully', res.message[0]);
            this.ActivateAddEditForm=false;
            this.router.navigate(['./product/category']);
            this.dataSource.data=this.categoryList;

    
          } else {
            this.toastr.warning('Warning!', res.message[0]);
          }
        }, (error: HttpErrorResponse) => {
          this.loading = false;
          this.toastr.error('Error!', 'Server not found!');
        });
      
      
    }
  }
    else
    {
      this.loading = false;
      this.toastr.error('Error!', 'Invalid Data!');
    }
    
  }


//   // Function to upload the image
// uploadImage(categoryId: number, description: string, file: File) {
//   // Check if an image file is provided
//   if (!file) {
//     return;
//   }

//   // Create a new FormData object
//   const formData = new FormData();

//   // Append the category ID and description to the FormData
//   formData.append('categoryId', categoryId.toString());
//   formData.append('description', description);

//   // Append the image file to the FormData
//   formData.append('image', file, file.name);

//   // Implement your actual image upload logic using HttpClient
//   this.baseService.post('your-image-upload-endpoint', formData).subscribe(
//     (res: ApiResponse) => {
//       if (res.success) {
//         // Image uploaded successfully
//         this.toastr.success('Success!', 'Image uploaded successfully');
//         // You can perform any additional actions here upon successful image upload
//       } else {
//         this.toastr.warning('Warning!', 'Image upload failed');
//       }
//     },
//     (error: HttpErrorResponse) => {
//       this.toastr.error('Error!', 'Server not found or image upload failed');
//     }
//   );
// }



// onSubmit(): void {
//   console.log('Valid?', this.myForm.valid);

//   if (this.myForm.valid) {
//     if (this.ActivateAddEditForm) {
//       // Create the category without the image
//       const categoryData = {
//         CategoryName: this.myForm.value.categoryName,
//         MotherCategoryId: this.myForm.value.motherCategoryId,
//         Description: this.myForm.value.description,
//       };

//       this.baseService.post(this.endpoint, categoryData).subscribe(
//         (res: ApiResponse) => {
//           if (res.success) {
//             this.newCategoryId = res.payload.id;

//             // Check if an image is selected
//             if (this.selectedFile) {
//               if (this.newCategoryId !== null && this.newCategoryId !== undefined) {
              // Upload the image if available
            //   this.uploadImage(this.newCategoryId, 'Image Description', this.selectedFile);
            // } else {
            //   // Handle the case where this.newCategoryId is null or undefined
            //   console.error('this.newCategoryId is null or undefined');
            //   // You can show an error message or perform other actions as needed
            // }
//             } else {
//               // No image selected, just display success message
//               this.toastr.success('Success! Category Created successfully', res.message[0]);
//               this.ActivateAddEditForm = false;
//               this.router.navigate(['./product/category']);
//               this.dataSource.data = this.categoryList;
//             }
//           } else {
//             this.toastr.warning('Warning!', res.message[0]);
//           }
//         },
//         (error: HttpErrorResponse) => {
//           this.loading = false;
//           this.toastr.error('Error!', 'Server not found!');
//           console.error('API Error:', error);
//         }
//       );
//     } else {
//       // Your edit logic here
//     }
//   } else {
//     this.loading = false;
//     this.toastr.error('Error!', 'Invalid Data!');
//   }
// }


}
