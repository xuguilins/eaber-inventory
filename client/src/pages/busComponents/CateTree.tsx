import { urls } from "@/api/urls";
import alovaInstance, { IReturnResult } from "@/utils/request";
import { DownOutlined } from "@ant-design/icons";
import { Tree } from "antd";
import { useEffect, useState } from "react";
export interface ICateData {
  id: string;
  label: string;
  value: string;
  key: string;
  children: Array<ICateData>;
}
const CateTree: React.FC<any> = (props: any) => {
  const keys: string[] = [];
  const [defaultKeys, setDefaultKeys] = useState<Array<string>>([]);
  const [treeData, setTreeData] = useState<Array<ICateData>>([]);
  const loadCates = async () => {
    const urlValue = urls.getCateTree + "?enable=false";
    const { success, data } = await alovaInstance
      .Get<IReturnResult, any>(urlValue)
      .send();
    if (success) {
      setTreeData(data);
      buildDefualtKeys(data);
      setDefaultKeys(keys);
    }
  };
  const buildDefualtKeys = (data: Array<ICateData>) => {
    data.forEach((item) => {
      if (item.children && item.children.length > 0) {
        keys.push(item.key);
        buildDefualtKeys(item.children);
      } else {
        keys.push(item.key);
      }
    });
  };
  useEffect(() => {
    async function init() {
      keys.length = 0;
      if (props.isLoad) {
        await loadCates();
      } else {
        setTreeData(props.listData);
        buildDefualtKeys(props.listData);
        setDefaultKeys(keys);
      }
    }
    init();
  }, [props.listData]);
  const onSelect = (keys: any, e: any) => {
    if (keys && keys.length > 0) {
      if (keys[0] === "1000") keys[0] = "";
      const json: ICateData = {
        id: keys[0],
        label: e.node.label,
        value: keys[0],
        children: [],
        key: keys[0],
      };
      props.onNodeClick(json);
    }
  };
  const showTree = (keys: any) => {
    if (keys && keys.length > 0) {
      return (
        <Tree
          showLine
          switcherIcon={<DownOutlined />}
          fieldNames={{
            children: "children",
            key: "value",
            title: "label",
          }}
          defaultExpandAll={true}
          defaultExpandedKeys={keys}
          onSelect={onSelect}
          treeData={treeData}
        />
      );
    }
  };
  return <>{showTree(defaultKeys)}</>;
};
export default CateTree;
