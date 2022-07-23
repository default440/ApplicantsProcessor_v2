import { HttpClient } from '@angular/common/http';
import { Component, Input, OnChanges, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { Applicant } from 'src/app/models/applicant';
import { Speciality } from 'src/app/models/speciality';
import { University } from 'src/app/models/University';

@Component({
  selector: 'applicants',
  templateUrl: './applicants.component.html',
  styleUrls: ['./applicants.component.css']
})
export class ApplicantsComponent implements OnDestroy, OnChanges {
    private readonly destroy$ = new Subject();

    private specialities: Speciality[] = [];

    applicants: Array<Array<Applicant>> = [[]];
    specialityName?: string = '';

    universityEnum = University;

    constructor(private httpClient: HttpClient) {
      let link = 'https://192.168.0.19:7101/specialities/get';

      console.log('GET from: ' + link);

      this.httpClient.get(link)
        .pipe(takeUntil(this.destroy$))
        .subscribe(x => {
          this.specialities = x as Speciality[];
          console.log(this.specialities);
        });
    }

    @Input() university: University = University.Default;
    @Input() code: string = '';

    ngOnChanges(){
      if (!this.code) return;
      
      let link = 'https://192.168.0.19:7101/applicants/get-by-code/' + this.university + '/' + this.code;
      console.log('GET from: ' + link);
      
      this.httpClient.get(link)
        .pipe(takeUntil(this.destroy$))
        .subscribe(x => {
          this.applicants = x as Array<Array<Applicant>>;
          console.log(this.applicants);
        });
      
      let speciality = this.specialities.find(x => x.code === this.code);

      this.specialityName = speciality?.name;
      console.log('specialityName: ' + this.specialityName);
      
      this.specialities
        .flatMap(x => x.links)
        .forEach(specialityLink => {
          let link = 'https://192.168.0.19:7101/applicants/get-by-link/' + this.university;

          if (speciality?.links.find(x => x.name === specialityLink.name)) return;

          this.httpClient.get(link, { params: { link: specialityLink.link } })
            .pipe(takeUntil(this.destroy$))
            .subscribe(x => {
              let applicants: Applicant[] = x as Applicant[];
              console.log('Received ' + applicants.length + ' applicants from ' + specialityLink.link);

              applicants.forEach(applicant => {
                this.applicants.forEach(applicantsBySpeciality => {
                  applicantsBySpeciality.find(x => x.id === applicant.id)?.plans.push(...applicant.plans);
                });
              });
            });
      });
    }

    ngOnDestroy() {
      this.destroy$.next(null);
      this.destroy$.complete();
    }
}