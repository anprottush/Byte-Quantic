import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Product } from './../../../shared/models/product/product';
// import { BaseService } from '../../../core/services/base.service';
import { ApiResponse } from '../../../shared/models/ApiResponse';
//import { ToastrService } from 'ngx-toastr';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ProductService } from './product.service';
import { takeUntil } from 'rxjs';
import { PageAbstractComponent } from 'src/app/generic/page-abstract/page-abstract.component';
import { GenericDialog, GenericDialogComponent } from 'src/app/generic/generic-dialog/generic-dialog.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent extends PageAbstractComponent implements OnInit, AfterViewInit, OnDestroy {
  public list: Product[] = [];
  public loading: boolean = false;
  public filter: string = '';

  public table: MatTableDataSource<Product> = new MatTableDataSource(this.list);

  columns: string[] = ['productName', 'description', 'imageUrl', 'actions', 'isActive'];

  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;

  constructor(
    private productService: ProductService,
    protected override router: Router,
    private dialog: MatDialog,
    private dialogComponent: GenericDialogComponent,
  ) {
    super(router);
  }

  ngOnInit(): void {
    // this.productService.loading
    //   .pipe(takeUntil(this.unsubscribe))
    //   .subscribe((loading: boolean) => this.loading = loading);
  }

  ngAfterViewInit(): void {
    this.table.sort = this.sort;
    this.table.paginator = this.paginator;

    // this.productService.data
    //   .pipe(takeUntil(this.unsubscribe))
    //   .subscribe((res: ApiResponse<any>) => {
    //     if (res.success) {
    //       this.list = res.payload?.data || [];
    //       this.table.data = this.list;
    //       this.dialogComponent.open(`Data retrieved successfully`, 'OK', 'snackbar');
    //       console.log(res.payload)
    //      }
    //      else{
    //       this.dialogComponent.open('Error occurred while fetching data', 'ERROR', 'snackbar');
    //      }
       
    //   });

  }

  public applyFilter(event: Event): void {
    const filterValue: string = (event.target as HTMLInputElement).value.toString();
    this.table.filter = filterValue.trim().toLowerCase();
    if (this.table.paginator) {
      this.table.paginator.firstPage();
    }
  }

  public async goToEditPage(item: Product): Promise<void> {
    //this.productService.setDataById = item;
    await this.router.navigateByUrl(`/product/product/${item.id}`);
  }

  public async goToCreatePage(): Promise<void> {
    //this.productService.setDataById = null;
    await this.router.navigateByUrl(`/product/product/create`);
  }

  public toogleActive(item: Product): void {
    let data: Product = { ...item, isActive: !item.isActive };
    console.log(data);
    this.productService.statusChange(item).subscribe((result: any) => {
      if (result.success) { 
        if(result.payload.isActive) {
          item.isActive = result.payload.isActive;
          console.log("active");
        }
        else {
          item.isActive = result.payload.isActive;
          console.log("inactive")
        }

        alert('Success! Status Changed');
        console.log(result.message);
        this.refreshComponent(`/product/product`) ;
        //this.navigate(`/product/brand`);
         
      }
      else { 
        alert('Failed! Status not changed');
        console.warn('Warning!');
        console.log(result.message);
      }
    }, (error: any) => {
      alert('Error! Server not found!');
      console.error('Http Error:', error);
    });
  }

  public deleteReason(item: Product):void {
    if (this.loading)
      return;

    // const dialog = this.dialog.open(GenericDialogComponent, <MatDialogConfig>{
    //   data: <GenericDialog>{
    //     title: 'Please Confirm',
    //     text: `Are you sure you want to delete - ${item.productName}`
    //   }
    // });

    // dialog.afterClosed().subscribe(result => {
    //   if (!result) return;
    //   this.productService.removeItem(item).subscribe();
    // });
    const confirmation = this.dialogComponent.openConfirmation(
      'Are you sure you want to delete this item?',
      'Confirm',
      'snackbar' // You can adjust the duration
    );
    confirmation.subscribe(() => {
      this.productService.removeItem(item).subscribe((result: any) => {
            if (result.success) { 
              this.dialogComponent.open('Data delete successfully', 'OK', 'snackbar');
            }
            else {
                this.dialogComponent.open('Cancel click', 'CANCEL', 'snackbar');
                // User canceled the action
              }
          });
      
      
    })



  }


  public uploadImage(event: Event, item: Product): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length >= 0) {
      const file = inputElement.files[0];
      const formData = new FormData();
      
      formData.append('files', file);
      this.productService.uploadImage(item,formData).subscribe((result: any) => {
        if (result.success) {  
          console.log(result);
          const listProduct=this.list.find(p=> p.id == item.id);
          if(listProduct){
            listProduct.imageUrl=result.payload[0].finalUrl;
            console.log(listProduct.imageUrl);
          }
          alert('Success! Image Uploaded');
          console.log(result.message);
          this.refreshComponent(`/product/product`) ;
        }
        else { 
          alert('Failed! Image Uploaded');
          console.warn('Warning!');
          console.log(result.message);
        }
      }, (error: any) => {
        alert('Error! Server not found!');
        console.error('Http Error:', error);
      });
      
    }
    else{

    }
    
    
  }
}

