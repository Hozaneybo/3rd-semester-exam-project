import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ResponseDto} from "../Models/LoginModels";

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {


  private readonly loginUrl = environment.apiEndpoint + 'api/account/login';

  constructor(private http: HttpClient) { }

  login(credentials: any): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.loginUrl, credentials);
  }
}
