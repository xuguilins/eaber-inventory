import { makeAutoObservable, runInAction } from "mobx";

export interface ISellStore {
    sellTime: string;
    sellUser: string;
    sellPhone: string;
    sellCode: string;
    remark:string;
    actuailMoney:number;
    offsetMoney:number;
    detail: ISellDetail[]
}
export interface ISellDetail {
    productCode: string;
    productModel: string;
    productName: string;
    unitName: string;
    inventoryCount: number;
    sellPrice: number;
    sellAllPrice: number;
    remark?: string;
}
class SellStore {
    public sellData: ISellStore;
    constructor() {
        this.sellData = {
            sellCode: '',
            sellPhone: '',
            sellTime: '',
            sellUser: '',
            remark:'',
            detail: [],
            actuailMoney:0,
            offsetMoney:0
        }
        makeAutoObservable(this)
    }
    setMainSell(user: string, phone: string, time: string,remark:string = '') {
        runInAction(() => {
            this.sellData.sellUser = user;
            this.sellData.sellPhone = phone;
            this.sellData.sellTime = time;
            this.sellData.remark = remark
        })

    }

    setMainCode(code: string) {
        runInAction(() => {
            this.sellData.sellCode = code;
        })
    }
    setActuailMoney(money:number){ 
        runInAction(()=>{
            this.sellData.actuailMoney = money
        })

    }
    setOffsetMoney(money:number){ 
        runInAction(()=>{
            this.sellData.offsetMoney = money
        })
    }
    setDetail(data: ISellDetail[]) {
        runInAction(() => {
            this.sellData.detail = data
        })
    }
    removeIndex(index: number) {
        runInAction(() => {
            this.sellData.detail.splice(index, 1)
        })
    }
    removeCode(code: string) {
        runInAction(() => {
            const findex = this.sellData.detail.findIndex(item => item.productCode === code)
            if (findex > -1)
                this.sellData.detail.splice(findex, 1)
        })
    }
}
export default SellStore