import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesLoginComponent } from './pages-login/pages-login.component';
import { PagesRegisterComponent } from './pages-register/pages-register.component';



@NgModule({
  declarations: [
    PagesLoginComponent,
    PagesRegisterComponent,
  ],
  imports: [
    CommonModule
  ]
})
export class AuthModuleModule { }
