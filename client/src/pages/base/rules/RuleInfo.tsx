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
import { IRuleInfo, useRuleManager } from "./index";

const RuleInfo: React.FC = () => {
  const columns: ColumnsType<IRuleInfo> = [
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
    {
      title: "编码类型",
      dataIndex: "ruleType",
      key: "ruleType",
      render(type) {
        return <Button type="primary">{ruleOption[type]}</Button>;
       
      },
    },
    { title: "编码名称", dataIndex: "name", key: "name" },
    { title: "编码前缀", dataIndex: "rulePix", key: "rulePix" },
    { title: "自增数", dataIndex: "identityNum", key: "identityNum" },
    { title: "格式", dataIndex: "ruleFormatter", key: "ruleFormatter" },
    { title: "补位数", dataIndex: "appendNum", key: "appendNum" },
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
    ruleMain,
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
    ruleSearch,
    ruleOption,
    ruleSelect
  } = useRuleManager();
  return (
    <>
      {contextHolder}
      {modelcontextHolder}
      <MainContent
        title={ruleMain.title}
        total={ruleMain.total}
        pageIndex={ruleSearch.pageIndex}
        onChange={(index: number) => ruleMain.onChange(index)}
        data={ruleMain.data}
        searchBar={
          <>
            <Form layout="inline" form={searchForm}>
              <Form.Item label="名称" name="keyWord">
                <Input placeholder="请输入编码名称" onPressEnter={onSearch} />
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
            dataSource={ruleMain.data}
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
          initialValues={ruleData}
        >
          <Form.Item label="id" name="id" style={{ display: "none" }}>
            <Input />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                label="编码类型"
                name="ruleType"
                rules={[{ required: true, message: "请选择编码类型" }]}
              >
                <Select
                fieldNames={{
                  label:'name',
                  value:'value'
                }}
                  options={ruleSelect}
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="编码名称"
                name="name"
                rules={[{ required: true, message: "请填写编码名称" }]}
              >
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                label="编码前缀"
                name="rulePix"
                rules={[{ required: true, message: "请填写编码前缀" }]}
              >
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="编码格式"
                name="ruleFormatter"
                rules={[{ required: true, message: "请选择编码格式" }]}
              >
                <Select
                  options={[
                    { label: "年月", value: "yyyyMM" },
                    { label: "年月日", value: "yyyyMMdd" },
                    { label: "年月日时", value: "yyyyMMddHH" },
                    { label: "年月日时分", value: "yyyyMMddHHmm" },
                    { label: "年月日时分秒", value: "yyyyMMddHHmmss" },
                  ]}
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="每次自增数" name="identityNum">
                <InputNumber min={1} max={6} style={{ width: "100%" }} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="前置补0数" name="appendNum">
                <InputNumber min={4} max={6} style={{ width: "100%" }} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="描述" name="remark">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
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
export default RuleInfo;
