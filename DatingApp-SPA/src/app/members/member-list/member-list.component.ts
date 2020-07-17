import { Component, OnInit } from '@angular/core';
import { Users } from '../../_models/users';
import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  public users: Users[];
  public user: Users = JSON.parse(localStorage.getItem('user'));
  public genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}]
  public userParams: any = {};
  public pageNumber = 1;
  public pageSize = 5;
  public pagination: Pagination;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    this.pagination = {
      itemsPerPage: this.pageSize,
      currentPage: this.pageNumber,
      totalItems: 0,
      totalPages: 0
    };
    this.resetFilters();
  }

  public resetFilters(){
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.loadUsers();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService
      .getUsers(this.pagination.currentPage, 
      this.pagination.itemsPerPage, this.userParams)
      .subscribe(
        (data: PaginatedResult<Users[]>) => {
          this.users = data.result;
          this.pagination = data.pagination;
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
}
