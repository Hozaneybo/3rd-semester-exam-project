import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import {LoginComponent} from "./components/login/login.component";
import {IonicModule} from "@ionic/angular";
import {ReactiveFormsModule} from "@angular/forms";
import {RegisterComponent} from "./components/register/register.component";
import {VerifyEmailComponent} from "./components/verify-email/verify-email.component";


@NgModule({
  declarations:
    [
      LoginComponent,
      RegisterComponent,
      VerifyEmailComponent
    ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    IonicModule,
    ReactiveFormsModule
  ]
})
export class SharedModule { }
