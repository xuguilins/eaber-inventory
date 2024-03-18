import { urls } from "@/api/urls"
import { useTime } from "@/utils";
import alovaInstance, { IReturnResult } from "@/utils/request"
import { Form, Modal, message } from "antd";
import { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
export interface IOutSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
    startTime: string;
    endTime: string;
    supileName: string;
    tel: string;
    userName: string
}
const useOutPage = () => {
    const navtion = useNavigate()
    const [search, setSearch] = useState<IOutSearch>({
        pageIndex: 1,
        pageSize: 10,
        keyWord: '',
        startTime: '',
        endTime: '',
        supileName: '',
        tel: '',
        userName: ''
    })
    const [tableData, setTableData] = useState<any>([])
    const [total, setTotal] = useState<number>(0)
    const [searchForm] = Form.useForm()
    const [modal,contextHolder] = Modal.useModal()
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([])
    const [drawShow,setDrawShow] = useState<boolean>(false)
    const  [drawId,setDrawId] = useState<string>('') 
    const [drawData,setDrawData] = useState<any>({})
    const [viewForm] =Form.useForm()
    const loadPage = async () => {
        const { success, data, total } = await alovaInstance.Post<IReturnResult, IOutSearch>(urls.getPuraseOutPage, search)
            .send()
        if (success) {
            setTableData(data)
            setTotal(total)
       
        }
    }
    useEffect(() => {
        async function init() {
            await loadPage()
        }
        init()
    }, [search])
    const onAddClick = () => {
        navtion('/purchases/outForm')
    }
    const onEdit = (id: string) => {
        navtion('/purchases/editOutOrder', {
            state: {
                id: id
            }
        })
    }
    const onRefresh = () => {
        searchForm.resetFields()
        onSearch()
    }
    const onSearch = () => {
        const name = searchForm.getFieldValue('userName') ?? ''
        const tel = searchForm.getFieldValue('tel') ?? ''
        let start = searchForm.getFieldValue('startTime') ?? ''
        if (start)
            start = useTime(start)
        let end = searchForm.getFieldValue('endTime') ?? ''
        if (end)
            end = useTime(end)
        setSearch({
            ...search,
            pageIndex: 1,
            startTime: start,
            endTime: end,
            userName: name,
            tel: tel
        })

    }
    const onSelectChange = (e) => {
        setSelectedRowKeys(e)
    }
    const rowSelection = {
        selectedRowKeys,
        onChange: onSelectChange,
    }
    const onPageChange=(index:number)=>{
         setSearch({
            ...search,
            pageIndex:index
         })
    }
    const onDeleteClick=async ()=>{
        if (selectedRowKeys.length === 0) {
            message.warning('请选择要删除的记录')
            return 
        }
        const confrim = await modal.confirm({
            title:'删除警告',
            content:'您确定要删除选择中的数据?一旦删除无法恢复!'
        })
       if (confrim) {
          const { success,message:msg} =  await alovaInstance.Delete<IReturnResult,string[]>
          (urls.deleteOutPurase,selectedRowKeys).send()
          if (success) {
            message.success(msg)
            onRefresh()
          } else {
            message.error(msg)
          }
       }
    }
    const onConfrim = async ()=>{
        const confrim = await modal.confirm({
            title:'确认提醒',
            content:'确认此退货单已完成?'
        })
       if (confrim) {
          const { success,message:msg} =  await alovaInstance.Post<IReturnResult,any>
          (urls.updateOutPurashStatus,{
            id:drawId,
            outStatus:2
          }).send()
          if (success) {
            message.success(msg)
            onRefresh()
            setDrawData({
                ...drawData,
                outStatus:2
            })
          } else {
            message.error(msg)
          }
       }
    }
    const loadDetails = async ()=>{
        const {data,success} = await alovaInstance.Get<IReturnResult,any>(urls.getOutOrder+"/"+drawId)
        .send()
        if (success) {
            viewForm.setFieldsValue(data)
            setDrawData(data)
        }
    }
    useEffect(()=>{
    async function loads(){
        if (drawId)
        await loadDetails()
    }
    loads()
    },[drawId])
    const handlderView = (data) => {
        setDrawId(data.id)
       setDrawShow(true)

    }
    const onCloseDlig=()=> {
        setDrawShow(false)
        setDrawId('')
    }
    return {
        onAddClick,
        tableData,
        total,
        search,
        onSearch,
        searchForm,
        onRefresh,
        onEdit,
        handlderView,
        rowSelection,
        onPageChange,
        onDeleteClick,
        drawShow,
        drawData,
        viewForm,
        contextHolder,
        onConfrim,
        onCloseDlig
    }

}
export {
    useOutPage
}