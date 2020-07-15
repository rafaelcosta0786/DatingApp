import { Photo } from './photo';

export interface Users {
  id: number;
  username: string;
  knownAs: string;
  age: number;
  gender: string;
  created: Date;
  lastActived: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string;
  introduction?: string;
  lookingFor?: string;
  photos?: Photo[];
}
