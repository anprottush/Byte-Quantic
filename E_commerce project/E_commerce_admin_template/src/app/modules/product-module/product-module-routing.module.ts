import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { CategoryComponent } from './category/category.component';
import { BrandComponent } from './brand/brand.component';

const routes: Routes = [
  { path: 'product', component: ProductComponent },
  { path: 'brand', component: BrandComponent },
  { path: 'category', component: CategoryComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductModuleRoutingModule {}
