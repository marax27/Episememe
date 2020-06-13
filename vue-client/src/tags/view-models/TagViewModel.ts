import { ITag } from '@/shared/models/ITag';

export class TagViewModel implements ITag {

  constructor(
    public name: string,
    public fullName: string,
    public children: string[],
    public parents: string[],
    public description?: string) {}
}
