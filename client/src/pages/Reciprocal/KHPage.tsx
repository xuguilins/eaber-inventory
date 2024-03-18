import { urls } from "@/api/urls";
import MainContent from "@/components/MainContent";
import alovaInstance, { IReturnResult } from "@/utils/request";
import {
  ArrowDownOutlined,
  DownCircleOutlined,
  DownOutlined,
} from "@ant-design/icons";
import {
  Form,
  Input,
  Button,
  Table,
  Row,
  Col,
  Drawer,
  Space,
  Tabs,
  TableColumnsType,
  Badge,
  Dropdown,
  Modal,
  DatePicker,
  Select,
  Radio,
} from "antd";
import { ColumnsType } from "antd/es/table";
import { useEffect, useState } from "react";
interface IKHSearch {
  pageIndex: number;
  pageSize: number;
  keyWord: string;
}
interface IUserSearch extends IKHSearch {
  userId: string;
  status: number;
}
interface IKHData {
  userId: string;
  customerName: string;
  customerUser: string;
  dzf: number;
  yzf: number;
  zf: number;
  yqx: number;
  ywc: number;
}
import "./KHPage.css";
import { keys, values } from "mobx";
import { useTime } from "@/utils";
interface DataType {
  key: React.Key;
  name: string;
  platform: string;
  version: string;
  upgradeNum: number;
  creator: string;
  createdAt: string;
}

