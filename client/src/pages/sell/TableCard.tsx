import Table from "antd/es/table";
import { useStore } from "@/store";
import { observer } from "mobx-react-lite";
import Input from "antd/es/input/Input";
import { Button, InputNumber } from "antd";
import { ISellDetail } from "@/store/SellStore";
import React, { useEffect, useState } from "react";
const TableCard: React.FC<any> = ({ tableValue,disable }) => {
  const { sellStore } = useStore();
  const SumFooter = (data: readonly ISellDetail[]) => {
    let sumCount = 0;
    let sumPrice = 0;
    let allPrice = 0;
    data.forEach((item) => {
      sumCount += Number(item.inventoryCount);
      sumPrice += Number(item.sellPrice);
      const value = Number(item.inventoryCount) * Number(item.sellPrice);
      allPrice += value;
    });
    return (
      <Table.Summary fixed>
        <Table.Summary.Row>
          <Table.Summary.Cell index={0}>合计</Table.Summary.Cell>
          <Table.Summary.Cell index={1}></Table.Summary.Cell>
          <Table.Summary.Cell index={2}></Table.Summary.Cell>
          <Table.Summary.Cell index={3}></Table.Summary.Cell>
          <Table.Summary.Cell index={4}></Table.Summary.Cell>
          <Table.Summary.Cell index={5}>{sumCount}</Table.Summary.Cell>
          <Table.Summary.Cell index={6}>{sumPrice}</Table.Summary.Cell>
          <Table.Summary.Cell index={7}>
            <span style={{ color: "red" }}>{allPrice}</span>
          </Table.Summary.Cell>
          <Table.Summary.Cell index={7}></Table.Summary.Cell>
        </Table.Summary.Row>
      </Table.Summary>
    );
  };
  const [tableData, setTableData] = useState<ISellDetail[]>([]);
  const onRemove = (code: string) => {
    const deepTable = [...tableData];
    const newTable = deepTable.filter((d) => d.productCode !== code);
    setTableData(newTable);
    sellStore.setDetail(newTable)
  };
  const onNumberChange=(propName:string, code:string,value:number )=>{
    const deepData = [...tableData]
    const index = deepData.findIndex(d=>d.productCode === code)
    if (index>-1) {
         deepData[index][propName] = value 
         deepData[index].sellAllPrice = (deepData[index].sellPrice * value)
         setTableData(deepData)
         sellStore.setDetail(deepData)
    }
  }
  const onInputChange =(code:string,value:string )=>{
    const deepData = [...tableData]
    const index = deepData.findIndex(d=>d.productCode === code)
    if (index>-1) {
         deepData[index].remark = value 
         setTableData(deepData)
         sellStore.setDetail(deepData)
    }
  }
  useEffect(() => {
    setTableData(tableValue);
  }, [tableValue]);
  return (
    <>
      <Table
        bordered
        size="small"
        columns={[
          {
            title: "操作",
            dataIndex: "control",
            key: "control",
            width: 100,
            render: (text, record) => {
              return (
                <Button danger disabled={disable} onClick={() => onRemove(record.productCode)}>
                  移除
                </Button>
              );
            },
          },
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
          },
          {
            title: "单位",
            dataIndex: "unitName",
            key: "unitName",
            width: 80,
          },
          {
            title: "数量",
            dataIndex: "inventoryCount",
            key: "inventoryCount",
            width: 80,
            render:(_,record)=>{
              return <>
                <InputNumber defaultValue={_}   disabled={disable} onChange={(e)=> onNumberChange('inventoryCount',record.productCode,Number(e)) }/>
              </>
            }
          },
          {
            title: "售价",
            dataIndex: "sellPrice",
            key: "sellPrice",
            width: 80,
            render:(_,record)=>{
              return <>
                <InputNumber defaultValue={_}   disabled={disable} onChange={(e)=> onNumberChange('sellPrice',record.productCode,Number(e)) }/>
              </>
            }
          },
          {
            title: "总价",
            dataIndex: "allPrice",
            key: "allPrice",
            width: 80,
            render:(_,record)=>{
              return <> { record.inventoryCount * record.sellPrice }</>
            }
          },
          {
            title: "描述",
            dataIndex: "remark",
            key: "remark",
            width: 280,
            render:(_,record)=>{
              return <> <Input defaultValue={_}  disabled={disable} onChange={(e)=>onInputChange(record.productCode,e.target.value)}  /> </>
            }
          },
        ]}
        rowKey="productCode"
        dataSource={tableData}
        pagination={false}
         summary={(data) => SumFooter(data)}
      />
    </>
  );
};
export default observer(TableCard);
