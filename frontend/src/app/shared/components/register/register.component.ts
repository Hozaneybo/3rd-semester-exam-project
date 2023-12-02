import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";
import {ToastController} from "@ionic/angular";
import {firstValueFrom} from "rxjs";
import {CustomValidators} from "../../CustomValidators";
import {AccountServiceService} from "../../services/account-service.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent  implements OnInit {
  ngOnInit() {}


  readonly form = this.fb.group({
    fullName: [null, Validators.required],
    email: [null, [Validators.required, Validators.email]],
    password: [null, [Validators.required, Validators.minLength(8)]],
    passwordRepeat: [null, [Validators.required, CustomValidators.matchOther('password')]],
    avatarUrl: [null],
  });

  constructor(private fb: FormBuilder, private service : AccountServiceService, private toast: ToastController ) { }

  async submit() {
    try {
      var response = await firstValueFrom(this.service.register(this.form.value));

      (await this.toast.create({
        message: response.messageToClient,
        color: "success",
        duration: 5000
      })).present();
    } catch (e) {
      await (await this.toast.create({
        message: (e as HttpErrorResponse).error.messageToClient,
        color: "danger",
        duration: 5000
      })).present();
    }
  }
}
