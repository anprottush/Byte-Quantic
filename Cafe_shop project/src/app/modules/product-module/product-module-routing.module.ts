import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductComponent } from './product/product.component';

// import { BrandComponent } from './brand/brand.component';

// import { BrandCreateComponent } from './brand/brand-create.component';
import { ProductCreateComponent } from './product/product-create.component';
const routes: Routes = [
  { path: 'product', component: ProductComponent },
  { path: 'product/create', component: ProductCreateComponent },
  { path: 'product/:id', component: ProductCreateComponent },
  // { path: 'brand', component: BrandComponent },
  // { path: 'brand/create', component: BrandCreateComponent },
  // { path: 'brand/:id', component: BrandCreateComponent },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductModuleRoutingModule { }
