import { Col, Row, Image } from "antd";
import React, { useEffect, useState } from "react";
import { Form, Input } from "antd";
import Table from "antd/es/table";
import { useStore } from "@/store";
import { ISellDetail } from "@/store/SellStore";
const { TextArea } = Input
const FourCard = () => {
  const [secondForm] = Form.useForm();
  const { sellStore } = useStore();
  const [tableData, setTableData] = useState<ISellDetail[]>([])
  useEffect(() => {
    secondForm.setFieldsValue({
      sellPhone: sellStore.sellData.sellPhone,
      sellTime: sellStore.sellData.sellTime,
      sellNumber:sellStore.sellData.sellCode,
      sellUnit: sellStore.sellData.sellUser,
      remark:sellStore.sellData.remark,
    });
    setTableData(sellStore.sellData.detail)

  }, []);
  return (
    <>
      <div className="secondMain">
        <p className="secondP">易步小店库存 零售单</p>
        <div className="cardIcon">
          <Image width={70} src="imgs/complete.png" />
        </div>
        <Form form={secondForm}>
          <Row gutter={20}>
            <Col span={12}>
              <Form.Item label="单据日期" name="sellTime">
                <Input placeholder="input placeholder" disabled />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="单据编号" name="sellNumber">
                <Input placeholder="input placeholder" disabled />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="购买单位" name="sellUnit">
                <Input placeholder="请输入购买单位" disabled />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="联系电话" name="sellPhone">
                <Input placeholder="请输入联系电话" disabled />
              </Form.Item>
            </Col>

            <Col span={24}>
              <Form.Item
                label="描述"
                name="remark"
              >
                <TextArea placeholder="请输入联系电话" disabled/>
              </Form.Item>
            </Col>
            <Col span={24}>
              <span>备注</span>
            </Col>
          </Row>
          <Table
            columns={[
              {
                title: "编号",
                dataIndex: "productCode",
                key: "productCode",
                width: 120,
              },
              {
                title: "名称",
                dataIndex: "productName",
                key: "productName",
              },
              {
                title: "型号",
                dataIndex: "productModel",
                key: "productModel",
              },
              {
                title: "单位",
                dataIndex: "unitName",
                key: "unitName",
                width: 80,
              },
              {
                title: "数量",
                dataIndex: "inventoryCount",
                key: "inventoryCount",
                width: 80,
              },
              {
                title: "售价",
                dataIndex: "sellPrice",
                key: "sellPrice",
                width: 80,
              },
              {
                title: "总价",
                dataIndex: "sellAllPrice",
                key: "sellAllPrice",
                width: 80
              },
            ]}
            rowKey="productCode"
            dataSource={tableData}
            pagination={false}
          />

        </Form>
      </div>
    </>
  );
};
export default React.memo(FourCard);
