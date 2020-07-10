import { Component, OnInit, Input } from '@angular/core';
import { Users } from 'src/app/_models/users';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: Users;

  constructor() { }

  ngOnInit() {
  }

}
