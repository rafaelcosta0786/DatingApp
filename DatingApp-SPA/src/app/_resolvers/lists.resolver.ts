import { Injectable } from '@angular/core';
import { Users } from '../_models/users';
import {
  Resolve,
  Router,
  ActivatedRoute,
  ActivatedRouteSnapshot,
} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<Users[]> {
  pageNumber = 1;
  pageSize = 5;
  likesParam = 'LikeUserOrigin';


  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  public resolve(route: ActivatedRouteSnapshot): Observable<Users[]> {
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParam).pipe(
      catchError((error) => {
        this.alertify.error('Problem retieving data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
