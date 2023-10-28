import { Injectable, Inject } from '@angular/core';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ProductModuleApiService {

  constructor(

  ) { }

  public apiProductList(): string {
    return `${environment}product`;
  }

  public apiProductCreate(): string {
    return `${environment}product`;
  }

  public apiProductUpdate(): string {
    return `${environment}product`;
  }
}
