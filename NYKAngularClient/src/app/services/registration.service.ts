import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { RegistrationListModel } from '../models';
import { RegistrationModel } from '../models/registration.model';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  constructor(private http: HttpClient) { }

  listByDate(date: Date): Observable<RegistrationListModel[]> {
    return this.http.get<RegistrationListModel[]>(`${environment.ApiURL}/registration?date=${date.toISOString().split('T')[0]}`)
      .pipe(
        map((data: RegistrationListModel[]) => {
          return data;
        }),
        catchError(err => {
            throw err;
        })
      );
  }

  send(model: RegistrationModel): Observable<any> {
    return this.http.put<any>(`${environment.ApiURL}/registration`, model);
  }
}
