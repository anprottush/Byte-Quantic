import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CustomerModuleRoutingModule } from './customer-module-routing.module';
import { CustomerComponent } from './customer/customer.component';
import { AddressComponent } from './address/address.component';

import { ReactiveFormsModule } from '@angular/forms';

import { MatToolbarModule } from "@angular/material/toolbar";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatInputModule } from "@angular/material/input";
import { MatListModule } from "@angular/material/list";
import { MatCardModule } from "@angular/material/card";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatSelectModule } from "@angular/material/select";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatMenuModule } from "@angular/material/menu";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatTabsModule } from "@angular/material/tabs";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatExpansionModule } from "@angular/material/expansion";

import { FormsModule } from '@angular/forms';
import { CustomerCreateComponent } from './customer/customer-create.component';
import { MatDialogModule } from '@angular/material/dialog';
import { AddressCreateComponent } from './address/address-create.component';


@NgModule({
  declarations: [
    CustomerComponent,
    AddressComponent,
    CustomerCreateComponent,
    AddressCreateComponent
  ],
  imports: [
    CommonModule,
    CustomerModuleRoutingModule,
    NgxDatatableModule,
    ReactiveFormsModule,
    FormsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatInputModule,
    MatListModule,
    MatCardModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatButtonToggleModule,
    MatButtonToggleModule,
    MatSelectModule,
    MatSelectModule,
    MatSnackBarModule,
    MatMenuModule,
    MatGridListModule,
    MatTabsModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatDialogModule,
  ]
})
export class CustomerModuleModule { }
