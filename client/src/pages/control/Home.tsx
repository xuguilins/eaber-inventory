import {
  Button,
  Card,
  Col,
  DatePicker,
  Divider,
  Row,
  Tabs,
  TabsProps,
} from "antd";

import "./index.css";
import React, { Suspense, lazy, useEffect, useState } from "react";
const { Meta } = Card;
import * as echarts from "echarts";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
//import XSColumns from "./components/XSColumns.tsx";
// import OrderCharts from "./components/OrderCharts";
const { RangePicker } = DatePicker;
interface ICardModel {
  allProduct: number;
  dayProduct: number;
  allSell: number;
  daySell: number;
  allProfile: number;
  dayProfile: number;
  allOrder: number;
  dayOrder: number;
}
interface IColumnDto {
  columnType: number;
  filterType: number;
  startTime: string;
  endTime: string;
  tabType:number;
}
const OrderCharts = lazy(() => import("./components/OrderCharts.tsx"));
const JHColumns = lazy(()=>import('./components/JHColumns.tsx'))
 const  XSColumns  =lazy(()=>import('./components/XSColumns.tsx'))
const Home: React.FC = () => {
  const [active, setActive] = useState<number>(1);
  const [cardModel, setCardModel] = useState<ICardModel>({
    allOrder: 0,
    allProduct: 0,
    allProfile: 0,
    allSell: 0,
    dayProduct: 0,
    dayOrder: 0,
    dayProfile: 0,
    daySell: 0,
  });
  const [cardLoading, setCardLoading] = useState<boolean>(false);
  const [columnData, setColumnData] = useState<IColumnDto>({
    columnType: 1,
    filterType: 1,
    startTime: "",
    endTime: "",
    tabType:1
  });
  const [orderData, setOrderData] = useState<any>({});
  const [productData, setProductData] = useState<any>([]);
  const [jhData,setJhData] = useState<any>({});
  const [xsData,setXSData] = useState<any>({});
  const renderTabBar: TabsProps["renderTabBar"] = (props, DefaultTabBar) => (
    <>
      <DefaultTabBar {...props}></DefaultTabBar>
      <div className="control">
        <RenderTypes />
        <RangePicker
          onChange={(date, dateStr) => onDateChange(date, dateStr)}
        />
      </div>
    </>
  );
  const defaultTypes = [
    { name: "今年", value: 1 },
    { name: "本月", value: 2 },
    { name: "本周", value: 3 },
  ];
  const RenderTypes: React.FC = () => {
    return defaultTypes.map((item) => {
      return (
        <span
          key={item.value}
          className={
            active === item.value ? "controlBar barActive" : "controlBar"
          }
          onClick={(e) => onBarClick(item)}
        >
          {item.name}
        </span>
      );
    });
  };
  const onBarClick = (record: any) => {
    setActive(record.value);
    setColumnData({
      ...columnData,
      filterType: record.value,
    });
  };
  const initCards = async () => {
    setCardLoading(true);
    const { success, data } = await alovaInstance
      .Get<IReturnResult>(urls.getHomeCards)
      .send();
    if (success) {
      const { productCount, orderCount, profilePrice, sellPrice } = data;
      const item: ICardModel = {
        allProduct: Number(productCount.split("-")[0]),
        dayProduct: Number(productCount.split("-")[1]),
        allOrder: Number(orderCount.split("-")[0]),
        dayOrder: Number(orderCount.split("-")[1]),
        allProfile: Number(profilePrice.split("-")[0]),
        dayProfile: Number(profilePrice.split("-")[1]),
        allSell: Number(sellPrice.split("-")[0]),
        daySell: Number(sellPrice.split("-")[1]),
      };
      setTimeout(() => {
        setCardModel(item);
        setCardLoading(false);
      }, 1500);
    }
  };
  //加载销售额
  const initSellColumns = async () => {
    const { success, data } = await alovaInstance
      .Post<IReturnResult, IColumnDto>(urls.getColumns, columnData)
      .send();
    if (success) {
        setXSData(data)
    }
  };
  //加载订单量
  const initOrderColumns = async () => {
    const { success, data } = await alovaInstance
      .Post<IReturnResult, IColumnDto>(urls.getOrderColumns, columnData)
      .send();
    if (success) setOrderData(data);
  };
  //加载商品排行
  const initHightList = async () => {
    const { success, data } = await alovaInstance
      .Post<IReturnResult, IColumnDto>(urls.getHeightProduct, columnData)
      .send();
    if (success) setProductData(data);
  };
  //加载进货数量
  const initTypeColumns = async () => {
    const { success, data } = await alovaInstance
    .Post<IReturnResult, IColumnDto>(urls.getColumns, columnData)
    .send();
    if (success) 
        setJhData(data)
  }
  const onDateChange = (date: any, dateStr: string[]) => {
    const value = date === null ? 1 : 4;
    setActive(value);
    setColumnData({
      ...columnData,
      filterType: value,
      startTime: dateStr[0],
      endTime: dateStr[1],
    });
  };
  const onTabChange = (e: string) => {
    const value = Number(e);
    if (value === 2) {
      //进货情况
      setColumnData({
        ...columnData,
        filterType:1,
        columnType:2,
        tabType:value 
      })
    } else if (value === 1) {

      //销售情况
      setColumnData({
        ...columnData,
        filterType:1,
        columnType:1,
        tabType:value 
      })
    } else if (value === 3) {
     // 订单量
      initOrderColumns();

      setColumnData({
        ...columnData,
        tabType:value 
      })
    } 
  };
  useEffect(() => {
    async function init() {
      // 卡片统计
      await initCards();
    }
    init();
  }, []);
  useEffect(() => {
    async function initColumns() {

      switch(columnData.tabType){
        case 1:
          await initSellColumns()
          break;
        case 2:
          await initTypeColumns()
          break;
        default:
          await initOrderColumns();
          await initHightList();
          break;
      }     
    }
    initColumns();
  }, [columnData]);
  return (
    <>
      <div className="homePage">
        <div className="homeRow">
          <Row gutter={16}>
            <Col span={6}>
              <Card bordered={false} loading={cardLoading}>
                <Meta
                  title={
                    <>
                      <span className="spanTitle">商品总数</span>
                    </>
                  }
                  description={
                    <>
                      <p className="scdesc">{cardModel.allProduct}</p>
                      <Divider />
                      <span className="daySpan">日新增商品数 </span>
                      <span className="daySpan left">
                        {cardModel.dayProduct}
                      </span>
                    </>
                  }
                />
              </Card>
            </Col>
            <Col span={6}>
              <Card bordered={false} loading={cardLoading}>
                <Meta
                  title={
                    <>
                      <span className="spanTitle">销售总额</span>
                    </>
                  }
                  description={
                    <>
                      <p className="scdesc">{cardModel.allSell}</p>
                      <Divider />
                      <span className="daySpan">日销售额 </span>
                      <span className="daySpan left">{cardModel.daySell} </span>
                    </>
                  }
                />
              </Card>
            </Col>
            <Col span={6}>
              <Card bordered={false} loading={cardLoading}>
                <Meta
                  title={
                    <>
                      <span className="spanTitle">利润总额</span>
                    </>
                  }
                  description={
                    <>
                      <p className="scdesc">{cardModel.allProfile}</p>
                      <Divider />
                      <span className="daySpan">日利润额 </span>
                      <span className="daySpan left">
                        {cardModel.dayProfile}
                      </span>
                    </>
                  }
                />
              </Card>
            </Col>
            <Col span={6}>
              <Card bordered={false} loading={cardLoading}>
                <Meta
                  title={
                    <>
                      <span className="spanTitle">有效订单总量</span>
                    </>
                  }
                  description={
                    <>
                      <p className="scdesc">{cardModel.allOrder}</p>
                      <Divider />
                      <span className="daySpan">日有效订单量 </span>
                      <span className="daySpan left">{cardModel.dayOrder}</span>
                    </>
                  }
                />
              </Card>
            </Col>
          </Row>
        </div>
        <div className="homeRow rowPadding">
          <Tabs
            defaultActiveKey="1"
            onChange={(e) => onTabChange(e)}
            items={[
              {
                label: "销售额",
                key: "1",
                children: (
                  <>
                  
                    <Suspense>
                    <XSColumns  order={xsData}/>
                    </Suspense>
                    
                  </>
                ),
              },
              {
                label: "进货情况",
                key: "2",
                children: (
                  <>
                     <Suspense>
                      <JHColumns order={jhData} />
                     </Suspense>
                  </>
                ),
              },
              {
                label: "订单量",
                key: "3",
                children: (
                  <>
                    <Suspense>
                      <OrderCharts order={orderData} product={productData} />
                    </Suspense>
                  </>
                ),
              },
            ]}
            renderTabBar={renderTabBar}
          ></Tabs>
        </div>
       
      </div>
    </>
  );
};
export default Home;
