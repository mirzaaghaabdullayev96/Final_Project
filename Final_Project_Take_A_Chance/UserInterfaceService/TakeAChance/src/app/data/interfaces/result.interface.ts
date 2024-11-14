export interface ResultPositive<T> {
  entities: T;
  message: string;
  statusCode: number;
  success: boolean;
}

export interface ResultNegative{
  message: string;
  propertyName: string;
  statusCode: number;
  success: boolean;
}

export interface ErrorFromApi{
  message:string;
  propertyName:string;
}