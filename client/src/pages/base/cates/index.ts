
import { IMainData } from "@/components";
import React, { useCallback, useEffect, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
import { ICateData } from "@/pages/busComponents/CateTree";
export interface ICateSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
    loading?: boolean;
    parentId: string;
}
export interface ICateInfo {
    id: string;
    name: string;
    parentId?: string;
    remark: string;
    enable: boolean;
}
const useCateManager = () => {
    const [cateMain, setCateMain] = useState<IMainData<ICateInfo>>({
        title: "分类数据维护",
        data: [],
        total: 0,
        pageIndex: 1,
        onChange: async (index) => {
            setCateSearch({
                ...cateSearch,
                pageIndex: index
            })
            // await loadCateList(index)
        },
    });
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [modal, modelcontextHolder] = Modal.useModal();
    const [modelShow, setModelShow] = useState<boolean>(false)
    const [ruleForm] = Form.useForm()
    const [searchForm] = Form.useForm()
    const [parentId, setParentId] = useState<string>('1000')
    const [cateSearch, setCateSearch] = useState<ICateSearch>({
        pageIndex: 1,
        pageSize: 10,
        keyWord: '',
        parentId: ''
    })
    const [ruleData] = useState<ICateInfo>({
        id: '',
        name: '',
        remark: '',
        enable: true
    })
    const [title, setTitle] = useState<string>('创建分类')
    const [treeData, setTreeData] = useState<ICateData[]>([])
    const loadCateList = async () => {
        const { data, success, total } = await alovaInstance.Post<IReturnResult, ICateSearch>(urls.getCatePage, {
            pageIndex: cateSearch.pageIndex,
            pageSize: cateSearch.pageSize,
            keyWord: searchForm.getFieldValue("keyWord") ?? '',
            parentId: cateSearch.parentId
        }).send()

        if (success) {
            setCateMain({
                ...cateMain,
                data: data,
                total: total
            })
        }
    }
    useEffect(() => {
        async function init() {
            await loadCateList()
        }
        init()
    }, [cateSearch.pageIndex, cateSearch.keyWord, cateSearch.parentId])
    const loadTreeCates = async () => {
        const { data, success } = await alovaInstance.Get<IReturnResult>(urls.getCateTree + "?enable=false").send()
        if (success)
            setTreeData(data)
        
    }
    useEffect(() => {
        async function initTree() {
            await loadTreeCates()
        }
        initTree()
    }, [modelShow])
    const onRefresh = async () => {
        setSelectedRowKeys([])
        setCateSearch({
            pageIndex: 1,
            pageSize: 10,
            keyWord: '',
            parentId: ''
        })
        await loadCateList()
        searchForm.resetFields()

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
            message.warning('请选择一条或多条要删除的数据')
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
                const { success, message: mgs } = await alovaInstance.Delete<IReturnResult, string[]>(urls.deleteCate, deletekeys)
                    .send()
                if (success) {
                    message.success(mgs)
                    close()
                    await loadCateList()
                } else {
                    message.error(mgs)
                }

            },
            onCancel: (close: Function) => {
                close()
            }
        })
    }
    const handlerAdd = async () => {
        setTitle('创建分类')
        ruleForm.setFieldValue('parentId', parentId)
        setModelShow(true)
    }
    const saveForm = async () => {
        const validate = await ruleForm.validateFields()
        if (validate) {
            const ruleValues = validate as ICateInfo
            if (ruleValues.parentId === '1000')
                ruleValues.parentId = ''
            if (validate.id) {
                const { success, message: msg } =
                    await alovaInstance.Post<IReturnResult, ICateInfo>(urls.updateCate, ruleValues).send();
                if (success) {
                    message.success(msg)
                    closeForm()
                    await loadCateList()

                } else {
                    message.error(msg)
                }

            } else {
                //创建

                const { success, message: msg } =
                    await alovaInstance.Post<IReturnResult, ICateInfo>(urls.createCate, ruleValues).send();
                if (success) {
                    message.success(msg)
                    setModelShow(false)
                    closeForm()
                    await loadCateList()

                } else {
                    message.error(msg)
                }
            }
        }

    }
    const handlerEdit = async (data: ICateInfo) => {
        ruleForm.resetFields()
        ruleForm.setFieldsValue(data)
        setTitle('编辑分类')
        await loadTreeCates()
        setModelShow(true)
    }
    const onStatuChange = async ({ id }: ICateInfo) => {
        const { success, message: msg } = await alovaInstance.Post<IReturnResult, string>(`${urls.updateCateStatus}/${id}`).send()
        if (success) {
            message.success(msg)
            await loadCateList()
        } else {
            message.error(msg)
        }
    }
    const onSearch = async () => {
        setCateSearch({
            ...cateSearch,
            pageIndex: 1
        })
        await loadCateList()
    }
    const closeForm = () => {
        ruleForm.resetFields()
        setModelShow(false)
    }
    const onNodeClick = useCallback((data: ICateData) => {
        setCateSearch({
            ...cateSearch,
            parentId: data.id
        })
        setParentId(data.id)
    }, [cateSearch.parentId])
    return {
        cateMain,
        onRefresh,
        rowSelection,
        searchForm,
        deleteHandler,
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
        cateSearch,
        treeData,
        onNodeClick

    }
}
export {
    useCateManager
}
