import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom, Observable} from "rxjs";
import {ResponseDto, User} from "../../admin/components/LoginModels";
import {SearchResultDto} from "../Models/SearchTerm";
import {ToastController} from "@ionic/angular";

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {


  private readonly url = '/api/account/' ;
  private intervalId: any;


  constructor(private http: HttpClient, private toastController : ToastController) { }

  login(email: string, password: string): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.url + 'login', { email, password });
  }

  register(userData: any): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.url + 'register', userData);
  }

  verifyEmail(token: string) : Observable<ResponseDto<any>> {
    return this.http.get<ResponseDto<any>>(`${this.url}verify-email`, { params: { token } });
  }

  requestResetPassword(email: any): Observable<ResponseDto<any>>{
    return this.http.post<ResponseDto<any>>(this.url + 'request-password-reset', {email});
  }

  resetPassword(token: string, newPassword: any): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(`${this.url}reset-password`, { token, newPassword });
  }

  whoAmI(): Observable<ResponseDto<any>> {
    return this.http.get<ResponseDto<any>>(this.url + 'whoami');
  }

  async logout(): Promise<ResponseDto<any>> {
    return await firstValueFrom(this.http.post<ResponseDto<User>>(this.url + 'logout', {}));
  }

  search(searchTerm: string): Observable<ResponseDto<SearchResultDto[]>> {
    return this.http.get<ResponseDto<SearchResultDto[]>>(`${this.url}search`, {
      params: { searchTerm },
      withCredentials: true
    });
  }

  updateUser(user: any, role: string | undefined, formData: FormData): Observable<ResponseDto<any>> {
    let endpoint: string;
    switch (role) {
      case 'Admin':
        endpoint = 'admin';
        break;
      case 'Teacher':
        endpoint = 'teacher';
        break;
      case 'Student':
        endpoint = 'student';
        break;
      default:
        throw new Error('User role not recognized');
    }

    return this.http.put<ResponseDto<any>>(`http://localhost:5000/api/${endpoint}/users/update/${user.id}`, formData, { withCredentials: true });
  }

   setupClock(): void {
    const updateClock = () => {
      const now = new Date();

      const seconds = now.getSeconds();
      const secondsDegrees = ((seconds / 60) * 360) + 90;
      const secondHand = document.querySelector('.second-hand') as HTMLElement;

      const mins = now.getMinutes();
      const minsDegrees = ((mins / 60) * 360) + 90;
      const minsHand = document.querySelector('.min-hand') as HTMLElement;

      const hour = now.getHours();
      const hourDegrees = ((hour / 12) * 360) + 90;
      const hourHand = document.querySelector('.hour-hand') as HTMLElement;

      if (secondHand && minsHand && hourHand) {
        secondHand.style.transform = `rotate(${secondsDegrees}deg)`;
        minsHand.style.transform = `rotate(${minsDegrees}deg)`;
        hourHand.style.transform = `rotate(${hourDegrees}deg)`;
      }
    };

    this.intervalId = setInterval(updateClock, 1000);
    updateClock();
  }

  async presentToast(message: string, color : string) {
    const toast = await this.toastController.create({
      message: message,
      duration: 2000,
      color : color
    });
    toast.present();
  }

}
