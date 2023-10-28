import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ProductModuleRoutingModule } from './product-module-routing.module';
import { ProductComponent } from './product/product.component';
// import { BrandComponent } from './brand/brand.component';

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

// import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { FormsModule } from '@angular/forms';

import { MatDialogModule } from '@angular/material/dialog';
// import { BrandCreateComponent } from './brand/brand-create.component';
import { ProductCreateComponent } from './product/product-create.component';


@NgModule({
  declarations: [
    ProductComponent,
    // BrandComponent,
    // BrandCreateComponent,
    ProductCreateComponent
  ],
  imports: [
    CommonModule,
    ProductModuleRoutingModule,
    // NgxDatatableModule,
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
    // NgxMatSelectSearchModule,
    MatSnackBarModule,
    MatMenuModule,
    MatGridListModule,
    MatTabsModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatDialogModule,
  ]
})
export class ProductModuleModule { }
