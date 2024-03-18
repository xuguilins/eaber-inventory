
import App from "../App";
import { createElement } from "react";
import RuleInfo from "@/pages/base/rules/RuleInfo";
import UnitInfo from '@/pages/base/units/UnitInfo';
import CateInfo from "@/pages/base/cates/CateInfo";
import Home from "@/pages/control/Home";
import SupilerInfo from "@/pages/base/supiles/SupilerInfo";
import ProductInfo from '@/pages/products/ProductInfo';
import ProdcutSell from "@/pages/sell/ProductSell";
import PayInfo from "@/pages/base/pays/PayInfo";
import PurchaseInOrderPage from "@/pages/Purchases/PurchaseInOrderPage"
import Order from "@/pages/orders/index.tsx";
import Login from "@/pages/Login";
import PurchaseInForm from '@/pages/Purchases/PurchaseInForm';
import NotFound from "@/pages/NotFound";
import OutPurchasePage from "@/pages/OutPurchase/OutPurchasePage";
import OutPurchaseForm from "@/pages/OutPurchase/OutPurchaseForm";
import EditPuraseForm from "@/pages/OutPurchase/EditPuraseForm";
import CustomerPage from "@/pages/base/customes/CustomerPage";
import ExtraInPage from "@/pages/base/extras/extraInPage";
import ExtraOutPage from "@/pages/base/extras/extraOutPage";
import ExtraPage from "@/pages/Extras/ExtraPage";
import { KHPage } from "@/pages/Reciprocal/KHPage";
import UserPage from "@/pages/users/userPage";
import { GYSJHPage } from "@/pages/Reciprocal/GYSJHPage";
const dRoutes: any = [
    {
        element: createElement(App),
        path: '/',
        children: [
            {
                path: '/', element: createElement(Home),
                index: true,
                meta: {
                    title: '工作台'
                }
            },
            {
                path: '/base/rule', index: true, element: createElement(RuleInfo),
                meta: {
                    title: '编码管理'
                }
            },
            {
                path: '/base/unit', element: createElement(UnitInfo), meta: {
                    title: '单位管理'
                }
            },
            {
                path: '/base/cate', element: createElement(CateInfo), meta: {
                    title: '分类管理'
                }
            },
            {
                path: '/base/customer', element: createElement(CustomerPage), meta: {
                    title: '客户管理'
                }
            },
            {
                path: '/base/pay', element: createElement(PayInfo), meta: {
                    title: '支付管理'
                }
            },

            {
                path: '/base/extrain', element: createElement(ExtraInPage), meta: {
                    title: '其它收入类型维护'
                }
            },
            {
                path: '/base/extraout', element: createElement(ExtraOutPage), meta: {
                    title: '其它支出类型维护'
                }
            },



            {
                path: '/base/supiler', element: createElement(SupilerInfo), meta: {
                    title: '供应商管理'
                }
            },
            {
                path: '/product', element: createElement(ProductInfo), meta: {
                    title: '商品管理'
                }
            },
            {
                path: '/sell/form', element: createElement(ProdcutSell), meta: {
                    title: '销售开单'
                }
            },
            {
                path: '/all/order', element: createElement(Order), meta: {
                    title: '全部订单'
                }
            },

            {
                path: '/extra/extralist', element: createElement(ExtraPage), meta: {
                    title: '其它信息维护'
                }
            },
            {
                path: '/purchases/InOrder', element: createElement(PurchaseInOrderPage), meta: {
                    title: '进货单'
                }
            },
            {
                path: '/purchases/InForm', element: createElement(PurchaseInForm), meta: {
                    title: '创建进货单'
                }
            },
            {
                path: '/purchases/outOrder', element: createElement(OutPurchasePage), meta: {
                    title: '退货单'
                },


            },
            {
                path: '/purchases/editOutOrder', element: createElement(EditPuraseForm), meta: {
                    title: '编辑退货单'
                },
            },

            {
                path: '/purchases/outForm', element: createElement(OutPurchaseForm), meta: {
                    title: '创建退货单'
                }
            },
            {
                path: '/customer/jiaoyi', element: createElement(KHPage), meta: {
                    title: '客户往来交易'
                }
            },
            {
                path: '/gysjh/jiaoyi', element: createElement(GYSJHPage), meta: {
                    title: '供应商往来交易(进货)'
                }
            },
            
            {
                path: '/user/userinfo', element: createElement(UserPage), meta: {
                    title: '个人中心'
                }
            },
            

            {
                path: '*', element: createElement(NotFound), meta: {
                    title: 'NOTFOUND'
                }
            }
        ]

    }, {
        path: '/login', element: createElement(Login), meta: {
            title: '系统登录'
        }
    }
]
export default dRoutes 