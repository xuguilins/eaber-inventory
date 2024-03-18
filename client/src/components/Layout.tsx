import React, { useEffect, useState } from "react";
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  AppstoreOutlined,
  GiftOutlined,
  HddOutlined,
  RotateLeftOutlined,
  RotateRightOutlined,
  TransactionOutlined,
  ScanOutlined,
  PieChartOutlined,
  InsertRowAboveOutlined,
  DatabaseOutlined,
  EuroCircleOutlined,
  TagOutlined,
  TagsOutlined,
  UsergroupDeleteOutlined,
  DownOutlined,
} from "@ant-design/icons";
import { Layout, Menu, Button, theme, Dropdown, Space, Badge } from "antd";
import type { MenuProps } from "antd";
const { Header, Sider, Content } = Layout;
import "./css/index.css";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "@/store";
import LayoutTab from "./LayoutTab";
import dRoutes from "@/router";
import HubClient from "@/utils/hubCenter";
const App: React.FC = () => {
  const { menuStore, homeStore, userStore } = useStore();
  const [collapsed, setCollapsed] = useState(false);
  const itemsMenus: MenuProps["items"] = [
    {
      label: "控制台",
      key: "/",
      icon: <AppstoreOutlined />,
    },
    {
      label: "商品管理",
      key: "/product",
      icon: <GiftOutlined />,
    },
    {
      label: "采购管理",
      key: "caigou",
      icon: <HddOutlined />,
      children: [
        {
          label: "进货单",
          key: "/purchases/InOrder",
          icon: <RotateLeftOutlined />,
        },
        {
          label:  (
            <>
              <span>
                退货单
                {/* <div className="hot">new</div> */}
              </span>
            </>
          ),
          key: "/purchases/outOrder",
          icon: <RotateRightOutlined />,
        },
      ],
    },
    {
      label: "销售管理",
      key: "sell",
      icon: <TransactionOutlined />,
      children: [
        {
          label: "销售开单",
          key: "/sell/form",
          icon: <ScanOutlined />,
        },
      ],
    },
    {
      label: "订单管理",
      key: "SubMenu",
      icon: <PieChartOutlined />,
      children: [
        {
          label: "全部订单",
          key: "/all/order",
          icon: <InsertRowAboveOutlined />,
        },
      ],
    },
    {
      label: "其它费用",
      key: "extra",
      icon: <PieChartOutlined />,
      children: [
        {
          label: "其它信息维护",
          key: "/extra/extralist",
          icon: <InsertRowAboveOutlined />,
        }
      ],
    },
    {
      label: "交易中心",
      key: "jyzx",
      icon: <PieChartOutlined />,
      children: [
        {
          label: "客户往来",
          key: "/customer/jiaoyi",
          icon: <InsertRowAboveOutlined />,
        },
        {
          label: "供应商往来(进货)",
          key: "/gysjh/jiaoyi",
          icon: <InsertRowAboveOutlined />,
        },
    
      ],
    },
    {
      label: "基础数据管理",
      key: "base",
      icon: <DatabaseOutlined />,
      children: [
        {
          label: "供应商管理",
          key: "/base/supiler",
          icon: <UsergroupDeleteOutlined />,
        },
        { label: "分类管理", key: "/base/cate", icon: <TagOutlined /> },
        {
          label: (
            <>
              <span>
                单位管理
                {/* <div className="hot">new</div> */}
              </span>
            </>
          ),

          key: "/base/unit",

          icon: <EuroCircleOutlined />,
        },
        { label: "编码管理", key: "/base/rule", icon: <TagsOutlined /> },
        {
          label: (
            <>
              <span>
                支付管理
                {/* <div className="hot">new</div> */}
              </span>
            </>
          ),

          key: "/base/pay",

          icon: <TagsOutlined />,
        },
        { label: "客户管理", key: "/base/customer", icon: <TagsOutlined /> },
        { label: "其它收入类型维护", key: "/base/extrain", icon: <TagsOutlined /> },
        { label: "其它支出类型维护", key: "/base/extraout", icon: <TagsOutlined /> },
      ],
    },
  ];
  const navtion = useNavigate();
  const items: MenuProps["items"] = [
    {
      label: <span onClick={()=>{
         navtion('/user/userinfo',{replace:true })
      }}>个人中心</span>,
      key: "0",
    },
    {
      type: "divider",
    },
    {
      label: (
        <span
          onClick={() => {
            sessionStorage.clear();

            userStore.setAccessToken("");
            HubClient.closeConnection();
            navtion("/login", { replace: true });
          }}
        >
          退出登陆
        </span>
      ),
      key: "1",
    },
  ];
  const location = useLocation();
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  const handlerMenuClick = (e) => {
    homeStore.setActiveKey(e.key);
    navtion(e.key);
  };
  HubClient.onReviceMessage("SendUser", async function () {
    console.log("message");
  });
  useEffect(() => {
    menuStore.setMenus(dRoutes);
  }, []);
  useEffect(() => {
    const key = location.pathname;
    let item = menuStore.plains.find((x) => x.key === key);
    if (!item) {
      const keyArrys = key.split("/");
      const key0 = keyArrys[1];
      item = menuStore.plains.find((x) => x.key.split(":")[0] === key0);
    }
    if (item) homeStore.addTab(item);
  }, [location]);

  return (
    <Layout>
      <Sider
        trigger={null}
        collapsible
        collapsed={collapsed}
        width="230"
        style={{
          height: "100vh",
          overflowX: "hidden",
          overflowY: "auto",
          width: "180",
        }}
      >
        <div className="demo-logo-vertical">易步小店库存管理</div>
        <Menu
          theme="dark"
          mode="inline"
          onClick={(e) => handlerMenuClick(e)}
          defaultSelectedKeys={[homeStore.activeKey]}
          selectedKeys={[homeStore.activeKey]}
          items={itemsMenus}
        />
      </Sider>
      <Layout>
        <Header
          style={{
            padding: 0,
            background: colorBgContainer,
            height: "40px",
            lineHeight: "40px",
          }}
        >
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{
              fontSize: "16px",
              width: 40,
              height: 40,
            }}
          />
          <Dropdown className="userDown" menu={{ items }}>
            <a onClick={(e) => e.preventDefault()}>
              <Space>
                {userStore.getLoginName()}
                <DownOutlined />
              </Space>
            </a>
          </Dropdown>
        </Header>
        <div className="layTab">
          <LayoutTab />
        </div>
        <Content
          style={{
            margin: "5px 5px",
            background: colorBgContainer,
          }}
        >
          <div>
            <Outlet />
          </div>
        </Content>
      </Layout>
      {/* <Modal title="更新日志(v.1.0.0)"
      footer={
        <> 
          <Button danger>关闭</Button>
        </>
      }
       open={updateShow} closable={false}>
        <p>1、发多少姐继续姐姐斯蒂芬森 是</p>
        <p>1、发多少姐继续姐姐斯蒂芬森 是</p>
        <p>1、发多少姐继续姐姐斯蒂芬森 是</p>
      </Modal> */}
    </Layout>
  );
};

export default observer(App);
