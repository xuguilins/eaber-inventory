
import { IMainData } from "@/components";
import React, { useEffect, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
export interface ICusSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
    extraType: number;
}
export interface ICustomerInfo {
    id: string;
    typeName: string;
    price: number;
    orderCode: string;
    extraType: string;
    remark: string;
    enable: boolean;
}
const useExtraManager = () => {
    const [unitMain, setUnitMain] = useState<IMainData<ICustomerInfo>>({
        title: "其它支出/收入维护",
        data: [],
        total: 0,
        pageIndex: 1,
        onChange: async (index) => {
            setUnitSearch({
                ...unitSearch,
                pageIndex: index
            })

        },

    });
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modelcontextHolder] = Modal.useModal();
    const [modelShow, setModelShow] = useState<boolean>(false)
    const [ruleForm] = Form.useForm()
    const [searchForm] = Form.useForm()
    const [unitSearch, setUnitSearch] = useState<ICusSearch>({
        pageIndex: 1,
        pageSize: 10,
        keyWord: '',
        extraType: -1
    })

    const [title, setTitle] = useState<string>('创建编码')
    const [radio, setRadio] = useState<number>(1)
    const [typeOption, setTypeOption] = useState<any>([])
    useEffect(() => {
        async function init() {
            await loadUnitList()
        }
        init()
    }, [unitSearch])
    const loadUnitList = async () => {
        const { data, success, total } = await alovaInstance.Post<IReturnResult, ICusSearch>(urls.getExtraPage, unitSearch).send()
    
        if (success) {
            setUnitMain({
                ...unitMain,
                data: data,
                total: total
            })
        }

    }
    const onSelectRadio = (e: any) => {
        setRadio(e.target.value)
    }
    const loadTypes = async () => {
        ruleForm.setFieldValue('typeName', '')
        let type = 3
        if (radio === 1)
            type = 4
        const { data, success } = await alovaInstance.Get<IReturnResult>(urls.getSysDics + "?type=" + type).send()
        if (success) {
            setTypeOption(data)
        }
    }
    useEffect(() => {
        async function itypes() {
            await loadTypes()
        }
        itypes()
    }, [radio])
    const onRefresh = async () => {
        setSelectedRowKeys([])
        setUnitSearch({
            ...unitSearch,
            keyWord:'',
            pageIndex:1,
            extraType: -1 
        })
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
            content: '确定删除所选数据吗?',
            okText: '确认',
            cancelText: '取消',
            onOk: async (close: Function) => {
                const deletekeys: string[] = []
                selectedRowKeys.forEach((key: React.Key) => {
                    deletekeys.push(key.toString())
                })
                const { success, message } = await alovaInstance.Delete<IReturnResult, string[]>(urls.deleteExtra, deletekeys)
                    .send()
                if (success) {
                    messageApi.success(message)
                    close()
                    onRefresh()
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
        setTitle('创建其它支出/收入信息')
        ruleForm.setFieldValue('extraType', 1)
        setModelShow(true)
    }
    const saveForm = async () => {
        const validate = await ruleForm.validateFields()
        
        if (validate) {
            if (validate.id) {
                // 更新
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, any>(urls.updateExtra, validate).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    onRefresh()
                } else {
                    messageApi.error(message)
                }

            } else {

                const { success, message } =
                    await alovaInstance.Post<IReturnResult, any>(urls.createExtra, validate).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    onRefresh()
                } else {
                    messageApi.error(message)
                }
            }
        }

    }
    const handlerEdit = (data: ICustomerInfo) => {
        ruleForm.resetFields()
        ruleForm.setFieldsValue(data)
        ruleForm.setFieldValue('id', data.id)
        ruleForm.setFieldValue('orderCode', data.orderCode)
        ruleForm.setFieldValue('extraType', data.extraType)
        ruleForm.setFieldValue('typeName', data.typeName)
        ruleForm.setFieldValue('price', data.price)
        ruleForm.setFieldValue('remark', data.remark)
        setTitle('编辑其它支出/收入信息')
        setModelShow(true)
    }
    const onStatuChange = async ({ id }: ICustomerInfo) => {
        const { success, message } = await alovaInstance.Post<IReturnResult, string>(`${urls.cancleExtra}/${id}`).send()
        if (success) {
            messageApi.success(message)
            onRefresh()
        } else {
            messageApi.error(message)
        }
    }
    const onSearch = async () => {
        const kwd =  searchForm.getFieldValue('keyWord') ?? ''
        const type =searchForm.getFieldValue('extraType')
        setUnitSearch({
            ...unitSearch,
            pageIndex: 1,
            keyWord:kwd,
            extraType:type
        })
    }
    const closeForm = () => {
        ruleForm.resetFields()
        setModelShow(false)
    }
    return {
        unitMain,
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

        title,
        onSearch,
        onStatuChange,
        unitSearch,
        onSelectRadio,
        typeOption

    }
}
export {
    useExtraManager
}
