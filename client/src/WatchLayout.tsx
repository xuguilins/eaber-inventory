import { useEffect } from "react";
import { Navigate, useLocation, useNavigate } from "react-router-dom";
import HubClient from "./utils/hubCenter";
import KeepAlive from "react-activation";

/**
 *  高阶授权监听组件
 * @param param0 高阶组件
 * @returns
 */
const WatchLayout: React.FC<any> = ({ authCom }: any) => {
  const navtion = useNavigate();
  const location = useLocation();
  const token = sessionStorage.getItem("access_Token");
  if (token) {
    if (location.pathname === "/login") {
      navtion("/", { replace: true });
    } else {
      
      return  authCom
    }
  } else if(location.pathname === '/login'){
    return  authCom
  }else {
    return <Navigate to='/login'></Navigate>
  }

};
export default WatchLayout;
