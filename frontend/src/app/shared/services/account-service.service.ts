import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ResponseDto} from "../Models/LoginModels";

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {


  private readonly url = environment.apiEndpoint + 'api/account/' ;


  constructor(private http: HttpClient) { }

  login(credentials: any): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.url + 'login', credentials);
  }

  register(userData: any): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.url + 'register', userData);
  }

  verifyEmail(token: string) {
    return this.http.get(`${this.url}verify-email`, { params: { token } });
  }

  requestResetPassword(email: any){
    return this.http.post(this.url + 'request-password-reset', {email});
  }

}
