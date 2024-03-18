import { AutoComplete, Button, Col, Form, Row } from "antd";
import { Input } from "antd";
import React, { useCallback, useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "@/store";
import { useTime } from "@/utils";
import ProdcutModal from "../busComponents/ProdcutModal";
import TableCard from "./TableCard";
import { ISellDetail } from "@/store/SellStore";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
const { TextArea } = Input;
export interface IBuyUser{
  name:string;
  tel:string;
}
const FirstCard: React.FC<any> = () => {
  const [secondForm] = Form.useForm();
  const { sellStore } = useStore();
  const [modalShow, setMoalShow] = useState(false);
  const [tableData, setTableData] = useState<Array<ISellDetail>>([]);
  const [users, setUsers] = useState<Array<IBuyUser>>([]);
  const [defautData,setDefaultData] = useState<Array<IBuyUser>>([])
  const onMoalSave = useCallback(
    (data: Array<ISellDetail>) => {
      setMoalShow(false);
      const oldTable = [...tableData];
      oldTable.push(...data);
      setTableData(oldTable);
      sellStore.setDetail(oldTable);
    },
    [modalShow]
  );
  const loadUsers = async () => {
    const { data, success } = await alovaInstance
      .Get<IReturnResult, any>(urls.getOrderUsers)
      .send();
    if (success) {
      setUsers(data)
      setDefaultData(data)
    }
  };
  const onQuerySearch =(e:any)=>{
     const data=  defautData.filter(d=>d.name.indexOf(e)>-1)
     setUsers(data)
  }
  const onUnitSelect =(e:string)=>{
    const tel= defautData.find(x=>x.name === e)
    if (tel) 
    secondForm.setFieldValue("sellPhone", tel.tel);
    const time = secondForm.getFieldValue("sellTime");
    const user = secondForm.getFieldValue("sellUnit");
    const remark = secondForm.getFieldValue("remark");
    sellStore.setMainSell(user, tel.tel, time, remark);

  }
  useEffect(() => {
    const table = sellStore.sellData.detail;
    sellStore.setDetail(table);
    setTableData(table);
    secondForm.setFieldValue("sellTime", useTime(new Date()));
    secondForm.setFieldValue("sellNumber", "系统自动生成");
    secondForm.setFieldValue("sellUnit", sellStore.sellData.sellUser);
    secondForm.setFieldValue("sellPhone", sellStore.sellData.sellPhone);
    secondForm.setFieldValue("remark", sellStore.sellData.remark);
    async function init() {
      await loadUsers();
    }
    init();
  }, []);
  const onUnitChange = (value: string) => {
    const time = secondForm.getFieldValue("sellTime");
    const phone = secondForm.getFieldValue("sellPhone");
    const remark = secondForm.getFieldValue("remark");
    sellStore.setMainSell(value, phone, time, remark);
  };
  const onTelChange = (value: string) => {
    const time = secondForm.getFieldValue("sellTime");
    const user = secondForm.getFieldValue("sellUnit");
    const remark = secondForm.getFieldValue("remark");
    sellStore.setMainSell(user, value, time, remark);
  };
  const onRemarkChange = (value: string) => {
    const time = secondForm.getFieldValue("sellTime");
    const phone = secondForm.getFieldValue("sellPhone");
    const user = secondForm.getFieldValue("sellUnit");
    sellStore.setMainSell(user, phone, time, value);
  };
  return (
    <>
      <div className="secondMain">
        <p className="secondP">易步小店库存 零售单</p>
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
              <Form.Item
                label="购买单位"
                name="sellUnit"
                rules={[{ required: true, message: "请输入购买单位" }]}
              >
                <AutoComplete
                  placeholder="请输入购买单位"
                  onChange={(e) => onUnitChange(e)}
                  style={{ width: "100%" }}
                  options={users}
                  onSelect={(e)=>onUnitSelect(e)}
                  onSearch={onQuerySearch}
                  fieldNames={{
                    label:'name',
                    value:'name'
                  }}
                />
                {/* <Input placeholder="请输入购买单位"  onChange={(e)=>onUnitChange(e.target.value)}/> */}
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="联系电话"
                name="sellPhone"
                rules={[{ required: true, message: "请输入联系电话" }]}
              >
                <Input
                  placeholder="请输入联系电话"
                  onChange={(e) => onTelChange(e.target.value)}
                />
              </Form.Item>
            </Col>
            <Col span={24}>
              <Form.Item label="描述" name="remark">
                <TextArea
                  placeholder="请输入联系电话"
                  onChange={(e) => onRemarkChange(e.target.value)}
                />
              </Form.Item>
            </Col>
            <Col span={24}>
              <span>备注</span>
            </Col>
          </Row>
          <div className="selectBtn">
            <Button type="primary" onClick={() => setMoalShow(true)}>
              选择产品
            </Button>
          </div>
          <TableCard tableValue={tableData} />
          <ProdcutModal
            visiable={modalShow}
            onMoalSave={(e) => onMoalSave(e)}
            onModalClose={() => {
              setMoalShow(false);
            }}
          ></ProdcutModal>
        </Form>
      </div>
    </>
  );
};
export default React.memo(observer(FirstCard));
