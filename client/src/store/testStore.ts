import { makeAutoObservable } from "mobx";

class testStore {
    list: Array<string>
    constructor() {
        this.list = []
        makeAutoObservable(this)
    }
    addItem(item:string) {
        this.list.push(item)
    }
}
export {
    testStore
}