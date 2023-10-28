import { publishFacade } from '@angular/compiler';
import { Component, Inject, Injectable } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';

import { MAT_SNACK_BAR_DATA, MatSnackBar } from '@angular/material/snack-bar';


export type GenericDialog = {
  title: string;
  text: string;
}

@Component({
  selector: 'app-generic-dialog',
  templateUrl: './generic-dialog.component.html',
  styleUrls: ['./generic-dialog.component.css']
})

@Injectable({
  providedIn: 'root',
})
export class GenericDialogComponent {
  // constructor(@Inject(MAT_SNACK_BAR_DATA) public data: GenericDialog) { }
  constructor(private snackBar: MatSnackBar) { }

  open(message: string, action: string = 'Close', className: string) {
    this.snackBar.open(message, action, {
      duration: 5000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: [className],
    });
  }

  openConfirmation(message: string, action: string, className: string): any {
    return this.snackBar
      .open(message, action, {
        duration: 5000,
        verticalPosition: 'top',
        horizontalPosition: 'center',
        panelClass: [className],
      })
      .onAction();
  }
}
