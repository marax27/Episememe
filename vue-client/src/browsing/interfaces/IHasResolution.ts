import { ResolutionModes } from '../types/ResolutionModes';

export interface IHasResolution {
  getResolution(): Promise<ResolutionModes>;
}
