import { urls } from "@/api/urls";
import MainContent from "@/components/MainContent";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { ArrowDownOutlined, DownCircleOutlined, DownOutlined } from "@ant-design/icons";
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
  Select,
  Radio,
  DatePicker,
} from "antd";
const {RangePicker }  =DatePicker
import { ColumnsType } from "antd/es/table";
import { useEffect, useState } from "react";
interface IKHSearch {
  pageIndex: number;
  pageSize: number;
  keyWord: string;
}
interface ISupSearch extends IKHSearch {
    status:number;
    supId:string
}
interface IKHData {
 supId: string;
 supName: string;
  providerUser: string;
  supPhone: string;
  jxz: number;
  ywc: number;
}

0
import "./KHPage.css";
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

export function GYSJHPage() {
  const onPageChange = (pageIndex: number) => {
    setSearch({ ...search, pageIndex });
  };
  const [searchForm] = Form.useForm();
  const [exportForm] = Form.useForm();
  const [hightShow, setHightShow] = useState<boolean>(false);
  const onSearch = () => {
    const kwd = searchForm.getFieldValue("keyWord");
    setSearch({ ...search, pageIndex: 1, keyWord: kwd });
  };
  const onRefresh = () => {
    setSearch({ ...search, pageIndex: 1 });
  };
  const columnsPage: ColumnsType<IKHData> = [
    {
      title: "操作",
      dataIndex: "control",
      render: (_, data) => {
        return (
          <Button type="link" onClick={() => showDetail(data)}>
            详情
          </Button>
        );
      },
    },
    {
      title: "供应商名称",
      dataIndex: "supName",
      key: "supName",
    },
    {
      title: "供应商联系人",
      dataIndex: "providerUser",
      key: "providerUser",
    },
    {
        title: "联系方式",
        dataIndex: "supPhone",
        key: "supPhone",
      },
    {
      title: "进行中",
      dataIndex: "jxz",
      key: "jxz",
      render: (text: string) => (
        <Button danger type="primary">
          {text}
        </Button>
      ),
    },
    {
      title: "已完成",
      dataIndex: "ywc",
      key: "ywc",
      render: (text: string) => (
        <Button  type="primary">
          {text}
        </Button>
      ),
    },
  ];
  const [search, setSearch] = useState<IKHSearch>({
    pageIndex: 1,
    pageSize: 10,
    keyWord: ""
  });
  const [total, setTotal] = useState<number>(0);
  const [khData, setKHData] = useState<IKHData[]>([]);
  const [show, setShow] = useState<boolean>(false);
  const [orders, setOrders] = useState<any>([]);

  const [orderTotal, setOrderTotal] = useState<number>(0);
  const [orderForm]= Form.useForm()
  const [childData,setChildData] = useState<Record<string,any>>({})
  const [userSearch, setUserSearch] = useState<ISupSearch>({
    pageIndex: 1,
    pageSize: 10,
    keyWord: "",
    status:1,
    supId:''
  });
  const  [btnData,setBtnData] = useState<any>({})
  const [tabKey,setTabKey] = useState<string>("0")
  const loadPage = async () => {
    const { success, data, total } = await alovaInstance
      .Post<IReturnResult, ISupSearch>(urls.getPurashInCusPage, search)
      .send();
    if (success) {
      setTotal(total);
      setKHData(data);
     
    }
  };
  const showDetail = async (mdata: IKHData) => {
    orderForm.setFieldsValue(mdata)
    setBtnData(mdata)
    setTabKey('1')
    setUserSearch({
      ...userSearch,
      pageIndex: 1,
      pageSize: 10,
      supId:mdata.supId,
      status: 1
    });
    setShow(true);
  };
  const loadUserDetail = async () => {
    const { success, data, total } = await alovaInstance
      .Post<IReturnResult, ISupSearch>(urls.getPurashInCusTablePage, userSearch)
      .send();
    if (success) {
      setOrders(data);
      setOrderTotal(total);
    }
  };
  const onExpand=async (event:boolean,record)=>{
    if (event) {
      var vaues = await loadODetail(record.id)
      setChildData({
        ...childData,
        [record.id]:vaues 
      })
    }
  }

  useEffect(() => {
    async function loadS() {
      await loadUserDetail();
    }
    loadS();
  }, [userSearch]);
  const loadODetail= async (id:string)=>{
    const { data } = await alovaInstance.Get<IReturnResult>(urls.getPurashInCusDetail+'/'+id).send()
    return data ?? []
  }
  const onTypeChange = (e: any) => {
    setUserSearch({
      ...userSearch,
      pageIndex: 1,
      pageSize: 10,
      status:Number(e)
    });
    setTabKey(e)
  };
  const onClose = () => {
    setShow(false);
  };
  const expandedRowRender =  (record) => {

    const columns: TableColumnsType<ExpandedDataType> = [
      { title: "商品编码", dataIndex: "productCode", key: "productCode" },
      { title: "商品名称", dataIndex: "productName", key: "productName" },
      { title: "商品型号", dataIndex: "productModel", key: "productModel" },
      { title: "进货数量", dataIndex: "productCount", key: "productCount",
      render: (text: string) => (
        <Button  danger type="primary">
          {text}
        </Button>
      ), },
      { title: "进货价格", dataIndex: "productPrice", key: "productPrice" ,
      render: (text: string) => (
        <Button danger type="primary">
          {text}
        </Button>
      ),
    },
      { title: "进货总价", dataIndex: "productAll", key: "productAll" ,
      render: (text: string) => (
        <Button danger type="primary">
          {text}
        </Button>
      ),}
   
    ];
    return <Table rowClassName="chidlRows" columns={columns} dataSource={childData[record.id]} pagination={false} rowKey="productCode" />;
  };

  const columns: TableColumnsType<DataType> = [
    { title: "单据编码", dataIndex: "purchaseCode", key: "purchaseCode" },
    { title: "单据时间", dataIndex: "inOrderTime", key: "inOrderTime" },
    { title: "供货渠道", dataIndex: "channelType", key: "channelType", 
    render(value) {
        return value === 1 ? (
          <Button>自行采购</Button>
        ) : (
          <Button>供应商采购</Button>
        );
      },

},
    { title: "供应商", dataIndex: "supName", key: "supName" },
    { title: "联系人", dataIndex: "inUser", key: "inUser" 
    
  },
  { title: "联系电话", dataIndex: "inPhone", key: "inPhone" },
  { title: "进货总数", dataIndex: "inCount", key: "inCount" },
  { title: "进货总价", dataIndex: "inPrice", key: "inPrice" },
  ];
  const tabOptions = [
    {
      key: "1",
      label: "进行中",
      children: (
        <>
          <Table
            columns={columns}
            rowKey="purchaseCode"
            pagination={{
              total: orderTotal,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
              showTotal:(total)=>{
                return (<span>共 {total} 条</span>)
              }
            }}
            expandable={{ expandedRowRender, 
          
              defaultExpandedRowKeys: [''] ,fixed:true,onExpand:(record, event)=>onExpand(record, event)}}
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
            rowKey="purchaseCode"
            pagination={{
              total: orderTotal,
              pageSize: userSearch.pageSize,
              defaultPageSize: userSearch.pageIndex,
              current: userSearch.pageIndex,
              showTotal:(total)=>{
                return (<span>共 {total} 条</span>)
              }
            }}
            expandable={{ expandedRowRender,
               defaultExpandedRowKeys: ["0"],fixed:true,onExpand:(record, event)=>onExpand(record, event) }}
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
                <Input placeholder="请输入供应商名称名称" onPressEnter={onSearch} />
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
            rowKey="supId"
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
            <Col span={8}>
              <Form.Item label="供应商名称" name="supName">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="供应商联系人" name="providerUser">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="联系方式" name="supPhone">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="交易进行中金额" name="jxz">
                 <Button danger type="primary">
                  {btnData.jxz}
                 </Button>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="交易已完成金额" name="ywc">
              <Button  type="primary">
              {btnData.ywc}
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
