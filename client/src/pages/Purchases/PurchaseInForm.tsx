import {
  AutoComplete,
  Button,
  Card,
  Col,
  DatePicker,
  Drawer,
  Form,
  Input,
  InputNumber,
  Result,
  Row,
  Select,
  Table,
  TreeSelect,
} from "antd";
import "./index.css";

import locale from "antd/es/date-picker/locale/zh_CN";
import useInForm from "./inForm.ts";
import { useNavigate } from "react-router-dom";
import { useStore } from "@/store/index.ts";

const PurchaseInForm: React.FC<any> = () => {
  const {
    puraseForm,
    chanel,
    setChanel,
    tabRow,
    searchProduct,
    onFoucus,
    unitData,
    treeData,
    onFocusCate,
    onFocusUnit,
    onRemoveRow,
    onProductSearch,
    onSelectValue,
    onSave,
    addRow,
    onRowColumnsChange,
    onSupileSelect,
    supileData,
    selectSupiler,
    supileShow,
    isCreate,
    titile,
    onCloseClick
  } = useInForm();
  const SupileItem = (type: number) => {
    if (type === 0) {
      return (
        <Form.Item label="供应商" name="supileName" required>
          <Input
            placeholder="请选择"
            addonAfter={
              <>
                {/* onClick={selectSupiler} */}
                <span style={{ cursor: "pointer" }} onClick={selectSupiler}>
                  选择
                </span>
              </>
            }
          />
        </Form.Item>
      );
    } else {
      return (
        <Form.Item label="客户名称" name="supileName" required>
          <Input placeholder="手动输入采购来源的客户名称" />
        </Form.Item>
      );
    }
  };
  const navtion = useNavigate()
  const { homeStore }  =useStore()
  const formShow = (isCreate) => {
    if (isCreate) {
      return (
        <Result
          status="success"
          title="采购进货单已创建成功"
          extra={[
            <Button type="primary"
            onClick={()=>  {
              homeStore.removeTab('/purchases/InForm')
              navtion('/')
            }}
         key="console">
              返回首页
            </Button>,
            <Button key="buy" 
              onClick={()=>{
                window.location.reload()
              }}
             >
              再来一单
            </Button>,
          ]}
        />
      );
    } else {
      return (
        <div className="productList">
          <p className="inFormP">{titile}</p>
          <div className="ImianForm">
            <Card title="基本信息" bordered={false}>
              <Form
                name="basic"
                form={puraseForm}
                labelCol={{ span: 6 }}
                wrapperCol={{ span: 24 }}
                autoComplete="off"
              >
                <Form.Item label="id" name="id" style={{ display: "none" }}>
                  <Input />
                </Form.Item>
                <Form.Item
                  label="supplierId"
                  name="supplierId"
                  style={{ display: "none" }}
                >
                  <Input />
                </Form.Item>
                <Row gutter={5}>
                  <Col span={8}>
                    <Form.Item label="进货编码" name="inOrderCode">
                      <Input placeholder="系统自动生成" disabled />
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="进货日期" name="inOrderTime" required>
                      <DatePicker
                        placeholder="请选择..."
                        style={{ width: "100%" }}
                        format="YYYY/MM/DD"
                        locale={locale}
                      />
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="供货渠道" name="channelType" required>
                      <Select
                        onChange={(e: number) => {
                          setChanel(e);
                          puraseForm.setFieldValue("supileName", "");
                          puraseForm.setFieldValue("inUser", "");
                          puraseForm.setFieldValue("inPhone", "");
                        }}
                        options={[
                          { label: "自行采购", value: 1 },
                          { label: "供应商采购", value: 0 },
                        ]}
                      ></Select>
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="物流单号" name="logistics">
                      <Input placeholder="请输入物流单号" />
                    </Form.Item>
                  </Col>
                  <Col span={8}>{SupileItem(chanel)}</Col>
                  <Col span={8}>
                    <Form.Item label="联系人" name="inUser">
                      <Input
                        placeholder="请输入联系人"
                        disabled={chanel === 0}
                      />
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="联系电话" name="inPhone">
                      <Input
                        placeholder="请输入联系电话"
                        disabled={chanel === 0}
                      />
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="描述" name="remark">
                      <Input />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Card>

            <Card title="商品明细" bordered={false}>
              <Button
                type="primary"
                onClick={addRow}
                style={{ marginBottom: 16 }}
              >
                添加商品
              </Button>

              <Table
                rowKey="id"
                size="small"
                pagination={false}
                bordered
                dataSource={[...tabRow]}
                scroll={{
                  x: 1500,
                }}
                columns={[
                  {
                    title: "序号",
                    dataIndex: "index",
                    width: 60,
                    fixed: "left",
                    render(_, record, index) {
                      return <>{index + 1}</>;
                    },
                  },
                  {
                    title: "操作",
                    dataIndex: "control",
                    width: 100,
                    fixed: "left",
                    render(_, record, index) {
                      return (
                        <Button
                          danger
                          onClick={() => onRemoveRow(String(record.id), index)}
                        >
                          移除
                        </Button>
                      );
                    },
                  },
                  {
                    title: "产品名称",
                    dataIndex: "productName",
                    width: 300,
                    render(text, record, index) {
                      return (
                        <AutoComplete
                          placeholder="请输入产品名称"
                          options={searchProduct}
                          onFocus={onFoucus}
                          value={record.productName}
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "productName")
                          }
                          onSelect={(a, b) =>
                            onSelectValue(a, b, record, index)
                          }
                          onSearch={(text) => onProductSearch(text)}
                          style={{ width: "100%" }}
                        />
                      );
                    },
                  },
                  {
                    title: "型号",
                    dataIndex: "productModel",
                    width:300,
                    // shouldCellUpdate(record, prevRecord) {
                    //   if (record.productModel && prevRecord.productModel) {
                    //     return record.productModel === prevRecord.productModel;
                    //   }
                    //   return false;
                    // },
                    render(text, record, index) {
                      return (
                        <Input
                          type="text"
                          value={text}
                          onChange={(e) =>
                            onRowColumnsChange(
                              e.target.value,
                              index,
                              "productModel"
                            )
                          }
                        />
                      );
                    },
                  },
                  {
                    title: "当前库存",
                    dataIndex: "invertCount",
                    width: 100,
                    // render(value, record, index) {
                    
                    //     return value
                    // },

                    // shouldCellUpdate(record, prevRecord) {
                    //   return record.invertCount === prevRecord.invertCount;
                    // },
                  },
                  {
                    title: "进货数量",
                    dataIndex: "productCount",
                    width: 120,
                    render(text, record, index) {
                      return (
                        <InputNumber
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "productCount")
                          }
                          min={0}
                          value={text}
                          controls={false}
                        />
                      );
                    },
                  },
                  {
                    title: "分类",
                    dataIndex: "cateId",
                    width: 200,
                    render(value, record, index) {
                      return (
                        <TreeSelect
                          showSearch
                          value={value}
                          placeholder="请选择"
                          allowClear
                          onFocus={onFocusCate}
                          style={{ width: "100%" }}
                          treeDefaultExpandAll
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "cateId")
                          }
                          treeData={treeData}
                        />
                      );
                    },
                    // shouldCellUpdate(record, prevRecord) {
                    //   if (record.cateId && prevRecord.cateId) {
                    //     return record.cateId === prevRecord.cateId;
                    //   }
                    //   return false;
                    // },
                  },
                  {
                    title: "单位",
                    dataIndex: "unitId",
                    width: 150,
                    // shouldCellUpdate(record, prevRecord) {
                    //   if (record.unitId && prevRecord.unitId) {
                    //     return record.unitId === prevRecord.unitId;
                    //   }
                    //   return false;
                    // },
                    render(value, record, index) {
                     
                      return (
                        <Select
                          defaultValue={value}
                          value={value}
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "unitId")
                          }
                          placeholder="选择单位"
                          style={{ width: "100%" }}
                          options={unitData}
                          onFocus={onFocusUnit}
                        />
                      );
                    },
                  },
                  // {
                  //   title: "成本价",
                  //   dataIndex: "productIncost",
                  //   width: 100,
                  //   // shouldCellUpdate(record, prevRecord) {
                  //   //   return record.productIncost === prevRecord.productIncost;
                  //   // },
                  //   render(text, record, index) {
                  //     return (
                  //       <InputNumber
                  //         min={0}
                  //         controls={false}
                  //         value={text}
                  //         onChange={(e) =>
                  //           onRowColumnsChange(e, index, "productIncost")
                  //         }
                  //       />
                  //     );
                  //   },
                  // },
                  {
                    title: "批发价",
                    dataIndex: "productWocost",
                    width: 100,
                    shouldCellUpdate(record, prevRecord) {
                      return record.productWocost === prevRecord.productWocost;
                    },
                    render(text, record, index) {
                      return (
                        <InputNumber
                          min={0}
                          controls={false}
                          value={text}
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "productWocost")
                          }
                        />
                      );
                    },
                  },
                  {
                    title: "进价单价",
                    dataIndex: "productPrice",
                    width: 100,
                    // shouldCellUpdate(record, prevRecord) {
                    //   return record.productPrice === prevRecord.productPrice;
                    // },
                    render(text, record, index) {
                      return (
                        <InputNumber
                          value={text}
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "productPrice")
                          }
                          min={0}
                          controls={false}
                        />
                      );
                    },
                  },
                  {
                    title: "售价",
                    dataIndex: "sellPrice",
                    width: 100,
                    // shouldCellUpdate(record, prevRecord) {
                    //   return record.sellPrice === prevRecord.sellPrice;
                    // },
                    render(text, record, index) {
                      return (
                        <InputNumber
                          min={0}
                          controls={false}
                          value={text}
                          onChange={(e) =>
                            onRowColumnsChange(e, index, "sellPrice")
                          }
                        />
                      );
                    },
                  },
                  {
                    title: "进价总价",
                    dataIndex: "productAll",
                    width: 100,
                    // shouldCellUpdate(record, prevRecord) {
                    //   return record.productAll === prevRecord.productAll;
                    // },
                    render(text, record) {
                      return Number(record.productPrice * record.productCount);
                    },
                  },
                ]}
              />
            </Card>
            <Button onClick={onSave} type="primary" className="saveBtn">
              保存
            </Button>
          </div>
        </div>
      );
    }
  };
  return (
    <>
      {formShow(isCreate)}
      {/* 选择供应商 */}
      {/* 选择供应商 */}
      <Drawer title="供应商明细" 
      onClose={onCloseClick}
      width="35%" open={supileShow}>
        <Row gutter={10}>
          {supileData.map((item) => {
            return (
              <Col span={12} style={{ marginTop: 5 }} key={item.id}>
                <Card
                  title={item.supileName}
                  extra={
                    <Button onClick={() => onSupileSelect(item)}>选择</Button>
                  }
                >
                  <p>编码: {item.supileCode}</p>
                  <p>联系人1: {item.supileUser}</p>
                  <p>联系人2: {item.supileUTEN}</p>
                  <p>联系电话1: {item.supileTel}</p>
                  <p>联系电话2: {item.supileSen}</p>
                </Card>
              </Col>
            );
          })}
        </Row>
      </Drawer>
    </>
  );
};
export default PurchaseInForm;
