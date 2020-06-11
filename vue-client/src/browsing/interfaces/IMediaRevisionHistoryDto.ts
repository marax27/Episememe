export interface IMediaRevisionHistoryDto {
  userId: string;
  mediaChangeType: ChangeType;
  timeStamp: string;
}

export enum ChangeType {
  Create = "Create",
  Update = "Update"
}
