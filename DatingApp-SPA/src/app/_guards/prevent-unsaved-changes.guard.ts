import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MembersEditComponent } from '../members/members-edit/members-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MembersEditComponent> {
  canDeactivate(component: MembersEditComponent){
    if (component.editForm.dirty){
      return confirm('Are you sure you wan to continue? Any unsaved changes will br lost');
    }
    return true;
  }
}
