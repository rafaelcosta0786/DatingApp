import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Users } from '../_models/users';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsersWithRoles() {
    return this.http.get(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(user: Users, roles: {}) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + user.userName, roles);
  }
}
