import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import {
  Button,
  Col,
  Form,
  Input,
  InputNumber,
  Modal,
  Radio,
  Row,
  Select,
  Switch,
} from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { ICustomerInfo, useExtraManager } from "./index";
import TextArea from "antd/es/input/TextArea";
const ExtraPage: React.FC = () => {
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
    { title: "单据编码", dataIndex: "orderCode", key: "orderCode" },
    {
      title: "收入/支出",
      dataIndex: "extraType",
      key: "extraType",
      render: (value) => {
        return value === 1 ? (
          <Button type="primary" danger>
            {" "}
            支出
          </Button>
        ) : (
          <Button type="primary"> 收入</Button>
        );
      },
    },

    { title: "类型", dataIndex: "typeName", key: "typeName" },
    { title: "费用金额", dataIndex: "price", key: "price" },

    {
      title: "描述",
      dataIndex: "remark",
      key: "remark",
    },
    {
      title: "单据是否有效",
      dataIndex: "enable",
      key: "enable",
      render: (data, record) => {
        return (
          <Switch
            checkedChildren="有效"
            unCheckedChildren="无效"
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

    title,
    onStatuChange,
    searchForm,
    onSearch,
    unitSearch,
    onSelectRadio,
    typeOption,
  } = useExtraManager();
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
              <Form.Item label="收入/支出" name="extraType">
                <Select
                  style={{ width: "100%" }}
                  options={[
                    { label: "全部", value: -1 },
                    { label: "支出", value: 1 },
                    { label: "收入", value: 2 },
                  ]}
                />
              </Form.Item>
              <Form.Item label="类型" name="keyWord">
                <Input
                  placeholder="请输入收入或支出的类型名称"
                  onPressEnter={onSearch}
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
          layout={window.webConfig.formLayOut}
        >
          <Form.Item label="id" name="id" style={{ display: "none" }}>
            <Input />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="支出/收入编码" name="orderCode">
                <Input disabled placeholder="系统自动生成" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="支出或收入"
                name="extraType"
                rules={[{ required: true, message: "请选择记录方式" }]}
              >
                <Radio.Group onChange={(e) => onSelectRadio(e)}>
                  <Radio value={1}>支出</Radio>
                  <Radio value={2}>收入</Radio>
                </Radio.Group>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="选择类型"
                name="typeName"
                rules={[{ required: true, message: "请填写选择类型" }]}
              >
                <Select
                  style={{ width: "100%" }}
                  fieldNames={{
                    label: "name",
                    value: "value",
                  }}
                  options={typeOption}
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="金额"
                name="price"
                rules={[{ required: true, message: "请填写金额" }]}
              >
                <InputNumber style={{ width: "100%" }} />
              </Form.Item>
            </Col>

            <Col span={8}>
              <Form.Item label="描述" name="remark">
                <TextArea />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </>
  );
};
export default ExtraPage;
