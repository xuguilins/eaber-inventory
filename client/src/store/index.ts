import React from "react";
import { testStore } from "./testStore";
import CountStore from "./CountStore";
import PurseProductStore from './PurseProduct'
import HomeStore from "./HomeStore";
import MenuStore from "./MenuStore";
import UserStore from "./UserStore";
import SellStore from './SellStore'
class RootStore {
    testStore: testStore
    countStore:CountStore
    pushProductStore:PurseProductStore
    homeStore:HomeStore
    menuStore:MenuStore
    userStore:UserStore;
    sellStore:SellStore;
    constructor() {
        this.testStore = new testStore()
        this.countStore = new CountStore()
        this.pushProductStore = new PurseProductStore()
        this.homeStore = new HomeStore()
        this.menuStore = new MenuStore()
        this.userStore = new UserStore()
        this.sellStore = new SellStore()
    }
    restStore() {

    }
}
const rootStore = new RootStore()
const context = React.createContext(rootStore)
const useStore = () => React.useContext(context)
export {
    useStore
}