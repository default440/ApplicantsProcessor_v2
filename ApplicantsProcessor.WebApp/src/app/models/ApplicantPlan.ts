import { University } from "./University";

export interface ApplicantPlan {
    name: string;
    university: University;
    priority: number;
    state: boolean;
    hasOriginal: boolean;
    hasAgreement: boolean;
}