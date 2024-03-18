import ReactDOM from "react-dom/client";
import {

  HashRouter,
  Route,
  Routes,
} from "react-router-dom";
import dRoutes from "./router/index.ts";
import locale from "antd/locale/zh_CN";
import "dayjs/locale/zh-cn";
import { ConfigProvider } from "antd";
import KeepAlive, { AliveScope } from "react-activation";
import WatchLayout from "./WatchLayout.tsx";
import Login from "./pages/Login.tsx";
const loadRouters = (routerList: any) => {
  return routerList.map((router: any, index: number) => {
    return (
      <Route
        key={index}
        path={router.path}
        loader={router.loader}
        element={
          <WatchLayout
            authCom={
              <KeepAlive
                autoFreeze={false}
                when={true}
                name={router.path}
                id={router.path}
              >
                {router.element}
              </KeepAlive>
            }
          ></WatchLayout>
        }
      >
        {router?.children && loadRouters(router.children)}
      </Route>
    );
  });
};
ReactDOM.createRoot(document.getElementById("root")!).render(
  <ConfigProvider locale={locale}>
    <HashRouter>
      <AliveScope>
        <Routes>
          {loadRouters(dRoutes)}
          <Route
            path="/login"
            element={<WatchLayout authCom={<Login />}></WatchLayout>}
          />
        </Routes>
      </AliveScope>
    </HashRouter>
  </ConfigProvider>
);
