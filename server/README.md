# 易步小店库存管理系统

## 介绍
 系统适用于小型门店管理系统，主要针对有库存管理需求的商家，
适用类型如【电脑维修商家】

## 技术栈
> 
前端： React18.2 + Typ
esccript + vite + mobx 

> 后端： .Net8 +EFCore8 + WebApi

> 第三方库：MediatR+AutoFac+FluntValidation+Swagger+JsonWebToken(JWT)

> 数据库：   **[SQLSERVER]** 

## 安装使用教程
如何使用?
>后端代码或服务 [将发行包或源代码下载下来即可] 


>前端代码 [前端代码](https://gitee.com/xuguilin/lcpcamdinclient)


需要哪些环境?
>请确保有 node（最低要求16.15.0）环境和.NET8 SDK

数据库支持？

> 开源版目前仅支持 [SQLSERVER] 数据库 


如何运行？
> 前端：安装 `npm install 或 cnpm install` 运行: `npm run dev`

```
后端
1、将发行包或源代码下载
2、修改数据库连接字符串,更改**appsettings.json** 文件中`ConnectionStrings:Default`
3、手动创建数据库，数据库名称为【LCPC】
4、运行源码中【DbCreate】文件夹内语句
```


## 功能清单
| 大功能模块  | 大功能详情               | 
|--------|---------------------|
| 控制台    | 系统数据统计报表            |
| 商品管理   | 用于商品的新增、删除、修改 导入导出等 |
| 采购管理   | 用于商品采购退货、采购今后       |
| 销售开单   | 销售订单                | 
| 订单管理   | 用于已产生订单的管理          | 
| 其他费用管理 | 用于除销售订单外的 店内其它途径产生的费用 | 
| 交易中心   | 客户往来交易、供应商往来交易 统计报表 |
| 基础数据管理 | 管理系统内的基础数据          |

## 功能截图
* 控制台
 > ![控制台.png](LCPC.Admin%2Fimages%2F%E6%8E%A7%E5%88%B6%E5%8F%B0.png)
* 商品管理
> ![商品管理.png](LCPC.Admin%2Fimages%2F%E5%95%86%E5%93%81%E7%AE%A1%E7%90%86.png)
* 采购管理
   * 采购进货
     > ![进货单.png](LCPC.Admin%2Fimages%2F%E8%BF%9B%E8%B4%A7%E5%8D%95.png)
   * 采购退货
     > ![退货单.png](LCPC.Admin%2Fimages%2F%E9%80%80%E8%B4%A7%E5%8D%95.png)
 
* 销售开单
> ![销售开单.png](LCPC.Admin%2Fimages%2F%E9%94%80%E5%94%AE%E5%BC%80%E5%8D%95.png)
* 订单管理

> ![全部订单.png](LCPC.Admin%2Fimages%2F%E5%85%A8%E9%83%A8%E8%AE%A2%E5%8D%95.png)
* 其它费用管理
> ![其它费用.png](LCPC.Admin%2Fimages%2F%E5%85%B6%E5%AE%83%E8%B4%B9%E7%94%A8.png)
* 交易中心
  * 客户往来交易
  > ![客户往来交易.png](LCPC.Admin%2Fimages%2F%E5%AE%A2%E6%88%B7%E5%BE%80%E6%9D%A5%E4%BA%A4%E6%98%93.png)
  * 供应商往来交易(进货)
  > ![供应商往来.png](LCPC.Admin%2Fimages%2F%E4%BE%9B%E5%BA%94%E5%95%86%E5%BE%80%E6%9D%A5.png)
* 基础数据管理
  > ![基础数据管理.png](LCPC.Admin%2Fimages%2F%E5%9F%BA%E7%A1%80%E6%95%B0%E6%8D%AE%E7%AE%A1%E7%90%86.png)
## 部署说明

1.  支持IIS、Linux部署
2.  支持Docker环境部署,需自己打包镜像


## 分支说明
>  **main** 为开源版本

>  **pro**  为商业版

## 关于作者
  * 如此项目对您有帮助，欢迎 **star**
  * 如您需要查看演示环境或体验演示环境，烦请联系作者
  * 如您需要本地部署或定制的需求，烦请联系作者
  >  ![user.jpeg](LCPC.Admin%2Fimages%2Fuser.jpeg)

## 开源协议
MIT License

Copyright (c) 2024 余温

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
