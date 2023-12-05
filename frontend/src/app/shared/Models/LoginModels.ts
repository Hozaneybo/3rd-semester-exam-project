export interface User {
  id: number;
  fullname: string;
  email: string;
  avatarUrl: string;
  role : string;

}

export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}
