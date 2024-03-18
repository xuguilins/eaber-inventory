import {
  Alert,
  Button,
  Card,
  Col,
  DatePicker,
  Form,
  Input,
  InputNumber,
  Result,
  Row,
  Table,
  message,
} from "antd";
import { IDetailModal, useOutForm } from "./outForm";
import TextArea from "antd/es/input/TextArea";
import PurashInModal from "../busComponents/PurashInModal";
import locale from "antd/es/date-picker/locale/zh_CN";
import { useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
import dayjs from "dayjs";
import { useTime } from "@/utils";
// import  * as b  from './outForm.ts'
const EditPuraseForm: React.FC = () => {
  const loaction = useLocation();
  const [puraseForm] = Form.useForm()
  const [detailModel,setDetailModel] = useState<IDetailModal[]>([])
  const [visiable,setVisiable] = useState<boolean>(false)
  const [loadId, setLoadId] = useState<string>('')
  const loadOutOrder = async (id: string) => {
    const {data,success} = await alovaInstance.Get<IReturnResult,any>(urls.getOutOrder+"/"+id)
    .send()
    if (success) {
   debugger
      setFormData(data)
    }
  };
  useEffect(() => {
    async function init() {
      if (loaction.state?.id) {
        await loadOutOrder(loaction.state.id);
      }
    }
    init();
  }, [loaction.state?.id]);
  useEffect(() => {
    async function init() {
        await loadDetail()
    }
    init()
}, [loadId])
  const setFormData= (data)=>{
    puraseForm.setFieldValue('id',data.id)
    puraseForm.setFieldValue('supplierId',data.supilerId)
    puraseForm.setFieldValue('outOrderCode',data.outOrderCode)
    puraseForm.setFieldValue('outOrderTime',dayjs(data.outOrderTime))
    puraseForm.setFieldValue('inOrderCode',data.inOrderCode)
    puraseForm.setFieldValue('logistics',data.logics)
    puraseForm.setFieldValue('inUser',data.inUser)
    puraseForm.setFieldValue('inPhone',data.inPhone)
    puraseForm.setFieldValue('remark',data.reason)
    setDetailModel(data.details)
  }
  const onRowEdit=(code,prop,value)=>{
    const editModel = [...detailModel]
    const codeIndex= editModel.findIndex(item=>item.productCode===code)
    if (codeIndex>-1) {
         editModel[codeIndex][prop]=value
         setDetailModel(editModel)
    }

  }
  const handlerRemove=(data)=>{
    const editModel = [...detailModel]
    const list =editModel.filter(item=>item.productCode!==data.productCode)
    setDetailModel(list)
  }
  const loadDetail = async () => {
    if (loadId) {
        const { success, data } = await alovaInstance.Get<IReturnResult>(urls.getPurashModalDetail + "/" + loadId).send();
        if (success) {
            setDetailModel(data)
        }
    }
}
  const onMoalSave=(data)=>{
    setVisiable(false)
    puraseForm.setFieldValue('inOrderCode', data.code)
    puraseForm.setFieldValue('inUser', data.userName)
    puraseForm.setFieldValue('inPhone', data.userTel)
    puraseForm.setFieldValue('supplierId', data.supilerId)
    setLoadId(data.id)
  }
  const onModalClose=()=>{
    setVisiable(false)
  }
  const onSelectClick=()=>{
    setVisiable(true)
  }
  const onSave=async ()=>{
    const id = puraseForm.getFieldValue('id')
    const time = puraseForm.getFieldValue('outOrderTime')
    if (!time) {
        message.error('请选择退货时间')
        return
    }
    const supilerId = puraseForm.getFieldValue('supplierId')
    if (!supilerId) {
        message.error('无效的进货单据数据')
        return
    }
    const code = puraseForm.getFieldValue('inOrderCode')
    if (!code) {
        message.error('请输入选择进货单据')
        return
    }
    const inUser = puraseForm.getFieldValue('inUser')
    if (!inUser) {
        message.error('请输入收货方联系人')
        return
    }
    const inPhone = puraseForm.getFieldValue('inPhone')
    if (!inPhone) {
        message.error('请输入收货方联系电话')
        return
    }
    const remark = puraseForm.getFieldValue('remark')
    if (!remark) {
        message.error('请输入退货原因')
        return
    }
    if (detailModel.length <= 0) {
        message.error('此进货单据异常，无退货明细，无法提交')
        return
    }
    const postData = {
         id: id,
        outOrderTime: useTime(time),
        inOrderCode: code,
        inUser: inUser,
        inPhone: inPhone,
        reason: remark,
        detail: detailModel,
        supilerId: supilerId,
        logics: puraseForm.getFieldValue('logistics') ?? ''
    }
    const { success, message: msg } =
    await alovaInstance.Post<IReturnResult, any>(urls.updateOutPurashe, postData)
        .send()
    if (success) {
        message.success(msg)
    } else {
        message.error(msg)
    }
  }
  return (
    <>
      <div className="productList">
        <p className="inFormP">编辑退货单</p>
        <div className="ImianForm">
          <Card title="基本信息" bordered={false}>
            <Form
              name="basic"
              labelCol={{ span: 6 }}
              wrapperCol={{ span: 24 }}
              autoComplete="off"
              form={puraseForm}
            >
              <Form.Item label="id" name="id" style={{ display: "none" }}>
                <Input />
              </Form.Item>
              <Form.Item
                label="supplierId"
                name="supplierId"
                style={{ display: "none" }}
              >
                <Input />
              </Form.Item>
              <Row gutter={5}>
                <Col span={8}>
                  <Form.Item label="退货编码" name="outOrderCode">
                    <Input placeholder="系统自动生成" disabled />
                  </Form.Item>
                </Col>
                <Col span={8}>
                  <Form.Item label="退货日期" name="outOrderTime" required>
                    <DatePicker
                      placeholder="请选择..."
                      style={{ width: "100%" }}
                      format="YYYY/MM/DD"
                      locale={locale}
                    />
                  </Form.Item>
                </Col>
                <Col span={8}>
                  <Form.Item label="选择进货单据" name="inOrderCode" required>
                    <Input
                      placeholder="请选择"
                      addonAfter={
                        <>
                          <span
                            style={{ cursor: "pointer" }}
                            onClick={onSelectClick}
                          >
                            选择
                          </span>
                        </>
                      }
                    />
                  </Form.Item>
                </Col>
                <Col span={8}>
                  <Form.Item label="退货物流单号" name="logistics">
                    <Input placeholder="请输入物流单号" />
                  </Form.Item>
                </Col>
                <Col span={8}>
                  <Form.Item label="收货方联系人" name="inUser" required>
                    <Input placeholder="请输入联系人" />
                  </Form.Item>
                </Col>
                <Col span={8}>
                  <Form.Item label="收货方联系电话" name="inPhone" required>
                    <Input placeholder="请输入联系电话" />
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item label="描述/退货原因" name="remark" required>
                    <TextArea rows={4} maxLength={6} />
                  </Form.Item>
                </Col>
              </Row>
            </Form>
          </Card>

          <Card title="退货商品明细" bordered={false}>
            <Table
              rowKey="id"
              size="small"
              pagination={false}
              bordered
              scroll={{
                x: 1500,
              }}
              dataSource={detailModel}
              columns={[
                {
                  title: "序号",
                  dataIndex: "index",
                  width: 60,
                  fixed: "left",
                  render(_, record, index) {
                    return <>{index + 1}</>;
                  },
                },
                {
                  title: "操作",
                  dataIndex: "control",
                  width: 100,
                  fixed: "left",
                  render: (_, record) => {
                    return (
                      <Button danger onClick={() => handlerRemove(record)}>
                        移除
                      </Button>
                    );
                  },
                },
                {
                  title: "产品编码",
                  dataIndex: "productCode",
                  width: 300,
                },
                {
                  title: "产品名称",
                  dataIndex: "productName",
                  width: 300,
                },
                {
                  title: "型号",
                  dataIndex: "productModel",
                  width: 300,
                },
                {
                  title: "进货数量",
                  dataIndex: "productCount",
                  width: 120,
                },
                {
                  title: "进货价",
                  dataIndex: "inPrice",
                  width: 120,
                },
                {
                  title: "退货数量",
                  dataIndex: "returnCount",
                  width: 120,
                  render: (_, record) => {
                    return (
                      <InputNumber
                        max={record.productCount}
                        min={0}
                        value={record.returnCount}
                        onChange={(e) =>
                          onRowEdit(record.productCode, "returnCount", e)
                        }
                      ></InputNumber>
                    );
                  },
                },
                {
                  title: "退货价格",
                  dataIndex: "outPrice",
                  width: 120,
                  render: (_, record) => {
                    return (
                      <InputNumber
                        max={record.inPrice}
                        min={0}
                        value={record.outPrice}
                        onChange={(e) =>
                          onRowEdit(record.productCode, "outPrice", e)
                        }
                      ></InputNumber>
                    );
                  },
                },
                {
                  title: "退货总价",
                  dataIndex: "outAll",
                  width: 120,
                  render: (_, record) => {
                    return <>{record.outPrice * record.returnCount}</>;
                  },
                },
              ]}
            />
          </Card>
          <Alert
            message="警告"
            description="请仔细核对退货信息，退货单一旦确认将会自动修改库存且无法修改！"
            type="error"
          />

          <Button onClick={onSave} type="primary" className="saveBtn">
            保存
          </Button>
        </div>
        <PurashInModal
          visiable={visiable}
          onMoalSave={(data) => onMoalSave(data)}
          onModalClose={onModalClose}
        ></PurashInModal>
      </div>
    </>
  );
};
export default EditPuraseForm;
