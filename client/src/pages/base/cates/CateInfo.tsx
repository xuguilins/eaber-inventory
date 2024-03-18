import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import {
  Button,
  Col,
  Divider,
  Form,
  Input,
  Modal,
  Radio,
  Row,
  Switch,
  TreeSelect,
} from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { ICateInfo, useCateManager } from "./index";
import { observer } from "mobx-react-lite";
import "./cates.css";
import CateTree from "@/pages/busComponents/CateTree";
const CateInfo: React.FC = () => {
  const columns: ColumnsType<ICateInfo> = [
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

    { title: "分类名称", dataIndex: "name", key: "name" },
    { title: "上级分类", dataIndex: "parentName", key: "parentName" },
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
    cateMain,
    onRefresh,
    rowSelection,
    deleteHandler,
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
    cateSearch,
    treeData,
    onNodeClick,
  } = useCateManager();
  return (
    <>
      <div className="cateMain">

         <div className="leftItem">
         <div className="treeMain">
              <CateTree
                onNodeClick={onNodeClick}
                isLoad={false}
                listData={treeData}
              />
              <Divider type="vertical" />
            </div>
         </div>
         <div className="rightItem">

         <MainContent
              title={cateMain.title}
              total={cateMain.total}
              pageIndex={cateSearch.pageIndex}
              data={cateMain.data}
              onChange={(index: number) => cateMain.onChange(index)}
              searchBar={
                <>
                  <Form layout="inline" form={searchForm}>
                    <Form.Item label="名称" name="keyWord">
                      <Input placeholder="请输入分类名称"  onPressEnter={onSearch}/>
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
                  <Button
                    type="primary"
                    onClick={handlerAdd}
                    icon={<PlusOutlined />}
                  >
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
                  dataSource={cateMain.data}
                  pagination={false}
                />
              }
            ></MainContent>
         </div>
      </div>
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
                label="分类名称"
                name="name"
                rules={[{ required: true, message: "请填写分类名称" }]}
              >
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="上级分类" name="parentId">
                <TreeSelect
                  showSearch
                  treeDefaultExpandAll
                  style={{ width: "100%" }}
                  dropdownStyle={{ maxHeight: 400, overflow: "auto" }}
                  placeholder="请选择"
                  allowClear
                  treeData={treeData}
                />
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
export default observer(CateInfo);
