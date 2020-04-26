interface ICategory {
  ParentId?: number;
  Id?: number;
  Name?: string;
  Childs?: Array<ICategory>;
  Link?: string;
}
interface IAjaxResult<T = any> {
  Status?: boolean;
  Data?: T;
  ExData?: any;
  Message?: string;
}
