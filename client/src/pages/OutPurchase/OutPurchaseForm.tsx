import {
  Alert,
  AutoComplete,
  Button,
  Card,
  Col,
  DatePicker,
  Form,
  Input,
  InputNumber,
  Result,
  Row,
  Select,
  Space,
  Table,
  TreeSelect,
} from "antd";
import { useOutForm } from "./outForm";
import TextArea from "antd/es/input/TextArea";
import PurashInModal from "../busComponents/PurashInModal";
import locale from "antd/es/date-picker/locale/zh_CN";
import { useNavigate } from "react-router-dom";
import { useStore } from "@/store";
// import  * as b  from './outForm.ts'
const OutPurchaseForm: React.FC = () => {
  const {
    title,
    puraseForm,
    visiable,
    onMoalSave,
    onModalClose,
    onSelectClick,
    detailModel,
    handlerRemove,
    onRowEdit,
    onSave,
    isCreate,
  } = useOutForm();
  const navtion = useNavigate()
  const { homeStore }  = useStore()
  const showForm = (type: boolean) => {
    if (type) {
      return (
        <Result
          status="success"
          title="退货单已创建成功"
          extra={[
            <Button type="primary" onClick={() => {
              navtion('/')
            }} key="console">
              返回首页
            </Button>,
            <Button
              key="buy"
              onClick={() => {
                homeStore.removeTab('/purchases/outForm')
                navtion('/purchases/outOrder')
              }}
            >
              回到列表
            </Button>,
          ]}
        />
      );
    } else {
      return (
      <div className="productList">
        <p className="inFormP">{title}</p>
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
                <Col span={8} style={{ display: "none" }}>
                  <Form.Item label="收货方联系人" name="supilerId">
                    <Input placeholder="请输入联系人" />
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
      </div>)
    }
  };
  return <>{showForm(isCreate)}</>;
};
export default OutPurchaseForm;
