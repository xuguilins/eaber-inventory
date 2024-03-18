import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import { Button, Col, Form, Input, Modal, Radio, Row, Switch } from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { ICustomerInfo, useCustomerManager } from "./index";
const CustomerPage: React.FC = () => {
  const columns: ColumnsType<ICustomerInfo> = [
    {
      title: "操作",
      key: "action",
      fixed: "left",
      render: (data: any) => (
        <Button type="link" onClick={() => handlerEdit(data)}>
          编辑
        </Button>
      ),
    },
    { title: "客户编码", dataIndex: "cuCode", key: "cuCode" },
    { title: "客户名称", dataIndex: "cuName", key: "cuName" },
    { title: "联系人", dataIndex: "cuUser", key: "cuUser" },
    { title: "手机", dataIndex: "cuTel", key: "cuTel" },
    { title: "电话", dataIndex: "cuPhone", key: "cuPhone" },
    { title: "联系地址", dataIndex: "cuAddress", key: "cuAddress" },
    {
      title: "描述",
      dataIndex: "remark",
      key: "remark",
    },
    {
      title: "状态",
      dataIndex: "enable",
      key: "enable",
      render: (data, record) => {
        return (
          <Switch
            checkedChildren="启用"
            unCheckedChildren="禁用"
            onChange={() => onStatuChange(record)}
            checked={data}
          />
        );
      },
    },
  ];
  const {
    unitMain,
    onRefresh,
    rowSelection,
    deleteHandler,
    contextHolder,
    modelcontextHolder,
    handlerAdd,
    modelShow,
    saveForm,
    closeForm,
    ruleForm,
    handlerEdit,
    ruleData,
    title,
    onStatuChange,
    searchForm,
    onSearch,
    unitSearch,
  } = useCustomerManager();
  return (
    <>
      {contextHolder}
      {modelcontextHolder}
      <MainContent
        title={unitMain.title}
        total={unitMain.total}
        pageIndex={unitSearch.pageIndex}
        data={unitMain.data}
        onChange={(index: number) => unitMain.onChange(index)}
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
            <Button type="primary" onClick={handlerAdd} icon={<PlusOutlined />}>
              新增
            </Button>
            <Button
              type="primary"
              icon={<CloseOutlined />}
              danger
              style={{ marginLeft: 8 }}
              onClick={deleteHandler}
            >
              删除
            </Button>
          </>
        }
        mainTable={
          <Table
            size="small"
            rowSelection={rowSelection}
            columns={columns}
            rowKey="id"
            dataSource={unitMain.data}
            pagination={false}
          />
        }
      ></MainContent>

      {/* 新增/编辑 */}
      <Modal
        title={title}
        closable={false}
        onOk={saveForm}
        open={modelShow}
        onCancel={closeForm}
        okText="保存"
        cancelText="关闭"
        maskClosable={false}
        width={window.webConfig.dialogWidth}
      >
        <Form
          form={ruleForm}
          name="basic"
          autoComplete="off"
          initialValues={ruleData}
          layout={window.webConfig.formLayOut}
        >
          <Form.Item label="id" name="id" style={{ display: "none" }}>
            <Input />
          </Form.Item>
          <Row gutter={16}>
            <Col span={8}>
              <Form.Item label="客户编码" name="customerCode">
                <Input disabled placeholder="系统自动生成" />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item
                label="客户名称"
                name="customerName"
                rules={[{ required: true, message: "请填写客户名称" }]}
              >
                <Input />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item
                label="联系人"
                name="customerUser"
                rules={[{ required: true, message: "请填写联系人" }]}
              >
                <Input />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="手机" name="telNumber">
                <Input />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="电话" name="phoneNumber">
                <Input />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="联系地址" name="address">
                <Input />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="描述" name="remark">
                <Input />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="状态" name="enable">
                <Radio.Group>
                  <Radio value={true}>启用</Radio>
                  <Radio value={false}>禁用</Radio>
                </Radio.Group>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </>
  );
};
export default CustomerPage;
