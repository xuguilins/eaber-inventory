import { makeObservable } from "mobx";

class UserStore {
    private access_Token: string;
    private refresh_Token: string;
    private login_Name: string;
    constructor() {
        makeObservable(this)
        this.access_Token = ''
        this.refresh_Token = ''
        this.login_Name = ''
    }
    getLoginName() {
        if (!this.login_Name) {
            this.login_Name = sessionStorage.getItem('login_Name') || ''
        }
        return this.login_Name
    }
    getAccessToken() {
        if (this.access_Token) {
            return this.access_Token
        } else {
            this.access_Token = sessionStorage.getItem('access_Token') || ''
            return this.access_Token
        }
    }
    getRefreshToken() {
        if (this.refresh_Token) {
            return this.refresh_Token
        } else {
            this.refresh_Token = sessionStorage.getItem('refresh_Token') || ''
            return this.refresh_Token
        }
    }
    setAccessToken(token: string) {
        this.access_Token = token
        sessionStorage.setItem('access_Token', token)
    }
    setRefreshToken(token: string) {
        sessionStorage.setItem('refresh_Token', token)
    }
    setLoginName(name: string) {
        this.login_Name = name
        sessionStorage.setItem('login_Name', name)
    }
}
export default UserStore