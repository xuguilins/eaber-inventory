import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Modal, Input, Table, TableColumnsType } from "antd";
import { ColumnsType } from "antd/es/table";
import React, { useEffect, useState } from "react";
import {} from "react-router-dom";
export interface IPurashProps {
  visiable: boolean;
  onMoalSave(data: any): void;
  onModalClose(): void;
}
export interface IPurashModel {
  code: string;
  pushTime: string;
  userName: string;
  userTel: string;
  id: string;
}
const { Search } = Input;
const PurashInModal: React.FC<IPurashProps> = (props: IPurashProps) => {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [keyword, setKeyword] = useState<string>("");
  const [total, setTotal] = useState<number>(0);
  const [tableData, setTableData] = useState<IPurashModel[]>([]);
  const [selectRow,setSelectRow] = useState<IPurashModel>()
  const [selectRowKeys,setSelectRowKeys]=useState<React.Key[]>([])
  const loadPages = async () => {
    const { success, data, total } = await alovaInstance
      .Post<IReturnResult, any>(urls.getPurashModal, {
        pageIndex: pageIndex,
        pageSize: 10,
        keyWord: keyword,
      })
      .send();
    setTotal(total);
    if (success) {
    
      setTableData(data);
    }
  };
  const columns: ColumnsType<any> = [
    {
      title: "编号",
      dataIndex: "code",
      key: "code",
    },
    {
      title: "进货日期",
      dataIndex: "pushTime",
      key: "pushTime",
    },
    {
      title: "联系人",
      dataIndex: "userName",
      key: "userName",
    },
    {
      title: "联系方式",
      dataIndex: "userTel",
      key: "userTel",
    },
  ];
  const onSelectChange =(key:any,rows:IPurashModel[])=>{
    setSelectRow(rows[0])
    setSelectRowKeys(key)
  }
  useEffect(() => {
    async function init() {
      await loadPages();
    }
    init();
  }, [props.visiable, keyword, pageIndex]);
  return (
    <>
      <Modal
        title="选择单据"
        open={props.visiable}
        onOk={() => {
          props.onMoalSave(selectRow)
          setSelectRowKeys([])
        }
        }
        onCancel={props.onModalClose}
        width="55%"
        style={{ top: 20 }}
      >
        <div className="searchModal">
          <Search
            placeholder="请输入单据编号或联系人"
            onSearch={(e) => setKeyword(e)}
            allowClear
            enterButton="搜索"
          />
        </div>
        <Table
          bordered
          size="small"
          rowSelection={{
            type: "radio",
            onChange:(key,rows)=>onSelectChange(key,rows),
            selectedRowKeys:selectRowKeys,
           
          }}
          columns={columns}
          dataSource={tableData}
          pagination={{
            showTotal: (total) => `共 ${total} 条`,
            pageSize: 10,
            total: total,
            current: pageIndex,
            onChange(page, pageSize) {
              setPageIndex(page);
            },
          }}
          rowKey="code"
        />
      </Modal>
    </>
  );
};
export default PurashInModal;
