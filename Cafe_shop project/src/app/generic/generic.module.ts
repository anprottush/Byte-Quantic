import { NgModule } from '@angular/core';
import { GenericDialogComponent } from './generic-dialog/generic-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';

import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
@NgModule({
  declarations: [
    GenericDialogComponent
  ],
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatSnackBarModule
  ]
})
export class GenericModule { }
