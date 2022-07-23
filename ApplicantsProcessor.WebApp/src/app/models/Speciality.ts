import { SpecialityLink } from "./SpecialityLink";

export interface Speciality {
    code: string;
    name: string;
    undergraduate: boolean;
    links: Array<SpecialityLink>;
}