import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Users } from '../_models/users';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(): Observable<Users[]> {
    return this.http.get<Users[]>(`${this.baseUrl}users`);
  }

  getUser(id: number): Observable<Users> {
    return this.http.get<Users>(`${this.baseUrl}users/${id}`);
  }

  updateUser(id: number, user: Users) {
    return this.http.put(`${this.baseUrl}users/${id}`, user);
  }

  setMainPhoto(userId: number, photoId: number) {
    return this.http.put(
      `${this.baseUrl}users/${userId}/photos/${photoId}/set-main`,
      null
    );
  }

  deletePhoto(userId: number, photoId: number) {
    return this.http.delete(`${this.baseUrl}users/${userId}/photos/${photoId}`);
  }
}
