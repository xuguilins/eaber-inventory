
import { IMainData } from "@/components";
import React, { useEffect, useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Form, Modal, message } from "antd";
export interface IProductSearch {
    pageIndex: number;
    pageSize: number;
    keyWord: string;
    productModel: string;
    remark: string;
    supileName: string;
    cateId: string;
}
export interface IProductInfo {
    id: string;
    productCode: string;
    productName: string;
    productModel: string;
    cateName?: string;
    unitName?: string;
    supName?: string;
    cateId: string;
    unitId: string;
    supilerId: string;
    conversionRate: string;
    inventoryCount: number;
    initialCost: number;
    purchase: number;
    sellPrice: number;
    wholesale: number;
    maxStock: number;
    minStock: number;
    remark: string;
    enable: boolean;
}
export interface IProductCate {
    cateId: string;
    cateName: string;
    children: Array<IProductCate>
}
export interface IProductUnit {
    name: string;
    id: string;
    enable: boolean;
    remark: string;
}
export interface IProductSuplier {
    name: string;
    supileId: string;
}
const useProductManager = () => {

    const [productMain, setProductMain] = useState<IMainData<IProductInfo>>({
        title: "产品数据维护",
        data: [],
        total: 0,
        pageIndex: 1,
        onChange: async (index: number) => {
            setProductSearch({
                ...productSearch,
                pageIndex: index
            })
            await loadProductList(index)
        },

    });
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modelcontextHolder] = Modal.useModal();
    const [modelShow, setModelShow] = useState<boolean>(false)
    const [ruleForm] = Form.useForm()
    const [searchForm] = Form.useForm()
    const [productSearch, setProductSearch] = useState<IProductSearch>({
        pageIndex: 1,
        pageSize: 10,
        keyWord: '',
        productModel: '',
        remark: '',
        supileName: '',
        cateId: ''
    })
    const [ruleData] = useState<IProductInfo>({
        id: '',
        productCode: '',
        productName: '',
        productModel: '',
        cateName: '',
        unitName: '',
        cateId: '',
        unitId: '',
        supilerId: '',
        conversionRate: '',
        inventoryCount: 1,
        initialCost: 1,
        purchase: 1,
        sellPrice: 1,
        wholesale: 1,
        maxStock: 0,
        minStock: 0,
        remark: '',
        enable: true

    })
    const [title, setTitle] = useState<string>('创建产品')
    const [treeData, setTreeData] = useState<any[]>([])
    const [productCate, setProductCate] = useState<any[]>([])
    const [productUnit, setProductUnit] = useState<IProductUnit[]>();
    const [productSupiler, setProductSupiler] = useState<IProductSuplier[]>([])
    const  [defaultCates,setDefaultCates] = useState<string[]>([])
   // const defaultCates=['66709DA25299052D46B6B','4855F17CEF74EEE24F7EA']
    useEffect(() => {
        async function init() {
            await loadProductList(1)
            await loadProductCate()
        }
        init()
    }, [])
    const loadProductList = async (pageIndex: number) => {
        const obj = buildSearch()
        const { data, success, total } = await alovaInstance.Post<IReturnResult, IProductSearch>(urls.getProductPages, {
            pageIndex: pageIndex,
            pageSize: productSearch.pageSize,
            ...obj,
        }).send()
        if (success) {
            setProductMain({
                ...productMain,
                data: data,
                total: total
            })
        }
    }
    const loadProductCate = async () => {
        const { data, success } = await alovaInstance.Get<IReturnResult>(urls.getProductCates).send()
        if (success) {
        
            setProductCate(data)
            const ids = data.map(d=>d.cateId)
            setDefaultCates(ids)
        }
    }
    const buildSearch = () => {
        const obj: any = {}
        Object.keys(productSearch).forEach(key => {
            if (key !== 'pageIndex' && key !== 'pageSize')
                obj[key] = searchForm.getFieldValue(key) ?? ''
        })
        return obj
    }
    const onRefresh = async () => {
        setSelectedRowKeys([])
        setProductSearch({
            ...productSearch,
            pageIndex: 1,
            pageSize: 10,
            keyWord: '',
            productModel: '',
            cateId: ''
        })
        searchForm.setFieldsValue([])
        await loadProductList(1)
        await loadProductCate()
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
                const { success, message } = await alovaInstance.Delete<IReturnResult, string[]>(urls.deleteProduct, deletekeys)
                    .send()
                if (success) {
                    messageApi.success(message)
                    close()
                    await loadProductList(1)
                } else {
                    messageApi.error(message)
                }

            },
            onCancel: (close: Function) => {
                close()
            }
        })
    }
    const handlerAdd = async () => {
        setTitle('创建商品')
        await loadTreeCates()
        await loadUnits()
        await loadSupilers()
        setModelShow(true)
    }
    const loadTreeCates = async () => {
        const { data, success } = await alovaInstance.Get<IReturnResult, any>(urls.getCateTree).send()
        if (success)
            setTreeData(data)
    }
    const loadUnits = async () => {
        const { data, success } = await alovaInstance.Get<IReturnResult, IProductUnit>(urls.getUnits).send()
        if (success)
            setProductUnit(data)
    }
    const loadSupilers = async () => {
        const { data, success } = await alovaInstance.Get<IReturnResult, IProductSuplier>(urls.getSupilers).send()
        if (success)
            setProductSupiler(data)

    }
    const handlerTreeSelect = async (data: React.Key[]) => {
        if (data && data.length > 0) {
            const id = data[0].toString()
            searchForm.setFieldValue('cateId', id)
            await loadProductList(1)
        }
    }
    const saveForm = async () => {
        const validate = await ruleForm.validateFields()
        if (validate) {
            const ruleValues = validate as IProductInfo
            if (validate.id) {
                // 更新
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, IProductInfo>(urls.updateProduct, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadProductList(productSearch.pageIndex)
                } else {
                    messageApi.error(message)
                }

            } else {
                //创建
                const { success, message } =
                    await alovaInstance.Post<IReturnResult, IProductInfo>(urls.createProduct, ruleValues).send();
                if (success) {
                    messageApi.success(message)
                    ruleForm.resetFields()
                    setModelShow(false)
                    await loadProductList(productSearch.pageIndex)
                } else {
                    messageApi.error(message)
                }
            }
        }

    }
    const handlerEdit = async (data: IProductInfo) => {
        ruleForm.resetFields()
        ruleForm.setFieldsValue(data)
        setTitle('编辑供应商')
        await loadTreeCates()
        await loadUnits()
        await loadSupilers()
        setModelShow(true)
    }
    const onStatuChange = async ({ id }: IProductInfo) => {
        const { success, message } = await alovaInstance.Post<IReturnResult, string>(`${urls.updateProductStatus}/${id}`).send()
        if (success) {
            messageApi.success(message)
            await loadProductList(productSearch.pageIndex)
        } else {
            messageApi.error(message)
        }
    }
    const onSearch = async () => {
        setProductSearch({
            ...productSearch,
            pageIndex: 1
        })
        await loadProductList(1)
    }
    const closeForm = () => {
        ruleForm.resetFields()
        setModelShow(false)
    }
    return {
        productMain,
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
        productSearch,
        treeData,
        productCate,
        productUnit,
        productSupiler,
        handlerTreeSelect,
        defaultCates

    }
}
export {
    useProductManager
}
