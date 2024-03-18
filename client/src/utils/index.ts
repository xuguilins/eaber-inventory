
import * as signalR from "@microsoft/signalr";
import { FullLoading } from "@/pages/fullLoading";
import React from "react";
import { createRoot } from "react-dom/client";

const useFullLoading = (title:string ='处理中...'): HTMLElement => {
    // 创建最大布局元素
    const div = document.createElement("div")
    div.className = 'fulldiv'
    document.body.append(div)
    const el = React.createElement(FullLoading,{title:title})
    createRoot(div).render(el)
    return div

}
export const hub = () => {
    const host = 'http://localhost:5269/hubclient'
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(host)
        .build();
    connection.on("SendAll", (mes) => {

        console.log('收到消息', mes)
    });
    connection.start()
}
export const useTime = (data:any) => {
    if(typeof data === 'string' || typeof data === 'object')
       data = new Date(data)
    const year = data.getFullYear()
    const month = data.getMonth() + 1
    const day = data.getDate()
    const monthStr = month >= 10 ? month : '0' + month
    const dayStr = day >= 10 ? day : '0' + day
    return `${year}/${monthStr}/${dayStr}`
}
export function useUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0,
        v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
export {

    useFullLoading

}