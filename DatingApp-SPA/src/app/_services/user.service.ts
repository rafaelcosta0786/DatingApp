import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Users } from '../_models/users';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(page?, itemsPerPage?, userParams?, likeParam?): Observable<PaginatedResult<Users[]>> {
    const paginatedResult: PaginatedResult<Users[]> = new PaginatedResult<Users[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null){
      params = params.append('gender', userParams.gender);
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('orderBy', userParams.orderBy);
    }

    if (likeParam === 'LikeUserOrigin'){
      params = params.append('LikeUserOrigin', 'true');
    }
    if (likeParam === 'LikeUserDestiny'){
      params = params.append('LikeUserDestiny', 'true');
    }    

    return this.http.get<Users[]>(`${this.baseUrl}users`, {observe: 'response', params})
   .pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'))
        }
        return paginatedResult;
      })
    );
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

  sendLike(userId: number, recipientId: number){
    return this.http.post(`${this.baseUrl}users/${userId}/like/${recipientId}`, {});
  }
}
