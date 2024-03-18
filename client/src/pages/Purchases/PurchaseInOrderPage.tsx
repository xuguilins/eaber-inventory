import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import "dayjs/locale/zh-cn";
import "./index.css";
import { Button, Col, DatePicker, Drawer, Form, Input, Row, Space } from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { IPurshaseInOrder, usePurchaseInOrderManager } from "./PurchaseInOrder";
import { observer } from "mobx-react-lite";
import FormItem from "antd/es/form/FormItem";
const PurchaseInOrderPage: React.FC = () => {
  const showAction = (data) => {
    if (data.inOStatus !== 2) {
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
  const Maincolumns: ColumnsType<IPurshaseInOrder> = [
    {
      title: "操作",
      key: "action",
      fixed: "left",
      render: (_, data) => {
        return showAction(data);
      }
    },
    { title: "单据编码", dataIndex: "inOrderCode", key: "inOrderCode" },
    { title: "进货日期", dataIndex: "inTime", key: "inTime", width: 100 },
    {
      title: "供货渠道",
      dataIndex: "chanpel",
      key: "chanpel",
      width: 100,
      render(value) {
        return value === 1 ? (
          <Button>自行采购</Button>
        ) : (
          <Button>供应商采购</Button>
        );
      },
    },
    { title: "物流单号", dataIndex: "logistics", key: "logistics" },
    { title: "联系人", dataIndex: "inUser", key: "inUser" },
    { title: "联系电话", dataIndex: "inPhone", key: "inPhone" },
    { title: "供应商", dataIndex: "supileName", key: "supileName" },
    { title: "进货总数", dataIndex: "allCount", key: "allCount" },
    { title: "进货总价", dataIndex: "allPrice", key: "allPrice" },
    {
      title: "进货单状态",
      dataIndex: "inOStatus",
      key: "inOStatus",
      render(value) {
        return value === 1 ? (
          <Button>进行中</Button>
        ) : (
          <Button type="primary">已完成</Button>
        );
      },
    },
    {
      title: "描述",
      dataIndex: "remark",
      key: "remark",
    },
  ];
  const {
    total,
    pageIndex,
    onRefresh,
    onAdd,
    onSearch,
    searchForm,
    onDelete,
    onPageChange,
    onEdit,
    purchaseList,
    rowSelection,
    modelcontextHolder,
    drawShow,
    handlderView,
    viewForm,
    tableData,
    handlerClose,
    payStatus,
    inStatus,
    onSaveStatus,
  } = usePurchaseInOrderManager();
  const BtnStatus = ( inStatus: number) => {
    if (inStatus === 1) {
      return (
        <>
          <Button type="primary" onClick={() => onSaveStatus(2)}>
            确定收货
          </Button>
        </>
      );
    } else {
      return (
        <>
          <Button type="primary" disabled>
            进货单已确认
          </Button>
        </>
      );
    }
  };
  return (
    <>
      {modelcontextHolder}
      <MainContent
        title="采购进货单"
        total={total}
        pageIndex={pageIndex}
        data={[]}
        onChange={(index: number) => onPageChange(index)}
        searchBar={
          <>
            <Form layout="inline" form={searchForm}>
              <Form.Item label="联系人" name="userName">
                <Input placeholder="请输入联系人" onPressEnter={onSearch} />
              </Form.Item>
              <Form.Item label="联系电话" name="tel">
                <Input placeholder="请输入联系电话" onPressEnter={onSearch} />
              </Form.Item>
              <Form.Item label="供应商名称" name="supileName">
                <Input placeholder="请输入供应商名称" onPressEnter={onSearch} />
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
            <Button type="primary" onClick={onAdd} icon={<PlusOutlined />}>
              新增
            </Button>
            <Button
              type="primary"
              icon={<CloseOutlined />}
              danger
              style={{ marginLeft: 8 }}
              onClick={onDelete}
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
            dataSource={purchaseList}
            pagination={false}
            onRow={(record) => {
              return {
                onDoubleClick: () => handlderView(record),
              };
            }}
          />
        }
      ></MainContent>

      {/* 抽屉 */}
      <Drawer
        title="进货单信息"
        placement="right"
        width="45%"
        mask
        maskClosable
        open={drawShow}
        closable={false}
        extra={
          <Space>
            <Button type="primary" onClick={handlerClose} danger>
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
              <Form.Item label="进货编号" name="inOrderCode">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="进货日期" name="inOrderTime">
                <Input readOnly />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="供货渠道" name="channelType">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <FormItem label="客户名称" name="supileName">
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
              <Form.Item label="本次进货数量" name="allCount">
                <Input readOnly />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="本次进货总价" name="allPrice">
                <Input readOnly />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="描述" name="remark">
                <Input readOnly />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={10}>
            <Col span={12}>
              <Form.Item label="进货状态" name="inOStatus">
                <Button type="primary">
                  {inStatus === 1  ? "进行中" : "已完成"}
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
              { title: "进货单价", dataIndex: "productPrice" },
              { title: "进货总价", dataIndex: "productAll" },
            ]}
            dataSource={tableData}
          />
          <div className="footPayBtn">{BtnStatus(inStatus)}</div>
        </Form>
      </Drawer>
    </>
  );
};
export default observer(PurchaseInOrderPage);
