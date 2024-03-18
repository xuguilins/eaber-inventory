import { Card, Col, Progress, Row } from "antd";
import "./index.css";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
import { useEffect, useState } from "react";
interface ISystemInfo {
  productName: string;
    productVersion: string;
    productType: string;
    authTarget: string;
    backService: string;
    frontService: string;
    deployment: string;
    databaseType: string;
    databaseVersion: string;
    hostName: string;
    systemName: string;
    macAddress:string;
    offSetDay:string;
    endTime:string;
}
const UserPage: React.FC<any> = () => {
  const [systemData,setSystemData] = useState<ISystemInfo>({
    productName: '',
    productVersion: '',
    productType: '',
    authTarget: '',
    backService: '',
    frontService: '',
    deployment: '',
    databaseType: '',
    databaseVersion: '',
    hostName: '',
    systemName: '',
    macAddress:'',
    offSetDay:'',
    endTime:'',
  });
  const loadInfo = async ()=>{

    const {success,data} = await alovaInstance.Get<IReturnResult,any>(urls.getSystemInfo)
    .send()
    if (success) {
      setSystemData(data)
    }
  }
  
  useEffect(()=>{
    async function inti() {
      await loadInfo();
    }
    inti()
  },[])
  return (
    <>
      <h3
        style={{ margin: 15, borderLeft: "5px solid #2e89ed", paddingLeft: 5 }}
      >
        系统信息
      </h3>
      <Row gutter={20} style={{ margin: 5 }}>
        <Col span={6}>
          <Card title="系统信息">
            <span className="spanTitle">产品名称: <b>{systemData.productName}</b></span>
            <span className="spanTitle">产品版本: <b>{systemData.productVersion}</b> </span>
            <span className="spanTitle">产品类型: <b>{systemData.productType}</b></span>
          </Card>
        </Col>
        <Col span={6}>
          <Card title="主机信息">
            <span className="spanTitle">主机名称: <b>{systemData.hostName}</b></span>
            <span className="spanTitle">MAC地址: <b>{systemData.macAddress}</b> </span>
            <span className="spanTitle">操作系统: <b>{systemData.systemName}</b></span>
        
          </Card>
        </Col>
        <Col span={6}>
          <Card title="授权信息">
            <span className="spanTitle">授权客户:<b>{systemData.authTarget}</b>  </span>
            <span className="spanTitle">
              剩余天数:  <span className="mIm">{systemData.offSetDay}</span>
            </span>
            <span className="spanTitle">
              到期日期:  <span className="mIm">{systemData.endTime}</span>
            </span>
          </Card>
        </Col>
        <Col span={6}>
          <Card title="技术架构">
            <span className="spanTitle">后端服务: <b> {systemData.backService}</b> </span>
            <span className="spanTitle">前端服务: <b>{systemData.frontService}</b></span>
            <span className="spanTitle">部署方式:  <b>{systemData.deployment}</b> </span>
            <span className="spanTitle">数据库类型: <b>{systemData.databaseType}</b></span>
            <span className="spanTitle">数据库版本: <b>{systemData.databaseVersion}</b> </span>
          </Card>
        </Col>
      </Row>
    </>
  );
};
export default UserPage;
