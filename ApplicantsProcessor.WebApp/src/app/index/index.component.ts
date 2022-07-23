import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent {
  constructor(private httpClient: HttpClient) { }

  onEdit(event: any) { // without type info
    console.log(event.target.value);

    this.httpClient.get("https://192.168.0.19:7101/specialities/get").subscribe(x => console.log(x))
  }
}


/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at https://angular.io/license
*/