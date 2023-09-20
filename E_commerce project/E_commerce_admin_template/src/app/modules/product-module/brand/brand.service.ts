import { Injectable } from '@angular/core';
import { Brand } from '../../../shared/models/product/brand';
import { CommonHttpService } from '../../../core/services/common-http.service';
@Injectable({
  providedIn: 'root'
})
export class BrandService {

  constructor(private httpService: CommonHttpService) { }

  getBrandList() {

  }
  addBrand() {

  }
}
