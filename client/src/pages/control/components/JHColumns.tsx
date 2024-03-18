import React, { useEffect, useState } from "react";
import * as echarts from "echarts";
import './orderChats.css'
const JHColumns:React.FC<any> =({ order})=>{
    const options = {
        tooltip: {
            trigger: "axis",
            axisPointer: {
              type: "cross",
              crossStyle: {
                color: "#999",
              },
            },
          },
          xAxis: {
            data: order.xTypes,
          },
          yAxis: {},
          series: [
            {
              name: "进货数量",
              type: "bar",
              data: order.yTypes,
              itemStyle: {
                color: "#2d89ed",
              },
              label: {
                show: true,
                position: "top",
                color: "#2d89ed",
              },
            },
          ],
    }
    const [instance,setInstance] = useState<echarts.ECharts>(null)
    useEffect(() => {
      if(order.xTypes) {
        const el = document.getElementById("jhCharts");
        if (instance) {
            instance.setOption(options);
        }else {
            const instance = echarts.init(el);
            setInstance(instance)
            instance.setOption(options);
        }
      }
        
      
      }, [order]);
    return <>
      <div id="jhCharts"></div>
    </>
}
export default JHColumns