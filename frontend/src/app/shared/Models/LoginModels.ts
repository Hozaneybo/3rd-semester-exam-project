export interface User {
  id: number;
  fullname: string;
  email: string;
  avatarUrl: string;
  role : string;

}

export interface UpdateProfileCommand {
  fullname?: string;
  email?: string;
  avatarUrl?: string;
  // Add other fields as per your backend requirements
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
