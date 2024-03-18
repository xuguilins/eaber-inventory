import { Button, Result } from "antd";
import React from "react";
import { useNavigate } from "react-router-dom";

const NotFound: React.FC = () => {
  const navtion = useNavigate()
  return (
    <Result
      status="404"
      title="404"
      subTitle="页面不存在或正在开发中...."
      extra={<Button type="primary" onClick={()=>{
        navtion('/')
      }}>Back Home</Button>}
    />
  );
};
export default NotFound;
