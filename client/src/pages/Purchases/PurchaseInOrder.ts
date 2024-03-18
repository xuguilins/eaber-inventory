
import React, { useEffect, useRef, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
import { useStore } from "@/store";
import {  ProFormInstance } from "@ant-design/pro-components";
import { useTime } from "@/utils";
import { useNavigate } from "react-router-dom";

export interface IUnitSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
}

export interface IPayInfo {
    id: string;
    payName: string;
    remark: string;
    enable: boolean;
}
export interface IEnableSupiler {
    id: string;
    supileCode: string;
    supileName: string;
    supileUser: string;
    supileUTEN: string;
    supileTel: string;
    supileSen: string;
}
export type PushProductType = {
    id: React.Key;
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
    invertCount: number;
}
export interface IPushProductData {
    inOrderTime: string;
    channelType: number;
    logistics: string;
    inUser: string;
    inPhone: string;
    supplierId: string;
    invertCount: number;
    inCount: number;
    inPrice: number;
    inOStatus: number;
    payStatus: number;
    remark: string;
    supileName: string;
    prdocutDetail: Array<PushProductType>
}
export interface IUpdateStatus {
    id:string;
    inOStatus: number;
}
export class IPurshaseInOrder {
    id: string;
    inOrderCode: string;
    inTime: string;
    chanpel: number;
    logistics: string;
    inUser: string;
    inPhone: string;
    supileName: string;
    allCount: number;
    allPrice: number;
    inOStatus: number;
    payStatus: number;
    remark: string;
    supplierId: string;
}
const usePurchaseInOrderManager = () => {
    const { pushProductStore } = useStore()
    const [modal, modelcontextHolder] = Modal.useModal();
    const [total, setTotal] = useState<number>(0);
    const [pageIndex, setPageIndex] = useState<number>(1)
    const [searchForm] = Form.useForm();
    const [chanel, setChanel] = useState<number>(0)
    const [puraseForm] = Form.useForm()
    const [dataSource] = useState<readonly PushProductType[]>([]);
    const [purchaseList, setPurchaseList] = useState<Array<IPurshaseInOrder>>([])
    const [selectedRowKeys,setSelectedRowKeys] = useState<React.Key[]>([])
    const [drawShow,setDrawShow] = useState<boolean>(false)
    const [viewForm] = Form.useForm()
    const [tableData,setTableData] = useState<Array<IPurshaseInOrder>>([])
    const [inStatus,setInStatus] = useState<number>(0)
    const [payStatus,setPayStatus] = useState<number>(0)
    const navation = useNavigate()
    useEffect(() => {
        async function init() {
            await loadPages()
        }
        init()
    }, [pageIndex])
    const loadPages = async () => {
        const search = buildSearch()
        const { data, success, total } = await alovaInstance.Post<IReturnResult, any>(urls.getInPurasePage, {
            pageIndex: pageIndex,
            pageSize: 10,
            ...search
        }).send()
        if (success) {
            const list: IPurshaseInOrder[] = []
            data.forEach(item => {
                list.push(item)
            })
            setPurchaseList(list)
            setTotal(total)
        }
    }
    const tableRef = useRef<ProFormInstance<any>>(null)
    const buildSearch = () => {
        let startTime = searchForm.getFieldValue('startTime')
        if (startTime)
            startTime = useTime(new Date(startTime))
        let endTime = searchForm.getFieldValue('endTime')
        if (endTime)
            endTime = useTime(new Date(endTime))
        const json = searchForm.getFieldsValue()
        json['startTime'] = startTime;
        json['endTime'] = endTime;
        const newJson: Record<string, any> = {}
        Object.keys(json).forEach(key => {
            if (json[key]) {
                newJson[key] = json[key]
            } else {
                newJson[key] = ''
            }
        })

        return newJson
    }
    // 新增
    const onAdd = () => {
        navation('/purchases/InForm')

    }

    // 删除
    const onDelete = () => {
        if(selectedRowKeys.length<=0) {
            message.error('请选择一条或多条您要删除的数据')
            return 
        }
        modal.confirm({
            title: '删除确认',
            content: '确定删除所选数据吗？',
            okText: '确认',
            cancelText: '取消',
            onOk: async (close: Function) => {
                const deletekeys: string[] = []
                selectedRowKeys.forEach((key: React.Key) => {
                    deletekeys.push(key.toString())
                })
                const { success, message:backMessage } = 
                await alovaInstance.Delete<IReturnResult, string[]>(urls.deletePushOrder, deletekeys)
                    .send()
                if (success) {
                     message.success(backMessage)
                    close()
                    await onRefresh()
                } else {
                    message.error(backMessage)
                }

            },
            onCancel: (close: Function) => {
                close()
            }
        })
        
    }

    // 查看
    const handlderView = async (record: IPurshaseInOrder) => {
        const {success,data} = await alovaInstance.Get<IReturnResult>(urls.getPurase+"/"+record.id).send()
        if (success) {
            setPayStatus(data.payStatus)
            setInStatus(data.inOStatus)
           
            viewForm.setFieldValue('id',data.id)
            viewForm.setFieldValue('inOrderCode',data.inOrderCode)
            viewForm.setFieldValue('inOrderTime',data.inOrderTime)
             viewForm.setFieldValue('supileName',data.supileName)
            viewForm.setFieldValue('inUser',data.inUser)
            viewForm.setFieldValue('inPhone',data.inPhone)
            viewForm.setFieldValue('allCount',data.inCount)
            viewForm.setFieldValue('allPrice',data.inPrice)
            viewForm.setFieldValue('remark',data.remark)
            viewForm.setFieldValue('channelType', Number(data.channelType)  ===  0 ?'供应商采购':'自行采购')
            viewForm.setFieldValue('payStatus',Number(data.payStatus) === 0 ?'待结算' :'已结算')
            viewForm.setFieldValue('inOStatus',Number(data.inOStatus) === 0 ?'进货中' :'已收货')
            setTableData(data.prdocutDetail)
            setDrawShow(true)
        }
        
    }
    const handlerClose =()=>{
        setDrawShow(!drawShow)
    }
    // 页码变化
    const onPageChange = (index: number) => {
        setPageIndex(index)

    }
    // 查询
    const onSearch = async () => {
        
        await loadPages()
 
    }
    // 刷新
    const onRefresh = async () => {
        searchForm.resetFields()
        setPageIndex(1)
        await loadPages()
    }
    // 编辑
    const onEdit = (id: string) => {
        navation('/purchases/InForm', { state: { id } })
    }
    const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
        setSelectedRowKeys(newSelectedRowKeys);
    };
    const onSaveStatus = async (inStatus:number)=>{
        modal.confirm({
            title: '操作提示',
            content: '确定执行当前操作？',
            okText: '确认',
            cancelText: '取消',
            onOk: async (close: Function) => {
                const json:IUpdateStatus = {
                    id: viewForm.getFieldValue('id'),
                    inOStatus:inStatus
                }
                setInStatus(json.inOStatus)
                const {success,message:controlMessage} = await alovaInstance.Post<IReturnResult,IUpdateStatus>(urls.updatePurashStatus,json).send()
                if (success) {
                    close()
                    message.success(controlMessage)
                    loadPages()
                } else {
                    message.error(controlMessage)
                }

            },
            onCancel: (close: Function) => {
                close()
            }
        })
       

    }
    const rowSelection = {
        selectedRowKeys,
        onChange: onSelectChange,
    };
 
    return {

        total,
        pageIndex,
        onRefresh,
        onAdd,
        onSearch,
        searchForm,
        onDelete,
        onPageChange,
        puraseForm,
        chanel,
        setChanel,
        pushProductStore,
        dataSource,
        tableRef,
        purchaseList,
        onEdit,
        rowSelection,
        modelcontextHolder,
        handlderView,
        drawShow,
        viewForm,
        handlerClose,
        tableData,
        payStatus,
        inStatus,
        onSaveStatus
    }
}
export {
    usePurchaseInOrderManager
}

