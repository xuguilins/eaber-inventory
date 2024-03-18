import { Button, Checkbox, Form, Input, Tabs } from "antd";
import "./Login.css";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { urls } from "@/api/urls";
import useMessage from "antd/es/message/useMessage";
import { useNavigate } from "react-router-dom";
import { useStore } from "@/store";
import HubClient from "@/utils/hubCenter";
type FieldType = {
  userName?: string;
  userPass?: string;
};

const Login: React.FC = () => {
  const [messageApi, contextHandler] = useMessage();
  const navtion = useNavigate();
  const { userStore } = useStore();
  const onLogin = async (values: any) => {
    const { success, data } = await alovaInstance
      .Post<IReturnResult, any>(urls.userlogin, values)
      .send();
    if (success) {
      const { access_Token, refresh_Token, login_Name } = data;
      messageApi.success("登陆成功");
      userStore.setAccessToken(access_Token);
      userStore.setRefreshToken(refresh_Token);
      userStore.setLoginName(login_Name);
     
      setTimeout(() => {
        navtion("/");
      }, 1000);
    }
  };
  return (
    <div className="login_bg">
      {contextHandler}
      <div
        className="login_adv"
        style={{ backgroundImage: "url(../img/auth.jpg)" }}
      >
        <div className="login_adv__title">
          <h2>易步小店库存管理系统</h2>
        </div>
        <div className="login_adv__mask"></div>
        <div className="login_adv__bottom">© 1.0.0</div>
      </div>
      <div className="login_main">
        <div className="login-form">
          <div className="login-header">
            <div className="logo">
              <img src="imgs/logo.jpg" ></img>
            </div>
            <Tabs
              defaultActiveKey="1"
              items={[
                {
                  key: "1",
                  label: "账号密码",
                  children: (
                    <>
                      <Form
                        name="basic"
                        labelCol={{ span: 6 }}
                        wrapperCol={{ span: 16 }}
                        style={{ maxWidth: 600 }}
                        autoComplete="off"
                        onFinish={onLogin}
                      >
                        <Form.Item<FieldType>
                          label="用户名"
                          name="userName"
                          rules={[
                            {
                              required: true,
                              message: "请输入用户名!",
                            },
                          ]}
                        >
                          <Input />
                        </Form.Item>

                        <Form.Item<FieldType>
                          label="密码"
                          name="userPass"
                          rules={[
                            {
                              required: true,
                              message: "请输入密码!",
                            },
                          ]}
                        >
                          <Input.Password />
                        </Form.Item>

                        <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
                          <Button type="primary" htmlType="submit">
                            立即登陆
                          </Button>
                        </Form.Item>
                      </Form>
                    </>
                  ),
                },
              ]}
            />
          </div>
        </div>
      </div>
    </div>
  );
};
export default Login;
