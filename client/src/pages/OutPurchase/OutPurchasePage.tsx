import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import "dayjs/locale/zh-cn";
import "./index.css";
import {
  Button,
  Card,
  Col,
  DatePicker,
  Drawer,
  Form,
  Input,
  Row,
  Space,
} from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { observer } from "mobx-react-lite";
import FormItem from "antd/es/form/FormItem";
import { useOutPage } from "./index";
const OutPurchasePage: React.FC = () => {
  const showAction = (data) => {
    if (data.outStatus !==2 ) {
      return (
        <>
          <Button type="link" onClick={() => onEdit(data.id)}>
            编辑
          </Button>
          <Button type="link" onClick={() => handlderView(data)}>
            状态操作
          </Button>
        </>
      );
    }  else {
      return (
        <>
          <Button type="link" onClick={() => handlderView(data)}>
            查看
          </Button>
        </>
      );
    }
  };
  const Maincolumns: ColumnsType<any> = [
    {
      title: "操作",
      key: "action",
      fixed: "left",
      render: (_, data) => 
          showAction(data)
      
    },
    { title: "单据编码", dataIndex: "outOrderCode", key: "outOrderCode" },
    { title: "单据时间", dataIndex: "outTime", key: "outTime", width: 100 },
    { title: "物流单号", dataIndex: "logistics", key: "logistics" },
    { title: "退货联系人", dataIndex: "outUser", key: "outUser" },
    { title: "联系电话", dataIndex: "outPhone", key: "outPhone" },
    { title: "退货总数", dataIndex: "outAllCount", key: "outAllCount" },
    { title: "退货总价", dataIndex: "outAllPrice", key: "outAllPrice" },
    {
      title: "退货状态",
      dataIndex: "outStatus",
      key: "outStatus",
      render(value) {
        return value === 1 ? (
          <Button >退货中</Button>
        ) : (
          <Button type="primary">已完成</Button>
        );
      },
    },
    
    {
      title: "退货原因/描述",
      dataIndex: "remark",
      key: "remark",
    },
  ];
  const showBtns = (data)=>{
    if(data) {
      if (data  === 1) {
        return <Button danger type="primary" onClick={onConfrim}>确认完成</Button>
      }else {
        return <Button>已确认</Button>
      }
    }
  
  }
  const {
    onAddClick,
    tableData,
    total,
    search,
    onSearch,
    searchForm,
    onRefresh,
    onEdit,
    handlderView,
    rowSelection,
    onPageChange,
    onDeleteClick,
    contextHolder,
    drawShow,
    drawData,
    viewForm,
    onConfrim,
    onCloseDlig
  } = useOutPage();
  return (
    <>
    {contextHolder}
      <MainContent
        title="采购退货单"
        total={total}
        pageIndex={search.pageIndex}
        data={[]}
         onChange={(index: number) => onPageChange(index)}
        searchBar={
          <>
            {/* form={searchForm} */}
            <Form layout="inline" form={searchForm}>
              <Form.Item label="联系人" name="userName">
                <Input placeholder="请输入联系人" />
              </Form.Item>
              <Form.Item label="联系电话" name="tel">
                <Input placeholder="请输入联系电话" />
              </Form.Item>
              <Form.Item label="单据开始时间" name="startTime">
                <DatePicker
                  placeholder="选择开始时间"
                  style={{ width: "100%" }}
                />
              </Form.Item>
              <Form.Item label="单据结束时间" name="endTime">
                <DatePicker
                  placeholder="选择结束时间"
                  style={{ width: "100%" }}
                />
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
            <Button type="primary" onClick={onAddClick} icon={<PlusOutlined />}>
              新增
            </Button>
            <Button
              type="primary"
              icon={<CloseOutlined />}
              danger
              style={{ marginLeft: 8 }}
              onClick={onDeleteClick}
            >
              删除
            </Button>
          </>
        }
        mainTable={
          <Table
            size="small"
             rowSelection={rowSelection}
            columns={Maincolumns}
            rowKey="id"
            dataSource={tableData}
            pagination={false}
            onRow={(record) => {
              return {
                onDoubleClick: () => {},
              };
            }}
          />
        }
      ></MainContent>

      {/* 抽屉 */}
      <Drawer
        title="退货单信息"
        placement="right"
        width={window.webConfig.dialogWidth}
        mask
        maskClosable
        open={drawShow}
        closable={false}
        forceRender
        extra={
          <Space>
            <Button type="primary" onClick={onCloseDlig} danger>
              关闭
            </Button>
          </Space>
        }
      >
        <Form
            form={viewForm}
          name="basic"
          labelCol={{ span: 6 }}
          wrapperCol={{ span: 24 }}
          autoComplete="off"
        >
          <p>基本信息</p>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="退货编码" name="outOrderCode">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="退货日期" name="outOrderTime">
                <Input readOnly />
              </Form.Item>
            </Col>
          
          </Row>
          <Row gutter={10}>
          <Col span={12}>
              <Form.Item label="关联进货单据" name="inOrderCode">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <FormItem label="供应商名称 " name="supildName">
                <Input readOnly />
              </FormItem>
            </Col>
          </Row>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="联系人" name="inUser">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="联系方式" name="inPhone">
                <Input readOnly />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="退货原因" name="reason">
                <Input readOnly />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="退货单状态" name="outStatus">
                <Button type="primary" danger>
                  {drawData.outStatus === 1  ? "进行中" : "已完成"}
                </Button>
              </Form.Item>
            </Col>
           
          </Row>
          <p>进货明细</p>
          <Table
            pagination={false}
            rowKey="id"
            key="id"
            columns={[
              { title: "商品编号", dataIndex: "productCode" },
              { title: "商品名称", dataIndex: "productName" },
              { title: "商品型号", dataIndex: "productModel" },
              { title: "进货数量", dataIndex: "productCount" },
              { title: "进货单价", dataIndex: "inPrice" },
              { title: "退货数量", dataIndex: "returnCount" },
              { title: "退货单价", dataIndex: "outPrice" },
              { title: "退货总价", dataIndex: "allprice",render(value, record:any, index) {
                  return (<span>{(Number(record.outPrice))* (Number(record.returnCount)) }</span>)
              }, },
            ]}
            dataSource={drawData.details}
          />
          <div className="footPayBtn">
             {showBtns(drawData?.outStatus)}
          </div>
        </Form>
      </Drawer>
    </>
  );
};
export default observer(OutPurchasePage);
