import { useEffect, useState } from "react"
import OrderMain, { IOrderSearch } from "./OrderMain"
import { urls } from "@/api/urls"
import alovaInstance, { IReturnResult } from "@/utils/request"
 import eventBus from "@/utils/eventBus"

const AllOrderPage:React.FC<any> = ()=>{
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
    const loadAllOrder =async ()=>{
        const { data, success, total } = 
        await alovaInstance.Post<IReturnResult, IOrderSearch>(urls.getOrderPage, search, {
            params: {
                type: -1
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
            await loadAllOrder()
        }
        init()
    },[search.pageIndex,search.endTime,search.price,search.startTime,search.tels,search.userName])
    const onRefresh=async ()=>{
        await loadAllOrder()
    }
    useEffect(()=>{
       eventBus.on('onSearch',(e:any)=>{
            if (e.type === -1) {
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
        eventBus.on('onRefresh',function(){
            loadAllOrder()
        })
    },[])
 
    return <>
      <OrderMain 
      totalCount={total}
       data={tableData}
       onRefresh={onRefresh}
       onPageChange={(e)=>onPageChange(e)}
      pageIndex={search.pageIndex}/>
    </>
}
export default AllOrderPage