import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutingModule } from './shared-routing.module';
import {LoginComponent} from "./components/login/login.component";
import {IonicModule} from "@ionic/angular";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RegisterComponent} from "./components/register/register.component";
import {VerifyEmailComponent} from "./components/verify-email/verify-email.component";
import {ResetPasswordRequestComponent} from "./components/reset-password-request/reset-password-request.component";
import {ResetPasswordComponent} from "./components/reset-password/reset-password.component";
import {SharedSearchComponent} from "./components/shared-search/shared-search.component";


@NgModule({
  declarations:
    [
      LoginComponent,
      RegisterComponent,
      VerifyEmailComponent,
      ResetPasswordRequestComponent,
      ResetPasswordComponent,
      SharedSearchComponent,

    ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    IonicModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  exports:[
    SharedSearchComponent
  ]
})
export class SharedModule { }
