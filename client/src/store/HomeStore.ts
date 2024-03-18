import { ITabModel } from "@/components";
import { makeAutoObservable } from "mobx";
class HomeStore {
    tabs:ITabModel[]
    activeKey:string;
    constructor() {
        makeAutoObservable(this)
        this.tabs=[{label:'控制台',key:'/'}]
        this.activeKey = '/'
    }
    addTab(tab:ITabModel) {
        this.activeKey = tab.key
        const item = this.tabs.find(item => item.key === tab.key)
        if (!item) 
          this.tabs.push(tab)
    }
    closeOthers (key:string){
       const tabs = this.tabs.filter(item => item.key === key)
       this.tabs = tabs;
    }
    removeTab(key:string) {
        const index = this.tabs.findIndex(item => item.key === key)
        if (index > -1)
          this.tabs.splice(index, 1)  
    }
    closeLeft(key:string) {
        const tabs = []
        const index = this.tabs.findIndex(item => item.key === key)
        for(let i=0;i<this.tabs.length;i++){
            if (i>=index)
                tabs.push(this.tabs[i])
        }
        this.tabs = tabs;
    }
    closeRight (key:string) {
        const tabs = []
        const index = this.tabs.findIndex(item => item.key === key)
        for(let i=0;i<this.tabs.length;i++){
            if (i<=index)
                tabs.push(this.tabs[i])
        }
        this.tabs = tabs;
    }
    setActiveKey(key:string) {
        this.activeKey = key;
    }
}
export default HomeStore