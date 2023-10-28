import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { AddtocartComponent } from './addtocart/addtocart.component';

const routes: Routes = [
  // { path: 'addtocart', component: AddtocartComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CartModuleRoutingModule { }
