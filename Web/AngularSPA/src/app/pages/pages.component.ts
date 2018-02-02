import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss']
})
export class PagesComponent implements OnInit, OnDestroy {

  email: string;
  isAuthorizedSubscription: Subscription;
  isAuthorized: boolean;
  userDataSubscription: Subscription;
  userData: boolean;
  constructor(public oidcSecurityService: OidcSecurityService) { }

  ngOnInit() {
    this.isAuthorizedSubscription = this.oidcSecurityService
      .getIsAuthorized()
      .subscribe((isAuthorized: boolean) => {
        this.isAuthorized = isAuthorized;
      });

    this.userDataSubscription = this.oidcSecurityService
      .getUserData()
      .subscribe((userData: any) => {
        if (userData && userData !== '') {
          this.email = userData.email;
        }
      });
  }

  ngOnDestroy(): void {
    this.userDataSubscription.unsubscribe();
  }

}
