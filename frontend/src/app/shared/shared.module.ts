import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import {LoginComponent} from "./components/login/login.component";
import {IonicModule} from "@ionic/angular";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RegisterComponent} from "./components/register/register.component";
import {VerifyEmailComponent} from "./components/verify-email/verify-email.component";
import {ResetPasswordComponent} from "./components/reset-password/reset-password.component";


@NgModule({
  declarations:
    [
      LoginComponent,
      RegisterComponent,
      VerifyEmailComponent,
      ResetPasswordComponent
    ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    IonicModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class SharedModule { }
