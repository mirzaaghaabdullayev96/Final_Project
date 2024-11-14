import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { catchError, map, tap } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';
import {
  ResultNegative,
  ResultPositive,
} from '../data/interfaces/result.interface';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  http = inject(HttpClient);
  cookieSerivce = inject(CookieService);

  baseApiUrl = environment.baseUserApiUrl;

  token: string | null = null;

  get isAuth() {
    if (!this.token) {
      this.token = this.cookieSerivce.get('token');
    }
    return !!this.token;
  }

  login(payload: LoginRequest) {
    return this.http
      .post<ResultPositive<LoginResponse>>(
        `${this.baseApiUrl}auth/login`,
        payload
      )
      .pipe(
        tap((val) => {
          this.token = val.entities.accessToken;
          this.cookieSerivce.set('token', this.token);
        }),
        map((response) => response),
        catchError((error) => {
          console.log(error);
          const resultNegative: ResultNegative = {
            message: error.error.message || 'Unknown error',
            propertyName: error.error.propertyName || '',
            statusCode: error.error.statusCode,
            success: false,
          };
          return throwError(() => resultNegative);
        })
      );
  }

  register(request: RegisterRequest) {
    return this.http
      .post<ResultPositive<LoginResponse>>(
        `${this.baseApiUrl}auth/registeruser`,
        request
      )
      .pipe(
        catchError((error) => {
          console.log(error);
          const resultNegative: ResultNegative = {
            message: error.error.message || 'Unknown error',
            propertyName: error.error.propertyName || '',
            statusCode: error.error.statusCode,
            success: false,
          };
          return throwError(() => resultNegative);
        })
      );
  }


  forgotPassword(request: ForgotPasswordRequest) {
    return this.http
      .post<ResultPositive<LoginResponse>>(
        `${this.baseApiUrl}users/forgotpassword`,
        request
      )
      .pipe(
        catchError((error) => {
          console.log(error);
          const resultNegative: ResultNegative = {
            message: error.error.message || 'Unknown error',
            propertyName: error.error.propertyName || '',
            statusCode: error.error.statusCode,
            success: false,
          };
          return throwError(() => resultNegative);
        })
      );
  }


  resetPassword(request:ResetPasswordRequest){
    return this.http
      .post<ResultPositive<LoginResponse>>(
        `${this.baseApiUrl}users/resetpassword`,
        request
      )
      .pipe(
        catchError((error) => {
          console.log(error);
          const resultNegative: ResultNegative = {
            message: error.error.message || 'Unknown error',
            propertyName: error.error.propertyName || '',
            statusCode: error.error.statusCode,
            success: false,
          };
          return throwError(() => resultNegative);
        })
      );
  }

}







export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  expirationDate: Date;
}

export interface RegisterRequest {
  name: string;
  surname: string | null;
  email: string;
  password: string;
  confirmpassword: string;
  birthdate:Date;
  username:string;
}

export interface ForgotPasswordRequest{
  email:string;
}

export interface ResetPasswordRequest{
  email:string;
  token:string;
  password:string;
  confirmpassword:string;
}