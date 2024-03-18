import alovaInstance, { IReturnResult } from "@/utils/request"
import { useEffect, useState } from "react"
// import { IOrderSearch } from '.'
import { urls } from "@/api/urls"
import OrderMain, { IOrderSearch } from "./OrderMain"
import eventBus from "@/utils/eventBus"
const DzfOrderPage:React.FC<any> = ()=>{
    const [search, setSearch ]= useState<IOrderSearch>({
        pageIndex:1,
        pageSize:10,
        startTime:'',
        endTime:'',
        userName:'',
        tels:'',
        price:''
    })
    const [tableData,setTableData] = useState<any>([])
    const [total,setTotal] = useState<number>(0)
    const loadDZFOrder =async ()=>{
        const { data, success, total } = await alovaInstance.Post<IReturnResult, IOrderSearch>(urls.getOrderPage, search, {
            params: {
                type: 0
            }
        }).send()
        if (success) {
            setTableData(data)
            setTotal(total)
        }
    }
    const onPageChange =(e:number)=>{
        setSearch({
            ...search,
            pageIndex:e
        })
    }
    useEffect(()=>{
        async function init() {
            await loadDZFOrder()
        }
        init()
    },[search.pageIndex,search.endTime,search.price,search.startTime,search.tels,search.userName])
    useEffect(()=>{
    eventBus.on('onSearch',(e:any)=>{
             if (e.type === 0) {
                 setSearch({
                     ...search,
                     startTime:e.startTime ?? '',
                     endTime:e.endTime ?? '',
                     price:e.price ?? '',
                     tels:e.tels ?? '',
                     userName:e.userName ?? '',
                 })
             }
         })
     },[])
    return <>
      <OrderMain 
      totalCount={total}
       data={tableData}
       onPageChange={(e)=>onPageChange(e)}
      pageIndex={search.pageIndex}/>
    </>
}
export default DzfOrderPage