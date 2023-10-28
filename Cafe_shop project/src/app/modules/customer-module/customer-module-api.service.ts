import { Injectable, Inject } from '@angular/core';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class CustomerModuleApiService {

  constructor(

  ) { }

  public apiCustomerList(): string {
    return `${environment}customer`;
  }

  public apiCustomerCreate(): string {
    return `${environment}customer`;
  }

  public apiCustomerUpdate(): string {
    return `${environment}customer`;
  }

  public apiAddressList(): string {
    return `${environment}address`;
  }

  public apiAddressCreate(): string {
    return `${environment}address`;
  }

  public apiAddressUpdate(): string {
    return `${environment}address`;
  }
}
