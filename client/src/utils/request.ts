
import { createAlova, useRequest } from "alova";
import GlobalFetch from 'alova/GlobalFetch';
import ReactHook from 'alova/react';
import { useFullLoading } from ".";
import { urls } from "@/api/urls";
import { message } from 'antd'
//import { useNavigate } from "react-router-dom";
//mport React from "react";

export interface IReturnResult {
    data: any,
    total: number;
    success: boolean;
    message: string;
}
let loadInstances: HTMLElement[] = []
const waitingList = []
let isRefresh =false 
const alovaInstance = createAlova({
    // ReactHook用于创建ref状态，包括请求状态loading、响应数据data、请求错误对象error等
    statesHook: ReactHook,
    localCache: null,
    baseURL: window.webConfig.host,
    beforeRequest: function (method: any) {
        let message = '处理中...'
        if(method.config.headers.title) {
            message  = method.config.headers.title
            method.config.headers = {}
        }
        const loading = useFullLoading(message)
        if (method.url.indexOf('importExcel') < 0)
            method.config.headers['Content-Type'] = 'application/json;charset=UTF-8'
        loadInstances.push(loading)
        method.config.headers["Authorization"] = "Bearer " + sessionStorage.getItem('access_Token')
    },
    // 请求适配器，推荐使用fetch请求适配器
    requestAdapter: GlobalFetch(),//onSuccess和onError
    // 全局的响应拦截器
    responded: {
        onSuccess: async (data: any, method: any) => {
            debugger
            if (data.status === 401) {
                if(!isRefresh) {
                    console.log('身份过期了')
                    isRefresh = true 
                    const { refresh_Token, access_Token } = await refreshToken()
                    sessionStorage.setItem('access_Token', access_Token)
                    sessionStorage.setItem('refresh_Token', refresh_Token)
                    waitingList.forEach((cb) => cb())
                    waitingList.length =0
                    //存储当前请求 
                    isRefresh = false
                    return  method.send()
                }else {
                    debugger
                    const resolve =createResolve(method)
                    waitingList.push(resolve)
                }
            } else {          
                if (method.url === urls.exportOrderCus ||
                     method.url === urls.exportHeightCus ||
                     method.url === urls.exportCusExcel ||
                     method.url === urls.exportCusHeightExcel) {
                    const blob = await data.blob()
                    const njson: IReturnResult = {
                        data: blob,
                        total: 0,
                        message: '',
                        success: true 
                    }
                    return njson
                }else {
                    const json = await data.json()
                    if (!json.success)
                        message.error(json.message)
                    const njson: IReturnResult = {
                        data: json.data,
                        total: json.totalCount,
                        message: json.message,
                        success: json.success
                    }
                    return njson
                }
               
            }
        },
        onError(error: any, a: any) {
            debugger
            message.error(`请求失败： `+error.message)
            return Promise.reject()
        },
        onComplete() {
            if (loadInstances.length > 0) {
                loadInstances.forEach(item => {
                    const id = setTimeout(() => {
                        item.remove()
                        clearTimeout(id)
                    }, 300)
                })
            }
        }
    }
});
export default alovaInstance
const createResolve=(method )=>{
    debugger
    const {type ,url,data ,config   } = method 
    return new Promise((resolve,reject)=>{
        debugger
        if (type.toUpperCase() === 'POST') {
            alovaInstance.Post<IReturnResult,any>(url, data, config)
            .send()
            .then(result=>{
                resolve(result)
            }) 
        }else if (type.toUpperCase() === 'GET') {
            alovaInstance.Get<IReturnResult,any>(url,config)
            .send()
            .then(result=>{
                resolve(result)
            }) 
        }else {
            alovaInstance.Delete<IReturnResult,any>(url,data, config)
            .send()
            .then(result=>{
                resolve(result)
            }) 
        }
    })
}
export const refreshToken = async () => {
    const { data } = await alovaInstance.Get<IReturnResult>(urls.refreshToken, {
        headers: {
            'AuthorizationX': sessionStorage.getItem('refresh_Token')
        }
    }).send();
    return data
}