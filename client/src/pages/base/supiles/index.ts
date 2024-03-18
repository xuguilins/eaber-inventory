
import { IMainData } from "@/components";
import React, { useEffect, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
export interface ISupilerSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
    telOne: string;
    address: string;
    phoneOne: string;
    userOne: string;
    remark: string
}
export interface ISupilerInfo {
    id: string;
    supileCode: string;
    supileName: string;
    telONE: string;
    telTWO: string;
    phoneONE: string;
    phoneTWO: string;
    userONE: string;
    userTWO: string;
    address: string;
    enable: boolean;
    remark: string
    

}
const useSupilerManager = () => {
    const [supilerMain, setSupilerMain] = useState<IMainData<ISupilerInfo>>({
        title: "供应商数据维护",
        data: [],
        total: 0,
        pageIndex: 1,
        onChange: async (index) => {
            setSuilerSearch({
                ...suilerSearch,
                pageIndex: index
            })
            await loadSupilerList(index)
        },

    });
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modelcontextHolder] = Modal.useModal();
    const [modelShow, setModelShow] = useState<boolean>(false)
    const [ruleForm] = Form.useForm()
    const [searchForm] = Form.useForm()
    const [suilerSearch, setSuilerSearch] = useState<ISupilerSearch>({
        pageIndex: 1,
        pageSize: 10,
        keyWord: '',
        telOne:'',
        address:'',
        phoneOne:'',
        userOne:'',
        remark:''
    })
    const [ruleData] = useState<ISupilerInfo>({
        id: '',
        supileCode: '',
        supileName: '',
        telONE: '',
        telTWO: '',
        phoneONE: '',
        phoneTWO: '',
        userONE: '',
        userTWO: '',
        address: '',
        enable: true,
        remark: ''

    })
    const [title, setTitle] = useState<string>('创建供应商')
    useEffect(() => {
        async function init() {
            await loadSupilerList(1)
        }
        init()
    }, [])
    const loadSupilerList = async (pageIndex: number) => {
        const obj =buildSearch()
        const { data, success, total } = await alovaInstance.Post<IReturnResult, ISupilerSearch>(urls.getSupilePage, {
            pageIndex: pageIndex,
            pageSize: suilerSearch.pageSize,
            ...obj
        }).send()
      
        if (success) {
            setSupilerMain({
                ...supilerMain,
                data: data,
                total: total
            })
        }

    }
    const buildSearch =()=>{
        const obj:any = {}
        Object.keys(suilerSearch).forEach(key=>{
            if (key !=='pageIndex' && key !== 'pageSize')
              obj[key] = searchForm.getFieldValue(key) ?? ''
        })
        return obj
    }
    const onRefresh = async () => {
        setSelectedRowKeys([])
        setSuilerSearch({
            ...suilerSearch,
            pageIndex: 1,
            pageSize: 10,
            keyWord: ''
        })
        await loadSupilerList(1)
    }
    const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
        setSelectedRowKeys(newSelectedRowKeys);
    };
    const rowSelection = {
        selectedRowKeys,
        onChange: onSelectChange,
    };
    const deleteHandler = () => {
        if (selectedRowKeys.length <= 0) {
            messageApi.warning('请选择一条或多条要删除的数据')
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
                const { success, message } = await alovaInstance.Delete<IReturnResult, string[]>(urls.deleteSupiler, deletekeys)
                    .send()
                if (success) {
                    messageApi.success(message)
                    close()
                    await loadSupilerList(1)
                } else {
                    messageApi.error(message)
                }

            },
            onCancel: (close: Function) => {
                close()
            }
        })
    }
    const handlerAdd = () => {
        setTitle('创建供应商')
        setModelShow(true)
    }
    const saveForm = async () => {
        const validate = await ruleForm.validateFields()
        if (validate) {
            const ruleValues = validate as ISupilerInfo
            if (validate.id) {
                // 更新
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, ISupilerInfo>(urls.updateSupiler, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadSupilerList(suilerSearch.pageIndex)
                } else {
                    messageApi.error(message)
                }

            } else {
                //创建
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, ISupilerInfo>(urls.createSupile, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadSupilerList(suilerSearch.pageIndex)
                } else {
                    messageApi.error(message)
                }
            }
        }

    }
    const handlerEdit = (data: ISupilerInfo) => {
        ruleForm.resetFields()
        ruleForm.setFieldsValue(data)
        setTitle('编辑供应商')
        setModelShow(true)
    }
    const onStatuChange = async ({ id }: ISupilerInfo) => {
        const { success, message } = await alovaInstance.Post<IReturnResult, string>(`${urls.updateSupilerStatus}/${id}`).send()
        if (success) {
            messageApi.success(message)
            await loadSupilerList(suilerSearch.pageIndex)
        } else {
            messageApi.error(message)
        }
    }
    const onSearch = async () => {
        setSuilerSearch({
            ...suilerSearch,
            pageIndex: 1
        })
        await loadSupilerList(1)
    }
    const closeForm = () => {
        ruleForm.resetFields()
        setModelShow(false)
    }
    return {
        supilerMain,
        onRefresh,
        rowSelection,
        searchForm,
        deleteHandler,
        contextHolder,
        modelcontextHolder,
        handlerAdd,
        modelShow,
        saveForm,
        ruleForm,
        closeForm,
        handlerEdit,
        ruleData,
        title,
        onSearch,
        onStatuChange,
        suilerSearch,

    }
}
export {
    useSupilerManager
}
