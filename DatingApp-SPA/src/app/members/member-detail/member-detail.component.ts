import { Component, OnInit, ViewChild } from '@angular/core';
import { Users } from 'src/app/_models/users';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', { static: true }) memberTabs: TabsetComponent;
  public user: Users;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.user = data['user'];
    });

    this.route.queryParams.subscribe((params) => {
      const selectedTab = params['tab'];
      this.selectTab(selectedTab > 0 ? selectedTab : 0);
    });
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  // loadUser() {
  //   const id = this.route.snapshot.params['id'];
  //   this.userService.getUser(+id).subscribe(
  //     (user: Users) => {
  //       this.user = user;
  //     },
  //     (error) => {
  //       this.alertify.error(error);
  //     }
  //   );
  // }
}
