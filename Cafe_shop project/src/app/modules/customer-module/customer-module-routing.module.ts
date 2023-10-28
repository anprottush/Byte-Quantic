import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './customer/customer.component';
import { AddressComponent } from './address/address.component';
import { CustomerCreateComponent } from './customer/customer-create.component';
import { AddressCreateComponent } from './address/address-create.component';


const routes: Routes = [
  { path: 'customer', component: CustomerComponent },
  { path: 'customer/create', component: CustomerCreateComponent },
  { path: 'customer/:id', component: CustomerCreateComponent },
  { path: 'address', component: AddressComponent },
  { path: 'address/create', component: AddressCreateComponent },
  { path: 'address/:id', component: AddressCreateComponent },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerModuleRoutingModule { }
