
import { IMainData } from "@/components";
import React, { useEffect, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
export interface ICusSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
}
export interface ICustomerInfo {
    id: string;
    cuCode: string;
    cuName: string;
    cuTel: string;
    cuUser: string;
    cuAddress: string;
    cuPhone: string;
    remark: string;
    enable: boolean;
}
const useCustomerManager = () => {
    const [unitMain, setUnitMain] = useState<IMainData<ICustomerInfo>>({
        title: "单位数据维护",
        data: [],
        total: 0,
        pageIndex: 1,
        onChange: async (index) => {
            setUnitSearch({
                ...unitSearch,
                pageIndex: index
            })
            await loadUnitList(index)
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
        keyWord: ''
    })
    const [ruleData] = useState<ICustomerInfo>({
        id: '',
        cuAddress: '',
        remark: '',
        cuCode: '',
        cuName: '',
        cuPhone: '',
        cuTel: '',
        enable: true,
        cuUser: ''
    })
    const [title, setTitle] = useState<string>('创建编码')
    useEffect(() => {
        async function init() {
            await loadUnitList(1)
        }
        init()
    }, [])
    const loadUnitList = async (pageIndex: number) => {
        const { data, success, total } = await alovaInstance.Post<IReturnResult, ICusSearch>(urls.getCustomerPage, {
            pageIndex: pageIndex,
            pageSize: unitSearch.pageSize,
            keyWord: searchForm.getFieldValue("keyWord") ?? ''
        }).send()
    
        if (success) {
            setUnitMain({
                ...unitMain,
                data: data,
                total: total
            })
        }

    }
    const onRefresh = async () => {
        setSelectedRowKeys([])
        setUnitSearch({
            pageIndex: 1,
            pageSize: 10,
            keyWord: ''
        })
        await loadUnitList(1)
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
                const { success, message } = await alovaInstance.Delete<IReturnResult, string[]>(urls.deleteCustomer, deletekeys)
                    .send()
                if (success) {
                    messageApi.success(message)
                    close()
                    await loadUnitList(1)
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
        setTitle('创建客户信息')
        setModelShow(true)
    }
    const saveForm = async () => {
        const validate = await ruleForm.validateFields()
        if (validate) {
            const ruleValues = validate as ICustomerInfo
            if (validate.id) {
                // 更新
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, ICustomerInfo>(urls.updateCustomer, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadUnitList(1)
                } else {
                    messageApi.error(message)
                }

            } else {
                //创建
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, ICustomerInfo>(urls.createCustomer, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadUnitList(1)
                } else {
                    messageApi.error(message)
                }
            }
        }

    }
    const handlerEdit = (data: ICustomerInfo) => {
        ruleForm.resetFields()
        ruleForm.setFieldValue('id', data.id)
        ruleForm.setFieldValue('customerCode', data.cuCode)
        ruleForm.setFieldValue('customerName', data.cuName)
        ruleForm.setFieldValue('customerUser', data.cuUser)
        ruleForm.setFieldValue('telNumber', data.cuTel)
        ruleForm.setFieldValue('phoneNumber', data.cuPhone)
        ruleForm.setFieldValue('address', data.cuAddress)
        ruleForm.setFieldValue('remark', data.remark)
        ruleForm.setFieldValue('enable', data.enable)
        setTitle('编辑客户信息')
        setModelShow(true)
    }
    const onStatuChange = async ({ id }: ICustomerInfo) => {
        const { success, message } = await alovaInstance.Post<IReturnResult, string>(`${urls.updateCustomerStatus}/${id}`).send()
        if (success) {
            messageApi.success(message)
            await loadUnitList(1)
        } else {
            messageApi.error(message)
        }
    }
    const onSearch = async () => {

        setUnitSearch({
            ...unitSearch,
            pageIndex: 1
        })
        await loadUnitList(1)
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
        ruleData,
        title,
        onSearch,
        onStatuChange,
        unitSearch,

    }
}
export {
    useCustomerManager
}
