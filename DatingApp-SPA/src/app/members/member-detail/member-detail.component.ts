import { Component, OnInit } from '@angular/core';
import { Users } from 'src/app/_models/users';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  public user: Users;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadUser();
  }

  loadUser() {
    const id = this.route.snapshot.params['id'];
    this.userService.getUser(+id).subscribe(
      (user: Users) => {
        this.user = user;
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }
}