interface ExpandedDataType {
  key: React.Key;
  date: string;
  name: string;
  upgradeNum: string;
}
const { RangePicker } = DatePicker;
export function KHPage() {
  const onPageChange = (pageIndex: number) => {
    setSearch({ ...search, pageIndex });
  };
  const [searchForm] = Form.useForm();
  const [exportForm] = Form.useForm();
  const [loadings, setLoadings] = useState<boolean>(false);
  const onSearch = () => {
    const kwd = searchForm.getFieldValue("keyWord");
    setSearch({ ...search, pageIndex: 1, keyWord: kwd });
  };
  const onRefresh = () => {
    setSearch({ ...search, pageIndex: 1 });
  };
  const handlerExport = async (type: number) => {
    if (type == 1) {
      const { success, data } = await alovaInstance
        .Post<IReturnResult>(urls.exportOrderCus)
        .send();
      if (success) {
        const url = window.URL.createObjectURL(data);
        const a = document.createElement("a");
        a.href = url;
        a.download = "客户列表.xlsx";
        document.body.appendChild(a);
        a.click();
        setTimeout(() => {
          a.remove();
        }, 1000);
      }
    } else {
      setHightShow(true);
    }
  };
  const columnsPage: ColumnsType<IKHData> = [
    {
      title: "操作",
      dataIndex: "control",
      render: (_, data) => {
        return (
          <>
            <Button type="link" onClick={() => showDetail(data)}>
              详情
            </Button>

          </>
        );
      },
    },
    {
      title: "客户名称",
      dataIndex: "customerName",
      key: "customerName",
    },
    {
      title: "客户联系人",
      dataIndex: "customerUser",
      key: "customerUser",
    },
    {
      title: "待支付",
      dataIndex: "dzf",
      key: "dzf",
      render: (text: string) => (
        <Button danger type="primary">
          {text}
        </Button>
      ),
    },
    {
      title: "已支付",
      dataIndex: "yzf",
      key: "yzf",
      render: (text: string) => <Button type="primary">{text}</Button>,
    },
    {
      title: "作废",
      dataIndex: "zf",
      key: "zf",
      render: (text: string) => <span style={{ color: "red" }}>{text}</span>,
    },
    {
      title: "已取消",
      dataIndex: "yqx",
      key: "yqx",
      render: (text: string) => <span style={{ color: "red" }}>{text}</span>,
    },
    {
      title: "已完成",
      dataIndex: "ywc",
      key: "ywc",
    },
  ];
  const [search, setSearch] = useState<IKHSearch>({
    pageIndex: 1,
    pageSize: 10,
    keyWord: "",
  });
  const [total, setTotal] = useState<number>(0);
  const [khData, setKHData] = useState<IKHData[]>([]);
  const [show, setShow] = useState<boolean>(false);
  const [orders, setOrders] = useState<any>([]);

  const [orderTotal, setOrderTotal] = useState<number>(0);
  const [orderForm] = Form.useForm();
  const [childData, setChildData] = useState<Record<string, any>>({});
  const [userSearch, setUserSearch] = useState<IUserSearch>({
    pageIndex: 1,
    pageSize: 10,
    keyWord: "",
    status: 0,
    userId: "",
  });
  const [btnData, setBtnData] = useState<any>({});
  const [tabKey, setTabKey] = useState<string>("0");
  const [hightShow, setHightShow] = useState<boolean>(false);
  const loadPage = async () => {
    const { success, data, total } = await alovaInstance
      .Post<IReturnResult, IKHSearch>(urls.getOrderCusPage, search)
      .send();
    if (success) {
      setTotal(total);
      setKHData(data);
    }
  };
  const showDetail = async (mdata: IKHData) => {
    orderForm.setFieldValue("userName", mdata.customerName);
    orderForm.setFieldValue("userLX", mdata.customerUser);
    setBtnData(mdata);
    setTabKey("0");
    setUserSearch({
      ...userSearch,
      pageIndex: 1,
      pageSize: 10,
      status: 0,
      userId: mdata.userId,
    });
    setShow(true);
  };
  const loadUserDetail = async () => {
    const { success, data, total } = await alovaInstance
      .Post<IReturnResult, IUserSearch>(urls.getUserOrderDetail, userSearch)
      .send();
    if (success) {
      setOrders(data);
      setOrderTotal(total);
    }
  };
  const onExpand = async (event: boolean, record) => {
    if (event) {
      var vaues = await loadODetail(record.id);
      setChildData({
        ...childData,
        [record.id]: vaues,
      });
    }
  };

  useEffect(() => {
    async function loadS() {
      await loadUserDetail();
    }
    loadS();
  }, [userSearch]);
  const loadODetail = async (id: string) => {
    const { data } = await alovaInstance
      .Get<IReturnResult>(urls.getOrderDetailUser + "/" + id)
      .send();
    return data ?? [];
  };
  const onTypeChange = (e: any) => {
    setUserSearch({
      ...userSearch,
      pageIndex: 1,
      pageSize: 10,
      status: Number(e),
    });
    setTabKey(e);
  };
  const onClose = () => {
    setShow(false);
  };
  const expandedRowRender = (record) => {
    const columns: TableColumnsType<ExpandedDataType> = [
      { title: "商品编码", dataIndex: "productCode", key: "productCode" },
      { title: "商品名称", dataIndex: "productName", key: "productName" },
      { title: "单位", dataIndex: "unitName", key: "unitName" },
      {
        title: "数量",
        dataIndex: "count",
        key: "count",
        render: (text: string) => (
          <Button danger type="primary">
            {text}
          </Button>
        ),
      },
      {
        title: "单价",
        dataIndex: "price",
        key: "price",
        render: (text: string) => (
          <Button danger type="primary">
            {text}
          </Button>
        ),
      },
      {
        title: "总价",
        dataIndex: "allPrice",
        key: "allPrice",
        render: (text: string) => (
          <Button danger type="primary">
            {text}
          </Button>
        ),
      },
      { title: "描述", dataIndex: "remark", key: "remark" },
    ];
    return (
      <Table
        rowClassName="chidlRows"
        columns={columns}
        dataSource={childData[record.id]}
        pagination={false}
        rowKey="productCode"
      />
    );
  };

  const columns: TableColumnsType<DataType> = [
    { title: "订单编码", dataIndex: "orderCode", key: "orderCode" },
    { title: "单据时间", dataIndex: "orderTime", key: "orderTime" },
    { title: "购买单位", dataIndex: "orderUser", key: "orderUser" },
    { title: "联系电话", dataIndex: "orderTel", key: "orderTel" },
    {
      title: "订单总额",
      dataIndex: "orderPrice",
      key: "orderPrice",
      render: (text: string) => <Button type="primary">{text}</Button>,
    },
    { title: "支付方式", dataIndex: "payName", key: "payName" },
    { title: "平台", dataIndex: "payClient", key: "payClient" },
    { title: "描述", dataIndex: "remark", key: "remark" },
  ];
  const tabOptions = [
    {
      key: "0",
      label: "待支付",
      children: (
        <>
          <Table
            columns={columns}
            rowKey="orderCode"
            pagination={{
              total: orderTotal,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
            }}
            expandable={{
              expandedRowRender,

              defaultExpandedRowKeys: [""],
              fixed: true,
              onExpand: (record, event) => onExpand(record, event),
            }}
            dataSource={orders}
            size="small"
          />
        </>
      ),
    },
    {
      key: "1",
      label: "已支付",
      children: (
        <>
          <Table
            columns={columns}
            rowKey="orderCode"
            pagination={{
              total: orderTotal,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
            }}
            expandable={{
              expandedRowRender,
              defaultExpandedRowKeys: ["0"],
              fixed: true,
              onExpand: (record, event) => onExpand(record, event),
            }}
            dataSource={orders}
            size="small"
          />
        </>
      ),
    },
    {
      key: "2",
      label: "已完成",
      children: (
        <>
          <Table
            columns={columns}
            rowKey="orderCode"
            pagination={{
              total: orderTotal,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
            }}
            expandable={{
              expandedRowRender,

              defaultExpandedRowKeys: ["0"],
              fixed: true,
              onExpand: (record, event) => onExpand(record, event),
            }}
            dataSource={orders}
            size="small"
          />
        </>
      ),
    },
    {
      key: "9",
      label: "已取消",
      children: (
        <>
          <Table
            columns={columns}
            rowKey="orderCode"
            pagination={{
              total: orderTotal,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
            }}
            expandable={{
              expandedRowRender,
              defaultExpandedRowKeys: ["0"],
              fixed: true,
              onExpand: (record, event) => onExpand(record, event),
            }}
            dataSource={orders}
            size="small"
          />
        </>
      ),
    },
    {
      key: "3",
      label: "已作废",
      children: (
        <>
          <Table
            columns={columns}
            rowKey="orderCode"
            pagination={{
              total: total,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
            }}
            expandable={{
              expandedRowRender,
              defaultExpandedRowKeys: ["0"],
              fixed: true,
              onExpand: (record, event) => onExpand(record, event),
            }}
            dataSource={orders}
            size="small"
          />
        </>
      ),
    },
  ];
  useEffect(() => {
    async function init() {
      await loadPage();
    }
    init();
  }, [search]);
  return (
    <>
      <MainContent
        title="客户往来交易"
        total={total}
        pageIndex={search.pageIndex}
        data={khData}
        onChange={(index: number) => onPageChange(index)}
        searchBar={
          <>
            <Form layout="inline" form={searchForm}>
              <Form.Item label="名称" name="keyWord">
                <Input placeholder="请输入单位名称" onPressEnter={onSearch} />
              </Form.Item>

              <Form.Item>
                <Button type="primary" onClick={onSearch}>
                  查询
                </Button>
              </Form.Item>
            </Form>
          </>
        }
        onRefresh={onRefresh}
        toolBar={
          <>
            
          </>
        }
        mainTable={
          <Table
            size="small"
            columns={columnsPage}
            rowKey="customerName"
            dataSource={khData}
            pagination={false}
          />
        }
      ></MainContent>

      <Drawer
        title="订单详情"
        width={window.webConfig.dialogWidth}
        open={show}
        closable={false}
        styles={{
          body: {
            paddingBottom: 80,
          },
        }}
        extra={
          <Space>
            <Button type="primary" danger onClick={onClose}>
              关闭
            </Button>
          </Space>
        }
      >
        <Form form={orderForm}>
          <h3>基本信息</h3>
          <Row gutter={20}>
            <Col span={12}>
              <Form.Item label="客户名称" name="userName">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="客户联系人" name="userLX">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="待支付金额" name="dzf">
                <Button danger type="primary">
                  {btnData.dzf}
                </Button>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="已支付金额" name="yzf">
                <Button type="primary">{btnData.yzf}</Button>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="已完成金额" name="ywc">
                <Button danger type="primary" style={{ background: "#3cb315" }}>
                  {btnData.ywc}
                </Button>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="已取消金额" name="yqx">
                <Button danger type="primary" style={{ background: "#f87032" }}>
                  {btnData.yqx}
                </Button>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="已作废金额" name="zf">
                <Button danger type="primary" style={{ background: "grey" }}>
                  {btnData.zf}
                </Button>
              </Form.Item>
            </Col>
          </Row>
          <h3>订单信息</h3>
          <div className="orderDetailRow">
            <Row gutter={20}>
              <Col span={24}>
                <Tabs
                  onChange={(e) => onTypeChange(e)}
                  defaultActiveKey={tabKey}
                  activeKey={tabKey}
                  tabPosition="left"
                  items={tabOptions}
                />
              </Col>
            </Row>
          </div>
        </Form>
      </Drawer>
    </>
  );
}
