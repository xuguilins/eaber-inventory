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
  Tree,
  TreeSelect,
} from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import { IProductInfo, useProductManager } from "./index";
import "./index.css";
const ProductInfo: React.FC = () => {
  const columns: ColumnsType<IProductInfo> = [
    {
      title: "操作",
      key: "action",
      fixed: "left",
      width: 100,
      render: (data: any) => (
        <Button type="link" onClick={() => handlerEdit(data)}>
          编辑
        </Button>
      ),
    },
    {
      title: "产品编码",
      dataIndex: "productCode",
      key: "productCode",
      width: 200,
    },
    {
      title: "产品名称",
      dataIndex: "productName",
      key: "productName",
      width: 200,
    },
    {
      title: "规格型号",
      dataIndex: "productModel",
      key: "productModel",
      width: 200,
    },
    { title: "供应商", dataIndex: "supName", key: "supName", width: 200 },
    
    { title: "单位", dataIndex: "cateName", key: "cateName", width: 200 },
    { title: "单位", dataIndex: "unitName", key: "unitName", width: 200 },
    {
      title: "换算率",
      dataIndex: "conversionRate",
      key: "conversionRate",
      width: 200,
    },
    {
      title: "库存数量",
      dataIndex: "inventoryCount",
      key: "inventoryCount",
      width: 200,
    },
    {
      title: "初期成本",
      dataIndex: "initialCost",
      key: "initialCost",
      width: 200,
    },
    { title: "进货价", dataIndex: "purchase", key: "purchase", width: 200 },
    { title: "零售价", dataIndex: "sellPrice", key: "sellPrice", width: 200 },
    { title: "批发价", dataIndex: "wholesale", key: "wholesale", width: 200 },
    { title: "最高库存", dataIndex: "maxStock", key: "maxStock", width: 200 },
    { title: "最低库存", dataIndex: "minStock", key: "minStock", width: 200 },
    {
      title: "描述",
      dataIndex: "remark",
      key: "remark",
      width: 200,
    },
    {
      title: "状态",
      dataIndex: "enable",
      key: "enable",
      fixed: "right",
      width: 100,
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
  const showTree =(keys:string[])=> {
    if (keys && keys.length>0) {
        return <>
         <Tree
              checkable={false}
              fieldNames={{
                title: "cateName",
                key: "cateId",
                children: "children",
              }}
               treeData={productCate}
               onSelect={(e)=>handlerTreeSelect(e)}
               defaultExpandedKeys={keys}
               defaultExpandAll={true}
            />
        </>
    }
  }
  const {
    productMain,
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
    productSearch,
    treeData,
    productCate,
    productUnit,
    productSupiler,
    handlerTreeSelect,
    defaultCates
  } = useProductManager();
  return (
    <>
      {contextHolder}
      {modelcontextHolder}

      <div className="productMain">
        <div className="productLeft">
          <div className="productTitle">分类</div>
          <div className="treeLeft">
             {showTree(defaultCates)}
          </div>
        </div>
        <div className="productRight">
          <MainContent
            title={productMain.title}
            total={productMain.total}
            haveImport
            exportType={5}
            pageIndex={productSearch.pageIndex}
            data={productMain.data}
            onChange={(index: number) => productMain.onChange(index)}
            searchBar={
              <>
                <Form layout="inline" form={searchForm}>
                <Form.Item label="产品名称" name="cateId" style={{display:'none'}}>
                    <Input placeholder="请输入产品名称" onPressEnter={onSearch}  />
                  </Form.Item>
                  <Form.Item label="产品名称" name="keyWord">
                    <Input placeholder="请输入产品名称" onPressEnter={onSearch}   />
                  </Form.Item>
                  <Form.Item label="供应商名称" name="supileName">
                    <Input placeholder="请输入供应商名称"  onPressEnter={onSearch}  />
                  </Form.Item>
                  <Form.Item label="规格型号" name="productModel">
                    <Input placeholder="请输入规格型号" onPressEnter={onSearch}  />
                  </Form.Item>

                  <Form.Item label="描述" name="remark">
                    <Input placeholder="请输入描述" onPressEnter={onSearch} />
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
                dataSource={productMain.data}
                pagination={false}
                scroll={{ x: 1000 }}
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
              layout={window.webConfig.formLayOut}
              autoComplete="off"
              initialValues={ruleData}
            >
              <Form.Item label="id" name="id" style={{ display: "none" }}>
                <Input />
              </Form.Item>

              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item label="产品编码" name="productCode">
                    <Input placeholder="系统自动生成" disabled />
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item
                    label="产品名称"
                    name="productName"
                    rules={[{ required: true, message: "请输入产品名称" }]}
                  >
                    <Input placeholder="请输入产品名称" />
                  </Form.Item>
                </Col>
              </Row>

              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item label="规格型号" name="productModel">
                    <Input placeholder="请输入规格型号" />
                  </Form.Item>
                </Col>

                <Col span={12}>
                  <Form.Item
                    label="所属分类"
                    name="cateId"
                    rules={[{ required: true, message: "请选择所属分类" }]}
                  >
                    <TreeSelect
                      showSearch
                      style={{ width: "100%" }}
                      treeData={treeData}
                      dropdownStyle={{ maxHeight: 400, overflow: "auto" }}
                      placeholder="Please select"
                      allowClear
                      treeDefaultExpandAll
                    />
                  </Form.Item>
                </Col>
              </Row>

              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item
                    label="单位名称"
                    name="unitId"
                    rules={[{ required: true, message: "请选择单位" }]}
                  >
                    <Select
                      showSearch
                      placeholder="选择单位"
                      optionFilterProp="children"
                      onSearch={onSearch}
                      fieldNames={{
                        label: "name",
                        value: "id",
                      }}
                      options={productUnit}
                    />
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item
                    label="供应商"
                    name="supilerId"
                    rules={[{ required: true, message: "请选择供应商" }]}
                  >
                    <Select
                      showSearch
                      placeholder="请选择供应商"
                      optionFilterProp="children"
                      onSearch={onSearch}
                      fieldNames={{
                        label: "name",
                        value: "supileId",
                      }}
                      options={productSupiler}
                    />
                  </Form.Item>
                </Col>
              </Row>
              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item label="换算率" name="conversionRate">
                    <Input placeholder="请输入换算率" />
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item
                    label="库存数量"
                    name="inventoryCount"
                    rules={[{ required: true, message: "请输入库存数量" }]}
                  >
                    <InputNumber
                      min={0}
                      placeholder="请输入库存数量"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
              </Row>

              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item
                    label="初期成本"
                    name="initialCost"
                    rules={[{ required: true, message: "请输入初期成本" }]}
                  >
                    <InputNumber
                      min={0}
                      placeholder="请输入初期成本"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item
                    label="进货价"
                    name="purchase"
                    rules={[{ required: true, message: "请输入进货价" }]}
                  >
                    <InputNumber
                      min={0}
                      placeholder="请输入进货价"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
              </Row>
              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item
                    label="零售价"
                    name="sellPrice"
                    rules={[{ required: true, message: "请输入零售价" }]}
                  >
                    <InputNumber
                      min={1}
                      placeholder="请输入零售价"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item
                    label="批发价"
                    name="wholesale"
                    rules={[{ required: true, message: "请输入批发价" }]}
                  >
                    <InputNumber
                      min={0}
                      placeholder="请输入批发价"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
              </Row>
              <Row gutter={8}>
                <Col span={12}>
                  <Form.Item
                    label="最高库存"
                    name="maxStock"
                    rules={[{ required: true, message: "请输入最高库存" }]}
                  >
                    <InputNumber
                      min={0}
                      placeholder="请输入最高库存"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item
                    label="最低库存"
                    name="minStock"
                    rules={[{ required: true, message: "请输入最低库存" }]}
                  >
                    <InputNumber
                      min={0}
                      placeholder="请输入最低库存"
                      style={{ width: "100%" }}
                    ></InputNumber>
                  </Form.Item>
                </Col>
              </Row>
              <Row gutter={8}>
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
        </div>
      </div>
    </>
  );
};
export default ProductInfo;
