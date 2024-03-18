
import { IMainData } from "@/components";
import React, { useEffect, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
import { IDicData, IOptionData } from "../dics/DicPage";
export interface IRuleSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
}
export interface IRuleInfo {
    id: string;
    ruleType: number;
    name: string;
    rulePix: string;
    identityNum: number;
    ruleFormatter: string;
    appendNum: number;
    remark: string;
    enable: boolean;
}
const useRuleManager = () => {
    const [ruleMain, setRuleMain] = useState<IMainData<IRuleInfo>>({
        title: "编码数据维护",
        total:0,
        onChange(index:number) {
            
        },
        pageIndex:1,  
        data: []
    });
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modelcontextHolder] = Modal.useModal();
    const [modelShow, setModelShow] = useState<boolean>(false)
    const [ruleOption,setRuleOption] = useState<Record<number, string>>({})
    const [ruleSelect,setRuleSelect] = useState<any>([])
    const [ruleForm] = Form.useForm()
    const [searchForm] = Form.useForm()
    const [ruleData] = useState<IRuleInfo>({
        id: '',
        ruleType: 1,
        name: '',
        identityNum: 1,
        ruleFormatter: '',
        appendNum: 4,
        remark: '',
        enable: true,
        rulePix: ''

    })
    const [title, setTitle] = useState<string>('创建编码')
    const [ruleSearch, setRuleSearch] = useState<IRuleSearch>({
        pageIndex: 1,
        pageSize: 10,
        keyWord: ''
    })
    useEffect(() => {
        async function init() {
            await loadRuleList()
        }
        init()
    }, [])
    useEffect(()=>{
        async function init() {
            await loadRuleTypes()
        }
        init()
    },[])
    const loadRuleList = async () => {
        const { data, success, total } = await alovaInstance.Post<IReturnResult, IRuleSearch>(urls.getRulesPage, {
            pageIndex: ruleSearch.pageIndex,
            pageSize: ruleSearch.pageSize,
            keyWord: searchForm.getFieldValue("keyWord")
        }).send()
        if (success) {
            setRuleMain({
                ...ruleMain,
                data: data,
                total:total
            })
        }
    }
    const loadRuleTypes = async ()=>{
        const { data, success } = await alovaInstance.Get<IReturnResult>(urls.getRuleTypes).send()
        if (success) {
            if (data && data.length>0) {
                setRuleSelect(data)
                const json = {};
                data.forEach((item) => {
                  json[item.value] = item.name;
                });
                setRuleOption(json);
            }

        }
    }
    const onRefresh = async () => {
        setSelectedRowKeys([])

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
                const { success, message } = await alovaInstance.Delete<IReturnResult, string[]>(urls.deleteRule, deletekeys)
                    .send()
                if (success) {
                    messageApi.success(message)
                    close()
                    await loadRuleList()
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
        setTitle('创建编码')
        setModelShow(true)
    }
    const saveForm = async () => {
     
        const validate = await ruleForm.validateFields()
        if (validate) {
            const ruleValues = validate as IRuleInfo
            if (validate.id) {
                // 更新
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, IRuleInfo>(urls.updateRule, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadRuleList()
                } else {
                    messageApi.error(message)
                }

            } else {
                //创建
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, IRuleInfo>(urls.createRule, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadRuleList()
                } else {
                    messageApi.error(message)
                }
            }
        }

    }
    const handlerEdit = (data: IRuleInfo) => {
        ruleForm.resetFields()
        ruleForm.setFieldsValue(data)
        setTitle('编辑编码')
        setModelShow(true)
    }
    const onStatuChange = async ({ id }: IRuleInfo) => {
        const { success, message } = await alovaInstance.Post<IReturnResult, string>(`${urls.updateRuleStatus}/${id}`).send()
        if (success) {
            messageApi.success(message)
            await loadRuleList()
        } else {
            messageApi.error(message)
        }
    }
    const onSearch = async () => {
        const keyValue = searchForm.getFieldValue("keyWord")
        setRuleSearch({
            ...ruleSearch,
            pageIndex:1,
            keyWord: keyValue
        })
       await loadRuleList()
    }
    const closeForm = () => {
        ruleForm.resetFields()
        setModelShow(false)
    }
    return {
        ruleMain,
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
        ruleSearch,
        ruleOption,
        ruleSelect
    }
}
export {
    useRuleManager
}
