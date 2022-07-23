import { ApplicantPlan } from "./ApplicantPlan";

export interface Applicant {
    id: string;
    score: number;
    plans: Array<ApplicantPlan>;
} 