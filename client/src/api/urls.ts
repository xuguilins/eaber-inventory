import { IUrlConfig } from "./urlConfig";
const urls: IUrlConfig = {
    getCatePage: `/api/Basic/getCatePages`,
    updateCateStatus: `/api/Basic/updateCateStatus`,
    getCateTree: `/api/Basic/getTreeCates`,
    createCate: `/api/Basic/createCate`,
    deleteCate: `/api/Basic/deleteCate`,
    getSignleCate: `api/Basic/getCate`,
    updateCate: `/api/Basic/updateCate`,
    getUnits: `/api/Basic/getUnits`,
    createRule: `/api/Basic/createRule`,
    updateRule: `/api/Basic/updateRule`,
    deleteRule: `/api/Basic/deleteRule`,
    updateRuleStatus: `/api/Basic/updateRuleStatus`,
    getRulesPage: `/api/Basic/getRulesPage`,
    getSignleRule: `/api/Basic/getRuleInfo`,

    getSupilePage: `/api/Supiler/getSupilePage`,
    createSupile: `/api/Supiler/createSupiler`,
    updateSupiler: `/api/Supiler/updateSupiler`,
    deleteSupiler: `/api/Supiler/deleteSupiler`,
    getSignleSupiler: `/api/Supiler/getSupiler`,
    getEnableSupilers:`/api/Supiler/getEnableSupilers`,
    updateSupilerStatus: `/api/Supiler/updateSupilerStatus`,
    importExcel: `/api/Excel/importExcel`,
    getSupilers: `/api/Supiler/getSupilers`,
    getProductPages: `/api/Product/getProductPages`,
    getProductCates: `/api/Product/getProductCates`,
    createProduct: `/api/Product/createProduct`,
    updateProduct: `/api/Product/updateProduct`,
    deleteProduct: `/api/Product/deleteProduct`,
    updateProductStatus: `/api/Product/updateProductStatus`,
    getProductSellPage: 'api/Product/getProductSellPage',
    checkProduct: `/api/Product/checkProduct`,
    createOrder: `/api/Order/createOrder`,
    getOrderPage: `/api/Order/getOrderPage`,
    getOrderCount: `/api/Order/getOrderCount`,
    confirmOrder: `/api/Order/confirmOrder`,
    getOrderInfo: `/api/Order/getOrder`,
    getOrderUsers: `/api/Order/getOrderUsers`,
    
    updateOrder:`/api/Order/updateOrder`,
    getPayInfoPages: `/api/Basic/getPayInfoPage`,
    updatePayInfo: `/api/Basic/updatePayCmd`,
    createPayInfo: `/api/Basic/createPayCmd`,
    deletePayInfo: `/api/Basic/deletePayCmd`,
    updatePayStatus: `/api/Basic/updatePayStatus`,

    getCustomerPage:`/api/Basic/getCustomerPage`,
    updateCustomerStatus:`/api/Basic/updateCustomerStatus`,
    deleteCustomer:`/api/Basic/deleteCustomer`,
    updateCustomer:`/api/Basic/updateCustomer`,
    createCustomer:`/api/Basic/createCustomer`,

    getPays: `/api/Basic/getPays`,
    searchProduct:`/api/Product/searchProduct`,
    createPushOrder:'/api/PuraseOrder/createPushOrder',
    getInPurasePage:`/api/PuraseOrder/getInPurasePage`,
    getPurase:`/api/PuraseOrder/getPurase`,
    userlogin:`/api/User/userlogin`,
    refreshToken:'/api/Auth/refreshToken',
    getHomeCards:`/api/Home/getHomeCard` ,
    getColumns:`/api/Home/getColumns`,
    updatePushOrder:`/api/PuraseOrder/updatePushOrder`,
    deletePushOrder:`/api/PuraseOrder/deletePurashe`,
    updatePurashStatus: `/api/PuraseOrder/updatePurashStatus`,
    getPurashModal:`/api/PuraseOrder/getPurashModal`,
    getPurashModalDetail:`/api/PuraseOrder/getModalDetail`,
    updateOutPurashe: `/api/PuraseOrder/updateOutPurashe`,
    deleteOutPurase: `/api/PuraseOrder/deleteOutPurase`,
    createOutPurashe:`/api/PuraseOrder/createOutPurashe`,    
    getPuraseOutPage:`/api/PuraseOrder/getOutPurasePage`,
    getOutOrder: `/api/PuraseOrder/getOutOrder`,
    getSystemDicPage:`/api/Basic/getSystemDicPage`,
    getAllTypes: '/api/Basic/getDicTypes',
    createSystemDic:'/api/Basic/createSystemDic',
    updateSystemDic:'/api/Basic/updateSystemDic',
    updateSystemStatus:'/api/Basic/updateSystemStatus',
    deleteSystem:`/api/Basic/deleteSystem`,
    getRuleTypes:`/api/Basic/getRuleTypes`,
    getSysDics:`/api/Basic/getSysDics`,
    createExtra:`/api/Order/createExtra`,
    updateExtra:`/api/Order/updateExtra`,
    deleteExtra:`/api/Order/deleteExtra`,
    getExtraPage:`/api/Order/getExtraPage`,
    cancleExtra:`/api/Order/cancleExtra`,
    getOrderCusPage:`/api/Order/getOrderCusPage`,
    exportOrderCus: `/api/Order/exportOrderCus`, 
    getUserOrderDetail: `/api/Order/getOrderByUser`, 
    getOrderDetailUser: `/api/Order/getDetailOrder`, 
    getSystemInfo:'/api/Home/getSystemInfo',
    getOrderColumns:'/api/Home/getOrderColumns',
    getHeightProduct: '/api/Home/getHeightProduct', 
    getPurashInCusPage:`/api/PuraseOrder/getPurashInCusPage`,
    getPurashInCusTablePage: `/api/PuraseOrder/getPurashInCusTablePage`, 
    getPurashInCusDetail:`/api/PuraseOrder/getPurashInCusDetail`, 
    updateOutPurashStatus: `/api/PuraseOrder/updateOutPurashStatus`,
    exportHeightCus:`/api/Order/exportHeightCus`,
    exportCusExcel:`/api/PuraseOrder/exportCusExcel`,
    exportCusHeightExcel:`/api/PuraseOrder/exportCusHeightExcel`
}
export {
    urls
}