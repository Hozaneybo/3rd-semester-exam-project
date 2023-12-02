import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {FormBuilder, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {ResponseDto} from "../../Models/LoginModels";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {

  constructor(private fb: FormBuilder, private http: HttpClient, private toast: ToastController ) { }

  readonly form = this.fb.group({
    email: [null, [Validators.required, Validators.email]],
    password: [null, Validators.required],
  });



  async submit() {
    const url = environment.apiEndpoint + 'api/account/login';
    try {
      var response = await firstValueFrom(this.http.post<ResponseDto<any>>(url, this.form.value));

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
