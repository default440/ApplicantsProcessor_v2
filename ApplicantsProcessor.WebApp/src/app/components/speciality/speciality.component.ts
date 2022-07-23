import { Component, EventEmitter, OnDestroy, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { University } from 'src/app/models/University';

@Component({
  selector: 'speciality',
  templateUrl: './speciality.component.html',
  styleUrls: ['./speciality.component.css']
})
export class SpecialityComponent implements OnDestroy {
  private readonly destroy$ = new Subject();
  private code: string = '';
  university = University;

  constructor() { }

  @Output() onChangedCode = new EventEmitter<string>();
  @Output() onChangedUniversity = new EventEmitter<University>();

  search(university: University){
    console.log(university + ' ' + this.code);
    
    this.onChangedCode.emit(this.code);
    this.onChangedUniversity.emit(university);
  }

  onEdit(event: any) {
    this.code = event.target.value;
  }

  ngOnDestroy() {
    this.destroy$.next(null);
    this.destroy$.complete();
  }
}