import { Button, Modal } from "antd";
import "./printForm.css";
import printJS from "print-js";
import { useEffect, useState } from "react";
import { useStore } from "@/store";
const PrintForm: React.FC<any> = (props: any) => {
  const { order, tableData, show, childClose } = props;
  const [sumCount, setSumCount] = useState<number>(0);
  const [sumPrice, setSumPrice] = useState<number>(0);
  const [allPrice, setAllPrice] = useState<number>(0);
  const { userStore } = useStore();
  useEffect(() => {
    let sumCount = 0;
    let sumPrice = 0;
    let allPrice = 0;
    tableData.forEach((item) => {
      sumCount += Number(item.count);
      sumPrice += Number(item.price);
      allPrice += Number(item.allPrice);
    });
    setSumCount(sumCount);
    setSumPrice(sumPrice);
    setAllPrice(allPrice);
  }, [tableData]);
  const onPrintClick = () => {
    printJS({
      type: "html",
      printable: document.getElementById("printContent"),
      ignoreElements: ["igBtn"],
      documentTitle: "",
      style: `
        .printTitle {
            text-align: center;
            font-size: 16px;
            font-weight: bold;
        }
        .printRow {
            float: left;
            width: 50%;
            font-size: 8px;
        }
        .mTTAble {
            border-spacing: 0;
            border: 1px solid black;
            width: 100%;
            word-spacing: 0;
            text-align: center;
        }
        .mTTAble th {
            border-bottom: none;
            border-right: 1px solid;
            font-size: 8px;
        }
        .mTTAble th:last-child {
            border-right: none;
        }
        .mTTAble td:last-child {
            border-right: none;
        }
        .mTTAble td {
            border-right: 1px solid;
            border-bottom: none;
            font-size: 8px;
            border-spacing: 0;
            text-align: left;
            white-space: nowrap;
            border-top: 1px solid;

        }
        .mTTAble td:first-child {
            text-align: center;
        }
        .mTTAble td:nth-child(4) {
            text-align: center;
        }
        .mTTAble td:nth-child(5) {
            text-align: center;
        }
        .mTTAble td:nth-child(6) {
            text-align: center;
        }
        .mTTAble td:nth-child(7) {
            text-align: center;
        }
        `,
      header: null,
      showModal: true,
      scanStyles: false,
    });
  };
  const onPrintClose = () => {
    childClose(false);
  };
  return (
    <>
      <Modal
        title="打印订单"
        mask={true}
        closable={false}
        width={"80%"}
        style={{ top: 20 }}
        footer={null}
        open={show}
      >
        <div className="mainPrint">
          <div className="printContent" id="printContent">
            <p className="printTitle"> 易步小店库存 零售单</p>
            <div className="printRow">单据日期：{order.orderTime}</div>
            <div className="printRow">单据编号：{order.orderCode}</div>
            <div className="printRow">购买单位：{order.orderUser}</div>
            <div className="printRow">联系电话： {order.orderTel}</div>
            <div className="printRow">备注：</div>
            <table className="mTTAble">
              <thead>
                <tr>
                  <th>序号</th>
                  <th>编号</th>
                  <th>名称</th>
                  <th>单位</th>
                  <th>数量</th>
                  <th>单价</th>
                  <th>金额</th>
                  <th>备注</th>
                </tr>
              </thead>
              <tbody>
                {tableData.map((item, index) => {
                  return (
                    <tr key={index}>
                      <td> {item.index}</td>
                      <td> {item.productCode}</td>
                      <td>{item.productName}</td>
                      <td> {item.unitName}</td>
                      <td> {item.count}</td>
                      <td> {item.price}</td>
                      <td> {item.allPrice}</td>
                      <td> {item.remark}</td>
                    </tr>
                  );
                })}
                <tr>
                  <td>合计</td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td>{sumCount}</td>
                  <td>{sumPrice}</td>
                  <td>{allPrice}</td>
                  <td></td>
                </tr>
              </tbody>
            </table>
            <div className="printRow">应收金额: {allPrice}</div>
            <div className="printRow">实收金额: {allPrice}</div>
            <div className="printRow"> 制单人: {userStore.getLoginName()}</div>
            <div className="printRow">签收人: </div>
            <div className="printRow"> 地址: 龙南市龙南镇广场西路楠木树下4号</div>
            <div className="printRow"> 微信/电话: 13517975697</div>
            
           
            <br />
            <Button type="primary" id="igBtn" onClick={onPrintClick}>
              打印
            </Button>
            <Button
              type="primary"
              id="igBtn"
              danger
              onClick={onPrintClose}
              style={{ margin: 5 }}
            >
              关闭
            </Button>
          </div>
        </div>
      </Modal>
    </>
  );
};
export default PrintForm;
