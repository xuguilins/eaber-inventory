import { ITabModel } from "@/components";
import { makeAutoObservable } from "mobx";

class MenuStore {
    menus:any;
    plains:Array<ITabModel>;
    constructor() {
        makeAutoObservable(this)
        this.plains = []
    }
    setMenus(menus:any){
        this.menus = menus
        this.resolveMenus(menus)
    }
    resolveMenus(menus) {
         menus.forEach(item=>{
          if (item.children && item.children.length>0) {
             this.resolveMenus(item.children)
          } else {
            this.plains.push({ key: item.path,label: item.meta?.title})
          }
       })
    }
    
}
export default MenuStore