import {
  Button,
  Col,
  DatePicker,
  Form,
  Input,
  Row,
  TabsProps,
  Tabs,
  Collapse,
} from "antd";
import "./order.css";
import { useState } from "react";
import AllOrderPage from "./AllOrderPage";
import YZFOrderPage from "./YZFOrderPage";
import DzfOrderPage from "./DzfOrderPage";
import YWCOrderPage from "./YWCOrderPage";
import YQXOrderPage from "./YQXOrderPage";
import ZFOrderPage from "./ZFOrderPage";
import eventBus from "@/utils/eventBus";
const Order: React.FC = () => {
  const [searchForm] = Form.useForm();
  const [tabType, setTabType] = useState<number>(-1);
  // const AllOrderPage: React.FC<any> = lazy(
  //   () => import("@/pages/orders/AllOrderPage.tsx")
  // );
  // const DZFOrderPage: React.FC<any> = lazy(
  //   () => import("@/pages/orders/DzfOrderPage.tsx")
  // );
  // const YZFOrderPage: React.FC<any> = lazy(
  //   () => import("@/pages/orders/YZFOrderPage.tsx")
  // );
  // const YWCOrderPage: React.FC<any> = lazy(
  //   () => import("@/pages/orders/YWCOrderPage")
  // );
  // const YQXOrderPage: React.FC<any> = lazy(
  //   () => import("@/pages/orders/YQXOrderPage")
  // );
  // const ZFOrderPage: React.FC<any> = lazy(
  //   () => import("@/pages/orders/ZFOrderPage")
  // );
  const items: TabsProps["items"] = [
    {
      key: "-1",
      label: "全部",
      children: (
        <>
          <AllOrderPage />
        </>
      ),
    },
    {
      key: "0",
      label: "待支付",
      children: (
        <>
          <DzfOrderPage />
        </>
      ),
    },
    {
      key: "1",
      label: "已支付",
      children: (
        <>
          <YZFOrderPage />
        </>
      ),
    },
    {
      key: "2",
      label: "已完成",
      children: (
        <>
          <YWCOrderPage />
        </>
      ),
    },
    {
      key: "9",
      label: "已取消",
      children: (
        <>
          <YQXOrderPage />
        </>
      ),
    },
    {
      key: "3",
      label: "已作废",
      children: (
        <>
          <ZFOrderPage />
        </>
      ),
    },
  ];
  const buildSearch = (): any => {
    let sD = searchForm.getFieldValue("startTime");
    if (sD) sD = `${sD.year()}/${sD.month() + 1}/${sD.date()}`;
    let eD = searchForm.getFieldValue("endTime");
    if (eD) eD = `${eD.year()}/${eD.month() + 1}/${eD.date()}`;
    const postSearchForm: any = {
      pageIndex: 1,
      pageSize: 10,
      userName: searchForm.getFieldValue("userName"),
      tels: searchForm.getFieldValue("tels"),
      price: searchForm.getFieldValue("price"),
      startTime: sD,
      endTime: eD,
    };
    return postSearchForm;
  };
  const handlerSearch = () => {
    const search = buildSearch()
    eventBus.emit('onSearch',{
       type: tabType,
       ...search
    })
  };
  const tabChange = (e: string) => {
    setTabType(Number(e));
  };
  return (
    <>
      <Collapse
        size="small"
        items={[
          {
            key: "1",
            label: "高级查询",
            children: (
              <>
                <Form layout="inline" form={searchForm}>
                  <Row gutter={10} style={{ padding: 10 }}>
                    <Col span={5} style={{ margin: "10px 0px" }}>
                      <Form.Item label="购买单位" name="userName">
                        <Input placeholder="请输入购买单位" onPressEnter={handlerSearch} />
                      </Form.Item>
                    </Col>
                    <Col span={5} style={{ margin: "10px 0px" }}>
                      <Form.Item label="联系电话" name="tels">
                        <Input placeholder="请输入联系电话"  onPressEnter={handlerSearch}/>
                      </Form.Item>
                    </Col>
                    <Col span={5} style={{ margin: "10px 0px" }}>
                      <Form.Item label="订单总额" name="price">
                        <Input placeholder="请输入订单总额" onPressEnter={handlerSearch} />
                      </Form.Item>
                    </Col>
                    <Col span={5} style={{ margin: "10px 0px" }}>
                      <Form.Item label="开始时间" name="startTime">
                        <DatePicker
                          placeholder="选择开始时间"
                          style={{ width: "100%" }}
                        
                        />
                      </Form.Item>
                    </Col>
                    <Col span={5} style={{ margin: "10px 0px" }}>
                      <Form.Item label="结束时间" name="endTime">
                        <DatePicker
                          placeholder="选择结束时间"
                          style={{ width: "100%" }}
                        />
                      </Form.Item>
                    </Col>
                    <Col span={5} style={{ margin: "10px 0px" }}>
                      <Form.Item>
                        <Button type="primary" onClick={handlerSearch}>
                          查询
                        </Button>
                      </Form.Item>
                    </Col>
                  </Row>
                </Form>
              </>
            ),
          },
        ]}
        expandIconPosition="end"
      />
      <Tabs
        defaultActiveKey="-1"
        items={items}
        style={{ padding: 10 }}
        onChange={(e) => tabChange(e)}
      />
    </>
  );
};
export default Order;
