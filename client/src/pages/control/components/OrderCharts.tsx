import { Col, Row } from "antd";
import "./orderChats.css";
import { useEffect, useState } from "react";
import * as echarts from "echarts";
const OrderCharts: React.FC<any> = ({ order, product }) => {
  const options = {
    tooltip: {
      trigger: "item",
    },
    legend: {
      data: ["待支付", "已支付", "已完成", "已取消", "已作废"],
    },
    xAxis: {
      type: "category",
      data: order.xTypes,
    },
    yAxis: {
      type: "value",
    },
    series: [
      {
        name: "待支付",
        data: order.dzfTypes,
        type: "bar",

        label: {
          show: true,
          position: "top",
          color: "#2d89ed",
        },
      },
      {
        name: "已支付",
        data: order.yzfTypes,
        type: "bar",
        label: {
          show: true,
          position: "top",
          color: "#2d89ed",
        },
      },
      {
        name: "已完成",
        data: order.ywcTypes,
        type: "bar",
        label: {
          show: true,
          position: "top",
          color: "#2d89ed",
        },
      },

      {
        name: "已取消",
        data: order.yqxTypes,
        type: "bar",
        label: {
          show: true,
          position: "top",
          color: "#2d89ed",
        },
      },
      {
        name: "已作废",
        data: order.zfTypes,
        type: "bar",
        label: {
          show: true,
          position: "top",
          color: "#2d89ed",
        },
      },
    ],
  };
  const [instance, setInstance] = useState<echarts.ECharts>(null);
  useEffect(() => {
    if (order.xTypes) {
      if (instance) {
        instance.setOption(options);
      } else {
        const el = document.getElementById("orderChart");
        const instance = echarts.init(el);
        instance.setOption(options);
        setInstance(instance);
      }
    }
  
  }, [order]);
  return (
    <>
      <Row gutter={20}>
        <Col lg={18} sm={24} md={18} xxl={18} xl={18}>
          <div id="orderChart"></div>
        </Col>
        <Col lg={6} sm={24} md={6} xxl={6} xl={6}>
          <p className="phTitlte">热销商品排行(可根据年/月/周以及条件筛选)</p>
          {product.map((item, index) => {
            return (
              <div className="proTitle" key={index}>
                <b>{index + 1}</b>、 {item.productName}{" "}
                <b className="countTitle">{item.sellCount}</b>{" "}
              </div>
            );
          })}
        </Col>
      </Row>
    </>
  );
};
export default OrderCharts;
