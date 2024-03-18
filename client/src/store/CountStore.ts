import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { makeAutoObservable, runInAction } from "mobx";

class CountStore {
    public count: number;
    constructor() {
        this.count = 0
        makeAutoObservable(this)
    }
    async getCount(type: number = 0) {
        if(this.count !==0)
          return this.count
        return runInAction(async () => {
            const { data } = await alovaInstance.Get<IReturnResult, any>(urls.getOrderCount, {
                params: {
                    status: type
                }
            }).send()
            return data
        })
    }
    setCount(count: number) {
        this.count = count
    }
}
export default CountStore