import {  Button, Steps, message } from "antd";
import "./index.css";
import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import ThirdCard from "./ThirdCard.tsx";
import FourCard from "./FourCard.tsx";
import FirstCard from "./FirstCard.tsx";
import { useStore } from "@/store/index.ts";
import alovaInstance, { IReturnResult } from "@/utils/request.ts";
import { urls } from "@/api/urls.ts";
const ShowCard: React.FC<any> = (mdoel: any) => {
  const type = mdoel.type;
  if (type === 0) return <FirstCard />;
  if (type === 1) return <ThirdCard />;
  if (type === 2) return <FourCard />;
};

const ProdcutSell: React.FC = () => {
  const [currentStep, setCurrentStep] = useState<number>(0);
  const [stepOption] = useState<any>([
    { title: "选择产品", descirion: "请先选择产品" },
    { title: "订单确认", descirion: "填写核对订单" },
    { title: "开单结果" },
  ]);
  const { sellStore } = useStore();
  const nextClick = async () => {
    // 验证购买单位
    if (checkForm()) {
      // 验证库存
      const stock = await checkStock();
      if (stock) {
        if (currentStep === 1) {
          const order = await createOrder();
          if (order) {
            setCurrentStep(currentStep + 1);
          }
        } else {
          setCurrentStep(currentStep + 1);
        }
      }
    }
  };
  const prevClick = () => {
    setCurrentStep(currentStep - 1);
  };
  const checkForm = () => {
    if (!sellStore.sellData.sellUser) {
      message.error("请填写购买单位");
      return false;
    }
    if (!sellStore.sellData.sellPhone) {
      message.error("请填写联系方式");
      return false;
    }
    if (sellStore.sellData.detail.length <= 0) {
      message.error("请选择商品");
      return false;
    }
    let deitalTrue = true;
    for (let i = 0; i < sellStore.sellData.detail.length; i++) {
      const j = i + 1;
      const sell = sellStore.sellData.detail[i];
      if (Number(sell.inventoryCount) <= 0) {
        deitalTrue = false;
        message.error(`第${j}行商品数量不符合`);
        break;
      }
    }
    return deitalTrue;
  };
  const checkStock = async () => {
    const postData = [];
    const list = sellStore.sellData.detail;
    for (let i = 0; i < list.length; i++) {
      const item = list[i];
      const json = {
        productCode: item.productCode,
        count: item.inventoryCount,
      };
      postData.push(json);
    }
    const { success } = await alovaInstance
      .Post<IReturnResult, any>(urls.checkProduct, postData)
      .send();
    return success;
  };
  const ShowBtn = (type: number) => {
    if (type === 0) {
      return (
        <>
          <Button type="primary" onClick={nextClick}>
            下一步
          </Button>
        </>
      );
    } else if (type === 1) {
      return (
        <>
          <Button style={{ margin: 10 }} onClick={prevClick}>
            上一步
          </Button>
          <Button type="primary" onClick={nextClick}>
            确认订单
          </Button>
        </>
      );
    } else {
      return (
        <>
          <Button type="primary" onClick={()=>{
            window.location.reload()
          }}>完成</Button>
        </>
      );
    }
  };
  const createOrder = async (): Promise<boolean> => {
    const data = sellStore.sellData;
    const products = sellStore.sellData.detail;
    const postData: any = {
      sellTime: data.sellTime,
      sellUser: data.sellUser,
      sellPhone: data.sellPhone,
      actuailMoney:data.actuailMoney,
      offsetMoney:data.offsetMoney,
      remark: data.remark,
      products: [],
    };
    products.forEach((item) => {
      postData.products.push({
        productCode: item.productCode,
        productName: item.productName,
        orderCount: item.inventoryCount,
        orderSigle: item.sellPrice,
        orderPrice: item.inventoryCount * item.sellPrice,
        remark: item.remark,
      });
    });
    const {
      success,
      data: serverData,
      message: serverMsg,
    } = await alovaInstance
      .Post<IReturnResult, any>(urls.createOrder, postData)
      .send();
    if (success) {
      message.success(serverMsg);
      sellStore.setMainCode(serverData)
    } else {
      message.error(serverMsg);
    }
     return success;
  };
  return (
    <>
      <div className="mainContent">
        <div className="headerBar">
          <Steps current={currentStep} items={stepOption} />
        </div>
        <div className="stepMain">
          <ShowCard type={currentStep} />
        </div>
        <div className="footbtn">{ShowBtn(currentStep)}</div>
      </div>
    </>
  );
};
export default observer(ProdcutSell);
