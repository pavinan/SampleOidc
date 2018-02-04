import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing.module';
import { PagesComponent } from './pages.component';
import { NavigationMenuComponent } from './shared/navigation-menu/navigation-menu.component';

@NgModule({
  imports: [
    CommonModule,
    PagesRoutingModule
  ],
  declarations: [PagesComponent, NavigationMenuComponent]
})
export class PagesModule { }
