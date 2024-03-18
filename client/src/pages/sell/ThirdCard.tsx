import { Col, Row, Image, InputNumber } from "antd";
import  { useEffect, useState } from "react";
import { Form, Input } from "antd";
import { useStore } from "@/store";
import { observer } from "mobx-react-lite";
import TableCard from './TableCard'
import { ISellDetail } from "@/store/SellStore";
const { TextArea } = Input
const ThirdCard = () => {
  const [secondForm] = Form.useForm();
  const { sellStore } = useStore();
  const [allMoney, setAllMoney] = useState(0);
  const [tableData, setTableData] = useState<ISellDetail[]>([])
  const [youhui,setYouHui ] = useState<number>(0)
  useEffect(() => {
    secondForm.setFieldsValue({
      sellPhone: sellStore.sellData.sellPhone,
      sellTime: sellStore.sellData.sellTime,
      sellNumber: sellStore.sellData.sellCode,
      sellUnit: sellStore.sellData.sellUser,
      remark: sellStore.sellData.remark,
    });
    setTableData(sellStore.sellData.detail)
   let allPrice = 0;
    for(let i =0;i<sellStore.sellData.detail.length;i++) {
       allPrice+=Number(sellStore.sellData.detail[i].sellAllPrice)
    }
    setAllMoney(allPrice);
    sellStore.setActuailMoney(allPrice)
    secondForm.setFieldValue('actulayMoney',allPrice)
  }, []);

  return (
    <>
      <div className="secondMain">
        <p className="secondP">易步小店库存 零售单</p>
        <div className="cardIcon">
          <Image width={70} src="/imgs/await.png" />
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
                <Input placeholder="系统自动生成" disabled />
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
          <TableCard  tableValue={tableData} disable={true}/>
          <Row gutter={20} style={{ marginTop: 10 }}>
            <Col span={8}>
              <span className="sumP">应收金额：{allMoney}</span>
            </Col>
            <Col span={8}>
              <Form.Item label="实收金额：" name="actulayMoney">
                <InputNumber
                  onChange={(e:any) => {
                    secondForm.setFieldValue('actulayMoney',e)
                    const offset =  Number(allMoney) -Number(e)
                    setYouHui(offset)
                    sellStore.setActuailMoney(e)
                    sellStore.setOffsetMoney(offset)
                  }}
                />
              </Form.Item>
            </Col>
            <Col span={8}>
              <span className="sumP" style={{color:'red'}}>优惠金额: {youhui}</span>
            </Col>
          </Row>
          <Row gutter={20} style={{ marginTop: 10 }}>
            <Col span={8}>
              <span className="sumP">制单人： 黄剑 </span>
            </Col>
            <Col span={8}>
              <span>签收人</span>
            </Col>
          </Row>
          <Row gutter={20} style={{ marginTop: 10 }}>
            <Col span={8}>
              <span className="sumP">
                地址：龙南市龙南镇广场西路楠木树下4号
              </span>
            </Col>
            <Col span={8}>
              <span>微信/电话：13517975697</span>
            </Col>
          </Row>
        </Form>
      </div>
    </>
  );
};
export default observer(ThirdCard);
