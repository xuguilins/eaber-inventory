import { useState, useEffect } from "react";
import * as echarts from "echarts";
const ColumnCard = () => {
  const [width, setWidth] = useState<number>(1000);
  useEffect(() => {
    const el = document.getElementById("columnRow");
    if (el) {
      setWidth(el.clientWidth);
      var myChart = echarts.init(el);
      // 绘制图表
      myChart.setOption({
        title: {
          text: "ECharts 入门示例",
        },
        tooltip: {},
        xAxis: {
          data: ["衬衫", "羊毛衫", "雪纺衫", "裤子", "高跟鞋", "袜子"],
        },
        yAxis: {},
        series: [
          {
            name: "销量",
            type: "bar",
            data: [5, 20, 36, 10, 10, 20],
          },
        ],
      });
    }
  }, []);

  return <>
   
  </>;
};
export default ColumnCard;
