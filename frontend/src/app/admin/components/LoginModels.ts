export interface User {
  id: number;
  fullname: string;
  email: string;
  avatarUrl: string;
  role : string;

}

export interface UserProfile {
  id: number;
  fullname: string;
  email: string;
  avatarUrl: string;
  role : string;
  emailVerified: boolean;

}

export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}
