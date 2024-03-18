
export  interface  IMainData<T = any> {
  title?:string;
  data?:T[]
  searchBar?:any,
  toolBar?:any,
  mainTable?:any;
  total?:number;
  pageIndex?:number;
  onRefresh?:()=>void;
  haveExport?:boolean;
  haveImport?:boolean;
  onChange?(index:number);
  exportType?:number;

}
export interface ITabModel {
  label:string;
  key:string;
}
