import { IResolution } from './IResolution';

export interface IHasResolution {
  getResolution(): Promise<IResolution>;
}
