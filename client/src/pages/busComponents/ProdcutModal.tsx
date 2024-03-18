import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { Button, Form, Input, InputNumber, Modal, Table } from "antd";
import React, { useEffect, useState } from "react";
import { ISellDetail } from "@/store/SellStore";
const { Search } = Input;
export interface IModalProps {
  visiable: boolean;
  onModalClose(): void;
  onMoalSave(data:Array<ISellDetail>): void;
}
const ProdcutModal: React.FC<IModalProps> = (props: IModalProps) => {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [total, setTotal] = useState<number>(0);
  const [tableData, setTableData] = useState<any[]>([]);
  const [selectRowKeys, setSelectRowKeys] = useState<React.Key[]>([]);
  const [keyword, setKeyword] = useState<string>("");
  const [saveData,setSaveData] = useState<ISellDetail[]>([])
  const rowSelection = {
    selectedRowKeys: selectRowKeys,
    onChange: (selectedRowKeys: React.Key[]) => {
      setSelectRowKeys(selectedRowKeys);
      saveStore(selectedRowKeys);
    },
  };
  const saveStore = (ids: React.Key[]) => {
    const storeArrys:ISellDetail[]=[]
    for (let i = 0; i < ids.length; i++) {
      const id = ids[i];
      const model = tableData.find((item) => item.productCode == id);
      if (model) {
        const store = modelStore(model);
        storeArrys.push(store)
      }
    }
    setSaveData(storeArrys)
  };
  const modelStore = (model: any) => {
    const store: ISellDetail = {
      productCode: model.productCode,
      productModel: model.productModel,
      productName: model.productName,
      unitName: model.unitName,
      inventoryCount: 0,
      sellPrice: model.sellPrice,
      sellAllPrice:0
    };
    return store;
  };
  const loadProduct = async () => {
    const { success, data, total } = await alovaInstance
      .Post<IReturnResult, any>(urls.getProductPages, {
        pageIndex: pageIndex,
        pageSize: 10,
        keyWord: keyword,
      })
      .send();
    if (success) {
      setTotal(total);
      setTableData(data);
    }
  };
  useEffect(() => {
    async function init() {
      await loadProduct();
    }
    init();
  }, [pageIndex, keyword]);
  useEffect(()=>{
    setSelectRowKeys([])
  },[props.visiable])
  return (
    <>
      <Modal
        title="选择商品"
        open={props.visiable}
        onOk={()=>props.onMoalSave(saveData)}
        onCancel={props.onModalClose}
        width="70%"
        style={{ top: 20 }}
      >
        <div className="searchModal">
          <Search
            placeholder="请输入商品名称"
            onSearch={(e) => setKeyword(e)}
            allowClear
            enterButton="搜索"
          />
        </div>
        <Table
          bordered
          size="small"
          rowSelection={rowSelection}
          columns={[
            {
              title: "编号",
              dataIndex: "productCode",
              key: "productCode",
              width: 120,
            },
            {
              title: "名称",
              dataIndex: "productName",
              key: "productName",
            },
            {
              title: "型号",
              dataIndex: "productModel",
              key: "productModel",
              width: 200,
            },
            {
              title: "单位",
              dataIndex: "unitName",
              key: "unitName",
              width: 100,
            },
            {
              title: "库存数量",
              dataIndex: "inventoryCount",
              key: "inventoryCount",
              width: 100,
            },
            {
              title: "售价",
              dataIndex: "sellPrice",
              key: "sellPrice",
              width: 100,
            },
          ]}
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
          rowKey="productCode"
        />
      </Modal>
    </>
  );
};
export default ProdcutModal;
