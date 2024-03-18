
import { ITabModel } from ".";
import "./css/index.css";
import { useStore } from "@/store";
import { observer } from "mobx-react-lite";
import { useLocation, useNavigate } from "react-router-dom";
import { useAliveController } from "react-activation";


const options = ['关闭当前','关闭左侧','关闭右侧','关闭其他']
const LayoutTab: React.FC<any> = () => {
  const { homeStore } = useStore();
  const navtion = useNavigate();
  const location  = useLocation()
  const { dropScope } = useAliveController()
  const handlerClick = (
    e: React.MouseEvent<HTMLDivElement, MouseEvent>,
    data: ITabModel
  ) => {
    navtion(data.key, { replace: true });
  };
  const onContextHandler = (e: React.MouseEvent<HTMLDivElement, MouseEvent>,tab:ITabModel) => {
    e.preventDefault();
    if (tab.key !=='/') {
        removeMenu()
        if (location.pathname === tab.key) 
           showMenu(e.pageX, e.pageY,tab);
    }
  };
  const removeMenu =()=>{
    const el =document.querySelectorAll('.rightMenu')
    if (el && el.length>0)
         el.forEach(v=>v.remove())

  }
  document.addEventListener('mousedown',function(e:any) {
    if (e.target.getAttribute("about") !== 'tab') {
      setTimeout(()=>{
        removeMenu()
      },200) 
    }
  })
  const handlerRemove =(index:number, tab:ITabModel)=>{
     if (index === 0) {
      // 关闭当前
       dropScope(tab.key)
       homeStore.removeTab(tab.key);
       const findex = homeStore.tabs.length -1 
       navtion(homeStore.tabs[findex].key, { replace: true });
     }else if (index === 1) {
        homeStore.closeLeft(tab.key)
     }else if (index === 2) {
        homeStore.closeRight(tab.key)
     }else {
       homeStore.closeOthers(tab.key)
     }
  }
  const showMenu = (x: number, y: number,tab:ITabModel) => {
    const div = document.createElement("div");
    div.className = "rightMenu";
    div.style.left = x + "px";
    div.style.top =( y+17) + "px";

   // 创建子项
   for(let i=0;i<options.length;i++) {
    const item = document.createElement("div");
    item.className='child'
    item.innerText = options[i]
    item.setAttribute("data-value-"+i, JSON.stringify(tab))
   
    //item.onclick
    // item.onclick = function() {
    //   handlerRemove(i,tab)
    // }
    div.appendChild(item)
   }
    document.body.appendChild(div);

    document.querySelectorAll(".child").forEach((item:any) => {
    
      item.onclick = function() {
        const attbuites = item. attributes
        if (attbuites && attbuites.length>0) {
          const attbuite = attbuites[1]
          const attName =attbuite.name
          const attValue = item.getAttribute(attName)
          const tab = JSON.parse(attValue)
          const index = attName.split('-')[2]
          handlerRemove(parseInt(index),tab)
        
        }
      }
  
    })
  };
  return homeStore.tabs.map((item, index) => {
    return (
      <div
        className={item.key === homeStore.activeKey ? "active" : ""}
        key={index}
        onClick={(e) => handlerClick(e, item)}
        onContextMenu={(e) => onContextHandler(e,item)}
        about="tab"
  
      >
        {item.label}
      </div>
    );
  });
};
export default observer(LayoutTab);
