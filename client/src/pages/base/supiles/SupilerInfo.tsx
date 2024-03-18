import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import { Button, Col, Form, Input, Modal, Radio, Row, Switch } from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { ISupilerInfo, useSupilerManager } from "./index";
import TextArea from "antd/es/input/TextArea";
const SupilerInfo: React.FC = () => {
  const columns: ColumnsType<ISupilerInfo> = [
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
    { title: "编码", dataIndex: "supileCode", key: "supileCode" },
    { title: "名称", dataIndex: "supileName", key: "supileName" },
    { title: "联系人1", dataIndex: "userONE", key: "userONE" },
    { title: "联系人2", dataIndex: "userTWO", key: "userTWO" },
    { title: "手机号1", dataIndex: "telONE", key: "telONE" },
    { title: "手机号2", dataIndex: "telTWO", key: "telTWO" },
    { title: "座机1", dataIndex: "phoneONE", key: "phoneONE" },
    { title: "座机2", dataIndex: "phoneTWO", key: "phoneTWO" },
    { title: "联系地址", dataIndex: "address", key: "address" },
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
    supilerMain,
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
    suilerSearch,
  } = useSupilerManager();
  return (
    <>
      {contextHolder}
      {modelcontextHolder}
      <MainContent
        title={supilerMain.title}
        total={supilerMain.total}
        haveImport
        exportType={1}
        pageIndex={suilerSearch.pageIndex}
        data={supilerMain.data}
        onChange={(index: number) => supilerMain.onChange(index)}
        searchBar={
          <>
            <Form layout="inline" form={searchForm}>
              <Row>
                <Col span={6}>
                  <Form.Item label="供应商名称" name="keyWord">
                    <Input placeholder="请输入单位名称" onPressEnter={onSearch} />
                  </Form.Item>
                </Col>
                <Col span={6}>
                  <Form.Item label="手机号1" name="telOne">
                    <Input placeholder="请输入手机号1" onPressEnter={onSearch} />
                  </Form.Item>
                </Col>
                <Col span={6}>
                  <Form.Item label="座机1" name="phoneOne">
                    <Input placeholder="请输入座机1"  onPressEnter={onSearch} />
                  </Form.Item>
                </Col>
                <Col span={6}>
                  <Form.Item label="联系人1" name="userOne">
                    <Input placeholder="请输入联系人1" onPressEnter={onSearch} />
                  </Form.Item>
                </Col>
                <Col span={6} style={{ margin: "10px 0" }}>
                  <Form.Item label="联系地址" name="address">
                    <Input placeholder="请输入联系地址 " onPressEnter={onSearch} />
                  </Form.Item>
                </Col>
                <Col span={6} style={{ margin: "10px 0" }}>
                  <Form.Item label="描述" name="remark">
                    <Input placeholder="请输入描述" onPressEnter={onSearch} />
                  </Form.Item>
                </Col>
                <Col span={6} style={{ margin: "10px 0" }}>
                  <Form.Item>
                    <Button type="primary" onClick={onSearch}>
                      查询
                    </Button>
                  </Form.Item>
                </Col>
              </Row>
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
            dataSource={supilerMain.data}
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
          labelAlign="left"
          layout={window.webConfig.formLayOut}
          autoComplete="off"
          initialValues={ruleData}
        >
          <Form.Item label="id" name="id" style={{ display: "none" }}>
            <Input />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="供应商编码" name="supileCode">
                <Input placeholder="系统自动生成" disabled />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                label="供应商名称"
                name="supileName"
                rules={[{ required: true, message: "请输入供应商名称" }]}
              >
                <Input placeholder="请输入供应商名称" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                label="手机号1"
                name="telONE"
                rules={[{ required: true, message: "请输入手机号1" }]}
              >
                <Input placeholder="请输入手机号1" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="手机号2" name="telTWO">
                <Input placeholder="请输入手机号1" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                label="座机1"
                name="phoneONE"
                rules={[{ required: true, message: "请输入座机1" }]}
              >
                <Input placeholder="请输入座机1" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="座机2" name="phoneTWO">
                <Input placeholder="请输入座机1" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                label="联系人1"
                name="userONE"
                rules={[{ required: true, message: "联系人1" }]}
              >
                <Input placeholder="请输入联系人1" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="联系人2" name="userTWO">
                <Input placeholder="请输入联系人1" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={24}>
              <Form.Item
                label="联系地址"
                name="address"
                rules={[{ required: true, message: "请输入联系地址" }]}
              >
                <TextArea placeholder="请输入联系地址" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="描述" name="remark">
                <TextArea />
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
export default SupilerInfo;
