import { useEffect, useState } from "react";
import { useTime, useUUID } from "@/utils";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
import { Form, message } from "antd";
import { useLocation  } from "react-router-dom";
import dayjs from "dayjs";
interface ICreateInForm {
    id?
    
    :string;
    inOrderTime: string;
    channelType: number;
    logistics: string;
    inUser: string;
    inPhone: string;
    supplierId: string;
    supileName: string;
    inCount: number;
    inPrice: number;
    inOStatus: number;
    payStatus: number;
    remark: string;
    prdocutDetail: IProductInForm[];
}

interface IProductInForm {
    id: string;
    productCode: string;
    productModel: string;
    productName: string;
    productCount: number;
    remark: string;
    cateId: string;
    unitId: string;
    invertCount: number;
    productIncost: number;
    productWocost: number;
    productPrice: number;
    sellPrice: number;
    productAll: number;
}
interface ISupileSelect {
    id: string;
    supileCode: string;
    supileName: string;
    supileUser: string;
    supileUTEN: string;
    supileTel: string;
    supileSen: string;
}

const useInForm = () => {
    const [puraseForm] = Form.useForm();
    const [chanel, setChanel] = useState<number>(-1);
    const [tabRow, setTabRow] = useState<Array<IProductInForm>>([]);
    const [defaultProduct, setDefaultProduct] = useState<Array<any>>([]);
    const [searchProduct, setSearchProduct] = useState<Array<any>>([]);
    const [unitData, setUnitData] = useState<Array<any>>([]);
    const [treeData, setTreeData] = useState<Array<any>>([]);
    const [name, setName] = useState<string>();
    const [supileData, setSupileData] = useState<Array<ISupileSelect>>([]);
    const [supileShow, setSupileShow] = useState<boolean>(false)
    const [isCreate,setIsCreate] = useState<boolean>(false)
    const [titile,setTitle] = useState<string>('创建进货单')
    const location = useLocation()
    const defaultaForm: ICreateInForm = {
        inOrderTime: "",
        channelType: 0,
        logistics: "",
        inUser: "",
        inPhone: "",
        supplierId: "",
        supileName: "",
        inCount: 0,
        inPrice: 0,
        inOStatus: 0,
        payStatus: 0,
        remark: "",
        prdocutDetail: [],
    };
    const onSave = async () => {
       
        const v = puraseForm.getFieldsValue();
        if (!v.inOrderTime) {
            message.error("请选择进货时间");
            return
        }
        if (v.channelType === undefined) {
            message.error("请选择供货渠道");
            return
        }
        if (!v.supileName) {
            message.error("客户名称不能为空或供应商不能为空");
            return
        }
        const newForm = { ...defaultaForm };
        Object.keys(v).forEach((key: string) => {
            const value = v[key];
            if (value !== undefined) {
                newForm[key] = key === "inOrderTime" ? useTime(new Date(value)) : value;
            }
        });
        newForm.prdocutDetail = tabRow;

        if (v.id) {
            
            const {success,message:erMessage} = await alovaInstance.Post<IReturnResult,ICreateInForm>(urls.updatePushOrder,newForm).send()
            if (success) {
                message.success(erMessage)
            } else {
                message.error(erMessage)
            }
    
        }else {
            const {success,message:erMessage} = await alovaInstance.Post<IReturnResult,ICreateInForm>(urls.createPushOrder,newForm).send()
            if (success) {
                message.success(erMessage)
               setIsCreate(true)
            } else {
                message.error(erMessage)
            }
    
        }
       

    };
    const addRow = () => {
        const defaultRow: IProductInForm = {
            id: useUUID(),
            productCode: "",
            productModel: "",
            productName: "",
            productCount: 0,
            remark: "",
            cateId: "",
            unitId: "",
            productIncost: 0,
            productWocost: 0,
            productPrice: 0,
            sellPrice: 0,
            productAll: 0,
            invertCount: 0,
        };
        const newRows = tabRow;
        newRows.push(defaultRow);
        setTabRow(newRows);
        setName(defaultRow.id);
    };
    const copryRows =(rows:any)=>{
        const newRows:IProductInForm[]= []
        rows.forEach(data=>{
            const defaultRow: IProductInForm = {
                id: data.id,
                productCode: data.productCode,
                productModel:data.productModel,
                productName: data.productName,
                productCount:data.productCount,
                remark: data.remark,
                cateId:data.cateId,
                unitId: data.unitId,
                productIncost: data.productIncost,
                productWocost: data.productWocost,
                productPrice: data.productPrice,
                sellPrice: data.sellPrice,
                productAll: data.productAll,
                invertCount: data.invertCount,
            };
            newRows.push(defaultRow)
        })
       setTabRow(newRows)
    }
    const onRemoveRow = (record: string, index: number) => {
        const newRows = tabRow.filter((item: any, i: number) => i !== index);
        setTabRow(newRows);
    };
    useEffect(() => {
        async function init() {
            const products = await onSearchProduct();
            setSearchProduct(products);
            await loadUnits();
            await loadCates();
        }
        init();
    }, []);
    const loadModel =async (id:string )=>{
        const {success,data} = await alovaInstance.Get<IReturnResult>(urls.getPurase+"/"+id).send()
        if (success) {
            Object.keys(data).forEach(key=>{
                if (key ==='prdocutDetail') {
                    copryRows(data[key])
                   // setTabRow(data[key])
                } else  if (key === 'inOrderTime') {
                    puraseForm.setFieldValue(key,dayjs(data[key]))
                }else {
                    puraseForm.setFieldValue(key,data[key])
                }
            })
           
            
        }
    }
    useEffect(()=>{
        async function loadSignler() {
            if (location.state) {
                const {id}=location.state
                if (id) {
                    setTitle('编辑进货单')
                    await loadModel(id)
                }
            }
            
        }
        loadSignler()
    },[])

    const selectSupiler = async () => {
        const { success, data } = await alovaInstance.Get<IReturnResult>(urls.getEnableSupilers).send();
        if (success) {
            setSupileData(data);
        }
        setSupileShow(true)
    }
    const onSupileSelect = (record: ISupileSelect) => {
        setSupileShow(false)
        puraseForm.setFieldValue('inUser', record.supileUser)
        puraseForm.setFieldValue('inPhone', record.supileTel)
        puraseForm.setFieldValue('supileName', record.supileName)
        puraseForm.setFieldValue('supplierId', record.id)
    }
    // selectSupiler
    const onSearchProduct = async (name: string = "") => {
        const { success, data } = await alovaInstance
            .Get<IReturnResult, string>(urls.searchProduct, {
                params: {
                    name: name,
                },
            })
            .send();
        const product = [];
        if (success) {
            setDefaultProduct(data);
            data.forEach((item) => {
                product.push({
                    label: item.productName + "《" + item.productCode + "》",
                    value: item.productName,
                });
            });
        }
        return product;
    };
    const buildDefaultproduct = (data: Array<any>) => {
        const product = [];
        if (data.length > 0) {
            data.forEach((item) => {
                product.push({
                    label: item.productName + "《" + item.productCode + "》",
                    value: item.productName,
                });
            });
        } else {
            defaultProduct.forEach((item) => {
                product.push({
                    label: item.productName + "《" + item.productCode + "》",
                    value: item.productName,
                });
            });
        }
        return product;
    };
    const onFoucus = async () => {
        if (searchProduct.length <= 0 || searchProduct.length != defaultProduct.length) {
            const products = await onSearchProduct();
            setSearchProduct(products);
        }
    };
    const onProductSearch = (text: string) => {
        let products = [];
        if (text) {
            const data = defaultProduct.filter(
                (x) => x.productName.indexOf(text) > -1
            );
            products = buildDefaultproduct(data);
            setSearchProduct(products);
        } else {
            products = buildDefaultproduct([]);
            setSearchProduct(products);
        }
    };
    const onSelectValue = (
        a: string,
        b: any,
        value: IProductInForm,
        index: number
    ) => {
        const startIndex = b.label.indexOf("《");
        const endIndex = b.label.lastIndexOf("》");
        const productCode = b.label.substring(startIndex + 1, endIndex);
        const product = defaultProduct.find((x) => x.productCode === productCode);
        if (product) {
            const newRows = [...tabRow]
            newRows[index].productCode = product.productCode;
            newRows[index].invertCount = product.inventoryCount;
            newRows[index].productModel = product.productModel;
            newRows[index].cateId = product.cateId;
            newRows[index].unitId = product.unitId;
            newRows[index].productIncost = product.initialCost; //成本
            newRows[index].productWocost = product.wholesale; //批发价
            newRows[index].productPrice = product.purchase; //进价
            newRows[index].sellPrice = product.sellPrice; // 售价
            newRows[index].productName = product.productName;
            setTabRow(newRows);
        }
    };
    const onRowColumnsChange = (e: any, index: number, key: string) => {
        const newRows = [...tabRow]
        if (newRows[index] && newRows[index][key] !== undefined)
            newRows[index][key] = e;
        setTabRow(newRows);
    }
    const loadUnits = async () => {
        const { success, data } = await alovaInstance
            .Get<IReturnResult, any>(urls.getUnits)
            .send();
        if (success) {
            const options = buildOptions(data, "name", "id");
            setUnitData(options);
        }
    };
    const onFocusUnit = async () => {
        if (unitData.length <= 0) {
            await loadUnits();
        }
    };
    const loadCates = async () => {
        const { success: cateSuccess, data: cateData } = await alovaInstance
            .Get<IReturnResult, any>(urls.getCateTree)
            .send();
        if (cateSuccess) setTreeData(cateData);
    };
    const onFocusCate = async () => {
        if (treeData.length <= 0) await loadCates();
    };
    const buildOptions = (data: Array<any>, key: string, value: string) => {
        const options = [];
        data.forEach((item) => {
            options.push({
                label: item[key],
                value: item[value],
            });
        });
        return options;
    };
    const onEdit =(id:string)=>{
         
    }
    const onCloseClick= ()=>{
        setSupileShow(false)
    }
    return {
        puraseForm,
        chanel,
        setChanel,
        tabRow,
        searchProduct,
        onFoucus,
        unitData,
        treeData,
        onFocusCate,
        onFocusUnit,
        onRemoveRow,
        onProductSearch,
        onSelectValue,
        onSave,
        addRow,
        onRowColumnsChange,
        onSupileSelect,
        supileData,
        selectSupiler,
        supileShow,
        isCreate,
        onEdit,
        titile,
        onCloseClick
    }
}
export default useInForm