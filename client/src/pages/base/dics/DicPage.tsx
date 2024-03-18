import MainContent from "@/components/MainContent";
import Table, { ColumnsType } from "antd/es/table";
import {
  Button,
  Col,
  Form,
  Input,
  Modal,
  Radio,
  Row,
  Switch,
  message,
} from "antd";
import { CloseOutlined, PlusOutlined } from "@ant-design/icons";
import React, { useEffect } from "react";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
export interface IDicSearch {
  pageIndex: number;
  pageSize: number;
  dicType: number;
  keyWord: string;
}
export interface IDicData {
  dicCode: string;
  dicName: string;
  dicType: number;
  enable: boolean;
  id: string;
  remark: string;
}
export interface IDicProps {
  type: number;
  title: string;
  dicName: string;
  dicCode: string;
}
export interface IOptionData {
  name: string;
  value: number;
}

const DicPage: React.FC<IDicProps> = (model: IDicProps) => {
  const [dicSearch, setDicSearch] = React.useState<IDicSearch>({
    pageIndex: 1,
    pageSize: 10,
    dicType: model.type,
    keyWord: "",
  });
  const [tableData, setTableData] = React.useState<IDicData[]>([]);
  const [total, setTotal] = React.useState<number>(0);
  const [option, setOption] = React.useState<Record<number, string>>();
  const [modelShow, setModelShow] = React.useState<boolean>(false);
  const [selectedRowKeys, setSelectedRowKeys] = React.useState<React.Key[]>([]);
  const [modal,contextHolder ] = Modal.useModal()
  const [dicForm] = Form.useForm();
  const columns: ColumnsType<any> = [
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
      title: "模块类型",
      dataIndex: "dicType",
      key: "dicType",
      render(value) {
        return option[value];
      },
    },
    { title: model.dicName, dataIndex: "dicName", key: "dicName" },
    { title: model.dicCode, dataIndex: "dicCode", key: "dicCode" },
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
  const [searchForm] = Form.useForm();
  const handlerEdit = (data: any) => {
    if (data) {
      Object.keys(data).forEach((key) => {
        const value = data[key];
        dicForm.setFieldValue(key, value);
      });
      setModelShow(true);
    }
  };
  const onStatuChange = async (data: any) => {
    const id = data.id 
    const nurl = urls.updateSystemStatus+'/'+id;
    const { success,message:msg  } = await alovaInstance.Post<IReturnResult, any>(nurl)
    .send()
    if (success) {
      message.success(msg)
      onRefresh()
    }else {
      message.error(msg)
    }

  };
  const onChange = (index: number) => {
    setDicSearch({
      ...dicSearch,
      pageIndex: index,
    });
  };
  const loadSysDic = async () => {
    const { data, success, total } = await alovaInstance
      .Post<IReturnResult, any>(urls.getSystemDicPage, dicSearch)
      .send();
    if (success) {
      setTableData(data);
      setTotal(total);
    }
  };
  useEffect(() => {
    async function init() {
      await loadSysDic();
    }
    init();
  }, [dicSearch]);
  const loadTypes = async () => {
    const { data, success } = await alovaInstance
      .Get<IReturnResult, any>(urls.getAllTypes)
      .send();
    if (success) {
      const json = {};
      data.forEach((item) => {
        json[item.value] = item.name;
      });
      setOption(json);
    }
  };
  useEffect(() => {
    async function init() {
      await loadTypes();
    }
    dicForm.setFieldValue("enable", true);
    init();
  }, []);
  const onSearch = () => {
    const kwd = searchForm.getFieldValue('keyWord') ?? ''
    setDicSearch({
      ...dicSearch,
      keyWord: kwd,
      pageIndex:1,
      pageSize:10,
      dicType:model.type
    })
  };
  const onRefresh = () => {
    setDicSearch({
      pageIndex: 1,
      pageSize: 10,
      keyWord: "",
      dicType: model.type,
    });
  };
  const handlerAdd = () => {
    setModelShow(true);
  };
  const deleteHandler = async () => {
    if (selectedRowKeys.length<=0) {
      message.warning('请选择要删除的数据')
      return 
    } else {
       const confirm = await  modal.confirm({
          title:'删除警告',
          mask:true,
          content:'确认删除选中的数据吗？',
          maskClosable:false,
          okText:'确认',
          cancelText:'取消',
        })
        if (confirm) {
          const {success,message:msg} = await alovaInstance.Delete<IReturnResult, any>(urls.deleteSystem, selectedRowKeys).send()
          if (success) {
            message.success(msg)
            onRefresh()
          } else {
            message.error(msg)
          }

        }
    }
  };
  const onSelectChange = (e: any) => {
    setSelectedRowKeys(e);
  }; 
   const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange,
  };
  const closeForm = () => {
    dicForm.resetFields();
    setModelShow(false);
  };
  const saveForm = async () => {
    const name = dicForm.getFieldValue("dicName");
    const code = dicForm.getFieldValue("dicCode");
    const id = dicForm.getFieldValue("id");
    const remark = dicForm.getFieldValue("remark");
    let enable = dicForm.getFieldValue("enable");
    if (enable === undefined) enable = false;
    if (!name) {
      message.error("请输入" + model.dicName);
      return;
    }
    if (id) {
      // 更新
      const postJson: IDicData = {
        id: id,
        dicCode: code ?? name,
        dicName: name,
        dicType: model.type,
        remark: remark ?? "",
        enable: enable,
      };
      const  {success,message:msg} = await alovaInstance.Post<IReturnResult,IDicData>(urls.updateSystemDic,postJson)
      .send()
      if (success) {
        message.success(msg)
        closeForm()
      } else {
        message.error(msg)
      }
      onRefresh()
    } else {
      // 新增
      const postJson: IDicData = {
        id: "",
        dicCode: code ?? name,
        dicName: name,
        dicType: model.type,
        remark: remark ?? "",
        enable: enable,
      };
      const  {success,message:msg} = await alovaInstance.Post<IReturnResult,IDicData>(urls.createSystemDic,postJson)
      .send()
      if (success) {
        message.success(msg)
       closeForm()
      } else {
        message.error(msg)
      }
      onRefresh()
    }
  };
  return (
    <>
    {contextHolder}
      <MainContent
        title={""}
        total={total}
        pageIndex={dicSearch.pageIndex}
        data={tableData}
        onChange={(index: number) => onChange(index)}
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
            dataSource={tableData}
            pagination={false}
          />
        }
      ></MainContent>

      {/* 新增/编辑 */}
      <Modal
        title={model.title}
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
          name="basic"
          autoComplete="off"
          form={dicForm}
          layout={window.webConfig.formLayOut}
        >
          <Form.Item label="id" name="id" style={{ display: "none" }}>
            <Input />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                label={model.dicName}
                name="dicName"
                rules={[{ required: true, message: "请填写" + model.dicName }]}
              >
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label={model.dicCode+"(默认与名称一致,可不填)"} name="dicCode">
                <Input placeholder={"默认与" + model.dicName + "保持一致"} />
              </Form.Item>
            </Col>
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
export default DicPage;
