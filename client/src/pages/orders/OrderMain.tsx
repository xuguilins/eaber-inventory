import {
  Button,
  Col,
  DatePicker,
  Drawer,
  Form,
  Input,
  Modal,
  Pagination,
  Row,
  Select,
  Space,
  Table,
  Tag,
  message,
} from "antd";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import "./order.css";
import alovaInstance, { IReturnResult } from "@/utils/request";
import dayjs from "dayjs";
import { urls } from "@/api/urls";
import locale from "antd/es/date-picker/locale/zh_CN";
import { useTime } from "@/utils";
import PrintForm from "./PrintForm";
import eventBus from "@/utils/eventBus";
export interface OrderMainProps {
  totalCount: number;
  data: Array<IOrderRow>;
  pageIndex: number;
  onPageChange: (pageIndex: number) => void;
  onRefresh?: () => void;
}
export interface IOrderSearch {
  pageIndex: number;
  pageSize: number;
  userName: string;
  tels: string;
  price: string;
  startTime: string;
  endTime: string;
}
export interface IOrderRow {
  id: string;
  orderCode: string;
  orderUser: string;
  orderTime: string;
  orderTel: string;
  orderPrice: number;
  remark: string;
  payName: string;
  payClient: string;
  status: number;
}
export interface IOrderRowDetail {
  index: number;
  productName: string;
  productCode: string;
  count: number;
  unitName: string;
  price: number;
  remark: string;
  allPrice: number;
}
const OrderMain: React.FC<OrderMainProps> = (props: OrderMainProps) => {
  const { TextArea } = Input;
  const showStatus = (type: number) => {
    if (type === 0) {
      return (
        <>
          <Tag color="#e4c007">待支付</Tag>
        </>
      );
    } else if (type === 1) {
      return (
        <>
          <Tag color="#1b07e4">已支付</Tag>
        </>
      );
    } else if (type === 2) {
      return (
        <>
          <Tag color="#55acee">已完成</Tag>
        </>
      );
    } else if (type === 3) {
      return (
        <>
          <Tag color="#878787">已作废</Tag>
        </>
      );
    } else {
      return (
        <>
          <Tag color="#f00912">已取消</Tag>
        </>
      );
    }
  };
  const showBtns = (type: number, data: IOrderRow) => {
    const arrys = [0, 1, 2];
    if (arrys.includes(type)) {
      return (
        <>
          <Button
            type="text"
            className="textView"
            onClick={() => handlerView(data)}
          >
            操作订单
          </Button>

          <Button
            type="text"
            className="textPrint"
            onClick={() => handlerPrint(data)}
          >
            打印
          </Button>
        </>
      );
    } else {
      return (
        <>
          <Button
            type="text"
            className="textView"
            onClick={() => handlerView(data)}
          >
            查看
          </Button>
          <Button
            type="text"
            className="textPrint"
            onClick={() => handlerPrint(data)}
          >
            打印
          </Button>
        </>
      );
    }
  };
  const showAction = (type: number) => {
    if (type === 0) {
      return (
        <>
          <Button type="primary" onClick={onSave}>
            更新订单
          </Button>
          <Button danger onClick={() => onPayConfirm(1)}>
            确认支付
          </Button>
          <Button type="primary" onClick={() => onPayConfirm(9)} danger>
            取消订单
          </Button>
        </>
      );
    } else if (type === 1) {
      return (
        <>
          <Button type="dashed" onClick={() => onPayConfirm(2)} danger>
            完成订单
          </Button>
          <Button type="primary" onClick={() => onPayConfirm(3)} danger>
            订单作废
          </Button>
        </>
      );
    } else if (type === 2) {
      return (
        <Button type="primary" onClick={() => onPayConfirm(3)} danger>
          订单作废
        </Button>
      );
    }
  };
  const [title, setTitle] = useState<string>("");
  const [open, setOpen] = useState<boolean>(false);
  const [orderId, setOrderId] = useState<string>("");
  const [orderForm] = Form.useForm();
  const [type, setType] = useState<number>(0);
  const [fdata, setFData] = useState<any>({});
  const [payData, setPayData] = useState<any>([]);
  const [modal, contextHandler] = Modal.useModal();
  const [isPrint, setIsPrint] = useState<boolean>(false);
  const [tableData, setTableData] = useState<Array<IOrderRowDetail>>([]);
  const [childPrint, setChildPrint] = useState<boolean>(false);
  const handlerView = async (data: IOrderRow) => {
    setType(3);
    setOrderId(data.id);
    setTitle("查看订单-" + data.orderCode);
    setOpen(true);
  };
  const handlerPrint = async (data: IOrderRow) => {
    setChildPrint(true);

    setType(1);
    setOrderId(data.id);
  };
  const handlerClose = () => {
    setOrderId("");
    setType(0);
    orderForm.resetFields();
    setOpen(false);
  };
  const onPayConfirm = async (status: number) => {
    const payName = orderForm.getFieldValue("payName");
    if (!payName && status === 1) {
      message.error("请在编辑处完成支付方式的确认");
      return;
    }
    const confirmed = await modal.confirm({
      title: "操作提示",
      okText: "确定",
      cancelText: "取消",
      content: `您确定要执行此操作？`,
    });

    if (confirmed) {
      const ids = [orderId];
      const { success, message: msg } = await alovaInstance
        .Post<IReturnResult, any>(urls.confirmOrder, {
          ids: ids,
          orderStatus: status,
          payName: payName,
        })
        .send();
      if (success) {
        message.success(msg);
        handlerClose();
        props.onRefresh();
      } else {
        message.error(msg);
      }
    }
  };
  const loadOrderInfo = async () => {
    const { data, success } = await alovaInstance
      .Get<IReturnResult>(`${urls.getOrderInfo}/${orderId}`)
      .send();
    if (success) {
      setFData(data);
      setFormData(data);
      setTableData(data.detailDtos);
    }
  };
  const loadPays = async () => {
    const { success, data } = await alovaInstance
      .Get<IReturnResult>(urls.getPays)
      .send();
    if (success) {
      setPayData(data);
    }
  };
  const childClose = useCallback((print: boolean) => {
    setChildPrint(print);
  }, []);
  const cachePays = useMemo(() => {
    return payData;
  }, [payData]);
  const setFormData = (data: any) => {
    orderForm.setFieldValue("orderCode", data.orderCode);
    if (data.status === 0) {
      orderForm.setFieldValue("orderTime", dayjs(data.orderTime));
    } else {
      orderForm.setFieldValue("orderTime", data.orderTime);
    }
    orderForm.setFieldValue("orderCode", data.orderCode);
    orderForm.setFieldValue("orderUser", data.orderUser);
    orderForm.setFieldValue("payName", data.payName);
    orderForm.setFieldValue("orderTel", data.orderTel);
    orderForm.setFieldValue("orderAll", data.orderPrice);
    orderForm.setFieldValue("orderClient", data.payClient);
    orderForm.setFieldValue("remark", data.remark);
  };
  const onTimeChange = (e: any) => {
    message.info("单据时间发生改变,请记得保存");
  };
  const onSelectChange = () => {
    message.info("支付方式发生改变,请记得保存");
  };
  const onSave = async () => {
    const payName = orderForm.getFieldValue("payName");
    let orderTime = orderForm.getFieldValue("orderTime");
    if (orderTime) orderTime = useTime(orderTime);
    if (!orderTime) {
      message.error("请选择单据时间");
      return;
    }
    if (!payName) {
      message.error("请选择支付方式");
      return;
    }
    const { success, message: msg } = await alovaInstance
      .Post<IReturnResult, any>(urls.updateOrder, {
        id: orderId,
        payName: payName,
        time: orderTime,
      })
      .send();
    if (success) {
      message.success(msg);
      props.onRefresh();
    } else {
      message.error(msg);
    }
  };
  const SumFooter = (data: readonly IOrderRowDetail[]) => {
    let sumCount = 0;
    let sumPrice = 0;
    let allPrice = 0;
    data.forEach((item) => {
      sumCount += Number(item.count);
      sumPrice += Number(item.price);
      allPrice += Number(item.allPrice);
    });
    return (
      <Table.Summary fixed>
        <Table.Summary.Row>
          <Table.Summary.Cell index={0}>合计</Table.Summary.Cell>
          <Table.Summary.Cell index={1}></Table.Summary.Cell>
          <Table.Summary.Cell index={2}></Table.Summary.Cell>
          <Table.Summary.Cell index={3}></Table.Summary.Cell>
          <Table.Summary.Cell index={4}>{sumCount}</Table.Summary.Cell>
          <Table.Summary.Cell index={5}>{sumPrice}</Table.Summary.Cell>
          <Table.Summary.Cell index={6}>{allPrice}</Table.Summary.Cell>
          <Table.Summary.Cell index={7}></Table.Summary.Cell>
        </Table.Summary.Row>
      </Table.Summary>
    );
  };
  const showContron = (type: number, status: number) => {
    if (type === 1) {
      return <></>;
    } else if (type === 2) {
      return (
        <>
          <Button type="primary" onClick={onSave} disabled={type !== 2}>
            保存
          </Button>
        </>
      );
    } else {
      return showAction(status);
    }
  };
  useEffect(() => {
    async function init() {
      if (orderId) await loadOrderInfo();
    }
    init();
  }, [orderId]);
  useEffect(() => {
    async function initPays() {
      await loadPays();
    }
    initPays();
  }, []);
  return (
    <>
      {contextHandler}
      <PrintForm
        childClose={childClose}
        show={childPrint}
        order={fdata}
        tableData={tableData}
      />
      <div className="orderMain">
        <div className="orderTop">
          <Table
            scroll={{ x: 1000 }}
            size="small"
            bordered
            columns={[
              {
                title: "操作",
                dataIndex: " control",
                key: "control",
                width: 200,
                fixed: "left",
                render: function (_, record) {
                  return showBtns(Number(record.status), record);
                },
              },
              {
                title: "订单编码",
                dataIndex: "orderCode",
                key: "orderCode",
                width: 120,
              },
              {
                title: "单据时间",
                dataIndex: "orderTime",
                key: "orderTime",
                width: 100,
              },
              {
                title: "购买单位",
                dataIndex: "orderUser",
                key: "orderUser",
                width: 100,
              },
              {
                title: "联系电话",
                dataIndex: "orderTel",
                key: "orderTel",
                width: 100,
              },
              {
                title: "订单总额",
                dataIndex: "orderPrice",
                key: "orderPrice",
                width: 80,
              },
              {
                title: "支付方式",
                dataIndex: "payName",
                key: "payName",
                width: 80,
              },
              {
                title: "平台",
                dataIndex: "payClient",
                key: "payClient",
                width: 80,
              },
              {
                title: "订单状态",
                dataIndex: "status",
                key: "status",
                width: 80,
                render(value, record, index) {
                  return showStatus(Number(value));
                },
              },
              {
                title: "描述",
                dataIndex: "remark",
                key: "remark",
                width: 200,
              },
            ]}
            dataSource={props.data}
            pagination={false}
            rowKey="orderCode"
          />
        </div>
        <div className="orderBottom">
          <Pagination
            total={props.totalCount}
            onChange={(e) => props.onPageChange(e)}
            current={props.pageIndex}
            style={{ lineHeight: "40px", marginRight: 30 }}
            showSizeChanger={false}
            showTotal={(total) => `共 ${total} 条`}
          />
        </div>
        {/* 订单弹出 */}
        <Drawer
          title={title}
          width="70%"
          open={open}
          closable={false}
          styles={{
            body: {
              paddingBottom: 80,
            },
          }}
          extra={
            <Space>
              <Button danger onClick={handlerClose}>
                关闭
              </Button>
              {showContron(type, fdata.status)}
            </Space>
          }
        >
          <Form layout="vertical" form={orderForm}>
            <Row gutter={16}>
              <Col span={8}>
                <Form.Item name="orderCode" label="订单编码">
                  <Input readOnly />
                </Form.Item>
              </Col>
              <Col span={8}>
                <Form.Item name="orderTime" label="单据时间">
                  {fdata.status !== 0 ? (
                    <Input readOnly />
                  ) : (
                    <DatePicker
                      onChange={(e) => onTimeChange(e)}
                      style={{ width: "100%" }}
                      format="YYYY/MM/DD"
                      locale={locale}
                    />
                  )}
                </Form.Item>
              </Col>
              <Col span={8}>
                <Form.Item name="orderUser" label="购买单位">
                  <Input readOnly />
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={16}>
              <Col span={8}>
                <Form.Item name="payName" label="支付方式">
                  {fdata.status !== 0 ? (
                    <Input readOnly />
                  ) : (
                    <Select
                      fieldNames={{ label: "payName", value: "payName" }}
                      options={cachePays}
                      onChange={onSelectChange}
                    ></Select>
                  )}
                </Form.Item>
              </Col>
              <Col span={8}>
                <Form.Item name="orderTel" label="联系电话">
                  <Input readOnly />
                </Form.Item>
              </Col>
              <Col span={8}>
                <Form.Item name="orderAll" label="订单总额">
                  <Input readOnly />
                </Form.Item>
              </Col>
            </Row>
            <Row gutter={16}>
              <Col span={12}>
                <Form.Item name="orderClient" label="订单生成平台">
                  <Input readOnly />
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item name="orderStatus" label="订单状态">
                  {showStatus(fdata.status)}
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item name="remark" label="订单描述">
                  <TextArea readOnly />
                </Form.Item>
              </Col>
            </Row>
          </Form>
          <p>商品明细</p>
          <Table
            bordered
            size="small"
            columns={[
              {
                title: "序号",
                dataIndex: "index",
                key: "index",
                width: 60,
              },
              {
                title: "编号",
                dataIndex: "productCode",
                key: "productCode",
                width: 120,
              },
              {
                title: "商品名称",
                dataIndex: "productName",
                key: "productName",
                width: 260,
              },

              {
                title: "单位",
                dataIndex: "unitName",
                key: "unitName",
                width: 80,
              },
              {
                title: "数量",
                dataIndex: "count",
                key: "count",
                width: 80,
              },
              {
                title: "单价",
                dataIndex: "price",
                key: "price",
                width: 80,
              },
              {
                title: "金额",
                dataIndex: "allPrice",
                key: "allPrice",
                width: 100,
              },
              {
                title: "备注",
                dataIndex: " remark",
                key: "remark",
                width: 150,
              },
            ]}
            rowKey="productCode"
            dataSource={tableData}
            pagination={false}
            summary={(record) => SumFooter(record)}
          />
        </Drawer>
      </div>
    </>
  );
};
{
  /* <PrintForm  show={isPrint} order={fdata} tableData={tableData}/> */
}
export default OrderMain;
