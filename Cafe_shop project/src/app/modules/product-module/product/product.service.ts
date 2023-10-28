import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from '../../../core/services/base.service';
import { ApiResponse, Payload } from '../../../shared/models/ApiResponse';

import { Product } from './../../../shared/models/product/product';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  // private _categorydata: BehaviorSubject<ApiResponse<Payload<Category[]>>> = new BehaviorSubject<ApiResponse<Payload<Category[]>>>({});
  // public categorydata: BehaviorSubject<ApiResponse<Payload<Category[]>>> = this._categorydata;
  // private get getCategoryData(): ApiResponse<Payload<Category[]>> { return this._categorydata.getValue(); }
  // private set setCategoryData(category: ApiResponse<Payload<Category[]>>) { this._categorydata.next(category); }

  // private _branddata: BehaviorSubject<ApiResponse<Payload<Brand[]>>> = new BehaviorSubject<ApiResponse<Payload<Brand[]>>>({});
  // public branddata: BehaviorSubject<ApiResponse<Payload<Brand[]>>> = this._branddata;
  // private get getBrandData(): ApiResponse<Payload<Brand[]>> { return this._branddata.getValue(); }
  // private set setBrandData(brand: ApiResponse<Payload<Brand[]>>) { this._branddata.next(brand); }

  // private _data: BehaviorSubject<ApiResponse<Payload<Product[]>>> = new BehaviorSubject<ApiResponse<Payload<Product[]>>>({});
  // public data: BehaviorSubject<ApiResponse<Payload<Product[]>>> = this._data;
  // private get getData(): ApiResponse<Payload<Product[]>> { return this._data.getValue(); }
  // private set setData(product: ApiResponse<Payload<Product[]>>) { this._data.next(product); }

  // private _loading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  // public loading: BehaviorSubject<boolean> = this._loading;
  // private get getLoading(): boolean { return this._loading.getValue(); }
  // private set setLoading(loading: boolean) { this._loading.next(loading); }

  // private _dataById: BehaviorSubject<Product | null> = new BehaviorSubject<Product | null>(null);
  // public dataById: BehaviorSubject<Product | null> = this._dataById;
  // public get getDataById(): Product | null { return this._dataById.getValue(); }
  // public set setDataById(product: Product | null) { this._dataById.next(product); }

  public idOnView: number = 0;
  private endpoint = 'product';
  private all_endpoint = `${this.endpoint}/all`;
  private image_endpoint = `${this.endpoint}/images`;
  private status_endpoint = `${this.endpoint}/status`;
  private categoryList_endpoint = 'category/all';
  private brandList_endpoint = 'brand/all';
  constructor(private http: HttpClient,
    private baseService: BaseService) {
    this.getListFromServer();
    this.getCategoryListFromServer();
    this.getBrandListFromServer();
  }

  public getListFromServer(): void {
    // this.setLoading = true;
    // this.baseService.getWithPagination(this.all_endpoint)
    //   .subscribe((res: ApiResponse<Payload<Product[]>>) => {
    //     this.setLoading = false;
    //     this.setData = res;
    //   }, (error: [ApiResponse<Product>]) => {
    //     this.setLoading = false;
    //   });
  }

  public getCategoryListFromServer(): void {
  //   this.setLoading = true;
  //  this.baseService.getWithPagination(this.categoryList_endpoint)
  //     .subscribe((res: ApiResponse<Payload<Category[]>>) => {
  //       this.setLoading = false;
  //       this.setCategoryData = res;
  //     }, (error: [ApiResponse<Category>]) => {
  //       this.setLoading = false;
  //     });
  }

  public getBrandListFromServer(): void {
  //   this.setLoading = true;
  //  this.baseService.getWithPagination(this.brandList_endpoint)
  //     .subscribe((res: ApiResponse<Payload<Brand[]>>) => {
  //       this.setLoading = false;
  //       this.setBrandData = res;
  //     }, (error: [ApiResponse<Brand>]) => {
  //       this.setLoading = false;
  //     });
  }

  public createItem(model: Product): Observable<any> {
    // this.setLoading = true;
    // let list: ApiResponse<Payload<Product[]>> = this.getData;
    // let oldList: ApiResponse<Payload<Product[]>> = JSON.parse(JSON.stringify(this.getData));

    // list.payload!.data!.unshift(model);
    // this.setData = list;

    return this.baseService.post(this.endpoint, model)
      .pipe(
        tap((response: ApiResponse<Product>) => {
          // list.payload!.data![0] = response.payload!;
          // this.setDataById = null;
          // this.setLoading = false;
        }),
        catchError((error: HttpErrorResponse) => {
          // this.setDataById = model;
          // this.setData = oldList;
          // this.setLoading = false;
          return throwError(() => error);
        })
      );
  }

  public updateItem(updatedData: Product): Observable<any> {
    // this.setLoading = true;
   
    // let list: ApiResponse<Payload<Product[]>> = this.getData;
    // let oldList: ApiResponse<Payload<Product[]>> = JSON.parse(JSON.stringify(this.getData));
    // let index: number = list.payload!.data!.indexOf(this.getDataById!);
    //  let model: Product = { ...this.getDataById, ...updatedData };
    
    // list.payload!.data!.splice(index, 1);
    // list.payload!.data!.unshift(model);
    // this.setData = list;
    let url: string = this.endpoint + `/${this.idOnView}`;
    let model
    return this.baseService.put(url, model)
      .pipe(
        tap((response: ApiResponse<Product>) => {
          // this.setDataById = null;
          // this.setLoading = false;
        }),
        catchError((error: HttpErrorResponse) => {
          // this.setData = oldList;
          // this.setLoading = false;
          return throwError(() => error);
        })
      )
  }

  public removeItem(deletedData: Product): Observable<any> {
    // this.setLoading = true;
    // let list: ApiResponse<Payload<Product[]>> = this.getData;
    // let oldList: ApiResponse<Payload<Product[]>> = JSON.parse(JSON.stringify(this.getData));
    // let index: number = list.payload!.data!.indexOf(deletedData);
     const url: string = this.endpoint + `/${deletedData.id}`;
    // list.payload!.data!.splice(index, 1);
    // this.setData = list;
    
    return this.baseService.delete(url)
      .pipe(
        tap((response: ApiResponse<Product>) => {
          //this.setLoading = false;
        }),
        catchError((error: HttpErrorResponse) => {
          // this.setData = oldList;
          // this.setLoading = false;
          return throwError(() => error);
        })
      )
  }

  public uploadImage(item: Product, formData: FormData): Observable<any>{
    //this.setLoading = true;
    const url: string = this.image_endpoint + `/${item.id}`;
    return this.baseService.postImage(url,formData)
      .pipe(
        tap((response: ApiResponse<Product>) => {
          //this.setLoading = false;

        }),
        catchError((error: HttpErrorResponse) => {
          
          //this.setLoading = false;
         
          return throwError(() => error);
        })
      )

  }

  public statusChange(item: Product): Observable<any>{
    //this.setLoading = true;
    const url: string = this.status_endpoint + `/${item.id}`;
    return this.baseService.putStatus(url)
      .pipe(
        tap((response: ApiResponse<Product>) => {
          //this.setLoading = false;

        }),
        catchError((error: HttpErrorResponse) => {
          
          //this.setLoading = false;
         
          return throwError(() => error);
        })
      )

  }

  
}
