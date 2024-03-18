import {
  Button,
  Collapse,
  CollapseProps,
  Form,
  Modal,
  Pagination,
  Progress,
  Upload,
  UploadFile,
  UploadProps,
  message,
} from "antd";
import { IMainData } from ".";
import {
  DownloadOutlined,
  ToTopOutlined,
  UndoOutlined,
  UploadOutlined,
} from "@ant-design/icons";
import React, { useState } from "react";
import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { RcFile } from "antd/es/upload";

const MainContent: React.FC<IMainData<any>> = ({
  title,
  searchBar,
  toolBar,
  mainTable,
  onRefresh,
  haveExport,
  haveImport,
  total,
  pageIndex,
  onChange,
  exportType,
}: IMainData<any>) => {
  const excelUrl = `/public/excels/${title}.xlsx`;
  const handlerRefresh = () => {
    onRefresh();
  };
  const [importShow, setImportShow] = useState<boolean>(false);
  const [fileList, setFileList] = useState<UploadFile[]>([]);
  const [prevent, setPrevent] = useState<number>(0);
  const [messageApi, contextHolder] = message.useMessage();
  const handlerCancle = () => {
    setFileList([]);
    setImportShow(false);
    setPrevent(0);
  };
  const props: UploadProps = {
    onRemove: (file) => {
      const index = fileList.indexOf(file);
      const newFileList = fileList.slice();
      newFileList.splice(index, 1);
      setFileList(newFileList);
    },
    beforeUpload: (file) => {
      if (fileList.length <= 0) {
        setFileList([...fileList, file]);
      } else {
        messageApi.error("只能上传一个文件");
      }
      return false;
    },
    fileList,
  };
  const SearchItems: CollapseProps['items'] = [
    {
      key: '1',
      label: '高级查询',
      children:searchBar,
    },
  ];
  return (
    <>
      {contextHolder}
      <div className="mainContent">
        <div className="mainControl">
           <Collapse  style={{float:'right'}} size="small" items={SearchItems} expandIconPosition="end"  />
          {/* <div className="searchBar">12111{searchBar}</div> */}
          <div className="toolBar">
            {toolBar}
            <Button
              type="default"
              icon={<UndoOutlined />}
              style={{ marginLeft: 8 }}
              onClick={handlerRefresh}
            >
              刷新
            </Button>
          </div>
          <div className="mainTable">{mainTable}</div>
        </div>
        {/* 导入数据 */}
        <div className="pageBar">
          <Pagination
            total={total}
            onChange={(e) => onChange(e)}
            current={pageIndex}
            style={{ lineHeight: "40px", marginRight: 30 }}
            showSizeChanger={false}
            showTotal={(total) => `共 ${total} 条`}
          />
        </div>
      </div>
     
    </>
  );
};
export default MainContent;
