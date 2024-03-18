import { Spin } from "antd";

export const FullLoading = (props:any) => {
  return (
    <div  style={{top:'45%',right:'50%' ,position:'absolute',width:'100px'}}>
      <Spin  tip={props.title} size="large">
        <div className="content"> </div>
      </Spin>
    </div>
  );
};
