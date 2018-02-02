import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  constructor(public oidcSecurityService: OidcSecurityService) {
    if (this.oidcSecurityService.moduleSetup) {
      this.doCallbackLogicIfRequired();
    } else {
      this.oidcSecurityService.onModuleSetup.subscribe(() => {
        this.doCallbackLogicIfRequired();
      });
    }
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    this.oidcSecurityService.onModuleSetup.unsubscribe();
    this.oidcSecurityService.onModuleSetup.unsubscribe();
  }

  refreshSession() {
    console.log('start refreshSession');
    this.oidcSecurityService.authorize();
  }

  private doCallbackLogicIfRequired() {
    if (window.location.hash) {
      this.oidcSecurityService.authorizedCallback();
    }
  }
}
