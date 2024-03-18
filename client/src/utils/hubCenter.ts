import * as signalR from "@microsoft/signalr";
class HubClient {
    static hubConnection: signalR.HubConnection | null;
    constructor() {
        HubClient.init()  
    }
     static getInstance () {
        return this.hubConnection
     }
    private static init() {
        if (!this.hubConnection) {
            console.log('正在连接服务端...')
            const host =  window.webConfig.host+'/hubclient'
            const connection = new signalR.HubConnectionBuilder()
                .withUrl(host,{
                    accessTokenFactory:()=>sessionStorage.getItem('access_Token')
                })
                .build();
            connection.start()
            this.hubConnection = connection;
            console.log('服务端成功连接...',connection.connectionId)
        }
    }
    /**
     * 接受服务端发送的消息
     * @param {string} method 监听服务端的方法
     * @param {Function }call 回调函数
     */
    static onReviceMessage(method: string, call: (message: string) => Promise<void>) {
    
        this.hubConnection?.on(method, function(){
             call('aaa')
        })
    }
    static closeConnection() {
        this.hubConnection?.stop()
        this.hubConnection= null 
    }
}
export default HubClient