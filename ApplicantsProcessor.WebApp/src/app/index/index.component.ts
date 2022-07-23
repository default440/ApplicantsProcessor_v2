import { Component } from '@angular/core';
import { University } from '../models/University';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent {
  code: string = '';
  university: University = University.Default;

  onChangedCode(event: any) {
    this.code = event as string;
  }

  onChangedUniversity(event: any) {
    this.university = event as University;
  }
}


/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at https://angular.io/license
*/