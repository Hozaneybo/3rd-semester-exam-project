import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";
import {firstValueFrom, Observable} from "rxjs";
import {AccountServiceService} from "../../services/account-service.service";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {

  constructor(private fb: FormBuilder, private service : AccountServiceService, private toast: ToastController ) { }

  readonly form = this.fb.group({
    email: [null, [Validators.required, Validators.email]],
    password: [null, Validators.required],
  });


  async submit() {
    try {
      var response = await firstValueFrom(this.service.login(this.form.value));

      (await this.toast.create({
        message: response.messageToClient,
        color: "success",
        duration: 5000
      })).present();
    } catch (e) {
      (await this.toast.create({
        message: (e as HttpErrorResponse).error.messageToClient,
        color: "danger",
        duration: 5000
      })).present();
    }
  }

}
