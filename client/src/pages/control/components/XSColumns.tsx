import React, { useEffect, useState } from "react";
import * as echarts from "echarts";
import './orderChats.css'
const XSColumns:React.FC<any> =({ order})=>{
  const { xTypes ,yTypes } = order
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
            data: xTypes,
          },
          yAxis: {},
          series: [
            {
              name: "销售额",
              type: "bar",
              data: yTypes,
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
      if(xTypes) {
        const el = document.getElementById("xsCharts");
        if (instance) {
            instance.setOption(options);
        }else {
            const instance = echarts.init(el);
            setInstance(instance)
            instance.setOption(options);
        }
       setTimeout(()=>{
         instance?.resize()
       },2000)
      }
      
      }, [order]);
    return <>
      <div id="xsCharts"></div>
    </>
}
export default XSColumns