import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { DayModel } from '../models/day.model';

@Injectable({
  providedIn: 'root'
})
export class DaysService {

  constructor(private http: HttpClient) { }

  getDays(): Observable<DayModel[]> {
    return this.http.get<DayModel[]>(`${environment.ApiURL}/days`)
      .pipe(
        map((data: DayModel[]) => {
          return data;
        }),
        catchError(err => {
          if (!environment.production && err.status == 404) {
            return of(this.fakeDayModelArray());
          }
          else
            throw err;
        })
      );
  }

  newDay(model: DayModel): Observable<DayModel> {
    return this.http.put<DayModel>(`${environment.ApiURL}/days`, model)
    .pipe(
      map((data: DayModel) => {
        return data;
      }),
      catchError(err => {
        if (!environment.production && (err.status == 404 || err.status == 405)) {
          model.id = Math.floor(Math.random() * 10000);
          return of(model);
        }
        else
          throw err;
      })
    );
  }

  modifyDay(model: DayModel): Observable<DayModel> {
    return this.http.post<DayModel>(`${environment.ApiURL}/days`, model)
    .pipe(
      map((data: DayModel) => {
        return data;
      }),
      catchError(err => {
        if (!environment.production && (err.status == 404 || err.status == 405)) {
          return of(model);
        }
        else
          throw err;
      })
    );
  }

  
  deleteDay(model: DayModel): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: model
    }
    
    return this.http.delete<any>(`${environment.ApiURL}/days`, options)
    .pipe(
      map((data: any) => {
        return data;
      }),
      catchError(err => {
        if (!environment.production && (err.status == 404 || err.status == 405)) {          
          return of(true);
        }
        else
          throw err;
      })
    );
  }


  private fakeDayModelArray(): DayModel[] {
    const days: DayModel[] = [];

    const d1 = new DayModel();
    d1.id = 1;
    d1.date = new Date('2100-01-01');
    d1.maxVisitors = 9999
    days.push(d1);

    const d2 = new DayModel();
    d2.id = 2;
    d2.date = new Date('2100-01-02');
    d2.maxVisitors = 9999
    days.push(d2);

    return days;
  }

}
