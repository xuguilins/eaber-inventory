import { makeAutoObservable, runInAction } from "mobx";

interface IPushProduct {
    productCode?: string;
    productModel: string;
    productName: string;
    productCount: number;
    remark: string;
    cateId: string;
    unitId: string;
    productIncost: number;
    productWocost: number;
    productPrice: number;
    sellPrice: number;
    productAll: number;
}
class PurseProductStore {
    products: Array<IPushProduct>
    constructor() {
        this.products = []
        makeAutoObservable(this)
    }
    AddProduct() {
        const item =this.defaultItem()
        this.products.unshift({...item})
    }
    Remove(name: string) {
        const findex = this.products.findIndex((item) => item.productName === name)
        runInAction(() => {
            this.products.splice(findex, 1)
        })
    }
    defaultItem() {
        const item: IPushProduct = {
            productCode: '',
            productModel: '',
            productName: '',
            productCount: 0,
            remark: '',
            cateId: '',
            unitId: '',
            productIncost: 0,
            productWocost: 0,
            productPrice: 0,
            sellPrice: 0,
            productAll: 0
        }
       return item
    }
}
export default PurseProductStore
