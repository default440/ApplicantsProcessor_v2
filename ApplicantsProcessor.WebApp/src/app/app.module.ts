import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { IndexComponent } from './index/index.component';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { SpecialityComponent } from './components/speciality/speciality.component';
import { ApplicantsComponent } from './components/applicants/applicants.component';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    MatSelectModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: IndexComponent },
    ])
  ],
  declarations: [
    AppComponent,
    TopBarComponent,
    SpecialityComponent,
    ApplicantsComponent,
    IndexComponent
  ],
  bootstrap: [
    AppComponent
  ],
  providers: [
    HttpClient
  ]
})
export class AppModule { }


/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at https://angular.io/license
*/