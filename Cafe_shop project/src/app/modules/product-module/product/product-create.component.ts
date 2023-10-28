import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from "@angular/router";
import { FormControl, FormGroup, FormGroupDirective, Validators } from "@angular/forms";
import { map, startWith, take, takeUntil } from "rxjs/operators";

//import { BrandComponent } from './../brand/brand.component';
import { ProductService } from './product.service';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject } from 'rxjs';
import { PageAbstractComponent } from 'src/app/generic/page-abstract/page-abstract.component';

import { Product } from './../../../shared/models/product/product';
import { MatSnackBar, MatSnackBarRef, MatSnackBarModule } from '@angular/material/snack-bar';

import { ApiResponse } from 'src/app/shared/models/ApiResponse';
//import { Observable } from 'rxjs';
import { GenericDialogComponent } from 'src/app/generic/generic-dialog/generic-dialog.component';


@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductCreateComponent extends PageAbstractComponent implements OnInit, AfterViewInit, OnDestroy {
  public id: number = 0;
  //public user: any = {};
  public loading: boolean = false;
  public createAnother: boolean = false;
  public categoryList: any;
  public brandList: any;
  public filter: string = '';

  public categories: any[] = [];
  public brands: any[] = [];
  
  

 
  
  @ViewChild('singleSelect', { static: true }) singleSelect!: MatSelect;
  @ViewChild('detailsForm')
  public detailsForm!: FormGroupDirective;

  public form: FormGroup = new FormGroup({
    
    categoryId: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
     brandId: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    productName: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    description: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    unitType: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    lengthUnitType: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    volumeUnitType: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    weightUnitType: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
    sku: new FormControl({ value: '', disabled: this.loading }, [Validators.required]),
  });
  

  



  constructor(
    private route: ActivatedRoute,
    protected override router: Router,
    public productService: ProductService,
    private dialogComponent: GenericDialogComponent,
    
  ) { super(router); 

    
          
  }


  


  public ngOnInit(): void {

   
    this.categoryLoadData();
    this.brandLoadData();


    this.route.params
      .pipe(takeUntil(this.unsubscribe))
      .subscribe((params: Params) => {
        this.id = Number(params['id']) || 0;
        this.productService.idOnView = this.id;
      });

    // this.productService.loading
    //   .pipe(takeUntil(this.unsubscribe))
    //   .subscribe((loading: boolean) => {
    //     this.loading = loading;
    //     if (this.loading) { this.form.disable(); } else { this.form.enable(); }
    //   });

    // this.productService.dataById
    //   .pipe(takeUntil(this.unsubscribe))
    //   .subscribe((product: Product | null) => {
    //     if (!product) {
    //       return;
    //     }
    //     this.form.setValue({
    //       categoryId: product.categoryId,
    //       brandId: product.brandId,
    //       productName: product.productName,
    //       description: product.description,
    //       unitType: product.unitType,
    //       lengthUnitType: product.lengthUnitType,
    //       volumeUnitType: product.volumeUnitType,
    //       weightUnitType: product.weightUnitType,
    //       sku: product.sku
    //     });
    //   });
  }

  ngAfterViewInit() {
    
  }

  

  public willCreateAnother($event: Event): void {
    this.createAnother = true;
    this.detailsForm.onSubmit($event);
  }

  public validateForm(form: FormGroup) {
    if (form.invalid) { return; }

    let data: Product = this.form.getRawValue();
    data = this.productService.idOnView ? { ...data, id: this.productService.idOnView } : data;

    if (this.productService.idOnView) {
      this.productService.updateItem(data)
        .subscribe(() => {
          this.dialogComponent.open(`Data updated successfully`, 'OK', 'snackbar');
          this.navigate('/product/product');
        }, (error: any) => {
          this.navigate(`/product/product/${data.id}`);
        });
    } else {
      this.productService.createItem(data)
        .subscribe(() => {
          if (this.createAnother) { 
            this.refreshComponent('/product/product/create');
            this.dialogComponent.open(`Data created successfully`, 'OK', 'snackbar');
            this.navigate('/product/product');  
          }
          else { 
            this.dialogComponent.open(`Data created successfully`, 'OK', 'snackbar');
            this.navigate('/product/product'); 
          }
        }, (error: any) => {
          this.navigate('/product/product/create');
        });
    }
  }



public categoryLoadData(){
  
}

public brandLoadData(){
  
}


}
