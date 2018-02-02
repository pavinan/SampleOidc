import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER  } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import {
  AuthModule,
  OidcSecurityService,
  OpenIDImplicitFlowConfiguration,
  OidcConfigService,
  AuthWellKnownEndpoints,
} from 'angular-auth-oidc-client';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';

export function loadConfig(oidcConfigService: OidcConfigService) {
  return () => oidcConfigService.load_using_stsServer('http://localhost:5105');
}

@NgModule({
  declarations: [AppComponent],
  imports: [
    HttpClientModule,
    AuthModule.forRoot(),
    BrowserModule,
    AppRoutingModule
  ],
  providers: [
    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [OidcConfigService],
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(public oidcSecurityService: OidcSecurityService,
    private oidcConfigService: OidcConfigService) {
      this.oidcConfigService.onConfigurationLoaded.subscribe(() => {
      const openIDImplicitFlowConfiguration = new OpenIDImplicitFlowConfiguration();
      openIDImplicitFlowConfiguration.stsServer = 'http://localhost:5105';
        openIDImplicitFlowConfiguration.redirect_url = 'http://localhost:5104/';
        // The Client MUST validate that the aud (audience) Claim contains its client_id value registered at the Issuer
        // identified by the iss (issuer) Claim as an audience.
        // The ID Token MUST be rejected if the ID Token does not list the Client as a valid audience,
        // or if it contains additional audiences not trusted by the Client.
        openIDImplicitFlowConfiguration.client_id = 'js';
        openIDImplicitFlowConfiguration.response_type = 'id_token token';
        openIDImplicitFlowConfiguration.scope = 'openid profile contacts';
        openIDImplicitFlowConfiguration.post_logout_redirect_uri = 'http://localhost:5104/unauthorized';
        openIDImplicitFlowConfiguration.start_checksession = true;
        openIDImplicitFlowConfiguration.silent_renew = false;
        openIDImplicitFlowConfiguration.post_login_route = '/';
        // HTTP 403
        openIDImplicitFlowConfiguration.forbidden_route = '/unauthorized';
        // HTTP 401
        openIDImplicitFlowConfiguration.unauthorized_route = '/unauthorized';
        openIDImplicitFlowConfiguration.log_console_warning_active = true;
        openIDImplicitFlowConfiguration.log_console_debug_active = false;
        // id_token C8: The iat Claim can be used to reject tokens that were issued too far away from the current time,
        // limiting the amount of time that nonces need to be stored to prevent attacks.The acceptable range is Client specific.
        openIDImplicitFlowConfiguration.max_id_token_iat_offset_allowed_in_seconds = 10;
        openIDImplicitFlowConfiguration.storage = localStorage;
        const authWellKnownEndpoints = new AuthWellKnownEndpoints();
        authWellKnownEndpoints.setWellKnownEndpoints(this.oidcConfigService.wellKnownEndpoints);

        this.oidcSecurityService.setupModule(openIDImplicitFlowConfiguration, authWellKnownEndpoints);
    });
  }
}
